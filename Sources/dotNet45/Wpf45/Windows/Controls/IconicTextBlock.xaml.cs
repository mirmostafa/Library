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

        public static readonly DependencyProperty PathStyleProperty = DependencyProperty.Register("PathStyle",
            typeof(Style),
            typeof(IconicTextBlock),
            new PropertyMetadata(default(Style)));

        public static readonly DependencyProperty PathWidthProperty = DependencyProperty.Register("PathWidth",
            typeof(double),
            typeof(IconicTextBlock),
            new PropertyMetadata(default(double)));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
            typeof(string),
            typeof(IconicTextBlock),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty TextBlockStyleProperty = DependencyProperty.Register("TextBlockStyle",
            typeof(Style),
            typeof(IconicTextBlock),
            new PropertyMetadata(default(Style)));

        public IconicTextBlock()
        {
            this.InitializeComponent();
            this.PathWidth = 12;
            this.PathHeight = 12;
        }

        public double PathHeight
        {
            get => (double)this.GetValue(PathHeightProperty);
            set => this.SetValue(PathHeightProperty, value);
        }

        public Style PathStyle
        {
            get => (Style)this.GetValue(PathStyleProperty);
            set => this.SetValue(PathStyleProperty, value);
        }

        public double PathWidth
        {
            get => (double)this.GetValue(PathWidthProperty);
            set => this.SetValue(PathWidthProperty, value);
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
    }
}