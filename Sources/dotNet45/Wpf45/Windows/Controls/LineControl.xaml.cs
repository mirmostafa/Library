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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public object Child
        {
            get { return this.GetValue(ChildProperty); }
            set
            {
                this.SetValue(ChildProperty, value);
                this.OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty LineBrushProperty = DependencyProperty.Register("LineBrush",
            typeof(Brush),
            typeof(LineControl),
            new PropertyMetadata(default(Brush)));

        public Brush LineBrush { get { return (Brush) this.GetValue(LineBrushProperty); } set { this.SetValue(LineBrushProperty, value); } }

        public static readonly DependencyProperty PropTypeProperty = ControlHelper.GetDependencyProperty<Brush, LineControl>("MouseMoveLineBrush",
            defaultValue: default(Brush));

        public Brush MouseMoveLineBrush { get { return (Brush) this.GetValue(PropTypeProperty); } set { this.SetValue(PropTypeProperty, value); } }

        public LineControl()
        {
            this.InitializeComponent();
            this.LineBrush = Brushes.RoyalBlue;
        }

        private void LineControl_OnMouseEnter(object sender, MouseEventArgs e) { }
        private void LineControl_OnMouseLeave(object sender, MouseEventArgs e) { }

        public FrameworkElement FlickerTextBlock => this.FlickableRectangle;
    }
}