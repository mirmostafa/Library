#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Library40.Win.Controls
{
	public partial class FormTitleBarControl : UserControl
	{
		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HT_CAPTION = 0x2;

		public FormTitleBarControl()
		{
			this.InitializeComponent();
			if (this.ParentForm != null)
			{
				this.ParentForm.TextChanged += (sender, e) => this.Title = (sender as Form).Text;
				this.Title = this.ParentForm.Text;
			}
		}

		[Category("Appearance")]
		[Description("Gets or sets the font of the title")]
		public Font TitleFont
		{
			set { this.lblTitle.Font = value; }
			get { return this.lblTitle.Font; }
		}
		[Category("Appearance")]
		[Description("Gets or sets the title of the title bar")]
		public string Title
		{
			set { this.lblTitle.Text = value; }
			get { return this.lblTitle.Text; }
		}
		[Category("Appearance")]
		[Description("Gets or sets the title text color")]
		public Color TitleForeColor
		{
			set { this.lblTitle.ForeColor = value; }
			get { return this.lblTitle.ForeColor; }
		}
		[Category("Appearance")]
		[Description("Gets or sets the title background color")]
		public Color TitleBackColor
		{
			set { this.lblTitle.BackColor = value; }
			get { return this.lblTitle.BackColor; }
		}

		[Category("Appearance")]
		[Description("Gets or sets maximize button visibility")]
		public bool Maximize
		{
			set { this.frmControlBox.Maximize = value; }
			get { return this.frmControlBox.Maximize; }
		}
		[Category("Appearance")]
		[Description("Gets or sets minimize button visibility")]
		public bool Minimize
		{
			set { this.frmControlBox.Minimize = value; }
			get { return this.frmControlBox.Minimize; }
		}

		[Category("Appearance")]
		[Description("Gets or sets close button visibility")]
		public bool Close
		{
			set { this.frmControlBox.Close = value; }
			get { return this.frmControlBox.Close; }
		}

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();

		private void PbTitleMouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				if ((e.Clicks == 1) && (this.ParentForm.WindowState != FormWindowState.Maximized))
				{
					ReleaseCapture();
					SendMessage(this.ParentForm.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
				}
		}

		private void Caption_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				if ((e.Clicks == 1) && (this.ParentForm.WindowState != FormWindowState.Maximized))
				{
					ReleaseCapture();
					SendMessage(this.ParentForm.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
				}
		}

		private void PbTitleClick(object sender, EventArgs e)
		{
		}

		private void PbTitleDoubleClick(object sender, EventArgs e)
		{
		}

		private void LblTitleDoubleClick(object sender, EventArgs e)
		{
			if (this.frmControlBox.Maximize)
				if (this.ParentForm.WindowState == FormWindowState.Maximized)
				{
					this.ParentForm.WindowState = FormWindowState.Normal;
					this.ParentForm.Show();
				}
				else if (this.ParentForm.WindowState == FormWindowState.Normal)
				{
					this.ParentForm.WindowState = FormWindowState.Maximized;
					this.ParentForm.Show();
				}
		}

		private void LblTitleClick(object sender, EventArgs e)
		{
		}

		private void PbTitleMouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (this.frmControlBox.Maximize)
				if (this.ParentForm.WindowState == FormWindowState.Maximized)
				{
					this.ParentForm.WindowState = FormWindowState.Normal;
					this.ParentForm.Show();
				}
				else if (this.ParentForm.WindowState == FormWindowState.Normal)
				{
					this.ParentForm.WindowState = FormWindowState.Maximized;
					this.ParentForm.Show();
				}
		}

		private void LblTitleMouseDoubleClick(object sender, MouseEventArgs e)
		{
		}

		private void LblTitleMouseUp(object sender, MouseEventArgs e)
		{
		}
	}
}