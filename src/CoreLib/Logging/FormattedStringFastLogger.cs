using Library.EventsArgs;

namespace Library.Logging;

public sealed class FormattedStringFastLogger : FastLoggerBase<object>
{
    public event EventHandler<ItemActedEventArgs<string>>? Logged;

    protected override void OnLogging(LogRecord<object> logRecord)
    {
        if (logRecord is not null)
        {
            Logged?.Invoke(this, new(logRecord.Reformat()));
        }
    }
}