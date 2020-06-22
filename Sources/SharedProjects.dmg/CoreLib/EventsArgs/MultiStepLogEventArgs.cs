using System;
using Mohammad.Logging;

namespace Mohammad.EventsArgs
{
    public class LogEventArgs : EventArgs
    {
        public object Log { get; set; }
        public object MoreInfo { get; set; }
        public LogLevel Level { get; set; }
        public object Sender { get; set; }
        public string MemberName { get; set; }
        public string SourceFilePath { get; set; }
        public int SourceLineNumber { get; set; }

        public LogEventArgs(object log, object moreInfo = null, LogLevel level = LogLevel.Info)
        {
            this.Log = log;
            this.MoreInfo = moreInfo;
            this.Level = level;
        }
    }

    public class MultiStepStartedLogEventArgs : LogEventArgs
    {
        public double Max { get; private set; }
        public double InitialValue { get; private set; }

        public MultiStepStartedLogEventArgs(double maxSteps, object log = null, object moreInfo = null, LogLevel level = LogLevel.Info, double initialValue = 0)
            : base(log, moreInfo, level)
        {
            this.Max = maxSteps;
            this.InitialValue = initialValue;
        }
    }

    public class MultiStepLogEventArgs : LogEventArgs
    {
        public double Step { get; private set; }
        public double Max { get; private set; }

        public MultiStepLogEventArgs(double step, object log = null, object moreInfo = null, object sender = null, LogLevel level = LogLevel.Info, double max = -1)
            : base(log, moreInfo, level)
        {
            this.Step = step;
            this.Sender = sender;
            this.Max = max;
        }
    }

    public class MultiStepEndedLogEventArgs : LogEventArgs
    {
        public bool IsSucceed { get; private set; }
        public bool IsCancelled { get; set; }

        public MultiStepEndedLogEventArgs(object log, bool isSucceed, bool isCancelled)
            : base(log, string.Empty)
        {
            this.IsSucceed = isSucceed;
            this.IsCancelled = isCancelled;
        }
    }

    public class MultiStepErrorOccurredEventArgs : LogEventArgs
    {
        public Exception Exception { get; }

        public MultiStepErrorOccurredEventArgs(Exception exception, object log)
            : base(log, string.Empty) { this.Exception = exception; }

        public override string ToString() => this.Exception?.GetBaseException().Message ?? base.ToString();
    }
}