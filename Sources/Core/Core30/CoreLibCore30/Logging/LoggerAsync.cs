using System;
using System.Threading.Tasks;

namespace Mohammad.Logging
{
    public class LoggerAsync : ILoggerAsync, IEventLoggerAsync, ISupportSilence
    {
        #region Implementation of ISupportSilence

        public bool IsSilent { get; set; }

        #endregion

        #region Implementation of IEventLoggerAsync

        public event EventHandler<ILog> Logged;

        #endregion

        protected virtual void OnLogged(ILog e)
        {
            this.Logged?.Invoke(this, e);
        }

        #region Implementation of ILoggerAsync

        public async Task WriteAsync(object message, LogLevel level = LogLevel.Info, DateTime? time = null) =>
            await this.WriteAsync(new Log(message, level, time));

        public async Task WriteAsync(ILog log)
        {
            await Task.Yield();
            if (!this.IsSilent)
            {
                this.OnLogged(log);
            }
        }

        #endregion
    }
}