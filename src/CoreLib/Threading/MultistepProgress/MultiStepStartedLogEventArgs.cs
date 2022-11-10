using Library;
using Library.Logging;

namespace Library.Threading.MultistepProgress;

public class MultiStepStartedLogEventArgs : LogEventArgs
{
    public double Max { get; private set; }
    public double InitialValue { get; private set; }

    public MultiStepStartedLogEventArgs(double maxSteps, object? log = null, object? moreInfo = null, LogLevel level = LogLevel.Info, double initialValue = 0)
        : base(log ?? new(), moreInfo, level)
    {
        this.Max = maxSteps;
        this.InitialValue = initialValue;
    }
}
