using System;

namespace Mohammad.Logging
{
    public interface ILogger<TLog>
        where TLog : ILog
    {
        TLog Write(object message, LogLevel level = LogLevel.Info, DateTime? time = null);
        TLog Write(TLog log);
    }
}