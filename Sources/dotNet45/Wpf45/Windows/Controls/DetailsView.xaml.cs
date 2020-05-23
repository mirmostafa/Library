using System.Windows;
using System.Windows.Media;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for DetailsView.xaml
    /// </summary>
    public partial class DetailsView
    {
        public static readonly DependencyProperty BudgetProperty = DependencyProperty.Register("Budget",
            typeof(object),
            typeof(DetailsView),
            new PropertyMetadata(default(object)));

        public static readonly DependencyProperty BudgetStyleProperty = DependencyProperty.Register("BudgetStyle",
            typeof(Style),
            typeof(DetailsView),
            new PropertyMetadata(default(Style)));

        public static readonly DependencyProperty DetailsProperty = DependencyProperty.Register("Details",
            typeof(object),
            typeof(DetailsView),
            new PropertyMetadata(default(object)));

        public static readonly DependencyProperty DetailsStyleProperty = DependencyProperty.Register("DetailsStyle",
            typeof(Style),
            typeof(DetailsView),
            new PropertyMetadata(default(Style)));

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header",
            typeof(string),
            typeof(DetailsView),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty HeaderStyleProperty = DependencyProperty.Register("HeaderStyle",
            typeof(Style),
            typeof(DetailsView),
            new PropertyMetadata(default(Style)));

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource",
            typeof(ImageSource),
            typeof(DetailsView),
            new PropertyMetadata(default(ImageSource)));

        public static readonly DependencyProperty ImageWidthProperty = DependencyProperty.Register("ImageWidth",
            typeof(double),
            typeof(DetailsView),
            new PropertyMetadata(default(double)));

        public object Budget
        {
            get => this.GetValue(BudgetProperty);
            set
            {
                this.SetValue(BudgetProperty, value);
                this.OnPropertyChanged();
            }
        }

        public Style BudgetStyle
        {
            get => (Style)this.GetValue(BudgetStyleProperty);
            set
            {
                this.SetValue(BudgetStyleProperty, value);
                this.OnPropertyChanged();
            }
        }

        public object Details
        {
            get => this.GetValue(DetailsProperty);
            set
            {
                this.SetValue(DetailsProperty, value);
                this.OnPropertyChanged();
            }
        }

        public Style DetailsStyle
        {
            get => (Style)this.GetValue(DetailsStyleProperty);
            set
            {
                this.SetValue(DetailsStyleProperty, value);
                this.OnPropertyChanged();
            }
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

        public ImageSource ImageSource
        {
            get => (ImageSource)this.GetValue(ImageSourceProperty);
            set
            {
                this.SetValue(ImageSourceProperty, value);
                this.OnPropertyChanged();
            }
        }

        public double ImageWidth
        {
            get => (double)this.GetValue(ImageWidthProperty);
            set
            {
                this.SetValue(ImageWidthProperty, value);
                this.OnPropertyChanged();
            }
        }

        public DetailsView()
        {
            this.InitializeComponent();
            this.HeaderStyle = (Style)this.FindResource("HighlightBlock");
            this.ImageWidth = this.Width / 5;
        }
    }
}