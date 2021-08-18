using Library.Globalization.Helpers;
using Library.Logging;

namespace Library.Helpers
{
    public static class LoggingHelper
    {
        public static bool MeetsLevel(this LogLevel level, LogLevel minLevel)
            => (minLevel & level) != 0;

        public static LogLevel ToLibLogLevel(this Microsoft.Extensions.Logging.LogLevel logLevel)
            => logLevel switch
            {
                Microsoft.Extensions.Logging.LogLevel.Trace => LogLevel.Trace,
                Microsoft.Extensions.Logging.LogLevel.Debug => LogLevel.Debug,
                Microsoft.Extensions.Logging.LogLevel.Information => LogLevel.Info,
                Microsoft.Extensions.Logging.LogLevel.Warning => LogLevel.Warning,
                Microsoft.Extensions.Logging.LogLevel.Error => LogLevel.Error,
                Microsoft.Extensions.Logging.LogLevel.Critical => LogLevel.Fatal,
                Microsoft.Extensions.Logging.LogLevel.None => LogLevel.None,
                _ => throw new NotImplementedException()
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
}
