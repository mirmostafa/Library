using Library.Logging;

namespace Library.Threading.MultistepProgress;

public sealed class MultiStepEndedLogEventArgs : LogEventArgs
{
    public MultiStepEndedLogEventArgs(object? log, bool isSucceed, bool isCancelled)
        : base(log ?? new(), string.Empty)
    {
        this.IsSucceed = isSucceed;
        this.IsCancelled = isCancelled;
    }

    public bool IsCancelled { get; set; }
    public bool IsSucceed { get; private set; }
}