using System.Windows;
using System.Windows.Media;
using Mohammad.Wpf.Helpers;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for SeparatorLabel.xaml
    /// </summary>
    public partial class SeparatorCheckBox : IBindable
    {
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header",
            typeof(string),
            typeof(SeparatorCheckBox),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked",
            typeof(bool?),
            typeof(SeparatorCheckBox),
            new PropertyMetadata(default(bool?)));

        public string Header
        {
            get => (string)this.GetValue(HeaderProperty);
            set => this.SetValue(HeaderProperty, value);
        }

        public bool? IsChecked
        {
            get => (bool?)this.GetValue(IsCheckedProperty);
            set => this.SetValue(IsCheckedProperty, value);
        }

        public Style ContentBlockStyle
        {
            get => this.CheckBox.Style;
            set => this.CheckBox.Style = value;
        }

        public new Brush Foreground
        {
            get => this.CheckBox.Foreground;
            set => this.CheckBox.Foreground = value;
        }

        public DependencyProperty BindingFieldProperty => IsCheckedProperty;

        public SeparatorCheckBox()
        {
            this.InitializeComponent();

            this.CheckBox.BindToElementPath(this, HeaderProperty, "Content");
            this.CheckBox.BindToElementPath(this, IsCheckedProperty, "IsChecked");
        }
    }
}