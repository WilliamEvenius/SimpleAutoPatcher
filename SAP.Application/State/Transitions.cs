using SAP.Interfaces.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Application.State
{
    internal record ToInitializing : ITransition { }
    internal record ToCheckingUpdates : ITransition { }
    internal record ToDownloading : ITransition { }
    internal record ToInstalling : ITransition { }
    internal record ToIdle : ITransition { }
}
