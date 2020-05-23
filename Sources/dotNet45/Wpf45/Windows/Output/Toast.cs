using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Mohammad.Helpers;
using Mohammad.Wpf.Windows.Controls.Tiles;

namespace Mohammad.Wpf.Windows.Output
{
    public class Toast
    {
        private FrameworkElement _Parent;
        private Popup _Popup;
        private static readonly List<Toast> _ActiveToasts = new List<Toast>();
        public static ToastScale DefaultScale = ToastScale.Large;
        public bool IsHidden { get { return this._Popup == null || !this._Popup.IsOpen; } }
        public static bool DefaultHostOnWindow { get; set; }
        public FrameworkElement Parent { get { return this._Parent ?? (this._Parent = Application.Current.MainWindow); } set { this._Parent = value; } }
        public static int DefaultWidth { get; set; } = 300;
        public static int DefaultHeight { get; set; } = 80;
        public static TimeSpan DefualtTimeout { get; set; } = TimeSpan.FromSeconds(5);
        public static bool IsSilentDefault { get; set; }
        public bool IsSilent { get; set; }
        public bool HostOnWindow { get; set; } = DefaultHostOnWindow;
        public ToastScale Scale { get; set; }
        private Toast(FrameworkElement parent) { this.Parent = parent; }

        public static async void Show(Toast toast, string title, string message, string badge, bool autoHide, TimeSpan timeout, Brush background, Brush forground)
        {
            timeout = CalcTimeout(title, message, timeout);
            var child = GetChild(title, message, badge, forground, background, toast.Scale);
            toast.Hide();
            toast._Popup = new Popup
                           {
                               Child = child,
                               PlacementTarget = toast.Parent,
                               Placement = PlacementMode.RelativePoint,
                               Width = child.Width.Equals(double.NaN) ? DefaultWidth : child.Width,
                               Height = child.Height.Equals(double.NaN) ? DefaultHeight : child.Height,
                               AllowsTransparency = true,
                               PopupAnimation = PopupAnimation.Fade
                           };
            if (!toast.HostOnWindow)
            {
                toast._Popup.Placement = PlacementMode.AbsolutePoint;
                toast._Popup.HorizontalOffset = SystemParameters.PrimaryScreenWidth; // -this._Popup.Width - 2;
                toast._Popup.VerticalOffset = 0 + 25;
            }
            else if (toast.Parent is Window)
            {
                var wnd = toast.Parent.As<Window>();
                if (wnd.WindowState == WindowState.Maximized || wnd.WindowState == WindowState.Minimized)
                {
                    toast._Popup.Placement = PlacementMode.AbsolutePoint;
                    toast._Popup.HorizontalOffset = SystemParameters.PrimaryScreenWidth - toast._Popup.Width - 2;
                    toast._Popup.VerticalOffset = 0 + 25;
                }
                else
                {
                    toast._Popup.HorizontalOffset = toast.Parent.Width - (toast._Popup.Width + 10);
                }
            }
            else
            {
                toast._Popup.HorizontalOffset = toast.Parent.Width - (toast._Popup.Width + 10);
            }
            toast._Popup.MouseUp += (_, __) => toast.Hide();

            toast._Popup.IsOpen = true;
            if (!autoHide)
                return;

            try
            {
                _ActiveToasts.Add(toast);
                await Task.Delay(timeout != default(TimeSpan) ? timeout : DefualtTimeout);
            }
            catch (TaskCanceledException) {}
            toast.Hide();
        }

        public static Toast Show(FrameworkElement parent = null, string title = null, string message = null, string badge = null, bool autoHide = true,
            TimeSpan timeout = default(TimeSpan), Brush background = null, Brush forground = null, ToastScale scale = ToastScale.Default)
        {
            var toast = GetToast(parent, scale);
            Show(toast, title, message, badge, autoHide, timeout, background, forground);
            return toast;
        }

