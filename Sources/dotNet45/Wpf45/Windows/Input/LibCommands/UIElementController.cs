using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Media;
using System.Windows.Shapes;
using Mohammad.Helpers;
using Mohammad.Wpf.Windows.Controls;

namespace Mohammad.Wpf.Windows.Input.LibCommands
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class UIElementController : INotifyPropertyChanged
    {
        private object _Content;
        private bool _IsCancel;
        private bool? _IsChecked;
        private bool _IsDefault;
        private bool _IsEnabled = true;
        private bool _IsReadOnly;
        private Path _Path;
        private Brush _PathFill;
        private Style _PathStyle;
        private string _Text;
        private string _ToolTip;
        private Visibility _Visibility;
        public UIElement Element { get; }

        public bool IsCancel
        {
            get { return this._IsCancel; }
            set
            {
                this._IsCancel = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsDefault
        {
            get { return this._IsDefault; }
            set
            {
                this._IsDefault = value;
                this.OnPropertyChanged();
            }
        }

        public object Content
        {
            get { return this._Content; }
            set
            {
                this._Content = value;
                this.OnPropertyChanged();
            }
        }

        public Visibility Visibility
        {
            get { return this._Visibility; }
            set
            {
                this._Visibility = value;
                this.OnPropertyChanged();
            }
        }

        public string ToolTip
        {
            get { return this._ToolTip; }
            set
            {
                this._ToolTip = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get { return this._IsEnabled; }
            set
            {
                this._IsEnabled = value;
                this.OnPropertyChanged();
            }
        }

        public bool? IsChecked
        {
            get { return this._IsChecked; }
            set
            {
                this._IsChecked = value;
                this.OnPropertyChanged();
            }
        }

        public string Text
        {
            get { return this._Text; }
            set
            {
                if (value == this._Text)
                    return;
                this._Text = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsReadOnly
        {
            get { return this._IsReadOnly; }
            set
            {
                if (value.Equals(this._IsReadOnly))
                    return;
                this._IsReadOnly = value;
                this.OnPropertyChanged();
            }
        }

        public Style PathStyle
        {
            get { return this._PathStyle; }
            set
            {
                if (value == null || value.Equals(this._PathStyle))
                    return;
                this._PathStyle = value;
                this.OnPropertyChanged();
            }
        }

        public Path Path
        {
            get { return this._Path; }
            set
            {
                if (value == null || value.Equals(this._Path))
                    return;
                this._Path = value;
                this.OnPropertyChanged();
            }
        }

        public Brush PathFill
        {
            get { return this._PathFill; }
            set
            {
                if (value == null || value.Equals(this._PathFill))
                    return;
                this._PathFill = value;
                this.OnPropertyChanged();
            }
        }

        public UIElementController(UIElement element)
        {
            this.Element = element;
            this.ReAssignProps();
        }

        private void ReAssignProps()
        {
            var content = this.Content;
            if (this.Content is string)
            {
                content = new AccessText {Text = this.Content.ToString()};
            }
            else if (this.PathStyle != null)
            {
                var path = new Path {Style = this.PathStyle};
                if (this.PathFill != null)
                {
                    path.Fill = this.PathFill;
                    path.IsEnabledChanged += (sende, e) => { sende.As<Path>().Fill = sende.As<Path>().IsEnabled ? this.PathFill : Brushes.Gray; };
                }
                content = path;
            }
            else if (this.Path != null)
            {
                content = this.Path;
            }

            this.Element.IsEnabled = this.IsEnabled;
            this.Element.Visibility = this.Visibility;

            if (this.Element is RibbonApplicationMenuItem)
            {
                var button = this.Element as RibbonMenuItem;

                if (this.Content != null)
                {
                    button.Header = this.Content;
                    var underscoreIndex = this.Content.ToString().IndexOf("_");
                    if (underscoreIndex != -1)
                    {
                        button.KeyTip = this.Content.ToString()[underscoreIndex + 1].ToString();
                        button.Header = this.Content.ToString().Replace("_", "");
                    }
                }
                if (!this.ToolTip.IsNullOrEmpty())
                    button.ToolTip = this.ToolTip;
                button.IsChecked = this.IsChecked ?? false;
            }

            else if (this.Element is RibbonButton)
            {
                var button = this.Element as RibbonButton;

                if (this.Content != null)
                {
                    button.Label = this.Content.ToString();
                    var underscoreIndex = this.Content.ToString().IndexOf("_");
                    if (underscoreIndex != -1)
                    {
                        button.KeyTip = this.Content.ToString()[underscoreIndex + 1].ToString();
                        button.Label = button.Label.Replace("_", "");
                    }
                }
                if (!this.ToolTip.IsNullOrEmpty())
                    button.ToolTip = this.ToolTip;
            }
            if (this.Element is ToggleButton)
            {
                var btn = this.Element.As<ToggleButton>();
                btn.IsChecked = this.IsChecked;
            }
            if (this.Element is ButtonBase)
            {
                var buttonBase = this.Element as ButtonBase;
                buttonBase.Content = content;
                if (!this.ToolTip.IsNullOrEmpty())
                    buttonBase.ToolTip = this.ToolTip;
                if (this.Element is Button)
                {
                    var button = this.Element as Button;
                    button.IsCancel = this.IsCancel;
                    button.IsDefault = this.IsDefault;
                }
            }

            else if (this.Element is TextBox)
            {
                var textbox = this.Element as TextBox;
                textbox.Text = this.Text;
                textbox.ToolTip = this.ToolTip;
                textbox.IsReadOnly = this.IsReadOnly;
            }

            else if (this.Element is MenuItem)
            {
                var menuItem = this.Element as MenuItem;
                if (this.Content != null)
                    menuItem.Header = this.Content;
                if (!this.ToolTip.IsNullOrEmpty())
                    menuItem.ToolTip = this.ToolTip;
                menuItem.IsEnabled = this.IsEnabled;
            }

            else if (this.Element is TextBoxButton)
            {
                var textBoxButton = this.Element as TextBoxButton;

                var buttonBase = textBoxButton.Button;
                buttonBase.Content = content;
                if (!this.ToolTip.IsNullOrEmpty())
                    buttonBase.ToolTip = this.ToolTip;
                buttonBase.IsCancel = this.IsCancel;
                buttonBase.IsDefault = this.IsDefault;

                textBoxButton.Text = this.Text;
                textBoxButton.ToolTip = this.ToolTip;
                //textBoxButton.IsTextBoxReadOnly = this.IsReadOnly;
            }

            this.Element.UpdateLayout();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.ReAssignProps();
            this.PropertyChanged.Raise(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}