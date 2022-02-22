using Library.Globalization.Helpers;
using Library.Logging;

namespace Library.Helpers;
public static class LoggingHelper
{
    public static bool MeetsLevel(this LogLevel level, LogLevel minLevel)
        => (minLevel & level) != 0;

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

    public static string Reformat(this LogRecord logRecord, string? format = LogFormat.DEFAULT_FORMAT)
        => InnerPatternMatching(logRecord, format);

    public static string Reformat<TMessage>(object logObject, string? format = LogFormat.DEFAULT_FORMAT)
        => InnerPatternMatching(logObject, format);

    public static string Reformat(object logObject, string? format = LogFormat.DEFAULT_FORMAT)
        => InnerPatternMatching(logObject, format);

    private static string InnerPatternMatching(object logObject, string? format)
        => logObject switch
        {
            LogRecord logRecord => Reformat(logRecord, format),
            LogRecord<object> logRecord => Reformat(logRecord, format),
            null => string.Empty,
            _ => logObject.ToString() ?? string.Empty
        };
}