using System.Reflection;

namespace Library.Logging;

public interface IMsLoggerMessageWrapper
{
    void Debug(string log);

    void Error(string log, Exception? exception = null);

    void Info(string log);

    void Warn(string log);
}

public sealed class MsLoggerMessageWrapper(IMsLogger logger, string? name = null, int? eventId = null)
    : MsLoggerMessageWrapperBase<MsLoggerMessageWrapper>(logger, name ?? CodeHelper.GetCallerMethodName() ?? Assembly.GetExecutingAssembly().GetName().Name ?? string.Empty, eventId ?? 0), IMsLoggerMessageWrapper
{
}