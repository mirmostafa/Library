using System;
using System.Threading.Tasks;
using Mohammad.Logging;

namespace Mohammad.Helpers
{
    public static class LoggerAsyncHelper
    {
        public static async Task Info(this ILoggerAsync logger, object message, DateTime? time = null)
        {
            await logger.WriteAsync(message, LogLevel.Info, time);
        }

        public static async Task Error(this ILoggerAsync logger, object message, DateTime? time = null)
        {
            await logger.WriteAsync(message, LogLevel.Error, time);
        }

        public static async Task Warn(this ILoggerAsync logger, object message, DateTime? time = null)
        {
            await logger.WriteAsync(message, LogLevel.Warning, time);
        }

        public static async Task Debug(this ILoggerAsync logger, object message, DateTime? time = null)
        {
            await logger.WriteAsync(message, LogLevel.Debug, time);
        }
    }
}