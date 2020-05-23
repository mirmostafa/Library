using System;
using System.Windows;
using Mohammad.Helpers;
using Mohammad.Wpf.Helpers;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for LibCheckBox.xaml
    /// </summary>
    public partial class LibCheckBox : IFlickable
    {
        public static readonly DependencyProperty AutoFlickProperty = DependencyProperty.Register("AutoFlick",
            typeof(bool),
            typeof(LibCheckBox),
            new PropertyMetadata(default(bool)));

        public bool AutoFlick
        {
            get { return (bool) this.GetValue(AutoFlickProperty); }
            set
            {
                this.SetValue(AutoFlickProperty, value);
                this.OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked",
            typeof(bool?),
            typeof(LibCheckBox),
            new PropertyMetadata(default(bool?)));

        public bool? IsChecked
        {
            get { return (bool?) this.GetValue(IsCheckedProperty); }
            set
            {
                this.SetValue(IsCheckedProperty, value);
                if (this.AutoFlick)
                    this.Flick();
                this.OnPropertyChanged();
                //this.OnIsCheckedChanged();
            }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
            typeof(string),
            typeof(LibCheckBox),
            new PropertyMetadata(default(string)));

        public string Text
        {
            get { return (string) this.GetValue(TextProperty); }
            set
            {
                this.SetValue(TextProperty, value);
                this.OnPropertyChanged();
            }
        }

        public LibCheckBox()
        {
            this.InitializeComponent();
            this.MouseLeftButtonUp += (_, e) =>
            {
                this.IsChecked = !(this.IsChecked ?? true);
                e.Handled = true;
            };
        }

        public event EventHandler IsCheckedChanged;
        protected virtual void OnIsCheckedChanged() { this.IsCheckedChanged.Raise(this); }
        public FrameworkElement FlickerTextBlock => this.TextBlock;
    }
}