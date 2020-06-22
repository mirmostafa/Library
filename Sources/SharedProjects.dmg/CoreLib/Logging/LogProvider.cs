using System;
using Mohammad.Logging.Gateways;

namespace Mohammad.Logging
{
    public class LogProvider : ILogProvider
    {
        private ILogger _Logger;

        public LogProvider() { }

        public LogProvider(ILogger logger, object defaultSender = null)
            : this()
        {
            this._Logger = logger;
            this.DefaultSender = defaultSender;
        }

        public LogProvider(string logFilePath, object defaultSender = null)
            : this(new Logger {Out = new FileLoggerGateway(logFilePath) {IsLogRotationEnabled = true}}, defaultSender) { }

        protected virtual ILogger OnInitializingLogger() { throw new NotImplementedException("Please implement OnInitializingLogger or use suitable constructor."); }

        public ILogger Logger => this._Logger ?? (this._Logger = this.OnInitializingLogger());

        public object DefaultSender { get; }
    }
}