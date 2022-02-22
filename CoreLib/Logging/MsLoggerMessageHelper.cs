using Microsoft.Extensions.Logging;

namespace Library.Logging;
public static class MsLoggerMessageHelper
{
    private static readonly Action<IMsLogger, string, Exception?> _info
        = LoggerMessage.Define<string>(MsLogLevel.Information, new EventId(1, nameof(Info)), "{Log}");
    private static readonly Action<IMsLogger, string, Exception?> _debug
        = LoggerMessage.Define<string>(MsLogLevel.Debug, new EventId(1, nameof(Debug)), "{Log}");
    private static readonly Action<IMsLogger, string, Exception?> _error
        = LoggerMessage.Define<string>(MsLogLevel.Error, new EventId(1, nameof(Error)), "{Log}");
    private static readonly Action<IMsLogger, string, Exception?> _warning
        = LoggerMessage.Define<string>(MsLogLevel.Warning, new EventId(1, nameof(Warn)), "{Log}");

    public static void Info(this IMsLogger logger, string log) => _info(logger, log, null);
    public static void Debug(this IMsLogger logger, string log) => _debug(logger, log, null);
    public static void Error(this IMsLogger logger, string log, Exception? exception = null) => _error(logger, log, exception);
    public static void Warn(this IMsLogger logger, string log) => _warning(logger, log, null);
}