namespace Library.Logging;

/// <summary>
/// Represents an empty logger that does not log anything.
/// </summary>
/// <typeparam name="TMessage">The type of the message.</typeparam>
public class EmptyLogger<TMessage> : ILogger<TMessage>
{
    public bool IsEnabled { get; set; } = false;

    public LogLevel LogLevel { get; set; } = LogLevel.None;

    public void Log(TMessage message, LogLevel level = LogLevel.Info, object? sender = null, DateTime? time = null, string? stackTrace = null)
    { }
}