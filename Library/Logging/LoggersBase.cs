using System.Collections;
using Library.DesignPatterns.ExceptionHandlingPattern;

namespace Library.Logging
{
    public abstract class LoggersBase<TMessage> : ILoggerCollection<TMessage, ILogger<TMessage>>, IExceptionHandlerContainer, ILogger<TMessage>
    {
        private List<ILogger<TMessage>> _Loggers;

        public IEnumerable<ILogger<TMessage>> Loggers => this._Loggers.AsEnumerable();
        public bool IsEnabled { get; set; }
        public LogLevel LogLevel { get; set; }
        public int Count { get; }
        public bool IsReadOnly { get; }
        public ExceptionHandling ExceptionHandling { get; private set; }

        protected LoggersBase(IEnumerable<ILogger<TMessage>> loggers, ExceptionHandling? exceptionHandling = null)
            => this.InitializeInstance(loggers, exceptionHandling);

        protected LoggersBase(params ILogger<TMessage>[] loggers)
            => this.InitializeInstance(loggers, null);

        protected LoggersBase(ExceptionHandling? exceptionHandling, params ILogger<TMessage>[] loggers)
            => this.InitializeInstance(loggers, exceptionHandling);

        public void Add(ILogger<TMessage> item)
            => this._Loggers.Add(item);
        public void Clear()
            => this._Loggers.Clear();
        public bool Contains(ILogger<TMessage> item)
            => this._Loggers.Contains(item);
        public void CopyTo(ILogger<TMessage>[] array, int arrayIndex)
            => this._Loggers.CopyTo(array, arrayIndex);
        public IEnumerator<ILogger<TMessage>> GetEnumerator()
            => this._Loggers.GetEnumerator();
        public bool Remove(ILogger<TMessage> item)
            => this._Loggers.Remove(item);

        IEnumerator IEnumerable.GetEnumerator()
            => this._Loggers.GetEnumerator();

        public void Log(TMessage message, LogLevel level = LogLevel.Info, object? sender = null, DateTime? time = null, string? stackTrace = null)
        {
            if (!this.IsEnabled || !level.MeetsLevel(this.LogLevel))
            {
                return;
            }
            foreach (var logger in this.Loggers.Compact())
            {
                Catch(() => logger.Log(message, level, sender, time, stackTrace), this.ExceptionHandling);
            }
        }

        protected void InitializeInstance(IEnumerable<ILogger<TMessage>>? loggers, ExceptionHandling? exceptionHandling)
        {
            this._Loggers = loggers is null ? new() : new(loggers);
            this.IsEnabled = true;
            this.LogLevel = LogLevel.Normal;
            this.ExceptionHandling = exceptionHandling ?? new();
        }
    }
}