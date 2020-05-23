using System;
using System.Windows;
using Mohammad.Primitives;
using Mohammad.Logging.Gateways;
using Mohammad.Wpf.Windows.Controls;
using Mohammad.Wpf.Windows.Dialogs;
using TestWpfApp45.Settings;

namespace TestWpfApp45
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static readonly ApplicationInjector<App, AppSettings> Injector = ApplicationInjector<App, AppSettings>.Instance;
        public static OperationWatcherPage WatcherPage { get; private set; }

        protected override void OnInitializing()
        {
            UseDefaultAppSettings = false;
            Injector.Logger.Out = new FileLoggerGateway("Logs.txt");
            Injector.Logger.Log("Test", null, this);
            CommandsStaticClassType = typeof(Commands);
            base.OnInitializing();
        }

        protected override void OnLoadSettings()
        {
            Injector.SetSettings(AppSettings.Load());
            base.OnLoadSettings();
        }

        protected override void OnSaveSttings()
        {
            Injector.Settings.LastExecutionTime = DateTime.Now;
            Injector.Settings.Save();
            base.OnSaveSttings();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            if (!AmIAlone)
            {
                this.Shutdown();
                return;
            }
            WatcherPage = new OperationWatcherPage {Logger = Injector.Logger};
            WatcherPage.WatchLogger(Injector.Logger);
            base.OnStartup(e);
        }

        protected override void OnMainWindowInitialized()
        {
            base.OnMainWindowInitialized();
            MsgBoxEx.DefaultWindow = Current.MainWindow;
        }

        //        MsgBoxEx.Exception(e.Exception);
        //    else
        //        MsgBoxEx.Exception(ex);
        //    if (ex != null)
        //    var ex = e.Exception as IException;
        //{
        //protected override void OnExceptionOccurred(object sender, ExceptionOccurredEventArgs<Exception> e)
        //    e.Handled = true;
        //    base.OnExceptionOccurred(sender, e);
        //}
    }
}