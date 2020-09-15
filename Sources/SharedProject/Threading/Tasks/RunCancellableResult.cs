using System.Threading;

namespace Mohammad.Threading.Tasks
{
    public class RunCancellableResult
    {
        public bool IsDone { get; internal set; }
        public CancellationTokenSource CancellationTokenSource { get; internal set; }
    }

    public class RunCancellableResult<TResult> : RunCancellableResult
    {
        public TResult Result { get; internal set; }
    }
}