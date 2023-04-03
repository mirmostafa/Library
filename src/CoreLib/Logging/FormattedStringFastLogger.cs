using Library.EventsArgs;

namespace Library.Logging;

public sealed class FormattedStringFastLogger : FastLoggerBase<object>, ILogger
{
    private string? _format;

    public event EventHandler<ItemActedEventArgs<string>>? Logged;
    public string Format { get => this._format.IfNullOrEmpty(LogFormat.FORMAT_DEFAULT); set => this._format = value; }

    public FormattedStringFastLogger(string? format = null)
        => this.Format = format ?? LogFormat.FORMAT_DEFAULT;

    protected override void OnLogging(LogRecord<object> logRecord)
    {
        if (logRecord is not null)
        {
            Logged?.Invoke(this, new(logRecord.Reformat(this._format)));
        }
    }
}
