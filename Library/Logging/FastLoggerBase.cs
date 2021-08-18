using Library.Coding;

namespace Library.Logging
{
    public abstract class FastLoggerBase<TLogMessage> : ILogger<TLogMessage>
    {
        private readonly Action<LogRecord<TLogMessage>> EmptyAction = _ => { };
        private readonly Action<LogRecord<TLogMessage>> LogAction;
        private bool _IsEnabled = true;
        private LogLevel logLevel = LogLevel.Normal;
        private Action<LogRecord<TLogMessage>> TraceAction;
        private Action<LogRecord<TLogMessage>> DebugAction;
        private Action<LogRecord<TLogMessage>> ErrorAction;
        private Action<LogRecord<TLogMessage>> FatalAction;
        private Action<LogRecord<TLogMessage>> InfoAction;
        private Action<LogRecord<TLogMessage>> WarnAction;

        protected FastLoggerBase()
        {
            this.LogAction = e => this.Log(e);
            this.IsEnabled = true;
            this.LogLevel = LogLevel.Normal;
        }

        public bool IsEnabled
        {
            get => this._IsEnabled;
            set
            {
                this._IsEnabled = value;
                this.ResetAction();
            }
        }
        public LogLevel LogLevel
        {
            get => this.logLevel;
            set
            {
                this.logLevel = value;
                this.ResetAction();
            }
        }

        private static object? ProcessSender(object? sender, LogLevel level)
            => (sender, level) switch
            {
                (not null, _) => sender,
                (_, LogLevel.Debug or LogLevel.Trace) => CodeHelper.GetCallerMethod(3)?.Name,
                (_, _) => CodeHelper.GetCallerMethod(3).DeclaringType?.Name,
            };

        public void Info(TLogMessage message, object? sender = null, DateTime? time = null)
            => this.InfoAction(new(message, LogLevel.Info, ProcessSender(sender, LogLevel.Info), time));
        public void Warn(TLogMessage message, object? sender = null, DateTime? time = null)
            => this.WarnAction(new(message, LogLevel.Warning, ProcessSender(sender, LogLevel.Warning), time));
        public void Error(TLogMessage message, object? sender = null, DateTime? time = null)
            => this.ErrorAction(new(message, LogLevel.Error, ProcessSender(sender, LogLevel.Error), time));
        public void Fatal(TLogMessage message, object? sender = null, DateTime? time = null)
            => this.FatalAction(new(message, LogLevel.Fatal, ProcessSender(sender, LogLevel.Fatal), time));
        public void Debug(TLogMessage message, object? sender = null, DateTime? time = null)
            => this.DebugAction(new(message, LogLevel.Debug, ProcessSender(sender, LogLevel.Debug), time));
        public void Trace(TLogMessage message, object? sender = null, DateTime? time = null, string? stackTrace = null)
            => this.TraceAction(new(message, LogLevel.Trace, ProcessSender(sender, LogLevel.Trace), time, stackTrace));
        public void Log(TLogMessage message, LogLevel level = LogLevel.Info, object? sender = null, DateTime? time = null, string? stackTrace = null)
            => this.Log(new(message, level, ProcessSender(sender, level), time, stackTrace));
        public void Log(LogRecord<TLogMessage> logRecord)
        {
            if (!this.IsEnabled || !logRecord.ArgumentNotNull(nameof(logRecord)).Level.MeetsLevel(this.LogLevel))
            {
                return;
            }

            this.OnLogging(logRecord);
        }

        protected abstract void OnLogging(LogRecord<TLogMessage> logRecord);

        private void ResetAction()
        {
            this.TraceAction = this.DebugAction = this.ErrorAction = this.FatalAction = this.InfoAction = this.WarnAction = this.EmptyAction;
            if (!this.IsEnabled)
            {
                return;
            }

            if (LogLevel.Trace.MeetsLevel(this.LogLevel))
            {
                this.TraceAction = this.LogAction;
            }
            if (LogLevel.Debug.MeetsLevel(this.LogLevel))
            {
                this.DebugAction = this.LogAction;
            }
            if (LogLevel.Info.MeetsLevel(this.LogLevel))
            {
                this.InfoAction = this.LogAction;
            }
            if (LogLevel.Error.MeetsLevel(this.LogLevel))
            {
                this.ErrorAction = this.LogAction;
            }
            if (LogLevel.Fatal.MeetsLevel(this.LogLevel))
            {
                this.FatalAction = this.LogAction;
            }
            if (LogLevel.Warning.MeetsLevel(this.LogLevel))
            {
                this.WarnAction = this.LogAction;
            }
        }
    }
}