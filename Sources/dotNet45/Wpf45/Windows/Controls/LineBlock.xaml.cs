using System.Windows;
using System.Windows.Media;
using Mohammad.Wpf.Helpers;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for LineBlock.xaml
    /// </summary>
    public partial class LineBlock : IFlickable, IBindable
    {
        public static readonly DependencyProperty LineBrushProperty = DependencyProperty.Register("LineBrush",
            typeof(Brush),
            typeof(LineBlock),
            new PropertyMetadata(default(Brush)));

        public Brush LineBrush
        {
            get { return (Brush) this.GetValue(LineBrushProperty); }
            set
            {
                this.SetValue(LineBrushProperty, value);
                this.OnPropertyChanged();
            }
        }

        //public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register("TextWrapping",
        //	typeof(TextWrapping),
        //	typeof(LineBlock),
        //	new PropertyMetadata(default(TextWrapping)));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
            typeof(string),
            typeof(LineBlock),
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

        public TextWrapping TextWrapping
        {
            get { return this.LibTextBlock.TextWrapping; }
            set
            {
                this.LibTextBlock.TextWrapping = value;
                this.OnPropertyChanged();
            }
        }

        public Style BlockStyle
        {
            get { return this.LibTextBlock.Style; }
            set
            {
                this.LibTextBlock.Style = value;
                this.OnPropertyChanged();
            }
        }

        public bool AutoFlick { get; set; }

        public LineBlock()
        {
            this.InitializeComponent();
            this.LineBrush = Brushes.RoyalBlue;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (this.AutoFlick && e.Property != null && e.Property.Name == "Text")
                this.Flick();

            base.OnPropertyChanged(e);
        }

        public DependencyProperty BindingFieldProperty { get { return TextProperty; } }
        public FrameworkElement FlickerTextBlock { get { return this.LibTextBlock.FlickerTextBlock; } }
    }
}