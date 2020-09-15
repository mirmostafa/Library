using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Mohammad.EventsArgs;
using Mohammad.Helpers;
using Mohammad.Logging;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for LibraryCommonDialog.xaml
    /// </summary>
    public partial class LibraryCommonDialog
    {
        public static readonly DependencyProperty UseAnimationProperty = DependencyProperty.Register("UseAnimation",
            typeof(bool),
            typeof(LibraryCommonDialog),
            new PropertyMetadata(default(bool)));

        private bool _IsDialog;
        private LibraryCommonPage _Page;

        public LibraryCommonDialog()
        {
            this.UseAnimation = true;
            this.InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public bool UseAnimation
        {
            get => (bool)this.GetValue(UseAnimationProperty);
            set => this.SetValue(UseAnimationProperty, value);
        }

        public LibraryCommonPage Page
        {
            get => this._Page;
            set
            {
                if (Equals(this._Page, value))
                {
                    return;
                }

                this._Page = value;
                this._Page.Window = this;
                this._Page.CommonDialog = this;
                this._Page.TitleChanged += this.Page_OnTitleChanged;
                this.Page_OnTitleChanged(this, EventArgs.Empty);
                this.PageHeader.Owner = value;
                this.MeasureSize();
            }
        }

        public bool IsDialog
        {
            get => this._IsDialog;
            set
            {
                if (this._IsDialog == value)
                {
                    return;
                }

                this._IsDialog = value;
                this.ResizeMode = value ? ResizeMode.NoResize : ResizeMode.CanResize;
                this.StatusBar.Visibility = !value ? Visibility.Visible : Visibility.Collapsed;
                this.IsWindowIconVisible = !value;
                if (value)
                {
                    this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                }

                this.MeasureSize();
            }
        }

        public bool ShowStatusBar
        {
            get => this.StatusBar.Visibility == Visibility.Visible;
            set
            {
                this.StatusBar.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                this.MeasureSize();
            }
        }

        public bool ShowPageHeader
        {
            get => this.PageHeader.Visibility == Visibility.Visible;
            set
            {
                this.PageHeader.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                this.OnPropertyChanged();
            }
        }

        public bool ShowCommandBar
        {
            get => this.CommandBar.Visibility == Visibility.Visible;
            set
            {
                this.CommandBar.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                this.OnPropertyChanged();
            }
        }

        public bool ShowOkButton
        {
            get => this.CommandBar.GetCommandByCommandName("OkCommand").Visibility == Visibility.Visible;
            set
            {
                this.CommandBar.GetCommandByCommandName("OkCommand").Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                this.OnPropertyChanged();
            }
        }

        public bool ShowCancelButton
        {
            get => this.CommandBar.GetCommandByCommandName("CancelCommand").Visibility == Visibility.Visible;
            set
            {
                this.CommandBar.GetCommandByCommandName("CancelCommand").Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                this.OnPropertyChanged();
            }
        }

        public object CancelButtonContent
        {
            get => this.CommandBar.GetCommandByCommandName("CancelCommand").Content;
            set => this.CommandBar.GetCommandByCommandName("CancelCommand").Content = value;
        }

        public object OkButtonContent
        {
            get => this.CommandBar.GetCommandByCommandName("OkCommand").Content;
            set => this.CommandBar.GetCommandByCommandName("OkCommand").Content = value;
        }

        protected override void OnInitializingStatus(out ProgressBar progressBar, out StatusBarItem statusBarItem, out ISimpleLogger logger)
        {
            base.OnInitializingStatus(out progressBar, out statusBarItem, out logger);
            if (statusBarItem == null)
            {
                statusBarItem = this.StatusBarItem;
            }

            if (progressBar == null)
            {
                progressBar = this.ProgressBar;
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            this.MeasureSize();
        }

        protected virtual void OnOkButtonClicked(object sender, ActingEventArgs e)
        {
            this.OkButtonClicked.Raise(sender, e);
        }

        protected virtual void OnCancelButtonClicked(ActingEventArgs e)
        {
            this.CancelButtonClicked.Raise(this, e);
        }

        private void Page_OnTitleChanged(object sender, EventArgs e)
        {
            var title = this.Page.Title;
            if (title == null)
            {
                return;
            }

            this.Title = title;
        }

        private void MeasureSize()
        {
            var page = this.Page;
            if (page == null)
            {
                return;
            }

            this.Width = page.Width;
            this.Width += page.Padding.Right + page.Padding.Left;
            this.Width += 16;

            this.Height = page.Height;
            if (this.ShowPageHeader)
            {
                this.Height += this.PageHeader.Height;
            }

            if (this.ShowStatusBar)
            {
                this.Height += this.StatusBar.Height;
            }

            if (this.ShowCommandBar)
            {
                this.Height += this.CommandBar.Height;
            }

            this.Height += page.Padding.Top + page.Padding.Bottom;
            this.Height += 44;
        }

        private void LibCommonDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.ShowStatusBar = this.Page.ShowStatusBar;
            this.ShowPageHeader = this.Page.ShowPageHeader;
            this.ShowCommandBar = this.Page.ShowCommandBar;
            this.WindowStartupLocation = this.Page.StartupLocation;
            this.Frame.Padding = this.Page.Padding;
            this.Frame.Navigate(this.Page);
        }

        private void OkCommand_OnClosing(object sender, ActingEventArgs e)
        {
            this.OnOkButtonClicked(sender, e);
            if (e.Handled)
            {
                return;
            }

            CodeHelper.Catch(() => this.DialogResult = true, ex => this.Close());
        }

        private void CancelCommand_OnClosing(object sender, ActingEventArgs e)
        {
            this.OnCancelButtonClicked(e);
            if (e.Handled)
            {
                return;
            }

            CodeHelper.Catch(() => this.DialogResult = false, ex => this.Close());
        }

        private void LibraryCommonDialog_OnClosing(object sender, CancelEventArgs e)
        {
        }

        public event EventHandler<ActingEventArgs> OkButtonClicked;

        public event EventHandler<ActingEventArgs> CancelButtonClicked;
    }
}