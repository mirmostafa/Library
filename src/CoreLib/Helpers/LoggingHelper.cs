using System.Runtime.CompilerServices;

using Library.Globalization.Helpers;
using Library.Logging;
using Library.Validations;

namespace Library.Helpers;

public static class LoggingHelper
{
    /// <summary>
    /// Logs a debug message to the logger associated with the specified container.
    /// </summary>
    /// <param name="container">The container.</param>
    /// <param name="message">The message.</param>
    /// <param name="sender">The sender.</param>
    /// <param name="time">The time.</param>
    public static void Debug(this ILoggerContainer container, [DisallowNull] object message, [CallerMemberName] object? sender = null, DateTime? time = null) =>
        container?.Logger?.Debug(message, sender, time);

    /// <summary>
    /// Logs an error message to the logger container.
    /// </summary>
    /// <param name="container">The logger container.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="sender">The sender of the message.</param>
    /// <param name="time">The time of the message.</param>
    public static void Error(this ILoggerContainer container, [DisallowNull] object message, [CallerMemberName] object? sender = null, DateTime? time = null) =>
        container?.Logger?.Debug(message, sender, time);

    /// <summary>
    /// Logs an informational message to the logger associated with the specified container.
    /// </summary>
    /// <param name="container">The container associated with the logger.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="sender">The name of the caller (optional).</param>
    /// <param name="time">The time of the log (optional).</param>
    public static void Info(this ILoggerContainer container, [DisallowNull] object message, [CallerMemberName] object? sender = null, DateTime? time = null)
        => container?.Logger?.Info(message, sender, time);

    /// <summary>
    /// Executes an action and logs the start, success, error and final events.
    /// </summary>
    /// <typeparam name="TLogger">The type of the logger.</typeparam>
    /// <param name="logger">The logger.</param>
    /// <param name="action">The action to execute.</param>
    /// <param name="onStart">The action to execute when the block starts.</param>
    /// <param name="onSucceed">The action to execute when the block succeeds.</param>
    /// <param name="onError">The action to execute when the block fails.</param>
    /// <param name="onFinal">The action to execute when the block ends.</param>
    /// <returns>The logger.</returns>
    public static async Task<TLogger> LogBlockAsync<TLogger>([DisallowNull] this TLogger logger,
            [DisallowNull] Func<Task> action,
            Action<TLogger>? onStart,
            Action<TLogger>? onSucceed,
            Action<Exception, TLogger>? onError,
            Action<TLogger>? onFinal = null)
            where TLogger : ILogger
    {
        Check.MustBeArgumentNotNull(action);
        try
        {
            onStart?.Invoke(logger);
            await action();
            onSucceed?.Invoke(logger);
        }
        catch (Exception ex)
        {
            onError?.Invoke(ex, logger);
        }
        return logger;
    }

    /// <summary>
    /// Logs a block of code asynchronously with optional start and succeed messages.
    /// </summary>
    /// <typeparam name="TLogger">The type of the logger.</typeparam>
    /// <param name="logger">The logger.</param>
    /// <param name="action">The action to log.</param>
    /// <param name="startMessage">The start message.</param>
    /// <param name="succeedMessage">The succeed message.</param>
    /// <returns>The logger.</returns>
    public static Task<TLogger> LogBlockAsync<TLogger>(
            [DisallowNull] this TLogger logger,
            [DisallowNull] Func<Task> action,
            string? startMessage = "Running...",
            string? succeedMessage = "Ready") where TLogger : ILogger
            => LogBlockAsync(
                logger,
                action,
                l => If(!startMessage.IsNullOrEmpty(), () => l.Debug(startMessage!)),
                l => If(!succeedMessage.IsNullOrEmpty(), () => l.Debug(succeedMessage!)),
                null,
                null);

    /// <summary>
    /// Checks if the given LogLevel meets the minimum LogLevel.
    /// </summary>
    public static bool MeetsLevel(this LogLevel level, LogLevel minLevel)
        => (minLevel & level) == level;

