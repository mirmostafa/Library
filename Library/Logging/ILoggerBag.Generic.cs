namespace Library.Logging
{
    public interface ILoggerBag<TLogMessage> : ILogger<TLogMessage>
    {
        IEnumerable<ILogger<TLogMessage>> Loggers { get; }
    }
}
