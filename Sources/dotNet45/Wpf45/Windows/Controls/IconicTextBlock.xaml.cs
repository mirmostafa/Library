using System.Windows;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for IconicTextBlock.xaml
    /// </summary>
    public partial class IconicTextBlock
    {
        public static readonly DependencyProperty PathHeightProperty = DependencyProperty.Register("PathHeight",
            typeof(double),
            typeof(IconicTextBlock),
            new PropertyMetadata(default(double)));

        public double PathHeight { get { return (double) this.GetValue(PathHeightProperty); } set { this.SetValue(PathHeightProperty, value); } }

        public static readonly DependencyProperty PathStyleProperty = DependencyProperty.Register("PathStyle",
            typeof(Style),
            typeof(IconicTextBlock),
            new PropertyMetadata(default(Style)));

        public Style PathStyle { get { return (Style) this.GetValue(PathStyleProperty); } set { this.SetValue(PathStyleProperty, value); } }

        public static readonly DependencyProperty PathWidthProperty = DependencyProperty.Register("PathWidth",
            typeof(double),
            typeof(IconicTextBlock),
            new PropertyMetadata(default(double)));

        public double PathWidth { get { return (double) this.GetValue(PathWidthProperty); } set { this.SetValue(PathWidthProperty, value); } }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
            typeof(string),
            typeof(IconicTextBlock),
            new PropertyMetadata(default(string)));

        public string Text { get { return (string) this.GetValue(TextProperty); } set { this.SetValue(TextProperty, value); } }

        public static readonly DependencyProperty TextBlockStyleProperty = DependencyProperty.Register("TextBlockStyle",
            typeof(Style),
            typeof(IconicTextBlock),
            new PropertyMetadata(default(Style)));

        public Style TextBlockStyle { get { return (Style) this.GetValue(TextBlockStyleProperty); } set { this.SetValue(TextBlockStyleProperty, value); } }

        public IconicTextBlock()
        {
            this.InitializeComponent();
            this.PathWidth = 12;
            this.PathHeight = 12;
        }
    }
}