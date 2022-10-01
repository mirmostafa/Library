namespace Library.MultistepProgress;

public class MultiStepErrorOccurredEventArgs : LogEventArgs
{
    public Exception Exception { get; }

    public MultiStepErrorOccurredEventArgs(Exception exception, object? log)
        : base(log ?? new(), string.Empty) => this.Exception = exception;

    public override string? ToString() => this.Exception?.GetBaseException().Message ?? base.ToString();
}
