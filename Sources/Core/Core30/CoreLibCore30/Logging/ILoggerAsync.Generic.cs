using System;
using System.Threading.Tasks;

namespace Mohammad.Logging
{
    public interface ILoggerAsync<TLog>
        where TLog : ILog
    {
        Task<TLog> WriteAsync(object message, LogLevel level = LogLevel.Info, DateTime? time = null);
        Task<TLog> WriteAsync(TLog log);
    }
}