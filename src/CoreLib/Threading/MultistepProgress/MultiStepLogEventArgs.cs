using Library.Logging;

namespace Library.Threading.MultistepProgress;

public sealed class MultiStepLogEventArgs : LogEventArgs
{
    public MultiStepLogEventArgs(
        double step,
        object? log = null,
        object? moreInfo = null,
        object? sender = null,
        LogLevel level = LogLevel.Info,
        double max = -1)
        : base(log ?? new(), moreInfo, level)
    {
        this.Step = step;
        this.Sender = sender;
        this.Max = max;
    }

    public double Max { get; private set; }
    public double Step { get; private set; }
}