#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:01 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Windows.Forms;
using Library35.Helpers;
using Library35.Helpers.Win;
using Library35.Windows.Forms.Internals;

namespace Library35.Windows.Forms
{
	public sealed class ProgressBox
	{
		private ProgressDialog ProgressDialog { get; set; }

		public string Status
		{
			get { return this.ProgressDialog.StatusLabel.Text; }
			set { this.ProgressDialog.RunInCurrentThread(() => this.ProgressDialog.StatusLabel.Text = value); }
		}

		public int ProgressValue
		{
			get { return this.ProgressDialog.progressBar.Value; }
			set { this.ProgressDialog.RunInCurrentThread(() => this.ProgressDialog.progressBar.Value = value); }
		}

		public int MaxProgressValue
		{
			get { return this.ProgressDialog.progressBar.Maximum; }
			set { this.ProgressDialog.RunInCurrentThread(() => this.ProgressDialog.progressBar.Maximum = value); }
		}

		public int MinProgressValue
		{
			get { return this.ProgressDialog.progressBar.Minimum; }
			set { this.ProgressDialog.RunInCurrentThread(() => this.ProgressDialog.progressBar.Minimum = value); }
		}

		public ProgressBarStyle Style
		{
			get { return this.ProgressDialog.progressBar.Style; }
			set { this.ProgressDialog.RunInCurrentThread(() => this.ProgressDialog.progressBar.Style = value); }
		}

		public bool TopMost
		{
			get { return this.ProgressDialog.TopMost; }
			set { this.ProgressDialog.RunInCurrentThread(() => this.ProgressDialog.TopMost = value); }
		}

		public void Show(Form parent)
		{
			this.Show(parent, string.Empty);
		}

		public void Show(Form parent, string status)
		{
			this.Show(parent, status, null);
		}

		public void Show(Form parent, string status, int? maxProgessValue)
		{
			this.Show(parent, status, null, maxProgessValue);
		}

		public void Show(Form parent, string status, int? minProgessValue, int? maxProgessValue)
		{
			this.Show(parent, status, minProgessValue, maxProgessValue, maxProgessValue.HasValue ? ProgressBarStyle.Blocks : ProgressBarStyle.Marquee);
		}

		public void Show(Form parent, string status, int? minProgessValue, int? maxProgessValue, ProgressBarStyle progressBarStyle)
		{
			if (this.ProgressDialog == null)
				this.ProgressDialog = new ProgressDialog();

			//this._ProgressDialog.Parent = parent;
			this.ProgressDialog.Left = (parent.Width / 2) - (this.ProgressDialog.Width / 2) + parent.Left;
			this.ProgressDialog.Top = (parent.Height / 2) - (this.ProgressDialog.Height / 2) + parent.Top;

			if (!status.IsNullOrEmpty())
				this.ProgressDialog.StatusLabel.Text = status;

			if (minProgessValue.HasValue)
				this.ProgressDialog.progressBar.Minimum = minProgessValue.Value;
			if (maxProgessValue.HasValue)
				this.ProgressDialog.progressBar.Maximum = maxProgessValue.Value;
			this.ProgressDialog.progressBar.Style = progressBarStyle;

			this.ProgressDialog.Show();

			this.ProgressDialog.Left = (parent.Width / 2) - (this.ProgressDialog.Width / 2) + parent.Left;
			this.ProgressDialog.Top = (parent.Height / 2) - (this.ProgressDialog.Height / 2) + parent.Top;
		}

		public void Close()
		{
			if (this.ProgressDialog == null)
				return;
			this.ProgressDialog.RunInCurrentThread(() =>
			                                       {
				                                       this.ProgressDialog.Close();
				                                       this.ProgressDialog.Dispose();
				                                       this.ProgressDialog = null;
			                                       });
		}
	}
}