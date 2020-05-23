#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Threading.Tasks;
using System.Windows.Forms;
using Library40.Helpers;
using Library40.Win.Forms.Internals;

namespace Library40.Win.Forms
{
	public sealed class ProgressBox
	{
		private Task _DialogTask;
		private ProgressDialog ProgressDialog { get; set; }

		public string Status
		{
			get { return this.ProgressDialog.StatusLabel.Text; }
			set { this._DialogTask = this._DialogTask.ContinueWith(task => this.ProgressDialog.StatusLabel.Text = value); }
		}

		public int ProgressValue
		{
			get { return this.ProgressDialog.progressBar.Value; }
			set { this._DialogTask = this._DialogTask.ContinueWith(task => this.ProgressDialog.progressBar.Value = value); }
		}

		public int MaxProgressValue
		{
			get { return this.ProgressDialog.progressBar.Maximum; }
			set { this._DialogTask = this._DialogTask.ContinueWith(task => this.ProgressDialog.progressBar.Maximum = value); }
		}

		public int MinProgressValue
		{
			get { return this.ProgressDialog.progressBar.Minimum; }
			set { this._DialogTask = this._DialogTask.ContinueWith(task => this.ProgressDialog.progressBar.Minimum = value); }
		}

		public ProgressBarStyle Style
		{
			get { return this.ProgressDialog.progressBar.Style; }
			set { this._DialogTask = this._DialogTask.ContinueWith(task => this.ProgressDialog.progressBar.Style = value); }
		}

		public bool TopMost
		{
			get { return this.ProgressDialog.TopMost; }
			set { this._DialogTask = this._DialogTask.ContinueWith(task => this.ProgressDialog.TopMost = value); }
		}

		public void Show(Form parent, string status = null, int? minProgessValue = null, int? maxProgessValue = null, ProgressBarStyle progressBarStyle = ProgressBarStyle.Continuous)
		{
			if (this.ProgressDialog == null)
				this._DialogTask = Task.Factory.StartNew(() => this.ProgressDialog = new ProgressDialog());
			this._DialogTask = this._DialogTask.ContinueWith(task =>
			                                                 {
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

				                                                 this.ProgressDialog.Update();
				                                                 this.ProgressDialog.Refresh();
			                                                 });
		}

		public void Close()
		{
			if (this.ProgressDialog == null)
				return;
			this._DialogTask = this._DialogTask.ContinueWith(task =>
			                                                 {
				                                                 this.ProgressDialog.Close();
				                                                 this.ProgressDialog.Dispose();
				                                                 this.ProgressDialog = null;
			                                                 });
		}
	}
}