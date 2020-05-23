#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:01 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.CompilerServices;

namespace Library35.Windows.Forms
{
	public class MsgBoxExDialogButton
	{
		public MsgBoxExDialogButton(MsgBoxExDialogResult vDialogResult)
		{
			this.MsgBoxExDialogResult = vDialogResult;
		}

		public MsgBoxExDialogButton(MsgBoxExDialogResult vDialogResult, string text)
		{
			this.UseCustomText = true;
			this.Text = text;
			this.MsgBoxExDialogResult = vDialogResult;
		}

		public MsgBoxExDialogButton(string text, EventHandler click)
		{
			this.UseCustomText = true;
			this.Text = text;
			this.MsgBoxExDialogResult = MsgBoxExDialogResult.None;
			this.Click += click;
		}

		public string Text { get; set; }

		public bool UseCustomText { get; set; }

		public MsgBoxExDialogResult MsgBoxExDialogResult { get; set; }

		public event EventHandler Click;

		internal void RaiseClickEvent(object sender, EventArgs e)
		{
			var click = this.Click;
			if (click != null)
				click(RuntimeHelpers.GetObjectValue(sender), e);
		}
	}
}