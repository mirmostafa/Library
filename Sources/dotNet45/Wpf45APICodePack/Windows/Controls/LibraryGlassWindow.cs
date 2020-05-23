using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using Mohammad.Helpers;
using Mohammad.Logging;
using Mohammad.Win32.Natives.IfacesEnumsStructsClasses;
using Mohammad.Wpf.Helpers;
using Mohammad.Wpf.Win32;

[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "Mohammad.Wpf.Windows.Controls")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2007/xaml/presentation", "Mohammad.Wpf.Windows.Controls")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2008/xaml/presentation", "Mohammad.Wpf.Windows.Controls")]
[assembly: XmlnsPrefix("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "Lib")]
[assembly: XmlnsPrefix("http://schemas.microsoft.com/winfx/2007/xaml/presentation", "Lib")]
[assembly: XmlnsPrefix("http://schemas.microsoft.com/winfx/2008/xaml/presentation", "Lib")]

namespace Mohammad.Wpf.Windows.Controls
{
    public class LibraryGlassWindow : LibraryWindow
    {
        private Status _Status;
        private Windows7Tools _Windows7Tools;
        private static readonly SolidColorBrush _DefaultGlassBackground = new BrushConverter().ConvertFromString("#12000900").To<SolidColorBrush>();

        public static readonly DependencyProperty GlassBackgroundProperty = ControlHelper.GetDependencyProperty<Brush, LibraryGlassWindow>("GlassBackground",
            defaultValue: _DefaultGlassBackground);

        public Brush GlassBackground { get { return (Brush) this.GetValue(GlassBackgroundProperty); } set { this.SetValue(GlassBackgroundProperty, value); } }

        public static readonly DependencyProperty IsGlassEnabledProperty = ControlHelper.GetDependencyProperty<bool, LibraryGlassWindow>("IsGlassEnabled",
            (me, e) =>
            {
                if (e)
                {
                    me.AllowsTransparency = true;
                    me.WindowStyle = WindowStyle.None;
                    me.Background = me.GlassBackground ?? _DefaultGlassBackground;
                }
                else
                {
                    me.AllowsTransparency = false;
                    me.WindowStyle = WindowStyle.SingleBorderWindow;
                    me.Background = null;
                }
            },
            defaultValue: false);

        //public bool IsAeroGlassEnabled { get; set; }

        /// <summary>
        ///     Compatible with Windows 10
        /// </summary>
        public bool IsGlassEnabled { get { return (bool) this.GetValue(IsGlassEnabledProperty); } set { this.SetValue(IsGlassEnabledProperty, value); } }

        public Status Status
        {
            get
            {
                if (this._Status != null)
                    return this._Status;
                ProgressBar progressBar;
                StatusBarItem statusBarItem;
                ISimpleLogger watcher;
                this.InitializeAppStatus(out progressBar, out statusBarItem, out watcher);
                this._Status = new Status(progressBar, statusBarItem, watcher, this);
                return this._Status;
            }
        }

        public static bool AeroGlassCompositionEnabled
        {
            get { return AeroGlassApi.DwmIsCompositionEnabled(); }
            set { AeroGlassApi.DwmEnableComposition(value ? CompositionEnable.Enable : CompositionEnable.Disable); }
        }

        public Windows7Tools Windows7Tools => this._Windows7Tools ?? (this._Windows7Tools = new Windows7Tools(this));
        public LibraryGlassWindow() { this.Loaded += this.LibraryGlassWindow_OnLoaded; }

        private void LibraryGlassWindow_OnLoaded(object sender, RoutedEventArgs e) { }

        private void InitializeAppStatus(out ProgressBar progressBar, out StatusBarItem statusBarItem, out ISimpleLogger watcher)
        {
            this.OnInitializingStatus(out progressBar, out statusBarItem, out watcher);
        }

        protected virtual void OnInitializingStatus(out ProgressBar progressBar, out StatusBarItem statusBarItem, out ISimpleLogger logger)
        {
            progressBar = null;
            statusBarItem = null;
            logger = null;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            this.ExtendGlass(new Thickness(-1));
        }

        [DebuggerStepThrough]
        protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg != WindowsMessages.WM_DWMCOMPOSITIONCHANGED)
                return IntPtr.Zero;
            this.ExtendGlass(new Thickness(-1));
            handled = true;
            return IntPtr.Zero;
        }
    }
}