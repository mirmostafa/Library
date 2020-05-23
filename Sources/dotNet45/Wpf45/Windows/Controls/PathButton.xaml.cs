using System.Windows;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for PathButton.xaml
    /// </summary>
    public partial class PathButton
    {
        public static readonly DependencyProperty PathStyleProperty = DependencyProperty.Register("PathStyle",
            typeof(Style),
            typeof(PathButton),
            new PropertyMetadata(default(Style)));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
            typeof(string),
            typeof(PathButton),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty TextBlockStyleProperty = DependencyProperty.Register("TextBlockStyle",
            typeof(Style),
            typeof(PathButton),
            new PropertyMetadata(default(Style)));

        public static readonly DependencyProperty TextContentProperty = DependencyProperty.Register("TextContent",
            typeof(string),
            typeof(PathButton),
            new PropertyMetadata(default(string)));

        public Style PathStyle
        {
            get => (Style)this.GetValue(PathStyleProperty);
            set => this.SetValue(PathStyleProperty, value);
        }

        public string Text
        {
            get => (string)this.GetValue(TextProperty);
            set => this.SetValue(TextProperty, value);
        }

        public Style TextBlockStyle
        {
            get => (Style)this.GetValue(TextBlockStyleProperty);
            set => this.SetValue(TextBlockStyleProperty, value);
        }

        public string TextContent
        {
            get => (string)this.GetValue(TextContentProperty);
            set => this.SetValue(TextContentProperty, value);
        }

        public PathButton()
        {
            this.InitializeComponent();
        }
    }
}