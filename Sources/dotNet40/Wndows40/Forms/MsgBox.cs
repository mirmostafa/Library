#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Windows.Forms;
using Library40.Helpers;
using Library40.Win.Helpers;

namespace Library40.Win.Forms
{
	/// <summary>
	///     Uses System.Windows.Forms.MessageBox.Show internally but much more flexible and simpler to use.
	/// </summary>
	public static class MsgBox
	{
		public static DialogResult Show(string text,
			string caption = null,
			MessageBoxButtons buttons = MessageBoxButtons.OK,
			MessageBoxIcon icons = MessageBoxIcon.Information,
			Control owner = null,
			MessageBoxOptions options = MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign)
		{
			ArgHelper.AssertNotNull(text, "text");
			if (string.IsNullOrEmpty(caption))
				caption = WindowsApplicationHelper.Title;
			if (owner == null || !owner.InvokeRequired)
				return MessageBox.Show(text, caption, buttons, icons, MessageBoxDefaultButton.Button1, options);
			owner.Invoke(new Func<string, string, MessageBoxButtons, MessageBoxIcon, Control, MessageBoxOptions, DialogResult>(Show), text, buttons, caption, icons, owner);
			return DialogResult.None;
		}

		public static DialogResult Error(string text, string caption = null, Control owner = null)
		{
			return Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error, owner);
		}

		public static DialogResult Ask(string text, string caption = "Confirmation")
		{
			return Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
		}

		public static DialogResult AskWithWarning(string text, string caption = "Confirmation")
		{
			return Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
		}

		public static DialogResult AskWithCancel(string text, string caption = "Confirmation")
		{
			return Show(text, caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
		}

		public static DialogResult Inform(string text, string caption = "Information", Control owner = null)
		{
			return Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information, owner);
		}

		public static DialogResult Warn(string text, string caption = null, Control owner = null)
		{
			return Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning, owner);
		}
	}
}