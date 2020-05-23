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

        public object Header
        {
            get { return this.GetValue(HeaderProperty); }
            set
            {
                if (!this.Set(HeaderProperty, value))
                    return;
                this.OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty LabelStyleProperty = DependencyProperty.Register("LabelStyle",
            typeof(Style),
            typeof(SeparatorLabel),
            new PropertyMetadata(default(Style)));

        public Style LabelStyle { get { return (Style) this.GetValue(LabelStyleProperty); } set { this.SetValue(LabelStyleProperty, value); } }

        public static readonly DependencyProperty SepratorColorProperty = DependencyProperty.Register("SepratorColor",
            typeof(Brush),
            typeof(SeparatorLabel),
            new PropertyMetadata(default(Brush)));

        public Brush SepratorColor { get { return (Brush) this.GetValue(SepratorColorProperty); } set { this.SetValue(SepratorColorProperty, value); } }

        public new Brush Foreground
        {
            get { return (Brush) this.GetValue(ForegroundProperty); }
            set
            {
                if (!this.Set(ForegroundProperty, value))
                    return;
                this.OnPropertyChanged();
            }
        }

        public SeparatorLabel()
        {
            this.InitializeComponent();
            this.SepratorColor = Brushes.RoyalBlue;
        }

        public DependencyProperty BindingFieldProperty { get { return HeaderProperty; } }
    }
}