using System;
using Mohammad.EventsArgs;

namespace Mohammad.Interfaces
{
    public interface IProgressiveOperation<T>
    {
        event EventHandler<ProgressiveOperationEventArgs<T>> ProgressChanged;
    }
}