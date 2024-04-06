using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Interfaces.State
{
    public interface IState
    {
        string Name { get; }
        IObservable<ITransition> Enter();
    }
}
