using Library.EventsArgs;

namespace Library.Logging;

/// <summary>
/// Represents a class that provides logging functionality and raises an event when logging.
/// </summary>
public sealed class FastLogger : FastLoggerBase<object>, ILogger, IEventualLogger
{
    public event EventHandler<ItemActedEventArgs<object>>? Logging;

    protected override void OnLogging(LogRecord<object> logRecord)
        => Catch(() => this.Logging?.Invoke(logRecord.Sender, new ItemActedEventArgs<object>(logRecord)));
}
