namespace Library.Logging
{
    public interface ILoggerCollection<TLogMessage, TLogger> : ILoggerBag<TLogMessage>, ICollection<TLogger>
        where TLogger : ILogger<TLogMessage>
    {

    }
}
