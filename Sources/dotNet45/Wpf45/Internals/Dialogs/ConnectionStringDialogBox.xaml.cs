using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Mohammad.Data.SqlServer.Dynamics;
using Mohammad.Helpers;
using Mohammad.Threading.Tasks;

namespace Mohammad.Wpf.Internals.Dialogs
{
    /// <summary>
    ///     Interaction logic for ConnectionStringDialog.xaml
    /// </summary>
    internal partial class ConnectionStringDialog
    {
        public string ConnectionString
        {
            get
            {
                var scsb = this.ParseConnectionString(true);
                return scsb.ConnectionString;
            }
            set
            {
                var scsb = new SqlConnectionStringBuilder(value);
                this.ServersComboBox.Text = scsb.DataSource;
                this.AuthWindowsRadioButton.IsChecked = scsb.IntegratedSecurity;
                this.UserNameTextBox.Text = scsb.UserID;
                this.PasswordTextBox.Password = scsb.Password;
                this.DbsComboBox.Text = scsb.InitialCatalog;
                this.TimeoutTextBox.Text = scsb.ConnectTimeout.ToString();
            }
        }

        public string Prompt
        {
            get { return this.PromptTextBlock.Text; }
            set
            {
                this.PromptTextBlock.Text = value;
                this.PromptTextBlock.Visibility = string.IsNullOrWhiteSpace(this.PromptTextBlock.Text) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public bool ValidateResult { get; set; }
        public ConnectionStringDialog() { this.InitializeComponent(); }

        private SqlConnectionStringBuilder ParseConnectionString(bool full)
        {
            var scsb = new SqlConnectionStringBuilder();
            if (!this.ServersComboBox.Text.IsNullOrEmpty())
                scsb.DataSource = this.ServersComboBox.Text;
            scsb.IntegratedSecurity = this.AuthWindowsRadioButton.IsChecked ?? false;
            if ((this.AuthUserPassRadioButton.IsChecked ?? false) && !this.UserNameTextBox.Text.IsNullOrEmpty())
                scsb.UserID = this.UserNameTextBox.Text;
            if ((this.AuthUserPassRadioButton.IsChecked ?? false) && !this.PasswordTextBox.Password.IsNullOrEmpty() && (this.SavePassCheckBox.IsChecked ?? false))
                scsb.Password = this.PasswordTextBox.Password;
            if (full && !this.DbsComboBox.Text.IsNullOrEmpty())
                scsb.InitialCatalog = this.DbsComboBox.Text;
            if (this.TimeoutTextBox.Text.IsNumber())
                scsb.ConnectTimeout = this.TimeoutTextBox.Text.ToInt();
            return scsb;
        }

        private void DoValidation(bool full) { }

        private void ServersComboBox_OnDropDownOpened(object sender, EventArgs e)
        {
            if (this.ServersComboBox.Items.Count == 0)
                this.RefreshServers();
        }

        private async void RefreshServers()
        {
            if (!this.RefreshButton.IsEnabled)
                return;
            this.RefreshButton.IsEnabled = false;
            var servers = await Async.Run(() => Server.Servers.Select(server => server.Name).ToArray());
            this.ServersComboBox.ItemsSource = servers;
            this.RefreshButton.IsEnabled = true;
        }

        private void RefreshButton_OnClick(object sender, RoutedEventArgs e) { this.RefreshServers(); }
        private void AuthWindowsRadioButton_OnChecked(object sender, RoutedEventArgs e) { this.UpdateUi(); }
        private void AuthUserPassRadioButton_OnChecked(object sender, RoutedEventArgs e) { this.UpdateUi(); }

        private void UpdateUi()
        {
            if (this.SelectDbGroupBox == null)
                return;
            this.SelectDbGroupBox.IsEnabled = (this.AuthWindowsRadioButton.IsChecked ?? false) || !this.UserNameTextBox.Text.IsNullOrEmpty();
            this.UserInfoPanel.IsEnabled = this.AuthUserPassRadioButton.IsChecked ?? false;
        }

        private void UserNameTextBoxText_OnChanged(object sender, TextChangedEventArgs e) { this.UpdateUi(); }

        private void DbsComboBoxDropDown_OnOpened(object sender, EventArgs e)
        {
            this.DoValidation(false);
            this.DbsComboBox.Items.Clear();
            var dbs = Database.GetDatabases(this.ParseConnectionString(false).ConnectionString).Select(db => db.Name).OrderBy(_ => _).ToArray();
            foreach (var db in dbs)
                this.DbsComboBox.Items.Add(db);
        }

        private void TestConnectionButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.DoValidation(true);
            try
            {
                using (var conn = new SqlConnection(this.ConnectionString))
                    conn.Open();
                MessageBox.Show("Connected Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message, "Connection Failure");
            }
        }

        private void Window_OnLoaded(object sender, RoutedEventArgs e) { this.UpdateUi(); }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.ValidateResult)
            {
                var ex = AdoHelper.CheckConnectionString(this.ConnectionString);
                if (ex != null)
                {
                    MessageBox.Show(ex.GetBaseException().Message, "Connection Failure");
                    return;
                }
            }
            this.DialogResult = true;
            this.Close();
        }
    }
}