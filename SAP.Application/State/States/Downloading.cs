using SAP.Application.Services;
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
    internal class Downloading(IPatcherService patcherService) : IState
    {
        public string Name => nameof(Downloading);

        public IObservable<ITransition> Enter()
        {
            return Observable.Create<ITransition>(
                async observer =>
                {
                    var downloadExists = await patcherService.DownloadQueued();

                    if (downloadExists)
                    {
                        observer.OnNext(new ToInstalling());
                    }
                    else
                    {
                        observer.OnNext(new ToIdle());
                    }
                }
            );
        }
    }
}
