using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Win32;
using Mohammad.Primitives;
using Mohammad.DesignPatterns.ExceptionHandlingPattern;
using Mohammad.DesignPatterns.ExceptionHandlingPattern.Handlers;
using Mohammad.EventsArgs;
using Mohammad.Exceptions;
using Mohammad.Helpers;
using Mohammad.Settings;
using Mohammad.Wpf.Windows.Intenals;

namespace Mohammad.Wpf.Windows
{
    public class LibraryApplication : Application, IApplication
    {
        private Mutex _Mutext;
        private static ExceptionHandling _ExceptionHandling;
        internal static Type InnerCommandsStaticClassType;
        private static TaskScheduler _MainTaskScheduler;
        private static SynchronizationContext _UiSyncContext;
        private static CancellationTokenSource _CancellationTokenSource;

        public static ExceptionHandling ExceptionHandling
        {
            get { return _ExceptionHandling ?? (_ExceptionHandling = Current.As<LibraryApplication>().OnExceptionHandlingRequrired() ?? new ExceptionHandling()); }
            protected set { _ExceptionHandling = value; }
        }

        public ExceptionHandling AppExceptionHandling { get { return ExceptionHandling; } protected set { ExceptionHandling = value; } }

        public static bool IsApplicationShuttingDown => CancellationTokenSource.IsCancellationRequested;

        public string AppName { get; protected set; }
        protected static CancellationTokenSource CancellationTokenSource => _CancellationTokenSource ?? (_CancellationTokenSource = new CancellationTokenSource());

        public static TaskScheduler MainTaskScheduler
        {
            get
            {
                if (_MainTaskScheduler == null)
                    throw new WindowMismatchException("Main window is not a LibraryWindow. Or MainWindow is not initialized yet.");
                return _MainTaskScheduler;
            }
        }

        public static bool IsDesignMode { get; private set; }
        protected static Type CommandsStaticClassType { get { return InnerCommandsStaticClassType; } set { InnerCommandsStaticClassType = value; } }
        internal static IAppSettings LibraryApplicationSettings { get; set; }
        public static bool AmIAlone { get; private set; }
        protected bool MonitorSingleInstance { get; set; } = true;
        protected static bool UseDefaultAppSettings { get; set; }
        protected bool IsMemberOfIntegrator { get; set; } = true;
        public bool AreExceptionsHandled { get; set; }

        public static SynchronizationContext UiSyncContext
        {
            get
            {
                if (_MainTaskScheduler == null)
                    throw new WindowMismatchException("Main window is not a LibraryWindow. Or MainWindow is not initialized yet.");
                return _UiSyncContext;
            }
        }

        public LibraryApplication()
        {
            IsDesignMode = true;
            this.Initialize();
        }

        public static void RunInUi(Action action) { Current.Dispatcher.Invoke(DispatcherPriority.Render, action); }
        public static TResult RunInUi<TResult>(Func<TResult> action) => Current.Dispatcher.Invoke(DispatcherPriority.Render, action).To<TResult>();
        protected virtual ExceptionHandling OnExceptionHandlingRequrired() { return new ExceptionHandling {RaiseExceptions = true}; }
        protected virtual void OnInitializing() { CodeHelper.Catch(() => ApplicationRegistration.RegisterMe()); }
        protected virtual void OnMainWindowInitialized() { }

        internal virtual void ExceptionOccurred(object sender, ExceptionOccurredEventArgs<Exception> e)
        {
            if (e.Exception is BreakException)
            {
                e.Handled = true;
                return;
            }

            this.OnExceptionOccurred(sender, e);
        }

        protected virtual void OnExceptionOccurred(object sender, ExceptionOccurredEventArgs<Exception> e)
        {
            if (!e.Handled)
                ExceptionHandling.HandleException(sender, e.Exception);
        }

        protected virtual void OnShuttingDown() { CancellationTokenSource.Cancel(); }

        internal void MainWindowIsInitializing()
        {
            if (_MainTaskScheduler == null)
                _MainTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            if (_UiSyncContext == null)
                _UiSyncContext = SynchronizationContext.Current;
            this.OnMainWindowInitialized();
        }

