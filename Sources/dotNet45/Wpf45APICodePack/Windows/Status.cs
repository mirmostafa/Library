using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Mohammad.DesignPatterns.Behavioral;
using Mohammad.DesignPatterns.ExceptionHandlingPattern;
using Mohammad.DesignPatterns.ExceptionHandlingPattern.Handlers;
using Mohammad.Helpers;
using Mohammad.Interfaces;
using Mohammad.Logging;
using Mohammad.Threading.Tasks;
using Mohammad.Wpf.EventsArgs;
using Mohammad.Wpf.Helpers;
using Mohammad.Wpf.Windows.Controls;
using Mohammad.Wpf.Windows.Input.LibCommands;

namespace Mohammad.Wpf.Windows
{
    public class Status : ISupportSilence, IExceptionHandlerContainer
    {
        private readonly TaskScheduler _Scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        private readonly LibraryGlassWindow _Window;
        private ExceptionHandling _ExceptionHandling;
        private TaskbarProgressBarState _TaskbarProgressBarState;
        public ProgressBar ProgressBar { get; }
        public StatusBarItem StatusBarItem { get; }
        public ISimpleLogger Logger { get; }

        public Status(ProgressBar progressBar, StatusBarItem statusBarItem, ISimpleLogger logger, LibraryGlassWindow window = null)
        {
            this._Window = window;
            this.ProgressBar = progressBar;
            this.StatusBarItem = statusBarItem;
            this.Logger = logger;
        }

        public void DoWork(Action onWork, string startPrompt = null, string endPrompt = null, params LibCommand[] commands)
        {
            foreach (var command in commands)
            {
                Memento.Set(command, command.IsEnabled);
                command.IsEnabled = false;
            }
            this.Set(startPrompt, true);
            onWork?.Invoke();
            foreach (var command in commands)
                command.IsEnabled = Memento.Get<bool>(command);
            this.Set(endPrompt, false);
        }

        public async Task DoWorkAsync(Action onWork, string startPrompt = null, string endPrompt = null, params LibCommand[] commands)
        {
            await Async.Run(() =>
                {
                    foreach (var command in commands)
                    {
                        Memento.Set(command, command.IsEnabled);
                        command.IsEnabled = false;
                    }
                    this.Set(startPrompt, true);
                    onWork?.Invoke();
                    foreach (var command in commands)
                        command.IsEnabled = Memento.Get<bool>(command);
                    this.Set(endPrompt, false);
                },
                scheduler: this._Scheduler);
        }

        public async void SetProgressStep(int value, int? max = null, string status = null, bool updateProgressBar = false, bool updateTaskbar = true)
        {
            await Async.Run(() =>
                {
                    if (!status.IsNullOrEmpty())
                    {
                        if (this.StatusBarItem != null)
                            this.StatusBarItem.Content = status;
                        this.Logger?.Log(status);
                    }
                    if (updateProgressBar && this.ProgressBar != null)
                    {
                        this.ProgressBar.IsIndeterminate = false;
                        if (max.HasValue)
                            this.ProgressBar.Maximum = max.Value;
                        this.ProgressBar.SetValue(value);
                    }
                    if (!updateTaskbar || this._Window == null)
                        return;
                    try
                    {
                        if (max.HasValue)
                            this._Window.Windows7Tools.Taskbar.ProgressBar.Maximum = max.Value;
                        this._Window.Windows7Tools.Taskbar.ProgressBar.Value = value;
                    }
                    catch
                    {
                        // ignored
                    }
                },
                scheduler: this._Scheduler);
        }

        public void Set() { this.Set("Ready", false); }
        public void Set(bool isWorking) { this.Set(string.Empty, isWorking); }
        public void Set(Exception exception, bool? isWorking = false) { this.Set(null, exception, isWorking); }

        public void Set(string status, Exception exception, bool? isWorking = false)
        {
            if (exception is FaultException)
            {
                var ex = exception.As<FaultException>();
                this.Set(status, detail: ex.CreateMessageFault().Reason.ToString().Replace(Environment.NewLine, " "), isWorking: isWorking, level: LogLevel.Error);
            }
            else
            {
                this.Set(status, detail: exception.Message.Replace(Environment.NewLine, " "), isWorking: isWorking, level: LogLevel.Error);
            }
        }

        public async void Set(string status, bool? isWorking = null, string detail = null, LogLevel level = LogLevel.Info)
        {
            if (!this.EnableRaisingEvents)
                return;
            this.OnSettingStatus(new SettingStatusEventArgs(status, isWorking, detail, level));
            await Async.Run(() =>
                {
                    if (!status.IsNullOrEmpty())
                    {
                        if (this.StatusBarItem != null)
                            this.StatusBarItem.Content = status;
                        this.Logger?.Log(status, detail, level: level);
                    }
                    if (isWorking.HasValue && this.ProgressBar != null)
                        this.ProgressBar.IsIndeterminate = isWorking.Value;
                    if (this._Window == null)
                        return;
                    try
                    {
                        var modify = false;
                        if (isWorking.HasValue)
                        {
                            modify = true;
                            this._TaskbarProgressBarState = isWorking.Value ? TaskbarProgressBarState.Indeterminate : TaskbarProgressBarState.NoProgress;
                        }
                        if (level == LogLevel.Error)
                        {
                            modify = true;
                            this._TaskbarProgressBarState = TaskbarProgressBarState.Error;
                            this.SetProgressStep(99, 100);
                        }
                        if (modify)
                            this._Window.Windows7Tools.Taskbar.ProgressBar.State = this._TaskbarProgressBarState;
                    }
                    catch (Exception ex)
                    {
                        this.ExceptionHandling.HandleException(ex);
                    }
                },
                scheduler: this._Scheduler);
        }

        public event EventHandler<SettingStatusEventArgs> SettingStatus;
        protected virtual void OnSettingStatus(SettingStatusEventArgs e) { this.SettingStatus.Raise(this, e); }

        public ExceptionHandling ExceptionHandling
        {
            get { return this._ExceptionHandling ?? (this._ExceptionHandling = new ExceptionHandling()); }
            set { this._ExceptionHandling = value; }
        }

        public bool EnableRaisingEvents { get; set; } = true;
    }
}