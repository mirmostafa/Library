using Library.Logging;

namespace Library.Threading.MultistepProgress;

public sealed class MultiStepLogEventArgs(
    double step,
    object? log = null,
    object? moreInfo = null,
    object? sender = null,
    LogLevel level = LogLevel.Info,
    double max = -1) : LogEventArgs(log ?? new(), moreInfo, level, sender)
{
    public double Max { get; private set; } = max;
    public double Step { get; private set; } = step;
}