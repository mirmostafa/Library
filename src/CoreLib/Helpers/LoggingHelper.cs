using System.Runtime.CompilerServices;

using Library.EventsArgs;
using Library.Globalization.Helpers;
using Library.Logging;
using Library.Threading.MultistepProgress;
using Library.Validations;

namespace Library.Helpers;

public static class LoggingHelper
{
    public static void Debug(this ILoggerContainer container, [DisallowNull] object message, [CallerMemberName] object? sender = null, DateTime? time = null)
        => container?.Logger?.Debug(message, sender, time);

    public static async Task<TLogger> DebugBlockAsync<TLogger>(
        [DisallowNull] this TLogger logger,
        [DisallowNull] Func<Task> action,
        string start,
        string end,
        string? exceptionFormat = null) where TLogger : ILogger
    {
        Check.IfArgumentNotNull(action);
        try
        {
            await action();
            if (!start.IsNullOrEmpty())
            {
                logger.Debug(start);
            }
        }
        catch (Exception ex)
        {
            if (!exceptionFormat.IsNullOrEmpty())
            {
                logger.Error(string.Format(exceptionFormat, ex.GetBaseException().Message));
            }
            else
                throw;
        }
        finally
        {
            if (!end.IsNullOrEmpty())
            {
                logger.Debug(end);
            }
        }
        return logger;
    }

    public static async Task<(TResult? Result, TLogger Logger)> DebugBlockAsync<TLogger, TResult>(
        [DisallowNull] this TLogger logger,
        [DisallowNull] Func<Task<TResult>> action,
        string start,
        string end,
        string? exceptionFormat = null) where TLogger : ILogger
    {
        Check.IfArgumentNotNull(action);
        try
        {
            if (!start.IsNullOrEmpty())
            {
                logger.Debug(start);
            }
            var result = await action();
            return (result, logger);
        }
        catch (Exception ex)
        {
            if (!exceptionFormat.IsNullOrEmpty())
            {
                logger.Error(string.Format(exceptionFormat, ex.GetBaseException().Message));
                return (default, logger);
            }
            else
                throw;
        }
        finally
        {
            if (!end.IsNullOrEmpty())
            {
                logger.Debug(end);
            }
        }
    }

    public static void Error(this ILoggerContainer container, [DisallowNull] object message, [CallerMemberName] object? sender = null, DateTime? time = null)
            => container?.Logger?.Debug(message, sender, time);

    public static TLogger HandleReporterEvents<TLogger>(this TLogger logger, IMultistepProcess reporter)
        where TLogger : ILogger
    {
        reporter.Reported -= Reporter_Reported;
        reporter.Ended -= Reporter_Ended;

        reporter.Reported += Reporter_Reported;
        reporter.Ended += Reporter_Ended;
        return logger;

        void Reporter_Ended(object? sender, ItemActedEventArgs<string?> e)
        {
            if (e.Item != null)
            {
                logger.Log(e.Item);
            }
        }

        void Reporter_Reported(object? sender, ItemActedEventArgs<(int Max, int Current, string? Description)> e)
            => logger.Log($"{e.Item.Current:00}-{e.Item.Max:00}] [{e.Item.Description}");
    }

    public static void Info(this ILoggerContainer container, [DisallowNull] object message, [CallerMemberName] object? sender = null, DateTime? time = null)
            => container?.Logger?.Info(message, sender, time);

    public static async Task<TLogger> LogBlockAsync<TLogger>([DisallowNull] this TLogger logger, [DisallowNull] Func<Task> action, Action<TLogger> onStart, Action<TLogger> onSucceed, Action<Exception, TLogger> onError, Action<TLogger> onFinal)
        where TLogger : ILogger
    {
        Check.IfArgumentNotNull(action);
        try
        {
            await action();
            onSucceed(logger);
        }
        catch (Exception ex)
        {
            onError(ex, logger);
        }
        return logger;
    }

    public static bool MeetsLevel(this LogLevel level, LogLevel minLevel)
        => (minLevel & level) == level;

    public static string Reformat<TMessage>(this LogRecord<TMessage> logRecord, string? format = LogFormat.DEFAULT_FORMAT)
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
            (LogFormat.DATE, (logRecord.Time ?? DateTime.Now).ToLocalString().Add(1) ?? string.Empty),
            (LogFormat.LEVEL, logRecord.Level.ToString().Add(1) ?? string.Empty),
            (LogFormat.MESSAGE, message.Add(1) ?? string.Empty),
            (LogFormat.NEW_LINE, Environment.NewLine),
            (LogFormat.SENDER, (logRecord.Sender?.ToString() ?? string.Empty).Add(1) ?? string.Empty),
            (LogFormat.STACK_TRACE, (logRecord.StackTrace?.ToString() ?? string.Empty).Add(1) ?? string.Empty))) ?? string.Empty;
    }

    //public static string Reformat(this LogRecord logRecord, string? format = LogFormat.DEFAULT_FORMAT)
    //{
    //    if (logRecord is null)
    //    {
    //        return string.Empty;
    //    }
    //    var message = logRecord.Message switch
    //    {
    //        IFormattableMessage formattable => formattable.Format(),
    //        Exception ex => logRecord.Level is not LogLevel.Trace ? ex.GetBaseException().Message : ex.GetFullMessage(),
    //        { } msg => msg.ToString() ?? string.Empty,
    //        _ => string.Empty,
    //    };
    //    return message.IsNullOrEmpty() ? string.Empty : (format?.ReplaceAll(
    //        (LogFormat.DATE, (logRecord.Time ?? DateTime.Now).ToLocalString().Add(1) ?? string.Empty),
    //        (LogFormat.LEVEL, logRecord.Level.ToString().Add(1) ?? string.Empty),
    //        (LogFormat.MESSAGE, message.Add(1) ?? string.Empty),
    //        (LogFormat.NEW_LINE, Environment.NewLine),
    //        (LogFormat.SENDER, (logRecord.Sender?.ToString() ?? string.Empty).Add(1) ?? string.Empty),
    //        (LogFormat.STACK_TRACE, (logRecord.StackTrace?.ToString() ?? string.Empty).Add(1) ?? string.Empty))) ?? string.Empty;
    //}

    public static string Reformat<TMessage>(object logObject, string? format = LogFormat.DEFAULT_FORMAT)
        => InnerPatternMatching(logObject, format);

    public static string Reformat(object logObject, string? format = LogFormat.DEFAULT_FORMAT)
        => InnerPatternMatching(logObject, format);

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