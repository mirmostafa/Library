#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:01 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Library35.Windows.Forms.Internals;

namespace Library35.Windows.Forms
{
	/// <summary>
	/// </summary>
	public partial class MsgBoxEx
	{
		private static RightToLeft _DefaultRightToLeft = RightToLeft.Inherit;
		public static RightToLeft DefaultRightToLeft
		{
			get { return _DefaultRightToLeft; }
			set { _DefaultRightToLeft = value; }
		}

		/// <summary>
		///     Gets or sets a value indicating whether [debug mode].
		/// </summary>
		/// <value>
		///     <c>true</c> if [debug mode]; otherwise, <c>false</c>.
		/// </value>
		public static bool DebugMode { get; set; }

		#region Extended Show
		public static MsgBoxExDialogResult Show(string content = null,
			string caption = null,
			IEnumerable<MsgBoxExDialogButton> buttons = null,
			string collapsedControlText = null,
			Control customControl = null,
			Image customFooterIcon = null,
			Image customMainIcon = null,
			MsgBoxExDialogDefaultButton defaultButton = MsgBoxExDialogDefaultButton.Button1,
			bool expandedByDefault = false,
			string expandedControlText = null,
			string expandedInformation = null,
			bool expandFooterArea = false,
			MsgBoxExDialogIcon footerIcon = MsgBoxExDialogIcon.None,
			string footerText = null,
			bool lockSystem = false,
			MsgBoxExDialogIcon mainIcon = MsgBoxExDialogIcon.None,
			string mainInstruction = null,
			IWin32Window owner = null,
			RightToLeft rightToLeft = RightToLeft.Inherit,
			bool rightToLeftLayout = false,
			CheckState verificationFlagChecked = CheckState.Unchecked,
			string verificationText = null,
			ISound sound = null,
			string windowTitle = null)
		{
			var msgBoxEx = new MsgBoxEx
			               {
				               Buttons = (buttons ?? new[]
				                                     {
					                                     new MsgBoxExDialogButton(MsgBoxExDialogResult.OK)
				                                     }).ToArray(),
				               CollapsedControlText = collapsedControlText,
				               Content = content,
				               CustomControl = customControl,
				               CustomFooterIcon = customFooterIcon,
				               CustomMainIcon = customMainIcon,
				               DefaultButton = defaultButton,
				               ExpandedByDefault = expandedByDefault,
				               ExpandedControlText = expandedControlText,
				               ExpandedInformation = expandedInformation,
				               ExpandFooterArea = expandFooterArea,
				               FooterIcon = footerIcon,
				               FooterText = footerText,
				               LockSystem = lockSystem,
				               MainIcon = mainIcon,
				               MainInstruction = mainInstruction,
				               Owner = owner ?? _NullWindow,
				               RightToLeft = rightToLeft == RightToLeft.Inherit ? DefaultRightToLeft : rightToLeft,
				               RightToLeftLayout = rightToLeftLayout,
				               VerificationFlagChecked = verificationFlagChecked,
				               VerificationText = verificationText,
				               Sound = sound,
				               WindowTitle = windowTitle,
			               };

			return msgBoxEx.Show();
		}
		#endregion

		#region Standard Show
		public static DialogResult Show(string text = null,
			string caption = null,
			MessageBoxButtons buttons = MessageBoxButtons.OK,
			MessageBoxIcon icon = MessageBoxIcon.None,
			MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1,
			IWin32Window owner = null)
		{
			if (owner == null)
				owner = _NullWindow;

			var dialog = new MsgBoxEx
			             {
				             Content = text,
				             WindowTitle = (caption ?? Application.ProductName),
			             };
			switch (buttons)
			{
				case MessageBoxButtons.OKCancel:
					dialog.Buttons = new[]
					                 {
						                 new MsgBoxExDialogButton(MsgBoxExDialogResult.OK), new MsgBoxExDialogButton(MsgBoxExDialogResult.Cancel)
					                 };
					break;

				case MessageBoxButtons.AbortRetryIgnore:
					dialog.Buttons = new[]
					                 {
						                 new MsgBoxExDialogButton(MsgBoxExDialogResult.Abort), new MsgBoxExDialogButton(MsgBoxExDialogResult.Retry),
						                 new MsgBoxExDialogButton(MsgBoxExDialogResult.Ignore)
					                 };
					break;

				case MessageBoxButtons.YesNoCancel:
					dialog.Buttons = new[]
					                 {
						                 new MsgBoxExDialogButton(MsgBoxExDialogResult.Yes), new MsgBoxExDialogButton(MsgBoxExDialogResult.No),
						                 new MsgBoxExDialogButton(MsgBoxExDialogResult.Cancel)
					                 };
					break;

				case MessageBoxButtons.YesNo:
					dialog.Buttons = new[]
					                 {
						                 new MsgBoxExDialogButton(MsgBoxExDialogResult.Yes), new MsgBoxExDialogButton(MsgBoxExDialogResult.No)
					                 };
					break;

				case MessageBoxButtons.RetryCancel:
					dialog.Buttons = new[]
					                 {
						                 new MsgBoxExDialogButton(MsgBoxExDialogResult.Retry), new MsgBoxExDialogButton(MsgBoxExDialogResult.Cancel)
					                 };
					break;

				default:
					dialog.Buttons = new[]
					                 {
						                 new MsgBoxExDialogButton(MsgBoxExDialogResult.OK)
					                 };
					break;
			}
			switch (icon)
			{
				case MessageBoxIcon.Hand:
					dialog.MainIcon = MsgBoxExDialogIcon.Error;
					break;

				case MessageBoxIcon.Question:
					dialog.MainIcon = MsgBoxExDialogIcon.Question;
					break;

				case MessageBoxIcon.Exclamation:
					dialog.MainIcon = MsgBoxExDialogIcon.Warning;
					break;

				case MessageBoxIcon.Asterisk:
					dialog.MainIcon = MsgBoxExDialogIcon.Information;
					break;

				default:
					dialog.CustomMainIcon = null;
					break;
			}
			dialog.DefaultButton = MsgBoxExExtensions.MakeMsgBoxExDialogDefaultButton(defaultButton);
			dialog.Owner = owner;
			dialog.Show();
			return MsgBoxExExtensions.MakeDialogResult(dialog.Result);
		}
		#endregion

