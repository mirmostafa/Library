namespace Library.Logging;

/// <summary>
/// Represents a collection of loggers that implement the ILogger<TLogMessage> interface.
/// </summary>
public interface ILoggerCollection<TLogMessage, TLogger> : ILoggers<TLogMessage>, ICollection<TLogger>
    where TLogger : ILogger<TLogMessage>
{

}
