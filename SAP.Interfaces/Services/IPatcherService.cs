using DynamicData;
using SAP.Interfaces.Models;
using System.Reactive.Subjects;

namespace SAP.Interfaces.Services
{
    public interface IPatcherService
    {
        Subject<IPatch> ActivePatchChanged { get; }
        IPatch ActivePatch { get; }
        IProgressReporter<float> Progress { get; }

        Task CheckForUpdates();
        IObservable<IChangeSet<IPatch, int>> Connect();
        Task<bool> DownloadQueued();
        Task InstallQueued();
        void QueuePatch(IPatch patch);
    }
}