using Library.Logging;

namespace Library.Threading.MultistepProgress;

public sealed class MultiStepEndedLogEventArgs(object? log, bool isSucceed, bool isCancelled) : LogEventArgs(log ?? new(), string.Empty)
{
    public bool IsCancelled { get; set; } = isSucceed;
    public bool IsSucceed { get; private set; } = isCancelled;
}