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

        public Style PathStyle { get { return (Style) this.GetValue(PathStyleProperty); } set { this.SetValue(PathStyleProperty, value); } }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
            typeof(string),
            typeof(PathButton),
            new PropertyMetadata(default(string)));

        public string Text { get { return (string) this.GetValue(TextProperty); } set { this.SetValue(TextProperty, value); } }

        public static readonly DependencyProperty TextBlockStyleProperty = DependencyProperty.Register("TextBlockStyle",
            typeof(Style),
            typeof(PathButton),
            new PropertyMetadata(default(Style)));

        public Style TextBlockStyle { get { return (Style) this.GetValue(TextBlockStyleProperty); } set { this.SetValue(TextBlockStyleProperty, value); } }

        public static readonly DependencyProperty TextContentProperty = DependencyProperty.Register("TextContent",
            typeof(string),
            typeof(PathButton),
            new PropertyMetadata(default(string)));

        public string TextContent { get { return (string) this.GetValue(TextContentProperty); } set { this.SetValue(TextContentProperty, value); } }
        public PathButton() { this.InitializeComponent(); }
    }
}