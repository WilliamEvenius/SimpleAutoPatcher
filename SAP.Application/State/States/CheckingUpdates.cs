using SAP.Application.Models;
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
    internal class CheckingUpdates(IPatcherService patcherService) : IState
    {
        public string Name => nameof(CheckingUpdates);
        public IObservable<ITransition> Enter()
        {
            return Observable.Create<ITransition>(
                async observer =>
                {
                    //await _dataStoreContext.InitializeAsync();
                    await patcherService.CheckForUpdates();
                    observer.OnNext(new ToDownloading());
                }
            );
        }
    }
}
