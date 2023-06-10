using Library.EventsArgs;

namespace Library.Logging;

public sealed class FormattedStringFastLogger(string? format = null) : FastLoggerBase<object>, ILogger
{
    private string? _format = format ?? LogFormat.FORMAT_DEFAULT;

    public event EventHandler<ItemActedEventArgs<string>>? Logged;

    public string Format { get => this._format.IfNullOrEmpty(LogFormat.FORMAT_DEFAULT); set => this._format = value; }

    protected override void OnLogging(LogRecord<object> logRecord)
    {
        if (logRecord is not null)
        {
            Logged?.Invoke(this, new(logRecord.Reformat(this._format)));
        }
    }
}