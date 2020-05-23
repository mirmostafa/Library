using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Mohammad.EventsArgs;
using Mohammad.Helpers;

// ReSharper disable ExplicitCallerInfoArgument

namespace Mohammad.Logging
{
    public class Logger : ILogger
    {
        private string _LogTextFormat;
        private string _MoreInfoTextFormat;
        public const string DEFAULT_LOG_TEXT_FORMAT = "[%datetime%][%level%][%sender%][%text%][%name%]";
        public static Logger Empty { get; } = new Logger("Empty Logger") {IsEnabled = false};

        public Logger(string name) { this.Name = name; }

        public Logger() { }

        public void Log(LogEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));
            this.Log(e.Log, e.MoreInfo, e.Sender, e.Level, e.MemberName, e.SourceFilePath, e.SourceLineNumber);
        }

        private static StringBuilder ParseException(Exception ex)
        {
            var moreInfo = new StringBuilder(ex?.GetBaseException().Message);

            if (ex == null)
                return moreInfo;
            moreInfo.AppendLine("\tException:");
            var error = ex;
            while (error != null)
            {
                moreInfo.AppendLine("\t\t" + error.Message);
                error = error.InnerException;
            }

            moreInfo.AppendLine("\tStack Trace:");
            moreInfo.AppendLine("\t\t" + ex.StackTrace);
            return moreInfo;
        }

        public void WriteLine(string log) { this.Log(log); }

        protected virtual LogEventArgs OnLogged(LogEventArgs e) => this.Logged.Raise(this, e);
        protected virtual LogEventArgs OnLogging(LogEventArgs e) => this.Logging.Raise(this, e);

        public static string FormatLogText(object text, LogLevel level = LogLevel.None, DateTime? dateTime = null, object sender = null, string name = null,
            string logTextFormat = DEFAULT_LOG_TEXT_FORMAT)
        {
            var logText = logTextFormat.IfNullOrEmpty(DEFAULT_LOG_TEXT_FORMAT);
            Func<string, string, string> replace = (format, value) => value.IsNullOrEmpty() ? logText.Remove($"[%{format}%]") : logText.Replace($"%{format}%", value);
            logText = replace("datetime", dateTime?.ToString());
            logText = replace("name", name);
            logText = replace("text", text?.ToString());
            logText = replace("sender", sender?.ToString());
            logText = replace("level", level == LogLevel.None ? null : level.ToString());
            return logText;
        }

        public static string FormatLogText(LogEventArgs e) => FormatLogText(e.Log, e.Level, sender: e.Sender);
        public string Name { get; set; }

        public string LogTextFormat
        {
            get { return this._LogTextFormat.IsNullOrEmpty() ? DEFAULT_LOG_TEXT_FORMAT : this._LogTextFormat; }
            set { this._LogTextFormat = value; }
        }

        public string MoreInfoTextFormat
        {
            get { return this._MoreInfoTextFormat.IsNullOrEmpty() ? "-------------------[%moreinfo%]" : this._MoreInfoTextFormat; }
            set { this._MoreInfoTextFormat = value; }
        }

        public bool IsDebugModeEnabled { get; set; }
        public TextWriter Out { get; set; }
        public bool EnableRaisingEvents { get; set; } = true;
        public bool IsEnabled { get; set; } = true;
        public event EventHandler<LogEventArgs> Logged;
        public event EventHandler<LogEventArgs> Logging;

        public void Info(object text, object moreInfo = null, object sender = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            this.Log(text, moreInfo, sender, LogLevel.Info, memberName, sourceFilePath, sourceLineNumber);
        }

        public void Warn(object text, object moreInfo = null, object sender = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            this.Log(text, moreInfo, sender, LogLevel.Warning, memberName, sourceFilePath, sourceLineNumber);
        }

        public void Exception(object text, Exception ex = null, object sender = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            this.Log(text, ParseException(ex).ToString(), sender, LogLevel.Error, memberName, sourceFilePath, sourceLineNumber);
        }

        public void Fatal(object text, Exception ex = null, object sender = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            this.Log(text, ParseException(ex).ToString(), sender, LogLevel.Fatal, memberName, sourceFilePath, sourceLineNumber);
        }

        public void Debug(object text, Exception ex = null, object sender = null, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            if (this.IsDebugModeEnabled)
                this.Log(text, ex?.GetBaseException().Message, sender, LogLevel.Debug, memberName, sourceFilePath, sourceLineNumber);
        }

        public void Log(object text, object moreInfo = null, object sender = null, LogLevel level = LogLevel.Internal, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (!this.IsEnabled)
                return;
            var logText = FormatLogText(text, level, DateTime.Now, sender, this.Name, this.LogTextFormat);
            System.Diagnostics.Debug.WriteLine(logText);

            if (this.EnableRaisingEvents)
                this.OnLogging(new LogEventArgs(text, moreInfo, level) {Sender = sender ?? this.Sender});
            TextWriter writer = null;
            switch (level)
            {
                case LogLevel.None:
                    break;
                case LogLevel.Normal:
                case LogLevel.Debug:
                case LogLevel.Status:
                case LogLevel.Warning:
                case LogLevel.Info:
                case LogLevel.Error:
                case LogLevel.Fatal:
                case LogLevel.Internal:
                    writer = this.Out;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level));
            }
            if (writer != null)
            {
                if (this.IsDebugModeEnabled || level == LogLevel.Debug || level == LogLevel.Fatal || level == LogLevel.Error)
                    logText = string.Concat(logText,
                        Environment.NewLine,
                        string.Format("\tFile Path:\t'{1}'{3}\tLine No:\t'{2}'{3}\tMember: \t'{0}'",
                            memberName,
                            sourceFilePath,
                            sourceLineNumber,
                            Environment.NewLine));
                lock (this)
                {
                    writer.WriteLine(logText);
                    if (moreInfo != null)
                        writer.WriteLine(this.MoreInfoTextFormat.Replace("%moreinfo%", moreInfo.NotNull("")));
                }
            }
            if (this.EnableRaisingEvents)
                this.OnLogged(new LogEventArgs(text, moreInfo, level) {Sender = sender});
        }

        public object Sender { get; set; }
    }
}