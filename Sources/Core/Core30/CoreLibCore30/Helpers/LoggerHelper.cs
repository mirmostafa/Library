using System;
using Mohammad.Logging;

namespace Mohammad.Helpers
{
    public static class LoggerHelper
    {
        public static void Info(this ILogger logger, object message, DateTime? time = null)
        {
            logger.Write(message, LogLevel.Info, time);
        }

        public static void Error(this ILogger logger, object message, DateTime? time = null)
        {
            logger.Write(message, LogLevel.Error, time);
        }

        public static void Warn(this ILogger logger, object message, DateTime? time = null)
        {
            logger.Write(message, LogLevel.Warning, time);
        }

        public static void Debug(this ILogger logger, object message, DateTime? time = null)
        {
            logger.Write(message, LogLevel.Debug, time);
        }
    }
}