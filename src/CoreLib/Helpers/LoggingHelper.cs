using System.Runtime.CompilerServices;

using Library.EventsArgs;
using Library.Globalization.Helpers;
using Library.Logging;
using Library.Threading.MultistepProgress;
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
    public static void Debug(this ILoggerContainer container, [DisallowNull] object message, [CallerMemberName] object? sender = null, DateTime? time = null)
            => container?.Logger?.Debug(message, sender, time);

    /// <summary>
    /// Executes an action and logs the start and end strings.
    /// </summary>
    /// <typeparam name="TLogger">The type of the logger.</typeparam>
    /// <param name="logger">The logger instance.</param>
    /// <param name="action">The action to be executed.</param>
    /// <param name="start">The start string to be logged.</param>
    /// <param name="end">The end string to be logged.</param>
    /// <param name="exceptionFormat">The exception format string to be logged.</param>
    /// <returns>The logger instance.</returns>
    public static async Task<TLogger> DebugBlockAsync<TLogger>(
                [DisallowNull] this TLogger logger, // this keyword allows the method to be called on an instance of the class
                [DisallowNull] Func<Task> action, // Func is a generic delegate that can be used to represent a method with a single parameter and a return type
                string start, // start is a string parameter
                string end, // end is a string parameter
                string? exceptionFormat = null) where TLogger : ILogger // TLogger is a generic type parameter that must implement the ILogger interface
    {
        Check.IfArgumentNotNull(action); // checks if the action parameter is not null
        try
        {
            await action(); // awaits the action to be completed
            if (!start.IsNullOrEmpty()) // checks if the start string is not null or empty
            {
                logger.Debug(start); // logs the start string
            }
        }
        catch (Exception ex) // catches any exceptions
        {
            if (!exceptionFormat.IsNullOrEmpty()) // checks if the exceptionFormat string is not null or empty
            {
                logger.Error(string.Format(exceptionFormat, ex.GetBaseException().Message)); // logs the exceptionFormat string
            }
            else
            {
                throw; // throws the exception
            }
        }
        finally
        {
            if (!end.IsNullOrEmpty()) // checks if the end string is not null or empty
            {
                logger.Debug(end); // logs the end string
            }
        }
        return logger; // returns the logger instance
    }

    /// <summary>
    /// Executes an action and logs the start and end messages.
    /// </summary>
    /// <typeparam name="TLogger">The type of the logger.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="logger">The logger.</param>
    /// <param name="action">The action.</param>
    /// <param name="start">The start message.</param>
    /// <param name="end">The end message.</param>
    /// <param name="exceptionFormat">The exception format.</param>
    /// <returns>A tuple containing the result and the logger.</returns>
    public static async Task<(TResult? Result, TLogger Logger)> DebugBlockAsync<TLogger, TResult>(
                [DisallowNull] this TLogger logger, // this keyword allows the method to be called as an extension method on the TLogger type
                [DisallowNull] Func<Task<TResult>> action, // Func is a delegate type that represents a function that takes no parameters and returns a value of type TResult
                string start, // start is a string that will be logged if it is not empty
                string end, // end is a string that will be logged if it is not empty
                string? exceptionFormat = null) where TLogger : ILogger // exceptionFormat is an optional string that will be used to format the exception message if it is not empty
    {
        Check.IfArgumentNotNull(action); // check if the action argument is not null
        try
        {
            if (!start.IsNullOrEmpty()) // check if the start string is not empty
            {
                logger.Debug(start); // log the start string
            }
            var result = await action(); // await the result of the action
            return (result, logger); // return the result and the logger
        }
        catch (Exception ex) // catch any exceptions
        {
            if (!exceptionFormat.IsNullOrEmpty()) // check if the exceptionFormat string is not empty
            {
                logger.Error(string.Format(exceptionFormat, ex.GetBaseException().Message)); // log the exception message using the exceptionFormat string
                return (default, logger); // return the default value and the logger
            }
            else
            {
                throw; // rethrow the exception
            }
        }
        finally
        {
            if (!end.IsNullOrEmpty()) // check if the end string is not empty
            {
                logger.Debug(end); // log the end string
            }
        }
    }

    /// <summary>
    /// Logs an error message to the logger container.
    /// </summary>
    /// <param name="container">The logger container.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="sender">The sender of the message.</param>
    /// <param name="time">The time of the message.</param>
    public static void Error(this ILoggerContainer container, [DisallowNull] object message, [CallerMemberName] object? sender = null, DateTime? time = null)
                => container?.Logger?.Debug(message, sender, time);

    /// <summary>
    /// Handles the reporter events for the specified logger.
    /// </summary>
    /// <typeparam name="TLogger">The type of the logger.</typeparam>
    /// <param name="logger">The logger.</param>
    /// <param name="reporter">The reporter.</param>
    /// <returns>The logger.</returns>
    public static TLogger HandleReporterEvents<TLogger>(this TLogger logger, IMultistepProcess reporter)
            where TLogger : ILogger
    {
        reporter.Reported -= Reporter_Reported;
        reporter.Ended -= Reporter_Ended;

        reporter.Reported += Reporter_Reported;
        reporter.Ended += Reporter_Ended;
        return logger;

        void Reporter_Ended(object? sender, ItemActedEventArgs<ProgressData?> e)
        {
            if (e.Item != null)
            {
                logger.Log(e.Item);
            }
        }

        void Reporter_Reported(object? sender, ItemActedEventArgs<ProgressData> e)
            => logger.Log(e.Item switch
            {
                (_, null, null) => e.Item.Description!,
                _ => $"{e.Item.Current:00}-{e.Item.Max:00} - {e.Item.Description}"
            }, sender: e.Item.Sender);
    }

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
        Check.IfArgumentNotNull(action);
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
    public static string Reformat<TMessage>(this LogRecord<TMessage> logRecord, string? format = LogFormat.FORMAT_DEFAULT)
    {
        if (logRecord is null)
        {
            return string.Empty;
        }
        var message = logRecord.Message switch
        {
            IFormattableMessage formattable => formattable.Format(),
            Exception ex => logRecord.Level is not LogLevel.Trace ? ex.GetBaseException().Message : ex.GetFullMessage(),
            { } msg => msg.ToString() ?? string.Empty,
            _ => string.Empty,
        };
        return message.IsNullOrEmpty() ? string.Empty : (format?.ReplaceAll(
            (LogFormat.LONG_DATE, (logRecord.Time ?? DateTime.Now).ToLocalString().Add(1) ?? string.Empty),
            (LogFormat.SHORT_TIME, (logRecord.Time ?? DateTime.Now).ToShortTimeString() ?? string.Empty),
            (LogFormat.LEVEL, logRecord.Level.ToString().Add(1) ?? string.Empty),
            (LogFormat.MESSAGE, message ?? string.Empty),
            (LogFormat.NEW_LINE, Environment.NewLine),
            (LogFormat.SENDER, (logRecord.Sender?.ToString() ?? string.Empty).Add(1, before: true) ?? string.Empty),
            (LogFormat.STACK_TRACE, (logRecord.StackTrace?.ToString() ?? string.Empty).Add(1) ?? string.Empty))
            .Replace("  ", " ")) ?? string.Empty;
    }

    /// <summary>
    /// Reformats the given log object using the specified format. 
    /// </summary>
    /// <typeparam name="TMessage">The type of the log object.</typeparam>
    /// <param name="logObject">The log object to reformat.</param>
    /// <param name="format">The format to use for reformatting. If not specified, the default format will be used.</param>
    /// <returns>The reformatted log object.</returns>
    public static string Reformat<TMessage>(object logObject, string? format = LogFormat.FORMAT_DEFAULT)
            => InnerPatternMatching(logObject, format);

    /// <summary>
    /// Reformats the given log object using the specified format. 
    /// If no format is specified, the default format is used.
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