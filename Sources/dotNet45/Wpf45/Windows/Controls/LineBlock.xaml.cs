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

        //public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register("TextWrapping",
        //	typeof(TextWrapping),
        //	typeof(LineBlock),
        //	new PropertyMetadata(default(TextWrapping)));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
            typeof(string),
            typeof(LineBlock),
            new PropertyMetadata(default(string)));

        public Brush LineBrush
        {
            get => (Brush)this.GetValue(LineBrushProperty);
            set
            {
                this.SetValue(LineBrushProperty, value);
                this.OnPropertyChanged();
            }
        }

        public string Text
        {
            get => (string)this.GetValue(TextProperty);
            set
            {
                this.SetValue(TextProperty, value);
                this.OnPropertyChanged();
            }
        }

        public TextWrapping TextWrapping
        {
            get => this.LibTextBlock.TextWrapping;
            set
            {
                this.LibTextBlock.TextWrapping = value;
                this.OnPropertyChanged();
            }
        }

        public Style BlockStyle
        {
            get => this.LibTextBlock.Style;
            set
            {
                this.LibTextBlock.Style = value;
                this.OnPropertyChanged();
            }
        }

        public bool AutoFlick { get; set; }

        public DependencyProperty BindingFieldProperty => TextProperty;
        public FrameworkElement FlickerTextBlock => this.LibTextBlock.FlickerTextBlock;

        public LineBlock()
        {
            this.InitializeComponent();
            this.LineBrush = Brushes.RoyalBlue;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (this.AutoFlick && e.Property != null && e.Property.Name == "Text")
            {
                this.Flick();
            }

            base.OnPropertyChanged(e);
        }
    }
}