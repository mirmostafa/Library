namespace Library.Logging;

public interface IMsLoggerMessageWapper
{
    void Debug(string log);
    void Error(string log, Exception? exception = null);
    void Info(string log);
    void Warn(string log);
}

public class MsLoggerMessageWapper : MsLoggerMessageWapperBase, IMsLoggerMessageWapper
{
    public MsLoggerMessageWapper(Microsoft.Extensions.Logging.ILogger logger, string name, int eventId)
        : base(logger, name, eventId)
    {
    }
}