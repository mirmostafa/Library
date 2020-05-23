using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using Microsoft.WindowsAPICodePack.Taskbar;
using Mohammad.Wpf.Windows;
using TaskbarProgressBarState = Mohammad.Wpf.Windows.TaskbarProgressBarState;

namespace Mohammad.Wpf.Internals
{
    public sealed class Taskbar
    {
        private readonly Windows7Tools _Owner;
        private TaskbarProgressBar _ProgressBar;
        private UIElement _ThumbnailClipControl;
        public TaskbarProgressBar ProgressBar => this._ProgressBar ?? (this._ProgressBar = new TaskbarProgressBar(this._Owner.Interop.Handle));

        internal Taskbar(Windows7Tools owner)
        {
            this._Owner = owner;
            this._Owner.Window.SizeChanged += (_, __) => this.SetThumbnailClip(this._ThumbnailClipControl);
        }

        public void SetThumbnailClip(UIElement ctrl)
        {
            try
            {
                this._ThumbnailClipControl = ctrl;
                var offset = VisualTreeHelper.GetOffset(ctrl);
                TaskbarManager.Instance.TabbedThumbnail.SetThumbnailClip(this._Owner.Interop.Handle,
                    new Rectangle((int) offset.X, (int) offset.Y, (int) ctrl.RenderSize.Width, (int) ctrl.RenderSize.Height));
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }

    public sealed class TaskbarProgressBar
    {
        private readonly IntPtr _WindowHandle;
        private int _Maximum;
        private int _Value;

        public TaskbarProgressBarState State
        {
            set { TaskbarManager.Instance.SetProgressState((Microsoft.WindowsAPICodePack.Taskbar.TaskbarProgressBarState) value); }
        }

        public int Value
        {
            private get { return this._Value; }
            set
            {
                if (IntPtr.Zero == this._WindowHandle)
                    TaskbarManager.Instance.SetProgressValue(value, this.Maximum);
                else
                    TaskbarManager.Instance.SetProgressValue(value, this.Maximum, this._WindowHandle);
                this._Value = value;
            }
        }

        public int Maximum
        {
            private get { return this._Maximum; }
            set
            {
                if (IntPtr.Zero == this._WindowHandle)
                    TaskbarManager.Instance.SetProgressValue(this.Value, value);
                else
                    TaskbarManager.Instance.SetProgressValue(this.Value, value, this._WindowHandle);
                this._Maximum = value;
            }
        }

        internal TaskbarProgressBar(IntPtr windowHandle) { this._WindowHandle = windowHandle; }
    }
}