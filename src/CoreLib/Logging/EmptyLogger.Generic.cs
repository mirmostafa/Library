namespace Library.Logging;

public class EmptyLogger<TMessage> : ILogger<TMessage>
{
    public bool IsEnabled { get; set; } = false;

    public LogLevel LogLevel { get; set; } = LogLevel.None;

    public void Log(TMessage message, LogLevel level = LogLevel.Info, object? sender = null, DateTime? time = null, string? stackTrace = null)
    { }
}