		#region Core Ask
		public static MsgBoxExDialogResult Ask(string content = null,
			string caption = null,
			IEnumerable<MsgBoxExDialogButton> buttons = null,
			string collapsedControlText = null,
			Control customControl = null,
			Image customFooterIcon = null,
			Image customMainIcon = null,
			MsgBoxExDialogDefaultButton defaultButton = MsgBoxExDialogDefaultButton.Button1,
			bool expandedByDefault = false,
			string expandedControlText = null,
			string expandedInformation = null,
			bool expandFooterArea = false,
			MsgBoxExDialogIcon footerIcon = MsgBoxExDialogIcon.None,
			string footerText = null,
			bool lockSystem = false,
			MsgBoxExDialogIcon mainIcon = MsgBoxExDialogIcon.Question,
			string mainInstruction = null,
			IWin32Window owner = null,
			RightToLeft rightToLeft = RightToLeft.Inherit,
			bool rightToLeftLayout = false,
			CheckState verificationFlagChecked = CheckState.Unchecked,
			string verificationText = null,
			ISound sound = null,
			string windowTitle = null)
		{
			return Show(content,
				caption,
				buttons ?? new[]
				           {
					           new MsgBoxExDialogButton(MsgBoxExDialogResult.Yes), new MsgBoxExDialogButton(MsgBoxExDialogResult.No)
				           },
				collapsedControlText,
				customControl,
				customFooterIcon,
				customMainIcon,
				defaultButton,
				expandedByDefault,
				expandedControlText,
				expandedInformation,
				expandFooterArea,
				footerIcon,
				footerText,
				lockSystem,
				mainIcon,
				mainInstruction,
				owner,
				rightToLeft,
				rightToLeftLayout,
				verificationFlagChecked,
				verificationText,
				sound,
				windowTitle);
		}
		#endregion

		public static MsgBoxExDialogResult AskWithWaring(string content = null,
			string caption = null,
			IEnumerable<MsgBoxExDialogButton> buttons = null,
			string collapsedControlText = null,
			Control customControl = null,
			Image customFooterIcon = null,
			Image customMainIcon = null,
			MsgBoxExDialogDefaultButton defaultButton = MsgBoxExDialogDefaultButton.Button1,
			bool expandedByDefault = false,
			string expandedControlText = null,
			string expandedInformation = null,
			bool expandFooterArea = false,
			MsgBoxExDialogIcon footerIcon = MsgBoxExDialogIcon.None,
			string footerText = null,
			bool lockSystem = false,
			MsgBoxExDialogIcon mainIcon = MsgBoxExDialogIcon.Warning,
			string mainInstruction = null,
			IWin32Window owner = null,
			RightToLeft rightToLeft = RightToLeft.Inherit,
			bool rightToLeftLayout = false,
			CheckState verificationFlagChecked = CheckState.Unchecked,
			string verificationText = null,
			ISound sound = null,
			string windowTitle = null)
		{
			return Show(content,
				caption,
				buttons ?? new[]
				           {
					           new MsgBoxExDialogButton(MsgBoxExDialogResult.Yes), new MsgBoxExDialogButton(MsgBoxExDialogResult.No)
				           },
				collapsedControlText,
				customControl,
				customFooterIcon,
				customMainIcon,
				defaultButton,
				expandedByDefault,
				expandedControlText,
				expandedInformation,
				expandFooterArea,
				footerIcon,
				footerText,
				lockSystem,
				mainIcon,
				mainInstruction,
				owner,
				rightToLeft,
				rightToLeftLayout,
				verificationFlagChecked,
				verificationText,
				sound,
				windowTitle);
		}

