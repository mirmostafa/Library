using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Mohammad.Wpf.Helpers;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for LineControl.xaml
    /// </summary>
    [ContentProperty("Child")]
    public partial class LineControl : IFlickable
    {
        public static readonly DependencyProperty ChildProperty = DependencyProperty.Register("Child",
            typeof(object),
            typeof(LineControl),
            new PropertyMetadata(default(object)));

        public static readonly DependencyProperty LineBrushProperty = DependencyProperty.Register("LineBrush",
            typeof(Brush),
            typeof(LineControl),
            new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty PropTypeProperty = ControlHelper.GetDependencyProperty<Brush, LineControl>("MouseMoveLineBrush",
            defaultValue: default(Brush));

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public object Child
        {
            get => this.GetValue(ChildProperty);
            set
            {
                this.SetValue(ChildProperty, value);
                this.OnPropertyChanged();
            }
        }

        public Brush LineBrush
        {
            get => (Brush)this.GetValue(LineBrushProperty);
            set => this.SetValue(LineBrushProperty, value);
        }

        public Brush MouseMoveLineBrush
        {
            get => (Brush)this.GetValue(PropTypeProperty);
            set => this.SetValue(PropTypeProperty, value);
        }

        public FrameworkElement FlickerTextBlock => this.FlickableRectangle;

        public LineControl()
        {
            this.InitializeComponent();
            this.LineBrush = Brushes.RoyalBlue;
        }

        private void LineControl_OnMouseEnter(object sender, MouseEventArgs e)
        {
        }

        private void LineControl_OnMouseLeave(object sender, MouseEventArgs e)
        {
        }
    }
}