using Library.Logging;

namespace Library.Threading.MultistepProgress;

public sealed class MultiStepStartedLogEventArgs(double maxSteps, object? log = null, object? moreInfo = null, LogLevel level = LogLevel.Info, double initialValue = 0)
    : LogEventArgs(log ?? new(), moreInfo, level)
{
    public double InitialValue { get; private set; } = initialValue;
    public double Max { get; private set; } = maxSteps;
}