using SAP.Interfaces.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Interfaces.Factories
{
    public interface IStateFactory
    {
        IState CheckingUpdates();
        IState Downloading();
        IState Idle();
        IState Initializing();
        IState Installing();
    }
}
