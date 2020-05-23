using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml.Serialization;
using Mohammad.Helpers;
using Mohammad.Wpf.EventsArgs;
using Mohammad.Wpf.Helpers;
using Mohammad.Wpf.Interfaces;

namespace Mohammad.Wpf.Windows.Settings
{
    [Serializable]
    public class WindowSettings : INotifyPropertyChanged
    {
        private double _Height;
        private bool _Initiated;
        private double _Left;
        private double _Top;
        private double _Width;

        [XmlIgnore]
        public Window Window { get; private set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public WindowState WindowState { get; set; }

        public double Left
        {
            get => this._Left;
            set
            {
                if (value.Equals(this._Left))
                {
                    return;
                }

                this._Left = value;
                this.OnPropertyChanged();
            }
        }

        public double Top
        {
            get => this._Top;
            set
            {
                if (value.Equals(this._Top))
                {
                    return;
                }

                this._Top = value;
                this.OnPropertyChanged();
            }
        }

        public double Height
        {
            get => this._Height;
            set
            {
                if (value.Equals(this._Height))
                {
                    return;
                }

                this._Height = value;
                this.OnPropertyChanged();
            }
        }

        public double Width
        {
            get => this._Width;
            set
            {
                if (value.Equals(this._Width))
                {
                    return;
                }

                this._Width = value;
                this.OnPropertyChanged();
            }
        }

        public bool Initiated
        {
            get => this._Initiated;
            set
            {
                if (value.Equals(this._Initiated))
                {
                    return;
                }

                this._Initiated = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetWindow(Window window)
        {
            this.Window = window;
            this.Window.Loaded += this.OnLoaded;
            this.Window.Closed += this.OnClosed;
            this.WindowSet.RaiseAsync(this);
        }

        public virtual void LoadState(Window window)
        {
            if (!this.Initiated)
            {
                return;
            }

            this.ApplyingToWindow.Raise(this, new ApplySettingsEventArgs(window));
            window.WindowState = this.WindowState;
            if (this.WindowState == WindowState.Maximized)
            {
                return;
            }

            window.Left = this.Left;
            window.Top = this.Top;
            window.Width = this.Width;
            window.Height = this.Height;

            this.ApplyedToWindow.Raise(this, new ApplySettingsEventArgs(window));
        }

        public virtual void SaveState(Window window)
        {
            this.Initiated = true;
            this.ApplyingToSettings.Raise(this, new ApplySettingsEventArgs(window));
            this.WindowState = window.WindowState;
            if (window.WindowState == WindowState.Maximized)
            {
                return;
            }

            this.Left = window.Left;
            this.Top = window.Top;
            this.Width = window.Width;
            this.Height = window.Height;

            this.ApplyedToSettings.Raise(this, new ApplySettingsEventArgs(window));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnClosed(object sender, EventArgs eventArgs)
        {
            var window = (Window)sender;
            this.SaveState(window);
            foreach (var control in window.GetControls<ISettingsEnabledElement>())
            {
                control.SaveSettings();
            }
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            var window = (Window)sender;
            this.LoadState(window);
            foreach (var control in window.GetControls<ISettingsEnabledElement>())
            {
                control.LoadSettings();
            }
        }

        public event EventHandler<ApplySettingsEventArgs> ApplyingToWindow;
        public event EventHandler<ApplySettingsEventArgs> ApplyedToWindow;
        public event EventHandler<ApplySettingsEventArgs> ApplyingToSettings;
        public event EventHandler<ApplySettingsEventArgs> ApplyedToSettings;
        public event EventHandler WindowSet;
    }
}