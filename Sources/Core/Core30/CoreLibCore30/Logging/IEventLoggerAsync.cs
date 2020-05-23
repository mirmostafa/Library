using System;

namespace Mohammad.Logging
{
    public interface IEventLoggerAsync : ILoggerAsync
    {
        event EventHandler<ILog> Logged;
    }
}