using Library.Logging;

namespace Library.Threading.MultistepProgress;

public sealed class MultiStepErrorOccurredEventArgs(Exception exception, object? log) : LogEventArgs(log ?? new(), string.Empty)
{
    public Exception Exception { get; } = exception;

    public override string? ToString() 
        => this.Exception?.GetBaseException().Message ?? base.ToString();
}