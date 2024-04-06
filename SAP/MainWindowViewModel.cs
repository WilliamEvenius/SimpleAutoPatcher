using DynamicData;
using DynamicData.Aggregation;
using ReactiveUI;
using SAP.Application.State;
using SAP.Interfaces;
using SAP.Interfaces.Services;
using SAP.Interfaces.State;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP
{
    public class MainWindowViewModel : ReactiveObject, IActivatableViewModel
    {
        public IState? CurrentState => _currentState?.Value;
        public float PatchProgress => _patchProgress?.Value ?? 0f;
        public float TotalPatchProgress => _totalPatchProgress?.Value ?? 0f;
        public int RemainingUpdates => _remainingUpdates?.Value ?? 0;
        public int TotalUpdates => _totalUpdates?.Value ?? 0;
        public ViewModelActivator Activator { get; } = new();
        public ReactiveCommand<Unit, Unit> StartGameCommand { get; }

        public MainWindowViewModel(IApplicationStateMachine stateMachine, IPatcherService patcherService)
        {
            _stateMachine = stateMachine;
            StartGameCommand = ReactiveCommand.CreateFromTask(StartGame, WhenIdle());

            this.WhenActivated(disposables =>
            {
                _currentState = WhenStateChanges()
                    .ToProperty(this, x => x.CurrentState)
                    .DisposeWith(disposables);

                _patchProgress = patcherService.Progress
                    .ValueChanged()
                    .Select(progress => progress * 100)
                    .ToProperty(this, x => x.PatchProgress)
                    .DisposeWith(disposables);

                _totalPatchProgress = this.WhenAnyValue(x => x.RemainingUpdates, x => x.PatchProgress, x => x.TotalUpdates,
                        (Remaining, CurrentPercent, TotalUpdates) => (Remaining, CurrentPercent, TotalUpdates))
                    .Select(work => GetTotal(work))
                    .ToProperty(this, x => x.TotalPatchProgress)
                    .DisposeWith (disposables);

                _remainingUpdates = patcherService.Connect()
                    .Filter(p => p.State is PatchState.Initial or PatchState.Downloaded)
                    .Count()
                    .ToProperty(this, x => x.RemainingUpdates)
                    .DisposeWith(disposables);

                _totalUpdates = patcherService.Connect()
                    .Count()
                    .ToProperty(this, x => x.TotalUpdates)
                    .DisposeWith(disposables);

                _stateMachine.Initialize().DisposeWith(disposables);
            });
        }

        private float GetTotal((int Remaining, float CurrentPercent, int TotalUpdates) work)
        {
            if (work.TotalUpdates < work.Remaining || work.TotalUpdates == 0) return 0;

            var currentPatch = TotalUpdates - work.Remaining;
            var baseProgress = ((float)currentPatch / work.TotalUpdates) * 100;

            var test = baseProgress + (work.CurrentPercent / work.TotalUpdates);

            return test;
        }

        private ObservableAsPropertyHelper<float>? _patchProgress;
        private ObservableAsPropertyHelper<float>? _totalPatchProgress;
        private ObservableAsPropertyHelper<int>? _remainingUpdates;
        private ObservableAsPropertyHelper<int>? _totalUpdates;
        private ObservableAsPropertyHelper<IState?>? _currentState;
        private readonly IApplicationStateMachine _stateMachine;

        private IObservable<IState?> WhenStateChanges() => this.WhenAnyValue(x => x._stateMachine.CurrentState);
        private IObservable<bool> WhenIdle() => WhenStateChanges().Select(state => state?.Name is "Idle");

        private async Task StartGame()
        {
            Console.WriteLine("Started game");
        }
    }
}
