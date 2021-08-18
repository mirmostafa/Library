using Library.EventsArgs;
using Library.Logging;

namespace Library;
public static class LibLogger
{

    public static event EventHandler<ItemActedEventArgs<LogRecord<object>>> Logged;

    public static bool IsEnabled { get => Logger.IsEnabled; set => Logger.IsEnabled = value; }
    public static LogLevel LogLevel { get => Logger.LogLevel; set => Logger.LogLevel = value; }
    private static Loggers Logger => new(new VsOutputLogger());

    public static void AddLogger(ILogger value) => Logger.Add(value);

    internal static void Log(object message, LogLevel level = LogLevel.Info, object? sender = null, DateTime? time = null, string? stackTrace = null)
    {
        _ = Catch(() => OnLogging(new(message, level, sender, time, stackTrace)));
        Logger.Log(message, level, sender, time, stackTrace);
    }

    /// <summary>
    /// Logs the specified message with LogLevel.Info.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="sender">The sender.</param>
    /// <param name="time">The time.</param>
    internal static void Info(object message, object? sender = null, DateTime? time = null) => Log(message, LogLevel.Info, sender: sender ?? GetCallerMethod(), time ?? DateTime.Now);

    /// <summary>
    /// logs the specified message with LogLevel.Warning.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="sender">The sender.</param>
    /// <param name="time">The time.</param>
    internal static void Warn(object message, object? sender = null, DateTime? time = null) => Log(message, LogLevel.Warning, sender: sender ?? GetCallerMethod(), time ?? DateTime.Now);

    /// <summary>
    /// Logs the specified message with LogLevel.Error.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="sender">The sender.</param>
    /// <param name="time">The time.</param>
    internal static void Error(object message, object? sender = null, DateTime? time = null) => Log(message, LogLevel.Error, sender: sender ?? GetCallerMethod(), time ?? DateTime.Now);

    /// <summary>
    /// Logs the specified message with LogLevel.Fatal.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="sender">The sender.</param>
    /// <param name="time">The time.</param>
    internal static void Fatal(object message, object? sender = null, DateTime? time = null) => Log(message, LogLevel.Fatal, sender: sender ?? GetCallerMethod(), time ?? DateTime.Now);

    /// <summary>
    /// Logs the specified message with LogLevel.Debug.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="sender">The sender.</param>
    /// <param name="time">The time.</param>
    internal static void Debug(object message, object? sender = null, DateTime? time = null) => Log(message, LogLevel.Debug, sender: sender ?? GetCallerMethod(), time ?? DateTime.Now);

    /// <summary>
    /// Logs the specified message with LogLevel.Trace.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="sender">The sender.</param>
    /// <param name="time">The time.</param>
    /// <param name="stackTrace">The stack trace.</param>
    internal static void Trace(object message, object? sender = null, DateTime? time = null, string? stackTrace = null) => Log(message, LogLevel.Trace, sender: sender ?? GetCallerMethod(), time ?? DateTime.Now, stackTrace);

    private static void OnLogging(LogRecord<object> logRecord) => Logged?.Invoke(null, new(logRecord));

    internal static void DebugStartingAction() => Debug(GetCallerMethodName() ?? $"({nameof(DebugStartingAction)} called)", sender: GetCallerMethod()?.DeclaringType?.Name);

    internal static void DebugEndedAction() => Debug(GetCallerMethodName() ?? $"({nameof(DebugEndedAction)} called)", sender: GetCallerMethod()?.DeclaringType?.Name);
}