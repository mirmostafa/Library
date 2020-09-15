using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Mohammad.EventsArgs;
using Mohammad.Helpers;
using Mohammad.Logging;
using Mohammad.ProgressiveOperations;
using Mohammad.Threading.Tasks;
using Mohammad.Wpf.Helpers;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for OperationWatcher.xaml
    /// </summary>
    public partial class OperationWatcher : ISimpleLogger
    {
        public static readonly DependencyProperty AutoScrollProperty = DependencyProperty.Register("AutoScroll",
            typeof(bool),
            typeof(OperationWatcher),
            new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty IsPausedProperty = DependencyProperty.Register("IsPaused",
            typeof(bool),
            typeof(OperationWatcher),
            new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty IsToolBarVisibleProperty = DependencyProperty.Register("IsToolBarVisible",
            typeof(bool),
            typeof(OperationWatcher),
            new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty LogAllProperty = DependencyProperty.Register("LogAll",
            typeof(bool),
            typeof(OperationWatcher),
            new PropertyMetadata(default(bool)));

        private readonly TaskScheduler _UiScheduler;
        private ILogger _Logger;
        private MultiStepOperation _Operation;
        private Visibility _StatusBarVisibility = Visibility.Visible;

        public OperationWatcher()
        {
            this._UiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            this.IsToolBarVisible = true;
            this.InitializeComponent();
            this.DataContext = this;
        }

        public bool AutoScroll
        {
            get => (bool)this.GetValue(AutoScrollProperty);
            set
            {
                this.SetValue(AutoScrollProperty, value);
                this.OnPropertyChanged();
            }
        }

        public bool IsPaused
        {
            get => (bool)this.GetValue(IsPausedProperty);
            set => this.SetValue(IsPausedProperty, value);
        }

        public bool IsToolBarVisible
        {
            get => (bool)this.GetValue(IsToolBarVisibleProperty);
            set
            {
                this.SetValue(IsToolBarVisibleProperty, value);
                this.OnPropertyChanged();
            }
        }

        public bool LogAll
        {
            get => (bool)this.GetValue(LogAllProperty);
            set => this.SetValue(LogAllProperty, value);
        }

        public MultiStepOperation Operation
        {
            get => this._Operation;
            set
            {
                if (this._Operation == value)
                {
                    return;
                }

                if (value != null)
                {
                    value.CurrentOperationCanceled -= this.Operation_OnCurrentOperationCanceled;
                    value.CurrentOperationEnded -= this.Operation_OnCurrentOperationEnded;
                    value.CurrentOperationStarted -= this.Operation_OnCurrentOperationStarted;
                    value.CurrentOperationStepIncreased -= this.Operation_OnCurrentOperationStepIncreased;
                    value.CurrentOperationStepIncreasing -= this.Operation_OnCurrentOperationStepIncreasing;
                    value.MainOperationCanceled -= this.Operation_OnMainOperationCanceled;
                    value.MainOperationEnded -= this.Operation_OnMainOperationEnded;
                    value.MainOperationStarted -= this.Operation_OnMainOperationStarted;
                    value.MainOperationStepIncreased -= this.Operation_OnMainOperationStepIncreased;
                    value.MainOperationStepIncreasing -= this.Operation_OnMainOperationStepIncreasing;
                    value.MultiStepErrorOccurred -= this.Operation_OnMultiStepErrorOccurred;
                }

                this._Operation = value;
                if (this._Operation == null)
                {
                    return;
                }

                this._Operation.CurrentOperationCanceled += this.Operation_OnCurrentOperationCanceled;
                this._Operation.CurrentOperationEnded += this.Operation_OnCurrentOperationEnded;
                this._Operation.CurrentOperationStarted += this.Operation_OnCurrentOperationStarted;
                this._Operation.CurrentOperationStepIncreased += this.Operation_OnCurrentOperationStepIncreased;
                this._Operation.CurrentOperationStepIncreasing += this.Operation_OnCurrentOperationStepIncreasing;
                this._Operation.MainOperationCanceled += this.Operation_OnMainOperationCanceled;
                this._Operation.MainOperationEnded += this.Operation_OnMainOperationEnded;
                this._Operation.MainOperationStarted += this.Operation_OnMainOperationStarted;
                this._Operation.MainOperationStepIncreased += this.Operation_OnMainOperationStepIncreased;
                this._Operation.MainOperationStepIncreasing += this.Operation_OnMainOperationStepIncreasing;
                this.Operation.MultiStepErrorOccurred += this.Operation_OnMultiStepErrorOccurred;
            }
        }

        public ILogger Logger
        {
            get => this._Logger;
            set
            {
                if (Equals(value, this._Logger))
                {
                    return;
                }

                this._Logger = value;
                this._Logger.Logging += this.Logger_OnLogging;
                this.OnPropertyChanged();
            }
        }

        public Visibility StatusBarVisibility
        {
            get => this._StatusBarVisibility;
            set
            {
                if (value == this._StatusBarVisibility)
                {
                    return;
                }

                this._StatusBarVisibility = value;
                this.OnPropertyChanged();
            }
        }

        public void Log(object text,
            object moreInfo = null,
            object sender = null,
            LogLevel level = LogLevel.Info,
            string memberName = "",
            string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            this.Log((text ?? string.Empty).ToString(), moreInfo, sender, DateTime.Now, level);
        }

        public async void Log(string description, object details = null, object sender = null, DateTime dateTime = default, LogLevel level = LogLevel.Info)
        {
            await Async.Run(() =>
                {
                    if (this.IsPaused)
                    {
                        return;
                    }

                    if (Equals(details, "0"))
                    {
                        return;
                    }

                    if (!this.LogAll && level == LogLevel.Debug)
                    {
                        return;
                    }

                    if (!this.LogAll && level == LogLevel.Status)
                    {
                        this.StatusBarItem.Content = description;
                        return;
                    }

                    if (!this.LogAll && EnumHelper.IsEnumInRange(level, LogLevel.Error, LogLevel.Fatal))
                    {
                        details = details is Exception ? details.As<Exception>().GetBaseException().Message : null;
                    }

                    var item = new ListViewItem
                    {
                        Content =
                            new
                            {
                                Time = dateTime == default ? DateTime.Now : dateTime,
                                Description = description,
                                Details = details,
                                Sender = sender
                            }
                    };
                    switch (level)
                    {
                        case LogLevel.Error:
                            item.Foreground = Brushes.Red;
                            break;
                        case LogLevel.Warning:
                            item.Foreground = Brushes.YellowGreen;
                            break;
                    }

                    this.LogListView.Items.Add(item);
                    if (this.AutoScroll)
                    {
                        item.EnsureVisible();
                    }
                },
                scheduler: this._UiScheduler);
        }

        [Obsolete("Incomplete", true)]
        public async void Log(string description,
            Brush brush,
            string details = null,
            object sender = null,
            DateTime dateTime = default,
            bool isStatus = false)
        {
            await Async.Run(() =>
                {
                    this.StatusBarItem.Content = description;
                    if (isStatus)
                    {
                        return;
                    }

                    var item = new ListViewItem
                    {
                        Content =
                            new
                            {
                                Time = dateTime == default ? DateTime.Now : dateTime,
                                Description = description,
                                Details = details,
                                Sender = sender
                            },
                        Foreground = brush
                    };
                    this.LogListView.Items.Add(item);
                    if (this.AutoScroll)
                    {
                        item.EnsureVisible();
                    }
                },
                scheduler: this._UiScheduler);
        }

        public void Clear()
        {
            this.LogListView.Items.Clear();
            this.StatusBarItem.Content = string.Empty;
            this.MainOperationProgressBar.Visibility = Visibility.Collapsed;
            this.CurrentOperationProgressBar.Visibility = Visibility.Collapsed;
        }

        public void SetTaskbarThumbnailClip(Action<UIElement> thumbnailClipSetter)
        {
            thumbnailClipSetter(this.MainOperationProgressBar);
        }

        internal void WatchLogger(ILogger logger)
        {
            logger.Logging += this.logger_OnLogging;
        }

        private void Operation_OnMultiStepErrorOccurred(object sender, MultiStepErrorOccurredEventArgs e)
        {
            this.Log(e.Exception.GetBaseException().Message, e.Log, e.Sender, LogLevel.Error);
        }

        private void Operation_OnMainOperationStepIncreasing(object sender, MultiStepLogEventArgs e)
        {
            if (!StringHelper.IsNullOrEmpty(e.Log))
            {
                this.Log(e.Log.ToString(), sender: e.Sender);
            }
        }

        private void Operation_OnMainOperationStepIncreased(object sender, MultiStepLogEventArgs e)
        {
            this.MainOperationProgressBar.SetValue(e.Step + 1);
            //if (!StringHelper.IsNullOrEmpty(e.Description))
            //	this.Log(e.Description.ToString(), sender: e.Sender);
        }

        private void Operation_OnMainOperationStarted(object sender, MultiStepStartedLogEventArgs e)
        {
            this.StatusBar.Visibility = this.StatusBarVisibility;
            this.MainOperationProgressBar.SetValue(Math.Abs(e.InitialValue - -1) < float.Epsilon ? 1 : e.InitialValue + 1);
            this.MainOperationProgressBar.Maximum = e.Max;
            this.MainOperationProgressBar.Visibility = Visibility.Visible;
            if (!StringHelper.IsNullOrEmpty(e.Log))
            {
                this.Log(e.Log.ToString(), sender: e.Sender);
            }
        }

        private void Operation_OnMainOperationEnded(object sender, MultiStepEndedLogEventArgs e)
        {
            this.StatusBar.Visibility = Visibility.Collapsed;
            this.MainOperationProgressBar.Visibility = Visibility.Collapsed;
            this.CurrentOperationProgressBar.Visibility = Visibility.Collapsed;
            if (!StringHelper.IsNullOrEmpty(e.Log))
            {
                this.Log(e.Log.ToString(), sender: e.Sender);
            }
        }

        private void Operation_OnMainOperationCanceled(object sender, EventArgs e)
        {
            this.MainOperationProgressBar.Visibility = Visibility.Hidden;
        }

        private void Operation_OnCurrentOperationStepIncreasing(object sender, MultiStepLogEventArgs e)
        {
            if (!StringHelper.IsNullOrEmpty(e.Log))
            {
                this.Log(e.Log.ToString(), level: LogLevel.Status, sender: e.Sender);
            }
        }

        private void Operation_OnCurrentOperationStepIncreased(object sender, MultiStepLogEventArgs e)
        {
            this.CurrentOperationProgressBar.SetValue(e.Step + 1);
            if (!StringHelper.IsNullOrEmpty(e.Log))
            {
                this.Log(e.Log.ToString(), level: LogLevel.Status, sender: e.Sender);
            }
        }

        private void Operation_OnCurrentOperationStarted(object sender, MultiStepStartedLogEventArgs e)
        {
            this.CurrentOperationProgressBar.SetValue(Math.Abs(e.InitialValue - -1) < float.Epsilon ? 1 : e.InitialValue + 1);
            this.CurrentOperationProgressBar.Maximum = e.Max;
            this.CurrentOperationProgressBar.Visibility = Visibility.Visible;
            if (!StringHelper.IsNullOrEmpty(e.Log))
            {
                this.Log(e.Log.ToString(), level: LogLevel.Status, sender: e.Sender);
            }
        }

        private void Operation_OnCurrentOperationEnded(object sender, MultiStepEndedLogEventArgs e)
        {
            this.CurrentOperationProgressBar.Visibility = Visibility.Collapsed;
        }

        private void Operation_OnCurrentOperationCanceled(object sender, EventArgs e)
        {
            this.CurrentOperationProgressBar.Visibility = Visibility.Collapsed;
        }

        private void Logger_OnLogging(object sender, LogEventArgs e)
        {
            this.Log(e.Log, e.MoreInfo, e.Sender, e.Level, e.MemberName, e.SourceFilePath, e.SourceLineNumber);
        }

        private void CleanButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.LogListView.Items.Clear();
        }

        private void logger_OnLogging(object sender, LogEventArgs e)
        {
            this.Log(e.Log, e.MoreInfo, e.Sender, e.Level, e.MemberName, e.SourceFilePath, e.SourceLineNumber);
        }
    }
}