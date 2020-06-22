using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Mohammad.DesignPatterns.Creational;
using Mohammad.DesignPatterns.ExceptionHandlingPattern;
using Mohammad.EventsArgs;
using Mohammad.Helpers;
using Mohammad.Logging;
using Mohammad.Threading.Tasks;
using static Mohammad.Helpers.CodeHelper;

// ReSharper disable ExplicitCallerInfoArgument

namespace Mohammad.Primitives
{
    public abstract class ApplicationMainThreadBase<TApp> : Singleton<TApp>, IExceptionHandlerContainer, ILoggerContainer
        where TApp : class, ISingleton<TApp>
    {
        private object _ApplicationLogSender;
        private CancellationTokenSource _CancellationTokenSource;
        private object _DefaultSender;
        private ExceptionHandling _ExceptionHandling;
        private LogProvider _Log;
        private ILogger _Logger;

        protected LogProvider Log => this._Log ?? (this._Log = this.OnInitializingLogProvider());
        public TaskScheduler MainTaskScheduler { get; private set; }
        public SynchronizationContext UiSyncContext { get; private set; }
        public object ApplicationLogSender { get => this._ApplicationLogSender ?? this.AppName; set => this._ApplicationLogSender = value; }
        public virtual string AppName => ApplicationHelper.ApplicationTitle;
        public bool IsCancellationRequested => this.CancellationTokenSource.IsCancellationRequested;
        public bool IsApplicationShuttingdown { get; private set; }
        protected virtual TextWriter Out { set => this.Logger.Out = value; get => this.Logger.Out; }

        protected CancellationTokenSource CancellationTokenSource
        {
            get => this._CancellationTokenSource ?? (this._CancellationTokenSource = new CancellationTokenSource());
            set => this._CancellationTokenSource = value;
        }

        // ReSharper disable once EmptyConstructor
        protected ApplicationMainThreadBase() { }

        protected virtual LogProvider OnInitializingLogProvider() => new LogProvider(this.Logger, this.DefaultSender);

        public event EventHandler Shuttingdown;

        protected virtual ILogger OnInitializingLogger() => new Logger();
        protected virtual ExceptionHandling OnExceptionHandlingRequired() => new ExceptionHandling(this.OnExceptionOccurred);

        protected virtual void OnExceptionOccurred(object sender, ExceptionOccurredEventArgs<Exception> e)
        {
            this.Logger.Exception($"An unhanded exception occurred on {this.ApplicationLogSender}", e.Exception, sender);
        }

        public void Start()
        {
            try
            {
                this.Logger.Debug("Initializing", null, this.ApplicationLogSender);
                this.InitializeComponents();
                this.Logger.Debug("Starting up", null, this.ApplicationLogSender);
                this.OnStartingup();
                this.Logger.Debug("Application is functional", null, this.ApplicationLogSender);
            }
            catch (Exception ex)
            {
                this.ExceptionHandling.HandleException(ex);
            }
        }

        private void InitializeComponents()
        {
            if (this.MainTaskScheduler == null)
                this.MainTaskScheduler = CatchFunc(()=>TaskScheduler.FromCurrentSynchronizationContext());
            if (this.UiSyncContext == null)
                this.UiSyncContext = CatchFunc(() => SynchronizationContext.Current);
            this.OnInitializing();
        }

        public async Task RunInUi(Action action) { await Async.Run(action, this.CancellationTokenSource.Token, this.MainTaskScheduler); }

        public async Task<TResult> RunInUi<TResult>(Func<TResult> action) => await Async.Run(action,
            this.CancellationTokenSource.Token,
            this.MainTaskScheduler);

        protected virtual void OnInitializing() { }
        protected virtual void OnStartingup() { }

        public void Shutdown()
        {
            this.Logger.Debug("Shutting down", null, this.ApplicationLogSender);
            this.IsApplicationShuttingdown = true;
            this.CancellationTokenSource.Cancel();
            this.OnShuttingdown();
            this.Logger.Debug("Shut down", null, this.ApplicationLogSender);
        }

        protected virtual void OnShuttingdown() { this.Shuttingdown.Raise(this); }

        //protected void Log(object text, object details = null, object sender = null, LogLevel level = LogLevel.Internal,
        //    [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        //{
        //    this.Logger.Log(text, details, sender ?? this.DefaultSender, level, memberName, sourceFilePath, sourceLineNumber);
        //}

        //protected void Debug(object text, Exception details = null, object sender = null, [CallerMemberName] string memberName = "",
        //    [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        //{
        //    this.Logger.Debug(text, details, sender ?? this.DefaultSender, memberName, sourceFilePath, sourceLineNumber);
        //}

        //protected void Info(object text, object details = null, object sender = null, [CallerMemberName] string memberName = "",
        //    [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        //{
        //    this.Logger.Info(text, details, sender ?? this.DefaultSender, memberName, sourceFilePath, sourceLineNumber);
        //}

        //protected void Warn(object text, object details = null, object sender = null, [CallerMemberName] string memberName = "",
        //    [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        //{
        //    this.Logger.Warn(text, details, sender ?? this.DefaultSender, memberName, sourceFilePath, sourceLineNumber);
        //}

        //protected void Fatal(object text, Exception exception = null, object sender = null, [CallerMemberName] string memberName = "",
        //    [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        //{
        //    this.Logger.Fatal(text, exception, sender ?? this.DefaultSender, memberName, sourceFilePath, sourceLineNumber);
        //}

        //protected void Error(object text, object details = null, object sender = null, [CallerMemberName] string memberName = "",
        //    [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        //{
        //    this.Logger.Warn(text, details, sender ?? this.DefaultSender, memberName, sourceFilePath, sourceLineNumber);
        //}

        protected virtual object OnInitializingDefaultSender() => this.GetType().Name;

        public virtual ExceptionHandling ExceptionHandling
        {
            get => this._ExceptionHandling ?? (this._ExceptionHandling = this.OnExceptionHandlingRequired());
            protected set => this._ExceptionHandling = value;
        }

        public object DefaultSender => this._DefaultSender ?? (this._DefaultSender = this.OnInitializingDefaultSender());

        public virtual ILogger Logger
        {
            get => this._Logger ?? (this._Logger = this.OnInitializingLogger());
            protected set => this._Logger = value;
        }
    }
}