        public static Toast Inform(string title = null, string message = null, string badge = null, bool autoHide = true, TimeSpan timeout = default(TimeSpan),
            ToastScale scale = ToastScale.Default, FrameworkElement parent = null)
        {
            var toast = GetToast(parent, scale);
            Inform(toast, title, message, badge, autoHide, timeout);
            return toast;
        }

        public static Toast Warn(string title = null, string message = null, string badge = null, bool autoHide = true, TimeSpan timeout = default(TimeSpan),
            ToastScale scale = ToastScale.Default, FrameworkElement parent = null)
        {
            var toast = GetToast(parent, scale);
            Warn(toast, title, message, badge, autoHide, timeout);
            return toast;
        }

        public static Toast Error(string title = null, string message = null, string badge = null, bool autoHide = true, TimeSpan timeout = default(TimeSpan),
            ToastScale scale = ToastScale.Default, FrameworkElement parent = null)
        {
            var toast = GetToast(parent, scale);
            Error(toast, title, message, badge, autoHide, timeout);
            return toast;
        }

        public void Hide()
        {
            if (this._Popup != null)
            {
                this._Popup.IsOpen = false;
                if (this._Popup.Child is IDisposable)
                    (this._Popup.Child as IDisposable).Dispose();
                this._Popup.Child = null;
                this._Popup = null;
            }
            _ActiveToasts.Remove(this);
            GC.Collect();
        }

        private static TimeSpan CalcTimeout(string title, string message, TimeSpan timeout)
        {
            if (timeout != default(TimeSpan))
                return timeout;
            var length = 0;
            if (!message.IsNullOrEmpty())
                length = message.Length;
            if (!title.IsNullOrEmpty())
                length += title.Length;
            timeout = TimeSpan.FromMilliseconds((length + 100) * 50);
            return timeout;
        }

        private static FrameworkElement GetChild(string title, string message, string badge, Brush forground, Brush background, ToastScale scale)
        {
            if (message == null && title == null)
                throw new ArgumentNullException("both message and title are null.");
            switch (scale == ToastScale.Default ? DefaultScale : scale)
            {
                case ToastScale.Wide:
                    if (title != null)
                        return new WideText14Tile {Header = title, Body = message, Badge = badge, Background = background, Foreground = forground, Opacity = .9};
                    return new WideText13Tile {Body = message, Badge = badge, Background = background, Foreground = forground, Opacity = .9};
                case ToastScale.Large:
                    if (title != null)
                        return new WideText09Tile {Header = title, Body = message, Badge = badge, Background = background, Foreground = forground, Opacity = .9};
                    return new WideText11Tile {Body = message, Badge = badge, Background = background, Foreground = forground, Opacity = .9};
                case ToastScale.Default:
                    throw new InvalidEnumArgumentException("Please set Scale or DefaultScale.");
                default:
                    throw new ArgumentOutOfRangeException("scale");
            }
        }

        public static Toast GetToast(FrameworkElement parent, ToastScale scale = ToastScale.Default) { return new Toast(parent) {Scale = scale}; }

        public static void Inform(Toast toast, string title = null, string message = null, string badge = null, bool autoHide = true,
            TimeSpan timeout = default(TimeSpan))
        {
            Show(toast, title, message, badge, autoHide, timeout, Brushes.RoyalBlue, Brushes.White);
        }

        public static void Warn(Toast toast, string title = null, string message = null, string badge = null, bool autoHide = true,
            TimeSpan timeout = default(TimeSpan))
        {
            Show(toast, title, message, badge, autoHide, timeout, Brushes.Yellow, Brushes.DarkRed);
        }

        public static void Error(Toast toast, string title = null, string message = null, string badge = null, bool autoHide = true,
            TimeSpan timeout = default(TimeSpan))
        {
            Show(toast, title, message, badge, autoHide, timeout, Brushes.DarkRed, Brushes.Yellow);
        }
    }

    public enum ToastScale
    {
        Default,
        Wide,
        Large
    }
}