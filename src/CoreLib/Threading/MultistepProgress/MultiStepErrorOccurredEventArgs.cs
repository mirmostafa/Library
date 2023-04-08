using Library;
using Library.Logging;

namespace Library.Threading.MultistepProgress;

public sealed class MultiStepErrorOccurredEventArgs : LogEventArgs
{
    public Exception Exception { get; }

    public MultiStepErrorOccurredEventArgs(Exception exception, object? log)
        : base(log ?? new(), string.Empty) => this.Exception = exception;

    public override string? ToString() => this.Exception?.GetBaseException().Message ?? base.ToString();
}
