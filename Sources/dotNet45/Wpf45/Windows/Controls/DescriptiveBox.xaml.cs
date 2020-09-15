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

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description",
            typeof(string),
            typeof(DescriptiveBox),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty DescriptionStyleProperty = DependencyProperty.Register("DescriptionStyle",
            typeof(Style),
            typeof(DescriptiveBox),
            new PropertyMetadata(default(Style)));

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header",
            typeof(string),
            typeof(DescriptiveBox),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty HeaderStyleProperty = DependencyProperty.Register("HeaderStyle",
            typeof(Style),
            typeof(DescriptiveBox),
            new PropertyMetadata(default(Style)));

        public DescriptiveBox()
        {
            this.InitializeComponent();
            this.HeaderStyle = this.FindResource("HighlightLibBlock").As<Style>();
            this.DescriptionStyle = this.FindResource("LowlightLibBlock").As<Style>();
        }

        [Bindable(true)]
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

        public string Description
        {
            get => (string)this.GetValue(DescriptionProperty);
            set
            {
                this.SetValue(DescriptionProperty, value);
                this.OnPropertyChanged();
            }
        }

        public Style DescriptionStyle
        {
            get => (Style)this.GetValue(DescriptionStyleProperty);
            set => this.SetValue(DescriptionStyleProperty, value);
        }

        public string Header
        {
            get => (string)this.GetValue(HeaderProperty);
            set
            {
                this.SetValue(HeaderProperty, value);
                this.OnPropertyChanged();
            }
        }

        public Style HeaderStyle
        {
            get => (Style)this.GetValue(HeaderStyleProperty);
            set
            {
                this.SetValue(HeaderStyleProperty, value);
                this.OnPropertyChanged();
            }
        }
    }
}