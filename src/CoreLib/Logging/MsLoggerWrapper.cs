using Microsoft.Extensions.Logging;

namespace Library.Logging;

public sealed class MsLoggerWrapper : FastLoggerBase<string>, ILogger
{
    private readonly Microsoft.Extensions.Logging.ILogger _logger;

    public MsLoggerWrapper(Microsoft.Extensions.Logging.ILogger logger) => this._logger = logger;

    public void Log([DisallowNull] object message, LogLevel level = LogLevel.Info, object? sender = null, DateTime? time = null, string? stackTrace = null)
        => base.Log(message?.ToString()!, level, sender, time, stackTrace);

    protected override void OnLogging(LogRecord<string> logRecord)
    {
        if (logRecord is null or { Message: null })
        {
            return;
        }
        this._logger.Log(logRecord.Level.ToMsLogLevel(), logRecord.Message);
    }
}
