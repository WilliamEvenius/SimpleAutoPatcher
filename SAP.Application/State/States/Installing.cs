using SAP.Interfaces.Services;
using SAP.Interfaces.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Application.State.States
{
    internal class Installing(IPatcherService patcherService) : IState
    {
        public string Name => nameof(Installing);
        public IObservable<ITransition> Enter()
        {
            return Observable.Create<ITransition>(
                async observer =>
                {
                    await patcherService.InstallQueued();
                    observer.OnNext(new ToDownloading());
                }
            );
        }
    }
}
