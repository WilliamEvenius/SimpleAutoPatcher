using SAP.Application.State.States;
using SAP.Interfaces.Factories;
using SAP.Interfaces.Services;
using SAP.Interfaces.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Application.Factories
{
    public class StateFactory(IPatcherService patcherService) : IStateFactory
    {
        IState IStateFactory.CheckingUpdates()
        {
            return new CheckingUpdates(patcherService);
        }

        IState IStateFactory.Downloading()
        {
            return new Downloading(patcherService);
        }

        IState IStateFactory.Idle()
        {
            return new Idle();
        }

        IState IStateFactory.Initializing()
        {
            return new Initializing();
        }

        IState IStateFactory.Installing()
        {
            return new Installing(patcherService);
        }
    }
}
