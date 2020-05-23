using System;

namespace Mohammad.Logging
{
    public interface IEventLogger<TLog> : ILogger<TLog>
        where TLog : ILog
    {
        event EventHandler<TLog> Logged;
    }
}