using Library.EventsArgs;

namespace Library.Logging;

public class FastLogger : FastLoggerBase<object>, ILogger, IEventualLogger
{
    public event EventHandler<ItemActedEventArgs<object>>? Logging;

    protected override void OnLogging(LogRecord<object> logRecord)
        => Catch(() => this.Logging?.Invoke(logRecord.Sender, new ItemActedEventArgs<object>(logRecord)));
}
