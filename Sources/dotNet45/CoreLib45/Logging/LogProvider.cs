#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;
using Mohammad.Logging.Gateways;

namespace Mohammad.Logging
{
    public class LogProvider : ILogProvider
    {
        private ILogger _Logger;

        public LogProvider()
        {
        }

        public LogProvider(ILogger logger, object defaultSender = null)
            : this()
        {
            this._Logger       = logger;
            this.DefaultLogSender = defaultSender;
        }

        public LogProvider(string logFilePath, object defaultSender = null)
            : this(new Logger
                   {
                       Out = new FileLoggerGateway(logFilePath)
                       {
                           IsLogRotationEnabled = true
                       }
                   },
                   defaultSender)
        {
        }

        public ILogger Logger => this._Logger ?? (this._Logger = this.OnInitializingLogger());

        public object DefaultLogSender { get; }

        protected virtual ILogger OnInitializingLogger() => throw new NotImplementedException(
                                                                "Please implement OnInitializingLogger or use suitable constructor.");
    }
}