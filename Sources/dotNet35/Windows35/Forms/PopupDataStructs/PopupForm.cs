#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:01 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Drawing;
using System.Windows.Forms;
using Library35.Helpers;
using Library35.Windows.Controls;

namespace Library35.Windows.Forms.PopupDataStructs
{
	internal partial class PopupForm : LibraryForm
	{
		private bool _IsMouseDown;
		private Point _MouseOffset;

		public PopupForm()
		{
			this.InitializeComponent();
			this.EnableDragAndDrop();
		}

		private void EnableDragAndDrop()
		{
			MouseEventHandler mouseDown = delegate(object sender, MouseEventArgs e)
			                              {
				                              if (e.Button != MouseButtons.Left)
					                              return;
				                              var xOffset = -e.X - SystemInformation.FrameBorderSize.Width - 1;
				                              var yOffset = -e.Y - SystemInformation.CaptionHeight - SystemInformation.FrameBorderSize.Height + SystemInformation.CaptionHeight - 1;
				                              this._MouseOffset = new Point(xOffset, yOffset);
				                              this._IsMouseDown = true;
				                              this.Opacity = 0.83;
			                              };
			MouseEventHandler mouseMove = delegate
			                              {
				                              if (!this._IsMouseDown)
					                              return;
				                              var mousePos = MousePosition;
				                              mousePos.Offset(this._MouseOffset.X, this._MouseOffset.Y);
				                              this.Location = mousePos;
			                              };
			MouseEventHandler mouseUp = delegate(object sender, MouseEventArgs e)
			                            {
				                            if (e.Button != MouseButtons.Left)
					                            return;
				                            this._IsMouseDown = false;
				                            this.Opacity = 1;
			                            };
			this.titlePanel.MouseDown += mouseDown;
			this.titlePanel.MouseMove += mouseMove;
			this.titlePanel.MouseUp += mouseUp;

			this.titleLabel.MouseDown += mouseDown;
			this.titleLabel.MouseMove += mouseMove;
			this.titleLabel.MouseUp += mouseUp;
		}

		public event EventHandler Hiding;

		private void closeButton_Click(object sender, EventArgs e)
		{
			this.Hiding.Raise(this);
			this.Hide();
		}
	}
}