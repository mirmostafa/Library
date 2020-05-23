using System;

namespace Mohammad.Logging
{
    public interface IEventLogger : ILogger
    {
        event EventHandler<ILog> Logged;
    }
}