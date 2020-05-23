using System;
using System.Runtime.CompilerServices;
using Mohammad.Logging;

// ReSharper disable ExplicitCallerInfoArgument

namespace Mohammad.Helpers
{
    public static class LogProviderHelper
    {
        public static void Log(this ILogProvider provider, object text, object detials = null, object sender = null, LogLevel level = LogLevel.Internal,
            [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            provider?.Logger.Log(text, detials, sender ?? provider.DefaultSender, level, memberName, sourceFilePath, sourceLineNumber);
        }

        public static void Debug(this ILogProvider provider, object text, Exception detials = null, object sender = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            provider?.Logger.Debug(text, detials, sender ?? provider.DefaultSender, memberName, sourceFilePath, sourceLineNumber);
        }

        public static void Info(this ILogProvider provider, object text, object detials = null, object sender = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            provider?.Logger.Info(text, detials, sender ?? provider.DefaultSender, memberName, sourceFilePath, sourceLineNumber);
        }

        public static void Warn(this ILogProvider provider, object text, object detials = null, object sender = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            provider?.Logger.Warn(text, detials, sender ?? provider.DefaultSender, memberName, sourceFilePath, sourceLineNumber);
        }

        public static void Fatal(this ILogProvider provider, object text, Exception exception = null, object sender = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            provider?.Logger.Fatal(text, exception, sender ?? provider.DefaultSender, memberName, sourceFilePath, sourceLineNumber);
        }

        public static void Error(this ILogProvider provider, object text, object detials = null, object sender = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            provider?.Logger.Warn(text, detials, sender ?? provider.DefaultSender, memberName, sourceFilePath, sourceLineNumber);
        }
    }
}