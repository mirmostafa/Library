using System.Windows;
using Mohammad.Primitives;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for ButtonBar.xaml
    /// </summary>
    public partial class VersionTextBox
    {
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly",
            typeof(bool),
            typeof(VersionTextBox),
            new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty VersionProperty = DependencyProperty.Register("Version",
            typeof(VersionInfo),
            typeof(VersionTextBox),
            new PropertyMetadata(default(VersionInfo)));

        public VersionTextBox()
        {
            this.InitializeComponent();
        }

        public bool IsReadOnly
        {
            get => (bool)this.GetValue(IsReadOnlyProperty);
            set
            {
                if (!this.Set(IsReadOnlyProperty, value))
                {
                    return;
                }

                this.OnPropertyChanged();
            }
        }

        public VersionInfo Version
        {
            get => (VersionInfo)(this.GetValue(VersionProperty) ?? (this.Version = new VersionInfo()));
            set
            {
                if (this.Set(VersionProperty, value))
                {
                    return;
                }

                value.PropertyChanged += (_, __) => this.OnPropertyChanged();
                this.DataContext = value;
                this.OnPropertyChanged();
            }
        }
    }
}