namespace Library.Logging;

public interface ILoggerCollection<TLogMessage, TLogger> : ILoggers<TLogMessage>, ICollection<TLogger>
    where TLogger : ILogger<TLogMessage>
{

}
