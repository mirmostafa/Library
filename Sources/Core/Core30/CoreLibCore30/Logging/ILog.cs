using System;

namespace Mohammad.Logging
{
    public interface ILog
    {
        DateTime Time { get; }
        object Message { get; }
        LogLevel Level { get; }
    }

    public class Log : ILog
    {
        public Log(object message, LogLevel level = LogLevel.Info, DateTime? time = null)
        {
            this.Time = time ?? DateTime.Now;
            this.Message = message ?? throw new ArgumentNullException(nameof(message));
            this.Level = level;
        }

        #region Implementation of ILog

        public DateTime Time { get; }
        public object Message { get; }
        public LogLevel Level { get; }

        #endregion
    }
}