        private void Initialize()
        {
            AppDomain.CurrentDomain.UnhandledException += this.CurrentDomainUnhandledException;
            Current.DispatcherUnhandledException += this.Current_OnDispatcherUnhandledException;
            this.OnInitializing();
            if (this.IsMemberOfIntegrator)
                try
                {
                    var key = Registry.CurrentUser.OpenSubKey("Software", true);
                    //key = key.CreateSubKey(ApplicationHelper.Company).CreateSubKey("Products");
                    key = key?.CreateSubKey("MOHAMMAD")?.CreateSubKey("Products");
                    if (!ApplicationHelper.ProductTitle.IsNullOrEmpty())
                        key = key?.CreateSubKey(ApplicationHelper.ProductTitle);
                    key = key?.CreateSubKey(ApplicationHelper.ApplicationTitle);
                    key = key?.CreateSubKey("IntegrityInfo");
                    if (key != null)
                    {
                        key.SetValue("ProductTitle", ApplicationHelper.ProductTitle);
                        key.SetValue("ApplicationTitle", ApplicationHelper.ApplicationTitle);
                        key.SetValue("Company", ApplicationHelper.Company);
                        key.SetValue("Version", ApplicationHelper.Version);
                        key.SetValue("Description", ApplicationHelper.Description);
                        key.SetValue("ExecutionPath", Environment.CommandLine);
                        key.Flush();
                        key.Close();
                    }
                }
                catch
                {
                    // ignored
                }
            if (this.MonitorSingleInstance)
            {
                var guid = ApplicationHelper.Guid;
                if (guid.IsNullOrEmpty())
                    throw new NoNullAllowedException("GUID cannot be empty. Or turn 'MonitorSingleInstance' off.");
                this._Mutext = null;
                if (Mutex.TryOpenExisting(guid, out this._Mutext))
                {
                    AmIAlone = false;
                }
                else
                {
                    this._Mutext = new Mutex(true, guid);
                    AmIAlone = true;
                }
            }
            this.OnApplyingTheme();
            this.OnLoadSettings();
        }

        protected virtual void OnApplyingTheme()
        {
            Current.Resources.MergedDictionaries.Add(new ResourceDictionary
                                                     {
                                                         Source =
                                                             new Uri("/Library45.Wpf;component/Themes/DefaultTheme.xaml",
                                                                 UriKind.RelativeOrAbsolute)
                                                     });
            Current.Resources.MergedDictionaries.Add(new ResourceDictionary
                                                     {
                                                         Source =
                                                             new Uri("/Library45.Wpf;component/Themes/ModernUI.xaml",
                                                                 UriKind.RelativeOrAbsolute)
                                                     });
        }

        private void Current_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var args = new ExceptionOccurredEventArgs<Exception>(e.Exception);
            this.ExceptionOccurred(sender, args);
            e.Handled = args.Handled || this.AreExceptionsHandled;
        }

        private void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var args = new ExceptionOccurredEventArgs<Exception>(e.ExceptionObject as Exception);
            this.ExceptionOccurred(sender, args);
        }

        internal void MainWindowIsClosed()
        {
            var exception = CodeHelper.Catch(this.OnShuttingDown);
            if (exception != null)
                this.ExceptionOccurred(this, new ExceptionOccurredEventArgs<Exception>(exception));
        }

        public static void DoEvents()
        {
            var frame = new DispatcherFrame(true);
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                (SendOrPostCallback) delegate(object arg)
                {
                    var f = arg as DispatcherFrame;
                    if (f != null)
                        f.Continue = false;
                },
                frame);
            Dispatcher.PushFrame(frame);
        }

        protected virtual void OnLoadSettings()
        {
            if (UseDefaultAppSettings)
                LibraryApplicationSettings = AppSettings.Load();
        }

        protected virtual void OnSaveSttings() { LibraryApplicationSettings?.Save(); }

        protected override void OnExit(ExitEventArgs e)
        {
            this.OnSaveSttings();
            base.OnExit(e);
        }
    }
}