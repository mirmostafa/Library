using Microsoft.Extensions.Logging;

namespace Library.Logging
{
    public sealed class MsLoggerWrapper : FastLoggerBase<string>
    {
        private readonly Microsoft.Extensions.Logging.ILogger _logger;

        public MsLoggerWrapper(Microsoft.Extensions.Logging.ILogger logger) => this._logger = logger;

        protected override void OnLogging(LogRecord<string> logRecord)
        {
            if (logRecord is not null)
            {
                this._logger.Log(logRecord.Level.ToMsLogLevel(), logRecord.Message);
            }
        }
    }
}
