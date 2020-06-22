using System;
using System.IO;
using System.Runtime.CompilerServices;
using Mohammad.EventsArgs;

namespace Mohammad.Logging
{
    public abstract class LoggerContainer : ILoggerContainer
    {
        private object _DefaultSender;
        private ILogger _Logger;
        public bool EnableRaisingEvents { get { return this.Logger.EnableRaisingEvents; } set { this.Logger.EnableRaisingEvents = value; } }

        public TextWriter Out
        {
            get { return this.Logger.Out; }
            set
            {
                if (this.Logger != null)
                    this.Logger.Out = value;
            }
        }

        protected LoggerContainer()
        {
            this.Logger.Logged += (_, e) => this.OnLogged(e);
            this.Logger.Logging += (_, e) => this.OnLogging(e);
        }

        protected virtual object OnInitializingDefaultSender() => this.GetType().Name;
        public event EventHandler<LogEventArgs> Logged;
        public event EventHandler<LogEventArgs> Logging;
        protected virtual void OnLogged(LogEventArgs e) { this.Logged?.Invoke(this, e); }
        protected virtual void OnLogging(LogEventArgs e) { this.Logging?.Invoke(this, e); }
        protected virtual ILogger OnInitializingLogger() => new Logger();

        protected void Log(object text, object detials = null, object sender = null, LogLevel level = LogLevel.Internal, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            this.Logger.Log(text, detials, sender ?? this.DefaultSender, level, memberName, sourceFilePath, sourceLineNumber);
        }

        protected void Debug(object text, Exception detials = null, object sender = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            this.Logger.Debug(text, detials, sender ?? this.DefaultSender, memberName, sourceFilePath, sourceLineNumber);
        }

        protected void Info(object text, object detials = null, object sender = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            this.Logger.Info(text, detials, sender ?? this.DefaultSender, memberName, sourceFilePath, sourceLineNumber);
        }

        protected void Warn(object text, object detials = null, object sender = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            this.Logger.Warn(text, detials, sender ?? this.DefaultSender, memberName, sourceFilePath, sourceLineNumber);
        }

        protected void Fatal(object text, Exception exception = null, object sender = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            this.Logger.Fatal(text, exception, sender ?? this.DefaultSender, memberName, sourceFilePath, sourceLineNumber);
        }

        protected void Error(object text, object detials = null, object sender = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            this.Logger.Warn(text, detials, sender ?? this.DefaultSender, memberName, sourceFilePath, sourceLineNumber);
        }

        public object DefaultSender => this._DefaultSender ?? (this._DefaultSender = this.OnInitializingDefaultSender());

        public ILogger Logger => this._Logger ?? (this._Logger = this.OnInitializingLogger());
    }
}