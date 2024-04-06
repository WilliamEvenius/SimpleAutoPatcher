using DynamicData;
using ReactiveUI;
using SAP.Application.Extensions;
using SAP.Application.Models;
using SAP.Interfaces;
using SAP.Interfaces.Models;
using SAP.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SAP.Application.Services
{
    public class PatcherService : DisposableBase, IPatcherService
    {
        public Subject<IPatch> ActivePatchChanged { get; }
        public IProgressReporter<float> Progress => _progressReporter;
        public IPatch ActivePatch => _activePatch.Value;

        public PatcherService(IProgressReporter<float> progress)
        {
            ActivePatchChanged = new Subject<IPatch>().DisposeWith(Garbage);
            _activePatch = ActivePatchChanged.ToProperty(this, x => x.ActivePatch).DisposeWith(Garbage);
            _progressReporter = progress;
            _queue = new SourceCache<IPatch, int>(x => x.Id);
        }

        private IProgressReporter<float> _progressReporter;
        private SourceCache<IPatch, int> _queue;
        private readonly ObservableAsPropertyHelper<IPatch> _activePatch;

        public IObservable<IChangeSet<IPatch, int>> Connect()
        {
            return _queue.Connect();
        }

        public async Task CheckForUpdates()
        {
            QueuePatch(new Patch() { Id = 1, Size = 1000, Uri = new Uri("https://redirector.gvt1.com/edgedl/android/studio/install/2023.2.1.24/android-studio-2023.2.1.24-windows.exe") });
            QueuePatch(new Patch() { Id = 2, Size = 1000, Uri = new Uri("https://redirector.gvt1.com/edgedl/android/studio/install/2023.2.1.24/android-studio-2023.2.1.24-windows.exe") });
            await Task.Delay(2000);
        }

        public void QueuePatch(IPatch patch)
        {
            _queue.AddOrUpdate(patch);
        }

        public async Task<bool> DownloadQueued()
        {
            var queue = _queue.Items.ToList();

            if (queue.Count(x => x.State is PatchState.Initial) == 0)
                return false;

            var nextPatch = queue.First(x => x.State is PatchState.Initial);
            ActivePatchChanged.OnNext(nextPatch);

            await DownloadPatch(nextPatch);

            return true;
        }

        private async Task DownloadPatch(IPatch patch)
        {
            try
            {
                // move to api service

                var filePath = Path.Combine(Environment.CurrentDirectory, $"downloadedFile{patch.Id}.exe");
                var cancellationToken = new CancellationToken();

                using var client = new HttpClient()
                {
                    Timeout = TimeSpan.FromMinutes(5)
                };

                using var file = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                await client.DownloadAsync(patch.Uri.ToString(), file, (IProgress<float>)_progressReporter, cancellationToken);
                patch.State = PatchState.Downloaded;
                _queue.AddOrUpdate(patch);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task InstallQueued()
        {
            var patch = ActivePatch;
            await Task.Delay(1000);
            patch.State = PatchState.Installed;
            _queue.AddOrUpdate(patch);
        }
    }
}
