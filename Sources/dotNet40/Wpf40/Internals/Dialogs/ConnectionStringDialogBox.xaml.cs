#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Library40.Data.SqlServer;
using Library40.Helpers;
using Library40.Threading;

namespace Library40.Wpf.Internals.Dialogs
{
	/// <summary>
	///     Interaction logic for ConnectionStringDialog.xaml
	/// </summary>
	internal partial class ConnectionStringDialog
	{
		public ConnectionStringDialog()
		{
			this.InitializeComponent();
		}

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
				this.authWindowsRadioButton.IsChecked = scsb.IntegratedSecurity;
				this.userNameTextBox.Text = scsb.UserID;
				this.passwordTextBox.Password = scsb.Password;
				this.dbsComboBox.Text = scsb.InitialCatalog;
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

		private SqlConnectionStringBuilder ParseConnectionString(bool full)
		{
			var scsb = new SqlConnectionStringBuilder();
			if (!this.serversComboBox.Text.IsNullOrEmpty())
				scsb.DataSource = this.serversComboBox.Text;
			scsb.IntegratedSecurity = this.authWindowsRadioButton.IsChecked ?? false;
			if ((this.authUserPassRadioButton.IsChecked ?? false) && !this.userNameTextBox.Text.IsNullOrEmpty())
				scsb.UserID = this.userNameTextBox.Text;
			if ((this.authUserPassRadioButton.IsChecked ?? false) && !this.passwordTextBox.Password.IsNullOrEmpty() && (this.savePassCheckBox.IsChecked ?? false))
				scsb.Password = this.passwordTextBox.Password;
			if (full && !this.dbsComboBox.Text.IsNullOrEmpty())
				scsb.InitialCatalog = this.dbsComboBox.Text;
			return scsb;
		}

		private void DoValidation(bool full)
		{
		}

		private void serversComboBox_DropDownOpened(object sender, EventArgs e)
		{
			if (this.serversComboBox.Items.Count == 0)
				this.RefreshServers();
		}

		private void RefreshServers()
		{
			if (!this.refreshButton.IsEnabled)
				return;
			String[] servers = null;
			var refreshServers = Async.GetAsyncInstance(() => { servers = Server.Servers.Select(server => server.Name).ToArray(); }, "Refreshing Servers");
			refreshServers.Ended += (sender, e) => this.Dispatcher.Invoke(new Action(() =>
			                                                                         {
				                                                                         this.serversComboBox.Items.Clear();
				                                                                         foreach (var server in servers)
					                                                                         this.serversComboBox.Items.Add(server);

				                                                                         this.refreshButton.IsEnabled = true;
			                                                                         }));
			refreshServers.Start();
			this.refreshButton.IsEnabled = false;
		}

		private void refreshButton_Click(object sender, RoutedEventArgs e)
		{
			this.RefreshServers();
		}

		private void authWindowsRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			this.UpdateUi();
		}

		private void authUserPassRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			this.UpdateUi();
		}

		private void UpdateUi()
		{
			if (this.selectDbGroupBox == null)
				return;
			this.selectDbGroupBox.IsEnabled = (this.authWindowsRadioButton.IsChecked ?? false) || !this.userNameTextBox.Text.IsNullOrEmpty();
			this.userInfoPanel.IsEnabled = this.authUserPassRadioButton.IsChecked ?? false;
		}

		private void userNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.UpdateUi();
		}

		private void dbsComboBox_DropDownOpened(object sender, EventArgs e)
		{
			this.DoValidation(false);
			this.dbsComboBox.Items.Clear();
			var dbs = Database.GetDatabases(this.ParseConnectionString(false).ConnectionString).Select(db => db.Name).OrderBy(db => db).ToArray();
			foreach (var db in dbs)
				this.dbsComboBox.Items.Add(db);
		}

		private void testConnectionButton_Click(object sender, RoutedEventArgs e)
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

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.UpdateUi();
		}

		private void okButton_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
			this.Close();
		}
	}
}