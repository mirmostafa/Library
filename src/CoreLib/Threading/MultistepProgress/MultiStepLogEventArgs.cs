using Library;
using Library.Logging;

namespace Library.Threading.MultistepProgress;

public class MultiStepLogEventArgs : LogEventArgs
{
    public double Step { get; private set; }
    public double Max { get; private set; }

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
}
