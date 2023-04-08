using Library;
using Library.Logging;

namespace Library.Threading.MultistepProgress;

public sealed class MultiStepEndedLogEventArgs : LogEventArgs
{
    public bool IsSucceed { get; private set; }
    public bool IsCancelled { get; set; }

    public MultiStepEndedLogEventArgs(object? log, bool isSucceed, bool isCancelled)
        : base(log ?? new(), string.Empty)
    {
        this.IsSucceed = isSucceed;
        this.IsCancelled = isCancelled;
    }
}
