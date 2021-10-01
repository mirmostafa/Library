using Library.Validations;
using Microsoft.Extensions.Logging;

namespace Library.Logging
{
    public class WebExtensionLogger : Microsoft.Extensions.Logging.ILogger, ILogger<string>
    {
        private readonly ILogger _MainLogger;

        public WebExtensionLogger(ILogger mainLogger)
            => this._MainLogger = mainLogger.ArgumentNotNull(nameof(mainLogger));

        public LogLevel LogLevel { get; set; }

        public bool IsEnabled { get => this._MainLogger.IsEnabled; set => this._MainLogger.IsEnabled = value; }

        public IDisposable BeginScope<TState>(TState state)
            => new EmptyDisposable();

        bool Microsoft.Extensions.Logging.ILogger.IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
            => this._MainLogger.IsEnabled;

        public void Log<TState>(
            Microsoft.Extensions.Logging.LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
            => this._MainLogger.Log(formatter.ArgumentNotNull(nameof(formatter))(state, exception));

        public void Log(string message, LogLevel level = LogLevel.Info, object? sender = null, DateTime? time = null, string? stackTrace = null)
            => this._MainLogger.Log(message, level, sender, time, stackTrace);
    }
}
