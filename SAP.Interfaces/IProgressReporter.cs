
namespace SAP.Interfaces
{
    public interface IProgressReporter<T>
    {
        void Report(T value);
        IObservable<T> ValueChanged();
    }
}