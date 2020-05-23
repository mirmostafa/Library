#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Library40.Helpers;
using Library40.Win.Forms.PopupDataStructs;
using Library40.Win.Helpers;

namespace Library40.Win.Forms
{
	[DefaultProperty("Message")]
	[DefaultEvent("VisibilityChanged")]
	public class PopupBox : Component
	{
		private readonly PopupForm _Form;
		private readonly Timer _HideTimer = new Timer();
		private bool _IsShown;
		private PopupType _PopupType;

		public PopupBox()
		{
			this._Form = new PopupForm();
			this._Form.Shown += (sender, e) => this.IsShown = true;
			this._Form.Hiding += (sender, e) => this.IsShown = false;
			this.PopupType = PopupType.Information;
			this.AutoHide = true;
			this._HideTimer.Tick += (sender, e) => this.Hide();
		}

		public Size Size
		{
			get { return this._Form.Size; }
			set { this._Form.Size = value; }
		}

		[DefaultValue(RightToLeft.No)]
		public RightToLeft RighToLeft
		{
			get { return this._Form.RightToLeft; }
			set { this._Form.RightToLeft = value; }
		}

		[DefaultValue("")]
		public string Title
		{
			get { return this._Form.titleLabel.Text; }
			set { this._Form.titleLabel.Text = value; }
		}

		[DefaultValue("")]
		public string Message
		{
			get { return this._Form.messageLabel.Text; }
			set { this._Form.messageLabel.Text = value; }
		}

		public bool IsShown
		{
			get { return this._IsShown; }
			set
			{
				if (this._IsShown == value)
					return;
				this._IsShown = value;
				this.VisibilityChanged.Raise(this);
			}
		}

		[DefaultValue(PopupType.Information)]
		public PopupType PopupType
		{
			get { return this._PopupType; }
			set
			{
				if (this._PopupType == value)
					return;
				switch (value)
				{
					case PopupType.Information:
						this._Form.titlePanel.BackColor = Color.LightGreen;
						this._Form.titlePanel.BackColor2 = Color.DarkGreen;
						this._Form.messagePanel.BackColor2 = Color.LightGreen;
						this._Form.messagePanel.BackColor = Color.DarkGreen;
						break;
					case PopupType.Warning:
						this._Form.titlePanel.BackColor = Color.LightGoldenrodYellow;
						this._Form.titlePanel.BackColor2 = Color.DarkGoldenrod;
						this._Form.messagePanel.BackColor2 = Color.LightGoldenrodYellow;
						this._Form.messagePanel.BackColor = Color.DarkGoldenrod;
						break;
					case PopupType.Alert:
						this._Form.titlePanel.BackColor = Color.LightCoral;
						this._Form.titlePanel.BackColor2 = Color.DarkOrange;
						this._Form.messagePanel.BackColor2 = Color.LightCoral;
						this._Form.messagePanel.BackColor = Color.DarkOrange;
						break;
					case PopupType.RedAlert:
						this._Form.titlePanel.BackColor = Color.Maroon;
						this._Form.titlePanel.BackColor2 = Color.DarkRed;
						this._Form.messagePanel.BackColor2 = Color.Maroon;
						this._Form.messagePanel.BackColor = Color.DarkRed;
						break;
					default:
						throw new ArgumentOutOfRangeException("value");
				}
				this._PopupType = value;
			}
		}

		[DefaultValue(true)]
		public bool AutoHide { get; set; }

		public event EventHandler VisibilityChanged;

		protected override void Dispose(bool disposing)
		{
			this._Form.Close();
			base.Dispose(disposing);
		}

		public void Show()
		{
			if (this.IsShown)
				return;

			this._Form.Show();
			this._Form.FadeIn(TimeSpan.FromMilliseconds(350));
			this.IsShown = true;
			this._HideTimer.Interval = this.Message.Length * 500 + 1000;
			this._HideTimer.Start();
		}

		public void Hide()
		{
			if (!this.IsShown)
				return;

			this._Form.FadeOut(TimeSpan.FromMilliseconds(500));
			this._Form.Hide();
			this.IsShown = false;
			this._HideTimer.Stop();
		}
	}

	public enum PopupType
	{
		Information,
		Warning,
		Alert,
		RedAlert
	}

	internal enum ViewDirection
	{
		UpDown,
		DownUp,
		Center,
		Fade
	}
}