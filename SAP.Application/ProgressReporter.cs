using ReactiveUI;
using SAP.Application.Services;
using SAP.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Application
{
    public class ProgressReporter<T> : ReactiveObject, IProgress<T>, IProgressReporter<T>
    {
        private T? _value = default;
        private T? Value
        {
            get => _value;
            set => this.RaiseAndSetIfChanged(ref _value, value);
        }

        public void Report(T value)
        {
            Value = value;
        }

        public IObservable<T> ValueChanged() => this.WhenAnyValue(x => x.Value)
            .WhereNotNull();
    }
}
