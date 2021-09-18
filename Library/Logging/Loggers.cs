using Library.DesignPatterns.ExceptionHandlingPattern;

namespace Library.Logging;

public sealed class Loggers : LoggersBase<object>, ILoggers, ILogger
{
    public Loggers(params ILogger<object>[] loggers)
        : base(loggers)
    {
    }

    public Loggers(IEnumerable<ILogger<object>> loggers, ExceptionHandling? exceptionHandling = null)
        : base(loggers, exceptionHandling)
    {
    }

    public Loggers(ExceptionHandling? exceptionHandling, params ILogger<object>[] loggers)
        : base(exceptionHandling, loggers)
    {
    }
}
