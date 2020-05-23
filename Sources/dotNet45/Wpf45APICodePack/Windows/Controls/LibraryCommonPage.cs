using System;
using System.ComponentModel;
using System.Windows;
using Mohammad.EventsArgs;
using Mohammad.Helpers;

namespace Mohammad.Wpf.Windows.Controls
{
    public class LibraryCommonPage : LibraryPage, IDescriptiveObject
    {
        public static readonly DependencyProperty IsDialogProperty = DependencyProperty.Register("IsDialog",
            typeof(bool),
            typeof(LibraryCommonPage),
            new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty StartupLocationProperty = DependencyProperty.Register("StartupLocation",
            typeof(WindowStartupLocation),
            typeof(LibraryCommonPage),
            new PropertyMetadata(default(WindowStartupLocation)));

        private object _CancelButtonContent;
        private LibraryCommonDialog _CommonDialog;
        private object _OkButtonContent;
        private bool _ShowCancelButton = true;
        private bool _ShowCommandBar;
        private bool _ShowOkButton = true;
        private bool _ShowPageHeader;
        private bool _ShowStatusBar;

        public bool IsDialog
        {
            get => (bool)this.GetValue(IsDialogProperty);
            set
            {
                this.SetValue(IsDialogProperty, value);
                if (this.CommonDialog != null)
                {
                    this.CommonDialog.IsDialog = this.IsDialog;
                }

                this.OnPropertyChanged();
            }
        }

        public WindowStartupLocation StartupLocation
        {
            get => (WindowStartupLocation)this.GetValue(StartupLocationProperty);
            set
            {
                this.SetValue(StartupLocationProperty, value);
                if (this.CommonDialog != null)
                {
                    this.CommonDialog.WindowStartupLocation = this.StartupLocation;
                }

                this.OnPropertyChanged();
            }
        }

        public LibraryCommonDialog CommonDialog
        {
            get => this._CommonDialog;
            internal set
            {
                if (Equals(this._CommonDialog, value))
                {
                    return;
                }

                this._CommonDialog = value;
                if (value == null)
                {
                    return;
                }

                value.Closing += this.CommonDialog_OnClosing;
                value.Closed += this.CommonDialog_OnClosed;
                value.OkButtonClicked += this.CommandDialog_OnOkButtonClicked;
                value.CancelButtonClicked += this.CommandDialog_OnCancelButtonClicked;
                value.IsDialog = this.IsDialog;
                value.ShowCommandBar = this.ShowCommandBar;
                value.ShowOkButton = this.ShowOkButton;
                value.ShowCancelButton = this.ShowCancelButton;
                value.CancelButtonContent = this.CancelButtonContent ?? "Cancel";
                value.OkButtonContent = this.OkButtonContent ?? "OK";
            }
        }

        public object OkButtonContent
        {
            get => this._OkButtonContent;
            set
            {
                if (Equals(value, this._OkButtonContent))
                {
                    return;
                }

                this._OkButtonContent = value;
                this.OnPropertyChanged();
            }
        }

        public bool? DialogResult
        {
            get => this.CommonDialog?.DialogResult;
            set
            {
                var dialog = this.CommonDialog;
                if (dialog != null)
                {
                    dialog.DialogResult = value;
                }
            }
        }

        public bool ShowStatusBar
        {
            get => this._ShowStatusBar;
            set
            {
                if (value.Equals(this._ShowStatusBar))
                {
                    return;
                }

                this._ShowStatusBar = value;
                this.OnPropertyChanged();
            }
        }

        public bool ShowCommandBar
        {
            get => this._ShowCommandBar;
            set
            {
                if (value.Equals(this._ShowCommandBar))
                {
                    return;
                }

                this._ShowCommandBar = value;
                this.OnPropertyChanged();
            }
        }

        public bool ShowOkButton
        {
            get => this._ShowOkButton;
            set
            {
                if (value.Equals(this._ShowOkButton))
                {
                    return;
                }

                this._ShowOkButton = value;
                this.OnPropertyChanged();
            }
        }

        public bool ShowCancelButton
        {
            get => this._ShowCancelButton;
            set
            {
                if (value.Equals(this._ShowCancelButton))
                {
                    return;
                }

                this._ShowCancelButton = value;
                this.OnPropertyChanged();
            }
        }

        public object CancelButtonContent
        {
            get => this._CancelButtonContent;
            set
            {
                if (Equals(value, this._CancelButtonContent))
                {
                    return;
                }

                this._CancelButtonContent = value;
                this.OnPropertyChanged();
            }
        }

        public bool ShowPageHeader
        {
            get => this._ShowPageHeader;
            set
            {
                if (value.Equals(this._ShowPageHeader))
                {
                    return;
                }

                this._ShowPageHeader = value;
                this.OnPropertyChanged();
            }
        }

        public Thickness Padding { get; set; }

        public string AppTitle { get; set; }

        protected virtual void OnCommonDialogClosed()
        {
            this.CommonDialogClosed?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnCommonDialogClosing(CancelEventArgs e)
        {
            this.CommonDialogClosing?.Invoke(this, e);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property.Name == IsDialogProperty.Name && this.CommonDialog != null)
            {
                this.CommonDialog.IsDialog = this.IsDialog;
            }

            base.OnPropertyChanged(e);
        }

        private void CommandDialog_OnCancelButtonClicked(object sender, ActingEventArgs e)
        {
            this.CancelButtonClicked.Raise(this, e);
        }

        private void CommandDialog_OnOkButtonClicked(object sender, ActingEventArgs e)
        {
            this.OkButtonClicked.Raise(sender, e);
        }

        private void CommonDialog_OnClosed(object sender, EventArgs e)
        {
            this.OnCommonDialogClosed();
        }

        private void CommonDialog_OnClosing(object sender, CancelEventArgs e)
        {
            this.OnCommonDialogClosing(e);
        }

        public event EventHandler<ActingEventArgs> OkButtonClicked;
        public event EventHandler<ActingEventArgs> CancelButtonClicked;
        public event EventHandler CommonDialogClosed;
        public event CancelEventHandler CommonDialogClosing;
    }
}