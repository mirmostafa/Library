using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using Mohammad.Helpers;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for DescriptiveBox.xaml
    /// </summary>
    [ContentProperty("Child")]
    public partial class DescriptiveBox
    {
        public static readonly DependencyProperty ChildProperty = DependencyProperty.Register("Child",
            typeof(object),
            typeof(DescriptiveBox),
            new PropertyMetadata(default(object)));

        [Bindable(true)]
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

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description",
            typeof(string),
            typeof(DescriptiveBox),
            new PropertyMetadata(default(string)));

        public string Description
        {
            get { return (string) this.GetValue(DescriptionProperty); }
            set
            {
                this.SetValue(DescriptionProperty, value);
                this.OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty DescriptionStyleProperty = DependencyProperty.Register("DescriptionStyle",
            typeof(Style),
            typeof(DescriptiveBox),
            new PropertyMetadata(default(Style)));

        public Style DescriptionStyle { get { return (Style) this.GetValue(DescriptionStyleProperty); } set { this.SetValue(DescriptionStyleProperty, value); } }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header",
            typeof(string),
            typeof(DescriptiveBox),
            new PropertyMetadata(default(string)));

        public string Header
        {
            get { return (string) this.GetValue(HeaderProperty); }
            set
            {
                this.SetValue(HeaderProperty, value);
                this.OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty HeaderStyleProperty = DependencyProperty.Register("HeaderStyle",
            typeof(Style),
            typeof(DescriptiveBox),
            new PropertyMetadata(default(Style)));

        public Style HeaderStyle
        {
            get { return (Style) this.GetValue(HeaderStyleProperty); }
            set
            {
                this.SetValue(HeaderStyleProperty, value);
                this.OnPropertyChanged();
            }
        }

        public DescriptiveBox()
        {
            this.InitializeComponent();
            this.HeaderStyle = this.FindResource("HighlightLibBlock").As<Style>();
            this.DescriptionStyle = this.FindResource("LowlightLibBlock").As<Style>();
        }
    }
}