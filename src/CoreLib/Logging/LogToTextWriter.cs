using Library.Validations;

namespace Library.Logging;

public sealed class LogToTextWriter<TLogMessage>([DisallowNull] TextWriter writer, Func<LogRecord, string>? formatter = null) : ILogger<TLogMessage>
{
    private readonly Func<LogRecord, string> _formatter = formatter ?? (log => LoggingHelper.Reformat(log));
    private readonly TextWriter _writer = writer.ArgumentNotNull(nameof(writer));

    public bool IsEnabled { get; set; }
    public LogLevel LogLevel { get; set; }

    public void Log(TLogMessage message, LogLevel level = LogLevel.Info, object? sender = null, DateTime? time = null, string? stackTrace = null)
    {
        if (this.IsEnabled && message != null && level.MeetsLevel(this.LogLevel))
        {
            this._writer.WriteLine(this._formatter(new LogRecord(message, level, sender, time, stackTrace)));
        }
    }
}