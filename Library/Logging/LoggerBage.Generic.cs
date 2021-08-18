using Library.DesignPatterns.ExceptionHandlingPattern;

namespace Library.Logging
{
    public sealed class LoggerBage<TMessage> : LoggersBase<TMessage>
    {
        public LoggerBage(params ILogger<TMessage>[] loggers)
            : base(loggers)
        {
        }

        public LoggerBage(IEnumerable<ILogger<TMessage>> loggers, ExceptionHandling? exceptionHandling = null)
            : base(loggers, exceptionHandling)
        {
        }

        public LoggerBage(ExceptionHandling? exceptionHandling, params ILogger<TMessage>[] loggers)
            : base(exceptionHandling, loggers)
        {
        }
    }
}