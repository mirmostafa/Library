using Library.EventsArgs;
using Library.Logging;

namespace Library;
public static class LibLogger
{
    private static Loggers? _loggers;

    public static event EventHandler<ItemActedEventArgs<LogRecord<object>>>? Logged;

    public static bool IsEnabled { get => Loggers.IsEnabled; set => Loggers.IsEnabled = value; }
    public static LogLevel LogLevel { get => Loggers.LogLevel; set => Loggers.LogLevel = value; }
    private static Loggers Loggers => _loggers ??= new(new VsOutputLogger());

    public static void AddLogger(ILogger value)
        => Loggers.Add(value);
    public static void AddLogger(Microsoft.Extensions.Logging.ILogger logger)
        => AddLogger(new MsLoggerWrapper(logger));

    internal static void Log(object message, LogLevel level = LogLevel.Info, object? sender = null, DateTime? time = null, string? stackTrace = null)
    {
        _ = Catch(() => OnLogging(new(message, level, sender, time, stackTrace)));
        Loggers.Log(message, level, sender, time, stackTrace);
    }

    /// <summary>
    /// Logs the specified message with LogLevel.Info.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="sender">The sender.</param>
    /// <param name="time">The time.</param>
    internal static void Info(object message, object? sender = null, DateTime? time = null)
        => Log(message, LogLevel.Info, sender: sender ?? CodeHelper.GetCallerMethod(), time ?? DateTime.Now);

    /// <summary>
    /// logs the specified message with LogLevel.Warning.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="sender">The sender.</param>
    /// <param name="time">The time.</param>
    internal static void Warn(object message, object? sender = null, DateTime? time = null)
        => Log(message, LogLevel.Warning, sender: sender ?? CodeHelper.GetCallerMethod(), time ?? DateTime.Now);

    /// <summary>
    /// Logs the specified message with LogLevel.Error.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="sender">The sender.</param>
    /// <param name="time">The time.</param>
    internal static void Error(object message, object? sender = null, DateTime? time = null)
        => Log(message, LogLevel.Error, sender: sender ?? CodeHelper.GetCallerMethod(), time ?? DateTime.Now);

    /// <summary>
    /// Logs the specified message with LogLevel.Fatal.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="sender">The sender.</param>
    /// <param name="time">The time.</param>
    internal static void Fatal(object message, object? sender = null, DateTime? time = null)
        => Log(message, LogLevel.Fatal, sender: sender ?? CodeHelper.GetCallerMethod(), time ?? DateTime.Now);

    /// <summary>
    /// Logs the specified message with LogLevel.Debug.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="sender">The sender.</param>
    /// <param name="time">The time.</param>
    internal static void Debug(object message, object? sender = null, DateTime? time = null)
        => Log(message, LogLevel.Debug, sender: sender ?? CodeHelper.GetCallerMethod(), time ?? DateTime.Now);

    /// <summary>
    /// Logs the specified message with LogLevel.Trace.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="sender">The sender.</param>
    /// <param name="time">The time.</param>
    /// <param name="stackTrace">The stack trace.</param>
    internal static void Trace(object message, object? sender = null, DateTime? time = null, string? stackTrace = null)
        => Log(message, LogLevel.Trace, sender: sender ?? CodeHelper.GetCallerMethod(), time ?? DateTime.Now, stackTrace);

    private static void OnLogging(LogRecord<object> logRecord)
        => Logged?.Invoke(null, new(logRecord));

    internal static void DebugStartingAction()
        => Debug(CodeHelper.GetCallerMethodName() ?? $"({nameof(DebugStartingAction)} calling)", sender: CodeHelper.GetCallerMethod()?.DeclaringType?.Name);

    internal static void DebugEndedAction()
        => Debug(CodeHelper.GetCallerMethodName() ?? $"({nameof(DebugEndedAction)} called)", sender: CodeHelper.GetCallerMethod()?.DeclaringType?.Name);
    public static void AddConsole() 
        => LibLogger.AddLogger(new TextWriterLogger(Console.Out));
}