#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:01 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Windows.Forms;
using Library35.Windows.Properties;

namespace Library35.Windows.Forms.Internals
{
	internal sealed class MsgBoxExExtensions
	{
		#region Methods
		public static MsgBoxExDialogDefaultButton MakeMsgBoxExDialogDefaultButton(MessageBoxDefaultButton defaultButton)
		{
			switch (defaultButton)
			{
				case MessageBoxDefaultButton.Button1:
					return MsgBoxExDialogDefaultButton.Button1;

				case MessageBoxDefaultButton.Button2:
					return MsgBoxExDialogDefaultButton.Button2;

				case MessageBoxDefaultButton.Button3:
					return MsgBoxExDialogDefaultButton.Button3;
			}
			return MsgBoxExDialogDefaultButton.None;
		}

		public static DialogResult MakeDialogResult(MsgBoxExDialogResult result)
		{
			switch (result)
			{
				case MsgBoxExDialogResult.OK:
				case MsgBoxExDialogResult.Continue:
					return DialogResult.OK;

				case MsgBoxExDialogResult.Cancel:
				case MsgBoxExDialogResult.Close:
					return DialogResult.Cancel;

				case MsgBoxExDialogResult.Yes:
				case MsgBoxExDialogResult.YesToAll:
					return DialogResult.Yes;

				case MsgBoxExDialogResult.No:
				case MsgBoxExDialogResult.NoToAll:
					return DialogResult.No;

				case MsgBoxExDialogResult.Abort:
					return DialogResult.Abort;

				case MsgBoxExDialogResult.Retry:
					return DialogResult.Retry;

				case MsgBoxExDialogResult.Ignore:
					return DialogResult.Ignore;
			}
			return DialogResult.None;
		}

		public static string GetButtonName(MsgBoxExDialogResult button)
		{
			switch (button)
			{
				case MsgBoxExDialogResult.OK:
					return Resources.OKText;

				case MsgBoxExDialogResult.Cancel:
					return Resources.CancelText;

				case MsgBoxExDialogResult.Close:
					return Resources.CloseText;

				case MsgBoxExDialogResult.Yes:
					return Resources.YesText;

				case MsgBoxExDialogResult.No:
					return Resources.NoText;

				case MsgBoxExDialogResult.YesToAll:
					return Resources.YesToAllText;

				case MsgBoxExDialogResult.NoToAll:
					return Resources.NoToAllText;

				case MsgBoxExDialogResult.Abort:
					return Resources.AbortText;

				case MsgBoxExDialogResult.Retry:
					return Resources.RetryText;

				case MsgBoxExDialogResult.Ignore:
					return Resources.IgnoreText;

				case MsgBoxExDialogResult.Continue:
					return Resources.ContinueText;
			}
			return Resources.NoneText;
		}
		#endregion
	}
}