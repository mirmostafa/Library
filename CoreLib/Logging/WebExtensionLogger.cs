using Library.Validations;
using Microsoft.Extensions.Logging;

namespace Library.Logging;

public class WebExtensionLogger : Microsoft.Extensions.Logging.ILogger, ILogger<string>
{
    private readonly ILogger _mainLogger;

    public WebExtensionLogger(ILogger mainLogger) =>
        this._mainLogger = mainLogger.ArgumentNotNull(nameof(mainLogger));

    public LogLevel LogLevel { get; set; }

    public bool IsEnabled { get => this._mainLogger.IsEnabled; set => this._mainLogger.IsEnabled = value; }

    public IDisposable BeginScope<TState>(TState state) =>
        EmptyDisposable.Empty;

    bool Microsoft.Extensions.Logging.ILogger.IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel) =>
        this._mainLogger.IsEnabled;

    public void Log(string message, LogLevel level = LogLevel.Info, object? sender = null, DateTime? time = null, string? stackTrace = null) =>
        this._mainLogger.Log(message, level, sender, time, stackTrace);

    public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel,
                            EventId eventId,
                            TState state,
                            Exception? exception,
                            Func<TState, Exception?, string> formatter) =>
         this._mainLogger.Log(formatter.ArgumentNotNull()(state, exception));
}
