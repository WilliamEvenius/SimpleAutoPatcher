
namespace SAP.Interfaces.State
{
    public interface IApplicationStateMachine
    {
        IState CurrentState { get; }
        IDisposable Initialize();
    }
}