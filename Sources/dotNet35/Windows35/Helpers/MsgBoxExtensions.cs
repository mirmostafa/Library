#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:01 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Library35.Windows.Forms;
using Library35.Windows.Helpers;
using Library35.Windows.Properties;

namespace Library35.Helpers
{
	public static class MsgBoxExtensions
	{
		public static void Show(this Exception ex,
			string friendlyText = "",
			string title = "",
			string instruction = "",
			string footertext = "",
			string verificationtext = "",
			string collapsedcontroltext = "",
			string expandedcontroltext = "",
			bool exitButton = false)
		{
			while (ex.InnerException != null)
				ex = ex.InnerException;
			var msgbox = new MsgBoxEx
			             {
				             Buttons = new[]
				                       {
					                       new MsgBoxExDialogButton(MsgBoxExDialogResult.OK)
				                       },
				             CollapsedControlText = collapsedcontroltext,
				             ExpandedControlText = expandedcontroltext,
				             WindowTitle = title ?? WindowsApplicationHelper.Title,
				             Content = friendlyText,
				             ExpandedInformation = ex.Message,
				             MainInstruction = instruction ?? Resources.MsgBoxExtensions_MainInstruction,
				             MainIcon = MsgBoxExDialogIcon.Error,
				             VerificationText = verificationtext,
			             };
			if (!footertext.IsNullOrEmpty())
			{
				msgbox.FooterText = footertext;
				msgbox.FooterIcon = MsgBoxExDialogIcon.Information;
			}
			if (exitButton)
			{
				var b = new Collection<MsgBoxExDialogButton>
				        {
					        new MsgBoxExDialogButton(MsgBoxExDialogResult.Close)
				        };
				b.AddMany(msgbox.Buttons);
				msgbox.Buttons = b.ToArray();
			}

			msgbox.Show();
		}
	}
}