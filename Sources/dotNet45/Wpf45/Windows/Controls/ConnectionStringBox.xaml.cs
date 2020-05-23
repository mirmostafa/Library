using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using Mohammad.Helpers;
using Mohammad.Wpf.Windows.Dialogs;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for ConnectionStringBox.xaml
    /// </summary>
    public partial class ConnectionStringBox
    {
        private string _PromptProperty;

        public static readonly DependencyProperty ConnectionStringProperty = DependencyProperty.Register("ConnectionString",
            typeof(string),
            typeof(ConnectionStringBox),
            new PropertyMetadata(default(string)));

        public string ConnectionString
        {
            get { return (string) this.GetValue(ConnectionStringProperty); }
            set
            {
                try
                {
                    if (!value.IsNullOrEmpty())
                    {
                        var builder = new SqlConnectionStringBuilder(value);
                        if (!builder.Password.IsNullOrEmpty())
                            builder.Password = "******";
                        this.ConnectionStringTextBox.Text = builder.ConnectionString;
                    }
                    else
                    {
                        this.ConnectionStringTextBox.Text = value;
                    }
                }
                catch
                {
                    this.ConnectionStringTextBox.Text = value;
                }
                this.SetValue(ConnectionStringProperty, value);
                this.OnPropertyChanged();
            }
        }

        public string Prompt
        {
            get { return this._PromptProperty; }
            set
            {
                this._PromptProperty = value;
                this.OnPropertyChanged();
            }
        }

        public ConnectionStringBox() { this.InitializeComponent(); }

        private void BrowseButton_OnClick(object sender, RoutedEventArgs e)
        {
            var current = this.ConnectionStringTextBox.Text;
            if (ConnectionStringDialog.Show(ref current, this.Prompt) ?? false)
                this.ConnectionString = current;
        }

        private void ConnectionStringTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //this.ConnectionString = this.ConnectionStringTextBox.Text;
        }
    }
}