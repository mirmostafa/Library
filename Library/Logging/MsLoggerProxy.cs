using Microsoft.Extensions.Logging;

namespace Library.Logging;
public class MsLoggerProxy : ILogger
{
    private readonly Microsoft.Extensions.Logging.ILogger _Logger;

    public MsLoggerProxy(Microsoft.Extensions.Logging.ILogger logger) => this._Logger = logger;
    public bool IsEnabled { get; set; }
    public LogLevel LogLevel { get; set; }

    public void Log(object message, LogLevel level = LogLevel.Info, object? sender = null, DateTime? time = null, string? stackTrace = null)
    {
        if (!this.IsEnabled)
        {
            return;
        }
        switch (level)
        {
            case LogLevel.None:
                break;
            case LogLevel.Info:
                this._Logger.LogInformation(message.ToString());
                break;
            case LogLevel.Warning:
                this._Logger.LogWarning(message.ToString());
                break;
            case LogLevel.Error:
                this._Logger.LogError(message.ToString());
                break;
            case LogLevel.Fatal:
                this._Logger.LogCritical(message.ToString());
                break;
            case LogLevel.Debug:
                this._Logger.LogDebug(message.ToString());
                break;
            case LogLevel.Trace:
                this._Logger.LogTrace(message.ToString());
                break;
            default:
                break;
        }
    }
}