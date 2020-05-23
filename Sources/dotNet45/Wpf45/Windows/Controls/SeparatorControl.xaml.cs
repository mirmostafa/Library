using System.Windows;
using System.Windows.Media;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for SeparatorLabel.xaml
    /// </summary>
    public partial class SeparatorControl : IBindable
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

        public static readonly DependencyProperty HeaderStyleProperty = DependencyProperty.Register("HeaderStyle",
            typeof(Style),
            typeof(SeparatorControl),
            new PropertyMetadata(default(Style)));

        public Style HeaderStyle { get { return (Style) this.GetValue(HeaderStyleProperty); } set { this.SetValue(HeaderStyleProperty, value); } }

        public static readonly DependencyProperty SepratorColorProperty = DependencyProperty.Register("SepratorColor",
            typeof(Brush),
            typeof(SeparatorLabel),
            new PropertyMetadata(default(Brush)));

        public Brush SepratorColor { get { return (Brush) this.GetValue(SepratorColorProperty); } set { this.SetValue(SepratorColorProperty, value); } }

        public SeparatorControl()
        {
            this.InitializeComponent();
            this.SepratorColor = Brushes.RoyalBlue;
        }

        public DependencyProperty BindingFieldProperty { get { return HeaderProperty; } }
    }
}