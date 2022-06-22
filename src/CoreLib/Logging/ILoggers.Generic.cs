namespace Library.Logging;

public interface ILoggers<TLogMessage> : ILogger<TLogMessage>
{
    IEnumerable<ILogger<TLogMessage>> Loggers { get; }
}
