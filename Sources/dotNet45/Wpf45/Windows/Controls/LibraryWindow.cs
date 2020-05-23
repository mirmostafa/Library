using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Markup;
using Mohammad.Helpers;
using Mohammad.Wpf.Helpers;
using Mohammad.Wpf.Interfaces;
using Mohammad.Wpf.Internals;

using Mohammad.Wpf.Windows.Input.LibCommands;
using Mohammad.Wpf.Windows.Intenals;
using Mohammad.Wpf.Windows.Media;
using Mohammad.Wpf.Windows.Settings;
using ApplicationHelper = Mohammad.Helpers.ApplicationHelper;

[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "Mohammad.Wpf.Windows.Controls")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2007/xaml/presentation", "Mohammad.Wpf.Windows.Controls")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2008/xaml/presentation", "Mohammad.Wpf.Windows.Controls")]
[assembly: XmlnsPrefix("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "Lib")]
[assembly: XmlnsPrefix("http://schemas.microsoft.com/winfx/2007/xaml/presentation", "Lib")]
[assembly: XmlnsPrefix("http://schemas.microsoft.com/winfx/2008/xaml/presentation", "Lib")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "Mohammad.Wpf.Windows.Input.LibCommands")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2007/xaml/presentation", "Mohammad.Wpf.Windows.Input.LibCommands")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2008/xaml/presentation", "Mohammad.Wpf.Windows.Input.LibCommands")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "Mohammad.Wpf.Windows.Input")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2007/xaml/presentation", "Mohammad.Wpf.Windows.Input")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2008/xaml/presentation", "Mohammad.Wpf.Windows.Input")]

namespace Mohammad.Wpf.Windows.Controls
{
    public class LibraryWindow : Window, ISettingsEnabledElement, IDescriptiveObject
    {
        private string _AppTitle;
        private Type _CommandsStaticClassType;
        private bool _IsDialogBox;
        private bool _UseAnimations = true;

        public static readonly DependencyProperty BackgroundContentProperty = DependencyProperty.Register("BackgroundContent", typeof(object), typeof(LibraryWindow));

        public object BackgroundContent { get { return this.GetValue(BackgroundContentProperty); } set { this.SetValue(BackgroundContentProperty, value); } }

        public static readonly DependencyProperty IsWindowIconVisibleProperty = DependencyProperty.Register("IsWindowIconVisible",
            typeof(bool?),
            typeof(LibraryWindow),
            new PropertyMetadata(default(bool?)));

        public bool? IsWindowIconVisible
        {
            get { return (bool?) this.GetValue(IsWindowIconVisibleProperty); }
            set { this.SetValue(IsWindowIconVisibleProperty, value); }
        }

        protected LibUserControlDynamicCollection Controls { get; private set; }

        protected Type CommandsStaticClassType
        {
            get { return this._CommandsStaticClassType; }
            set
            {
                this._CommandsStaticClassType = value;
                if (value != null)
                    LibCommandManager.Initialize(this, this.CommandsStaticClassType);
            }
        }

        protected LibCommandDynamicCollection Commands { get; }

        public bool IsDialogBox
        {
            get { return this._IsDialogBox; }
            set
            {
                if (value.Equals(this._IsDialogBox))
                    return;
                this._IsDialogBox = value;
                this.ShowInTaskbar = false;
                this.IsWindowIconVisible = false;
                this.ResizeMode = ResizeMode.NoResize;
                this.OnPropertyChanged();
            }
        }

        public IntPtr Handle => this.GetHandle();

        public new bool IsEnabled
        {
            get { return (bool) this.GetValue(IsEnabledProperty); }
            set
            {
                this.SetValue(IsEnabledProperty, value);
                this.OnIsEnabledChanged(EventArgs.Empty);
                this.OnPropertyChanged();
            }
        }

        public bool UseAnimations
        {
            get { return this._UseAnimations; }
            set
            {
                if (value.Equals(this._UseAnimations))
                    return;
                this._UseAnimations = value;
                this.OnPropertyChanged();
            }
        }

        public LibraryWindow()
        {
            if (Equals(this, Application.Current.MainWindow) && Application.Current is LibraryApplication)
                Application.Current.As<LibraryApplication>().MainWindowIsInitializing();
            this.Commands = new LibCommandDynamicCollection(this);
            this.Controls = new LibUserControlDynamicCollection(this);
            if (LibraryApplication.InnerCommandsStaticClassType != null)
                LibCommandManager.Initialize(this, LibraryApplication.InnerCommandsStaticClassType);
            this.Loaded += this.LibraryWindow_OnLoaded;
        }

        protected virtual IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) { return IntPtr.Zero; }

        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            if (!(this.IsWindowIconVisible ?? true))
                this.RemoveIcon();
            base.OnSourceInitialized(e);
        }

        protected virtual WindowSettings OnWindowSettingsRequired()
        {
            var settings = LibraryApplication.LibraryApplicationSettings.As<AppSettings>();
            return settings == null ? null : (Equals(this, Application.Current.MainWindow) ? settings.LibraryMainWindow : null);
        }

        private void LibraryWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.OnLoaded();
            var source = HwndSource.FromHwnd(this.GetHandle());
            source?.AddHook(this.WndProc);

            foreach (var command in this.Commands)
                command.Initialize(this);
            this.LoadSettings();
        }

        protected virtual void OnLoaded() { }

        protected virtual void OnIsEnabledChanged(EventArgs empty)
        {
            if (!this.UseAnimations)
                return;
            if (!this.IsEnabled)
                Animations.FadeOut(this, .75);
            else
                Animations.FadeIn(this);
        }

        protected override void OnClosed(EventArgs e)
        {
            if (Application.Current.MainWindow == null)
                Application.Current.MainWindow = this;
            this.SaveSettings();
            if (Equals(this, Application.Current.MainWindow) && Application.Current is LibraryApplication)
                Application.Current.As<LibraryApplication>().MainWindowIsClosed();
            if (Equals(Application.Current.MainWindow, this))
                Application.Current.MainWindow = null;
            base.OnClosed(e);
        }

        protected TResource FindResource<TResource>(string resourceKey) => this.FindResource(resourceKey).To<TResource>();

        public string Description { get; set; }

        public string AppTitle
        {
            get { return this._AppTitle.IsNullOrEmpty() ? (this._AppTitle = ApplicationHelper.ApplicationTitle) : this._AppTitle; }
            set { this._AppTitle = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler LoadingSettings;
        public event EventHandler SavingSettings;

        public void LoadSettings()
        {
            this.OnWindowSettingsRequired()?.LoadState(this);
            this.LoadingSettings.Raise(this);
        }

        public void SaveSettings()
        {
            this.OnWindowSettingsRequired()?.SaveState(this);
            this.SavingSettings.Raise(this);
        }
    }
}