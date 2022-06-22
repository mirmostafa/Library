using Library.Validations;

namespace Library.Logging;

public sealed class LogToTextWriter<TLogMessage> : ILogger<TLogMessage>
{
    private readonly TextWriter _writer;
    private readonly Func<LogRecord, string> _formatter;

    public LogToTextWriter([DisallowNull] TextWriter writer, Func<LogRecord, string>? formatter = null)
    {
        this._writer = writer.ArgumentNotNull(nameof(writer));
        this._formatter = formatter ?? (log => LoggingHelper.Reformat(log));
    }

    public bool IsEnabled { get; set; }
    public LogLevel LogLevel { get; set; }

    public void Log(TLogMessage message, LogLevel level = LogLevel.Info, object? sender = null, DateTime? time = null, string? stackTrace = null)
    {
        if (message == null || !this.IsEnabled || level.MeetsLevel(this.LogLevel))
        {
            return;
        }

        this._writer.WriteLine(this._formatter(new LogRecord(message, level, sender, time, stackTrace)));
    }
}
