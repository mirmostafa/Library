namespace Library.Logging;

public record LogRecord<TLogMessage>(TLogMessage Message, LogLevel Level = LogLevel.Info, object? Sender = null, DateTime? Time = null, string? StackTrace = null, string? Format = LogFormat.FORMAT_DEFAULT);

public record LogRecord : LogRecord<object>
{
    public LogRecord(object Message, LogLevel Level = LogLevel.Info, object? Sender = null, DateTime? Time = null, string? StackTrace = null, string? format = LogFormat.FORMAT_DEFAULT)
        : base(Message, Level, Sender, Time, StackTrace)
    {
    }

    protected LogRecord(LogRecord<object> original) 
        : base(original)
    {
    }
}