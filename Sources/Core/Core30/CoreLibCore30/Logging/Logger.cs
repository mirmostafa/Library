using System;

namespace Mohammad.Logging
{
    public sealed class Logger : IEventLogger, ISupportSilence
    {
        #region Implementation of IEventLogger

        public event EventHandler<ILog> Logged;

        #endregion

        #region Implementation of ISupportSilence

        public bool IsSilent { get; set; }

        #endregion

        private void OnLogged(ILog e)
        {
            this.Logged?.Invoke(this, e);
        }

        #region Implementation of ILogger

        public void Write(object message, LogLevel level = LogLevel.Info, DateTime? time = null)
        {
            this.Write(new Log(message, level, time));
        }

        public void Write(ILog log)
        {
            if (!this.IsSilent) this.OnLogged(log);
        }

        #endregion
    }
}