using System.Diagnostics.CodeAnalysis;

namespace Library.Logging;

public sealed class LogToTextWriter<TLogMessage> : ILogger<TLogMessage>
{
    private readonly TextWriter _writer;
    private readonly Func<LogRecord, string> _formatter;

    public LogToTextWriter([DisallowNull] TextWriter writer, Func<LogRecord, string>? formatter = null)
    {
        this._writer = writer.ArgumentNotNull(nameof(writer));
        this._formatter = formatter ?? DefaulFormatter;
    }
    public bool IsEnabled { get; set; }
    public LogLevel LogLevel { get; set; }
    public static Func<LogRecord, string> DefaulFormatter = log => LoggingHelper.Reformat(log);

    public void Log(TLogMessage message, LogLevel level = LogLevel.Info, object? sender = null, DateTime? time = null, string? stackTrace = null)
    {
        if (message == null)
        {
            return;
        }

        if (!this.IsEnabled)
        {
            return;
        }

        if (level < this.LogLevel)
        {
            return;
        }
        this._writer.WriteLine(this._formatter(new LogRecord(message, level, sender, time, stackTrace)));
    }
}
