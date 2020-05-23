using Mohammad.Exceptions;
using Mohammad.Logging;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for OperationWatcherPage.xaml
    /// </summary>
    public partial class OperationWatcherPage : ISimpleLogger
    {
        private static OperationWatcherPage _SingleInstance;

        public ILogger Logger
        {
            get { return this.Watcher.Logger; }
            set
            {
                if (Equals(value, this.Watcher.Logger))
                    return;
                this.Watcher.Logger = value;
                this.OnPropertyChanged();
            }
        }

        public static OperationWatcherPage SingleInstance
        {
            get { return _SingleInstance ?? (_SingleInstance = new OperationWatcherPage()); }
            set
            {
                if (_SingleInstance != null)
                    LibraryException.WrapThrow("Cannot initialize more than once.");
                _SingleInstance = value;
            }
        }

        public OperationWatcherPage() { this.InitializeComponent(); }
        public void WatchLogger(ILogger logger) { this.Watcher.WatchLogger(logger); }

        public void Log(object text, object moreInfo = null, object sender = null, LogLevel level = LogLevel.Info, string memberName = "", string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            this.Watcher.Log(text, moreInfo, sender, level, memberName, sourceFilePath, sourceLineNumber);
        }
    }
}