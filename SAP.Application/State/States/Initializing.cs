using SAP.Interfaces.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Application.State.States
{
    internal class Initializing : IState
    {
        public string Name => nameof(Initializing);
        public IObservable<ITransition> Enter()
        {
            return Observable.Create<ITransition>(
                async observer =>
                {
                    await Task.Delay(2000);

                    observer.OnNext(new ToCheckingUpdates());
                }
            );
        }
    }
}
