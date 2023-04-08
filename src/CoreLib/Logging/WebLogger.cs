using Library.Types;
using Library.Validations;

using Microsoft.Extensions.Logging;

namespace Library.Logging;

public class WebLogger : IMsLogger, ILogger<string>
{
    private readonly ILogger _mainLogger;

    public WebLogger(ILogger mainLogger) =>
        this._mainLogger = mainLogger.ArgumentNotNull();

    public bool IsEnabled { get => this._mainLogger.IsEnabled; set => this._mainLogger.IsEnabled = value; }
    public LogLevel LogLevel { get; set; }

    public IDisposable BeginScope<TState>(TState state)
        => EmptyDisposable.NewEmpty();

    public void Log(string message, LogLevel level = LogLevel.Info, object? sender = null, DateTime? time = null, string? stackTrace = null)
        => this._mainLogger.Log(message, level, sender, time, stackTrace);

    public void Log<TState>(MsLogLevel logLevel,
                            EventId eventId,
                            TState state,
                            Exception? exception,
                            Func<TState, Exception?, string> formatter)
        => this._mainLogger.Log(formatter.ArgumentNotNull()(state, exception));

    bool IMsLogger.IsEnabled(MsLogLevel logLevel)
        => this._mainLogger.IsEnabled;
}

public sealed class WebLogger<TCategoryName> : WebLogger, Microsoft.Extensions.Logging.ILogger<TCategoryName>
{
    public WebLogger(ILogger mainLogger)
        : base(mainLogger)
    {
    }
}