using System;

namespace Mohammad.Logging
{
    public interface ILogger
    {
        void Write(object message, LogLevel level = LogLevel.Info, DateTime? time = null);
        void Write(ILog   log);
    }
}