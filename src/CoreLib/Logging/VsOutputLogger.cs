using System.Diagnostics;

using Library.Helpers;

namespace Library.Logging;

public sealed class VsOutputLogger : FastLoggerBase<object>, ILogger
{
    public void WriteLine(string log)
    {
        if (this.LogLevel.MeetsLevel(LogLevel.Debug))
        {
            Debugger.Log(0, null, log);
        }
    }

    protected override void OnLogging(LogRecord<object> logRecord)
    {
        var message = logRecord.Reformat();
        Debugger.Log(logRecord.Level.ToInt(), logRecord.Sender?.ToString(), message);
    }
}
