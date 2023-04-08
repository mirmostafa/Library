using Library.EventsArgs;

namespace Library.Logging;

public sealed class StringLogger : FastLoggerBase<string>
{
    public event EventHandler<ItemActedEventArgs<LogRecord<string>>>? Logging;
    protected override void OnLogging(LogRecord<string> logRecord) => Logging?.Invoke(this, new(logRecord));
}
