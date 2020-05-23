#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:01 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Library35.Windows.Forms.Internals
{
	internal class MsgBoxExBackgroundForm : Form
	{
		#region Fields

		#region background
		private readonly Bitmap background;
		#endregion

		#endregion

		#region Methods

		#region OnShown
		protected override void OnShown(EventArgs e)
		{
			this.BackgroundImage = this.background;
			this.DoubleBuffered = true;
			base.OnShown(e);
		}
		#endregion

		#region MsgBoxExBackgroundForm
		public MsgBoxExBackgroundForm(Bitmap background)
		{
			this.BackColor = Color.Black;
			this.FormBorderStyle = FormBorderStyle.None;
			this.Location = Point.Empty;
			this.Size = Screen.PrimaryScreen.Bounds.Size;
			this.StartPosition = FormStartPosition.Manual;
			this.Visible = true;
			this.background = background;
		}
		#endregion

		#endregion
	}
}