		public static MsgBoxExDialogResult Inform(string content = null,
			string caption = null,
			IEnumerable<MsgBoxExDialogButton> buttons = null,
			string collapsedControlText = null,
			Control customControl = null,
			Image customFooterIcon = null,
			Image customMainIcon = null,
			MsgBoxExDialogDefaultButton defaultButton = MsgBoxExDialogDefaultButton.Button1,
			bool expandedByDefault = false,
			string expandedControlText = null,
			string expandedInformation = null,
			bool expandFooterArea = false,
			MsgBoxExDialogIcon footerIcon = MsgBoxExDialogIcon.None,
			string footerText = null,
			bool lockSystem = false,
			MsgBoxExDialogIcon mainIcon = MsgBoxExDialogIcon.Information,
			string mainInstruction = null,
			IWin32Window owner = null,
			RightToLeft rightToLeft = RightToLeft.Inherit,
			bool rightToLeftLayout = false,
			CheckState verificationFlagChecked = CheckState.Unchecked,
			string verificationText = null,
			ISound sound = null,
			string windowTitle = null)
		{
			return Show(content,
				caption,
				buttons ?? new[]
				           {
					           new MsgBoxExDialogButton(MsgBoxExDialogResult.OK)
				           },
				collapsedControlText,
				customControl,
				customFooterIcon,
				customMainIcon,
				defaultButton,
				expandedByDefault,
				expandedControlText,
				expandedInformation,
				expandFooterArea,
				footerIcon,
				footerText,
				lockSystem,
				mainIcon,
				mainInstruction,
				owner,
				rightToLeft,
				rightToLeftLayout,
				verificationFlagChecked,
				verificationText,
				sound,
				windowTitle);
		}

		public static MsgBoxExDialogResult Warn(string content = null,
			string caption = null,
			IEnumerable<MsgBoxExDialogButton> buttons = null,
			string collapsedControlText = null,
			Control customControl = null,
			Image customFooterIcon = null,
			Image customMainIcon = null,
			MsgBoxExDialogDefaultButton defaultButton = MsgBoxExDialogDefaultButton.Button1,
			bool expandedByDefault = false,
			string expandedControlText = null,
			string expandedInformation = null,
			bool expandFooterArea = false,
			MsgBoxExDialogIcon footerIcon = MsgBoxExDialogIcon.None,
			string footerText = null,
			bool lockSystem = false,
			MsgBoxExDialogIcon mainIcon = MsgBoxExDialogIcon.Warning,
			string mainInstruction = null,
			IWin32Window owner = null,
			RightToLeft rightToLeft = RightToLeft.Inherit,
			bool rightToLeftLayout = false,
			CheckState verificationFlagChecked = CheckState.Unchecked,
			string verificationText = null,
			ISound sound = null,
			string windowTitle = null)
		{
			return Show(content,
				caption,
				buttons ?? new[]
				           {
					           new MsgBoxExDialogButton(MsgBoxExDialogResult.OK)
				           },
				collapsedControlText,
				customControl,
				customFooterIcon,
				customMainIcon,
				defaultButton,
				expandedByDefault,
				expandedControlText,
				expandedInformation,
				expandFooterArea,
				footerIcon,
				footerText,
				lockSystem,
				mainIcon,
				mainInstruction,
				owner,
				rightToLeft,
				rightToLeftLayout,
				verificationFlagChecked,
				verificationText,
				sound,
				windowTitle);
		}

		public static MsgBoxExDialogResult Error(string content = null,
			string caption = null,
			IEnumerable<MsgBoxExDialogButton> buttons = null,
			string collapsedControlText = null,
			Control customControl = null,
			Image customFooterIcon = null,
			Image customMainIcon = null,
			MsgBoxExDialogDefaultButton defaultButton = MsgBoxExDialogDefaultButton.Button1,
			bool expandedByDefault = false,
			string expandedControlText = null,
			string expandedInformation = null,
			bool expandFooterArea = false,
			MsgBoxExDialogIcon footerIcon = MsgBoxExDialogIcon.None,
			string footerText = null,
			bool lockSystem = false,
			MsgBoxExDialogIcon mainIcon = MsgBoxExDialogIcon.Error,
			string mainInstruction = null,
			IWin32Window owner = null,
			RightToLeft rightToLeft = RightToLeft.Inherit,
			bool rightToLeftLayout = false,
			CheckState verificationFlagChecked = CheckState.Unchecked,
			string verificationText = null,
			ISound sound = null,
			string windowTitle = null)
		{
			return Show(content,
				caption,
				buttons ?? new[]
				           {
					           new MsgBoxExDialogButton(MsgBoxExDialogResult.OK)
				           },
				collapsedControlText,
				customControl,
				customFooterIcon,
				customMainIcon,
				defaultButton,
				expandedByDefault,
				expandedControlText,
				expandedInformation,
				expandFooterArea,
				footerIcon,
				footerText,
				lockSystem,
				mainIcon,
				mainInstruction,
				owner,
				rightToLeft,
				rightToLeftLayout,
				verificationFlagChecked,
				verificationText,
				sound,
				windowTitle);
		}

		/// <summary>
		///     Shows the content is [DebugMode] is true
		/// </summary>
		/// <param name="content">The content.</param>
		public static void Debug(string content)
		{
			//MethodHelper.ExecOnDebugger(() => Inform(content));
			if (DebugMode)
				Inform(content);
		}
	}
}