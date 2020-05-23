#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:06 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using Library40.Helpers;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Library40.Windows.Dialogs.Internals
{
	public abstract class MsgBoxEx
	{
		protected static TaskDialog GetTaskDialog(string instructionText = null,
			string text = null,
			string caption = null,
			TaskDialogStandardIcon icon = TaskDialogStandardIcon.None,
			TaskDialogStandardButtons buttons = TaskDialogStandardButtons.None,
			string detailsExpandedLabel = null,
			string detailsExpandedText = null,
			bool cancelable = true,
			string detailsCollapsedLabel = null,
			bool detailsExpanded = false,
			bool? footerCheckBoxChecked = null,
			string footerCheckBoxText = null,
			TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None,
			string footerText = null,
			bool hyperlinksEnabled = true,
			TaskDialogStartupLocation startupLocation = TaskDialogStartupLocation.CenterOwner,
			TaskDialogProgressBarState progressBarState = TaskDialogProgressBarState.None,
			int progressbarMinValue = 0,
			int progressbarMaxValue = 0,
			int? progressbarCurrValue = null,
			Action<TaskDialog> action = null,
			IntPtr hWnd = default(IntPtr),
			params TaskDialogControl[] controls)
		{
			var result = new TaskDialog
			             {
				             Icon = icon,
				             Caption = caption ?? ApplicationHelper.ProductTitle,
				             InstructionText = instructionText,
				             Text = text,
				             DetailsExpandedLabel = detailsExpandedLabel,
				             DetailsExpandedText = detailsExpandedText,
				             ExpansionMode = TaskDialogExpandedDetailsLocation.ExpandFooter,
				             Cancelable = cancelable,
				             DetailsCollapsedLabel = detailsCollapsedLabel,
				             DetailsExpanded = detailsExpanded,
				             FooterCheckBoxChecked = footerCheckBoxChecked,
				             FooterCheckBoxText = footerCheckBoxText,
				             FooterIcon = footerIcon,
				             FooterText = footerText,
				             HyperlinksEnabled = hyperlinksEnabled,
				             StartupLocation = startupLocation,
				             OwnerWindowHandle = hWnd,
			             };
			if (controls != null && controls.Length > 0)
				result.Controls.AddMany(controls);
			else
				result.StandardButtons = buttons;
			if ((progressBarState != TaskDialogProgressBarState.None) || (progressbarMaxValue > 0))
				result.ProgressBar = new TaskDialogProgressBar(progressbarMinValue, progressbarMaxValue, progressbarCurrValue ?? progressbarMinValue)
				                     {
					                     State = progressBarState == TaskDialogProgressBarState.None ? TaskDialogProgressBarState.Normal : progressBarState
				                     };

			result.Opened += (e1, e2) => MethodHelper.Catch(() =>
			                                                {
				                                                result.Icon = icon;
				                                                if (!result.FooterText.IsNullOrEmpty())
					                                                result.FooterIcon = footerIcon;
				                                                if (action != null)
					                                                action(result);
			                                                });

			return result;
		}
	}
}