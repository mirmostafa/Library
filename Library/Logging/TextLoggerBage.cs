using Library.DesignPatterns.ExceptionHandlingPattern;

namespace Library.Logging
{
    public sealed class TextLoggerBage : LoggersBase<string>
    {
        public TextLoggerBage(params ILogger<string>[] loggers)
            : base(loggers)
        {
        }

        public TextLoggerBage(IEnumerable<ILogger<string>> loggers, ExceptionHandling? exceptionHandling = null)
            : base(loggers, exceptionHandling)
        {
        }

        public TextLoggerBage(ExceptionHandling? exceptionHandling, params ILogger<string>[] loggers)
            : base(exceptionHandling, loggers)
        {
        }
    }
}