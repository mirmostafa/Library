using System.Reflection;

namespace Library.Logging;

public interface IMsLoggerMessageWapper
{
    void Debug(string log);
    void Error(string log, Exception? exception = null);
    void Info(string log);
    void Warn(string log);
}
public class MsLoggerMessageWapper : MsLoggerMessageWrapperBase<MsLoggerMessageWapper>, IMsLoggerMessageWapper
{
    public MsLoggerMessageWapper(Microsoft.Extensions.Logging.ILogger logger, string? name = null, int? eventId = null)
        : base(logger, name ?? CodeHelper.GetCallerMethodName() ?? Assembly.GetExecutingAssembly().GetName().Name ?? string.Empty, eventId ?? 0)
    {
    }
}