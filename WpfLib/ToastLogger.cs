using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Library.Logging;
using Library.Validations;
using Microsoft.Toolkit.Uwp.Notifications;

namespace Library.Wpf;

public class ToastLogger : ILogger, ILogger<string>
{
    public bool IsEnabled { get; set; }
    public LogLevel LogLevel { get; set; }

    public void Log([DisallowNull] object message,
                    LogLevel level = LogLevel.Info,
                    [CallerMemberName] object? sender = null,
                    DateTime? time = null,
                    string? stackTrace = null) =>
        this.Log(message.ArgumentNotNull().ToString()!, level, sender, time, stackTrace);

    public void Log([DisallowNull] string message,
                    LogLevel level = LogLevel.Info,
                    [CallerMemberName] object? sender = null,
                    DateTime? time = null,
                    string? stackTrace = null)
    {
        if (!LoggingHelper.MeetsLevel(level, this.LogLevel))
        {
            return;
        }
        //new ToastContentBuilder()
        //    .AddText(message)
        //    .AddText()
    }
}

public static class ToastLoggerExtensions
{

}
