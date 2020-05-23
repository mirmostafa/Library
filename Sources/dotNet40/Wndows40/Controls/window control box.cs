#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.ComponentModel;
using System.Windows.Forms;
using Library40.Win.Properties;

namespace Library40.Win.Controls
{
	public partial class FormControlBox : UserControl
	{
		public FormControlBox()
		{
			this.InitializeComponent();
		}

		[Category("Appearance")]
		[Description("Gets or sets maximize button visibility")]
		public bool Maximize
		{
			set { this.lblMaximize.Visible = value; }
			get { return this.lblMaximize.Visible; }
		}
		[Category("Appearance")]
		[Description("Gets or sets minimize button visibility")]
		public bool Minimize
		{
			set { this.lblMinimize.Visible = value; }
			get { return this.lblMinimize.Visible; }
		}

		[Category("Appearance")]
		[Description("Gets or sets close button visibility")]
		public bool Close
		{
			set { this.lblClose.Visible = value; }
			get { return this.lblClose.Visible; }
		}

		private void lblClose_MouseMove(object sender, MouseEventArgs e)
		{
			this.lblClose.Image = Resources.close_sele;
		}

		private void lblClose_MouseLeave(object sender, EventArgs e)
		{
			this.lblClose.Image = Resources.close;
		}

		private void lblMaximize_MouseLeave(object sender, EventArgs e)
		{
			this.lblMaximize.Image = Resources.maximize;
		}

		private void lblMaximize_MouseMove(object sender, MouseEventArgs e)
		{
			this.lblMaximize.Image = Resources.maximize_sele;
		}

		private void lblMinimize_MouseMove(object sender, MouseEventArgs e)
		{
			this.lblMinimize.Image = Resources.minimize_sele;
		}

		private void lblMinimize_MouseLeave(object sender, EventArgs e)
		{
			this.lblMinimize.Image = Resources.minimize;
		}

		private void lblClose_Click(object sender, EventArgs e)
		{
			this.ParentForm.Close();
		}

		private void lblMaximize_Click(object sender, EventArgs e)
		{
			if (this.ParentForm.WindowState == FormWindowState.Maximized)
				this.ParentForm.WindowState = FormWindowState.Normal;
			else if (this.ParentForm.WindowState == FormWindowState.Normal)
				this.ParentForm.WindowState = FormWindowState.Maximized;
			this.ParentForm.Show();
		}

		private void lblMinimize_Click(object sender, EventArgs e)
		{
			this.ParentForm.WindowState = FormWindowState.Minimized;
			this.ParentForm.Show();
		}

		private void FormControlBox_Load(object sender, EventArgs e)
		{
		}
	}
}