    /// <summary>
    /// Reformats a LogRecord object according to the given format string.
    /// </summary>
    /// <param name="logRecord">The LogRecord object to be reformatted.</param>
    /// <param name="format">The format string to be used for reformatting.</param>
    /// <returns>A string containing the reformatted LogRecord.</returns>
    [return: NotNull]
    public static string Reformat<TMessage>(this LogRecord<TMessage> logRecord, in string? format = null)
    {
        if (logRecord is null)
        {
            return string.Empty;
        }
        var f = format ?? logRecord.Format;
        var message = logRecord.Message switch
        {
            IFormattableMessage formattable => formattable.Format(),
            Exception ex => logRecord.Level is not LogLevel.Trace ? ex.GetBaseException().Message : ex.GetFullMessage(),
            { } msg => msg.ToString() ?? string.Empty,
            _ => string.Empty,
        };
        var result = message.IsNullOrEmpty()
            ? string.Empty
            : f.IsNullOrEmpty()
                ? message
                : f.ReplaceAll(
                    (LogFormat.LONG_DATE, (logRecord.Time ?? DateTime.Now).ToLocalString()),
                    (LogFormat.SHORT_TIME, (logRecord.Time ?? DateTime.Now).ToShortTimeString()),
                    (LogFormat.LEVEL, logRecord.Level.ToString()),
                    (LogFormat.MESSAGE, message ?? string.Empty),
                    (LogFormat.NEW_LINE, Environment.NewLine),
                    (LogFormat.SENDER, logRecord.Sender?.ToString() ?? string.Empty),
                    (LogFormat.STACK_TRACE, logRecord.StackTrace?.ToString() ?? string.Empty))
                    .Replace("  ", " ");
        return result;
    }

    /// <summary>
    /// Reformats the given log object using the specified format. If no format is specified, the
    /// default format is used.
    /// </summary>
    public static string Reformat(object logObject, string? format = LogFormat.FORMAT_DEFAULT)
        => InnerPatternMatching(logObject, format);

    /// <summary>
    /// Converts a MsLogLevel to a Library LogLevel.
    /// </summary>
    /// <param name="logLevel">The MsLogLevel to convert.</param>
    /// <returns>The converted LibLogLevel.</returns>
    public static LogLevel ToLibLogLevel(this MsLogLevel logLevel)
        => logLevel switch
        {
            MsLogLevel.Trace => LogLevel.Trace,
            MsLogLevel.Debug => LogLevel.Debug,
            MsLogLevel.Information => LogLevel.Info,
            MsLogLevel.Warning => LogLevel.Warning,
            MsLogLevel.Error => LogLevel.Error,
            MsLogLevel.Critical => LogLevel.Fatal,
            MsLogLevel.None => LogLevel.None,
            _ => throw new NotImplementedException()
        };

    /// <summary>
    /// Converts a LogLevel to a MsLogLevel.
    /// </summary>
    /// <param name="logLevel">The LogLevel to convert.</param>
    /// <returns>The MsLogLevel corresponding to the LogLevel.</returns>
    public static MsLogLevel ToMsLogLevel(this LogLevel logLevel)
        => logLevel switch
        {
            LogLevel.None => MsLogLevel.None,
            LogLevel.Info => MsLogLevel.Information,
            LogLevel.Warning => MsLogLevel.Warning,
            LogLevel.Error => MsLogLevel.Error,
            LogLevel.Fatal => MsLogLevel.Critical,
            LogLevel.Debug => MsLogLevel.Debug,
            LogLevel.Trace => MsLogLevel.Trace,
            _ => throw new NotImplementedException(),
        };

    private static string InnerPatternMatching(object logObject, string? format)
        => logObject switch
        {
            LogRecord logRecord => Reformat(logRecord, format),
            LogRecord<object> logRecord => Reformat(logRecord, format),
            null => string.Empty,
            _ => logObject.ToString() ?? string.Empty
        };
}