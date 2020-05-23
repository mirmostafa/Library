using System;
using System.IO;
using System.Runtime.CompilerServices;
using Mohammad.EventsArgs;
using Mohammad.Interfaces;

namespace Mohammad.Logging
{
    public interface ILogger : ISimpleLogger, ISupportSilence
    {
        object Sender { get; set; }
        string Name { get; set; }
        bool IsEnabled { get; set; }
        bool IsDebugModeEnabled { get; set; }
        TextWriter Out { get; set; }
        string LogTextFormat { get; set; }
        string MoreInfoTextFormat { get; set; }
        event EventHandler<LogEventArgs> Logged;
        event EventHandler<LogEventArgs> Logging;

        void Info(object text, object moreInfo = null, object sender = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Warn(object text, object moreInfo = null, object sender = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Exception(object text, Exception ex = null, object sender = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Fatal(object text, Exception ex = null, object sender = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Debug(object text, Exception ex = null, object sender = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);
    }

    public interface ILoggerContainer
    {
        ILogger Logger { get; }
        object DefaultSender { get; }
    }

    public interface ILogProvider : ILoggerContainer {}
}