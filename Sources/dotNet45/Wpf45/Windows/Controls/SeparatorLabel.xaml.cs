using System.Windows;
using System.Windows.Media;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for SeparatorLabel.xaml
    /// </summary>
    public partial class SeparatorLabel : IBindable
    {
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header",
            typeof(object),
            typeof(SeparatorLabel),
            new PropertyMetadata(default(object)));

        public static readonly DependencyProperty LabelStyleProperty = DependencyProperty.Register("LabelStyle",
            typeof(Style),
            typeof(SeparatorLabel),
            new PropertyMetadata(default(Style)));

        public static readonly DependencyProperty SepratorColorProperty = DependencyProperty.Register("SepratorColor",
            typeof(Brush),
            typeof(SeparatorLabel),
            new PropertyMetadata(default(Brush)));

        public SeparatorLabel()
        {
            this.InitializeComponent();
            this.SepratorColor = Brushes.RoyalBlue;
        }

        public object Header
        {
            get => this.GetValue(HeaderProperty);
            set
            {
                if (!this.Set(HeaderProperty, value))
                {
                    return;
                }

                this.OnPropertyChanged();
            }
        }

        public Style LabelStyle
        {
            get => (Style)this.GetValue(LabelStyleProperty);
            set => this.SetValue(LabelStyleProperty, value);
        }

        public Brush SepratorColor
        {
            get => (Brush)this.GetValue(SepratorColorProperty);
            set => this.SetValue(SepratorColorProperty, value);
        }

        public new Brush Foreground
        {
            get => (Brush)this.GetValue(ForegroundProperty);
            set
            {
                if (!this.Set(ForegroundProperty, value))
                {
                    return;
                }

                this.OnPropertyChanged();
            }
        }

        public DependencyProperty BindingFieldProperty => HeaderProperty;
    }
}