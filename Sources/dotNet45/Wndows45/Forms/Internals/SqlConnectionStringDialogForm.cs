using System;
using System.Data.SqlClient;
using System.Linq;
using Mohammad.Data.SqlServer.Dynamics;
using Mohammad.Helpers;
using Mohammad.Threading;
using Mohammad.Validation.Exceptions;
using Mohammad.Win.Controls;
using Mohammad.Win.Forms.Validation;
using Mohammad.Win.Helpers;

namespace Mohammad.Win.Forms.Internals
{
    public partial class SqlConnectionStringDialogForm : LibraryForm
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
                this.serversComboBox.Text = scsb.DataSource;
                this.authWindowsRadioButton.Checked = scsb.IntegratedSecurity;
                this.userNameTextBox.Text = scsb.UserID;
                this.passwordTextBox.Text = scsb.Password;
                this.dbsComboBox.Text = scsb.InitialCatalog;
            }
        }

        public SqlConnectionStringDialogForm() { this.InitializeComponent(); }

        private SqlConnectionStringBuilder ParseConnectionString(bool full)
        {
            var scsb = new SqlConnectionStringBuilder();
            if (!this.serversComboBox.Text.IsNullOrEmpty())
                scsb.DataSource = this.serversComboBox.Text;
            scsb.IntegratedSecurity = this.authWindowsRadioButton.Checked;
            if (this.authUserPassRadioButton.Checked && !this.userNameTextBox.Text.IsNullOrEmpty())
                scsb.UserID = this.userNameTextBox.Text;
            if (this.authUserPassRadioButton.Checked && !this.passwordTextBox.Text.IsNullOrEmpty() && this.savePassCheckBox.Checked)
                scsb.Password = this.passwordTextBox.Text;
            if (full && !this.dbsComboBox.Text.IsNullOrEmpty())
                scsb.InitialCatalog = this.dbsComboBox.Text;
            return scsb;
        }

        private void DoValidation(bool full)
        {
            try
            {
                this.serversComboBox.AssertNotNull("Server name");
                if (this.authUserPassRadioButton.Checked)
                    this.userNameTextBox.AssertNotNull("User name");
                if (full)
                    this.dbsComboBox.AssertNotNull("Database name");
            }
            catch (NotNullOrZeroValidationException ex)
            {
                var message = string.Format("{0} cannot be empty.", ex.ParameterName);
                throw new Exception(message);
            }
        }

        private void serversComboBox_DropDown(object sender, EventArgs e)
        {
            if (this.serversComboBox.Items.Count == 0)
                this.RefreshServers();
        }

        private void RefreshServers()
        {
            if (!this.refreshButton.Enabled)
                return;
            string[] servers = null;
            var refreshServers = Async.GetAsyncInstance(() => { servers = Server.Servers.Select(server => server.Name).ToArray(); }, "Refreshing Servers");
            refreshServers.Ended += (sender, e) => this.RunInCurrentThread(() =>
            {
                this.serversComboBox.Items.Clear();
                this.serversComboBox.Items.AddRange(servers);
                this.refreshButton.Enabled = true;
            });
            refreshServers.Start();
            this.refreshButton.Enabled = false;
        }

        private void refreshButton_Click(object sender, EventArgs e) { this.RefreshServers(); }
        private void authWindowsRadioButton_CheckedChanged(object sender, EventArgs e) { this.UpdateUi(); }
        private void authUserPassRadioButton_CheckedChanged(object sender, EventArgs e) { this.UpdateUi(); }

        private void UpdateUi()
        {
            this.userInfoPanel.Enabled = this.authUserPassRadioButton.Checked;
            this.selectDbGroupBox.Enabled = this.authWindowsRadioButton.Checked || !this.userNameTextBox.Text.IsNullOrEmpty();
        }

        private void userNameTextBox_TextChanged(object sender, EventArgs e) { this.UpdateUi(); }

        private void dbsComboBox_DropDown(object sender, EventArgs e)
        {
            this.DoValidation(false);
            this.dbsComboBox.Items.Clear();
            this.dbsComboBox.Items.AddRange(
                Database.GetDatabases(this.ParseConnectionString(false).ConnectionString).Select(db => db.Name).OrderBy(db => db).ToArray());
        }

        private void testConnectionButton_Click(object sender, EventArgs e)
        {
            this.DoValidation(true);
            using (var conn = new SqlConnection(this.ConnectionString))
                conn.Open();
            MsgBox.Inform("Connected Successfully");
        }

        private void showPassCheckBox_CheckedChanged(object sender, EventArgs e) { this.passwordTextBox.UseSystemPasswordChar = !this.showPassCheckBox.Checked; }
    }
}