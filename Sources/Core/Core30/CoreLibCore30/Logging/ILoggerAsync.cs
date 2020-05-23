using System;
using System.Threading.Tasks;

namespace Mohammad.Logging
{
    public interface ILoggerAsync
    {
        Task WriteAsync(object message, LogLevel level = LogLevel.Info, DateTime? time = null);
        Task WriteAsync(ILog log);
    }
}