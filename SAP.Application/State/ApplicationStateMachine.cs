using ReactiveUI;
using SAP.Interfaces.Factories;
using SAP.Interfaces.State;
using Stateless.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Application.State
{
    public class ApplicationStateMachine : DisposableBase, IApplicationStateMachine
    {
        public IState CurrentState => _currentState.Value;

        private readonly ObservableAsPropertyHelper<IState> _currentState;
        private readonly IStateFactory _factory;
        private readonly Subject<IState> _state;

        public ApplicationStateMachine(IStateFactory factory)
        {
            _factory = factory;
            _state = new Subject<IState>().DisposeWith(Garbage);
            _currentState = _state.ToProperty(this, x => x.CurrentState).DisposeWith(Garbage);
        }

        public IDisposable Initialize()
        {
            IObservable<ITransition> transitions = _state
                .StartWith(_factory.Initializing())
                .Select(state => state.Enter())
                .Switch()
                .Publish()
                .RefCount();

            IObservable<IState> states = Observable.Merge(
                transitions.OfType<ToInitializing>().Select(transition => _factory.Initializing()),
                transitions.OfType<ToCheckingUpdates>().Select(transition => _factory.CheckingUpdates()),
                transitions.OfType<ToDownloading>().Select(transition => _factory.Downloading()),
                transitions.OfType<ToInstalling>().Select(transition => _factory.Installing()),
                transitions.OfType<ToIdle>().Select(transition => _factory.Idle())
            );

            return states
                .ObserveOn(Scheduler.CurrentThread)
                .Subscribe(_state);
        }
    }
}
