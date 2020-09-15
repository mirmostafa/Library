using System;
using Mohammad.Logging;

namespace Mohammad.EventsArgs
{
    public class LogEventArgs : EventArgs
    {
        public LogEventArgs(object log, object moreInfo = null, LogLevel level = LogLevel.Info)
        {
            this.Log = log;
            this.MoreInfo = moreInfo;
            this.Level = level;
        }

        public object Log { get; set; }
        public object MoreInfo { get; set; }
        public LogLevel Level { get; set; }
        public object Sender { get; set; }
        public string MemberName { get; set; }
        public string SourceFilePath { get; set; }
        public int SourceLineNumber { get; set; }
    }

    public class MultiStepStartedLogEventArgs : LogEventArgs
    {
        public MultiStepStartedLogEventArgs(double maxSteps,
            object log = null,
            object moreInfo = null,
            LogLevel level = LogLevel.Info,
            double initialValue = 0)
            : base(log, moreInfo, level)
        {
            this.Max = maxSteps;
            this.InitialValue = initialValue;
        }

        public double Max { get; }
        public double InitialValue { get; }
    }

    public class MultiStepLogEventArgs : LogEventArgs
    {
        public MultiStepLogEventArgs(double step,
            object log = null,
            object moreInfo = null,
            object sender = null,
            LogLevel level = LogLevel.Info,
            double max = -1)
            : base(log, moreInfo, level)
        {
            this.Step = step;
            this.Sender = sender;
            this.Max = max;
        }

        public double Step { get; }
        public double Max { get; }
    }

    public class MultiStepEndedLogEventArgs : LogEventArgs
    {
        public MultiStepEndedLogEventArgs(object log, bool isSucceed, bool isCancelled)
            : base(log, string.Empty)
        {
            this.IsSucceed = isSucceed;
            this.IsCancelled = isCancelled;
        }

        public bool IsSucceed { get; }
        public bool IsCancelled { get; set; }
    }

    public class MultiStepErrorOccurredEventArgs : LogEventArgs
    {
        public MultiStepErrorOccurredEventArgs(Exception exception, object log)
            : base(log, string.Empty) => this.Exception = exception;

        public Exception Exception { get; }

        public override string ToString() => this.Exception?.GetBaseException().Message ?? base.ToString();
    }
}