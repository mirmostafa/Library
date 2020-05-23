#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Forms;
using Library40.Helpers;
using Library40.Win.Forms;

namespace Library40.Win.Controls
{
	[DefaultEvent("ConnectionStringChanged")]
	[DefaultProperty("ConnectionString")]
	public partial class ConnectionStringBox : UserControl
	{
		private string _ConnectionString;
		private bool _InternallyChanging;

		public ConnectionStringBox()
		{
			this.InitializeComponent();
		}

		public string ConnectionString
		{
			get { return this._ConnectionString; }
			set
			{
				if (value == this._ConnectionString)
					return;
				this._ConnectionString = value;
				this.textBox.Text = value;
				this.ReformatConnectionStringTextBox();
				this.OnConnectionStringChanged(EventArgs.Empty);
			}
		}

		public string DialogPrompt { get; set; }
		public string DialogText { get; set; }
		public event EventHandler ConnectionStringChanged;

		protected virtual void OnConnectionStringChanged(EventArgs e)
		{
			this.ConnectionStringChanged.Raise(this, e);
		}

		private void button_Click(object sender, EventArgs e)
		{
			var connectionString = this.ConnectionString;
			if (SqlConnectionStringBox.Show(ref connectionString, this.DialogPrompt, this.DialogText) == DialogResult.OK)
				this.ConnectionString = connectionString;
		}

		private void ReformatConnectionStringTextBox()
		{
			var scsb = new SqlConnectionStringBuilder(this.ConnectionString);
			if (!scsb.Password.IsNullOrEmpty())
				scsb.Password = "*****";
			this.textBox.Text = scsb.ConnectionString;
		}

		private void ConnectionStringBox_Validated(object sender, EventArgs e)
		{
			if (this._InternallyChanging)
				return;
			this._InternallyChanging = true;
			try
			{
				this.ReformatConnectionStringTextBox();
			}
			finally
			{
				this._InternallyChanging = false;
			}
		}

		private void textBox_TextChanged(object sender, EventArgs e)
		{
			if (this._InternallyChanging)
				return;
			this._InternallyChanging = true;
			try
			{
				this.ConnectionString = this.textBox.Text;
				this.ReformatConnectionStringTextBox();
			}
			finally
			{
				this._InternallyChanging = false;
			}
		}
	}
}