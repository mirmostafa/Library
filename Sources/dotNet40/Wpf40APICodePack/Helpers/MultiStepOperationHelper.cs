#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:06 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Windows;
using Library40.Bcl.MutistepOperations;
using Library40.Wpf.Windows.Dialogs;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace Library40.Helpers
{
	public static class MultiStepOperationHelper
	{
		public static void ShowProgress(this MultiStepOperation operation,
			Window owner = null,
			TaskDialog taskDialog = null,
			string instructionText = null,
			bool cancelable = true,
			string cancelButtonText = "Cancel",
			string donePrompt = null,
			string exceptionPrompt = null,
			bool updateTaskbar = true)
		{
			var owned = false;
			TaskDialogButton cancelButton = null;
			if (taskDialog == null)
			{
				owned = true;
				cancelButton = new TaskDialogButton("CancelButton", cancelButtonText);
				taskDialog = MsgBoxEx.GetTaskDialog(window: owner,
					instructionText: instructionText,
					progressbarMaxValue: operation.StepsCount - 1,
					cancelable: cancelable,
					controls: cancelButton);
				cancelButton.Click += (sender, args) =>
				                      {
					                      taskDialog.Close();
					                      if (updateTaskbar)
						                      TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
				                      };
			}
			operation.ProgressIncreasing += (sender, e) => taskDialog.Text = e.Description;
			operation.ProgressIncreased += (sender, e) =>
			                               {
				                               if (updateTaskbar)
					                               TaskbarManager.Instance.SetProgressValue(e.Step, e.Max - 1);
				                               taskDialog.ProgressBar.Value = e.Step;
			                               };
			operation.Ended += (sender, args) =>
			                   {
				                   if (donePrompt.IsNullOrEmpty() && owned)
				                   {
					                   taskDialog.Close();
					                   if (updateTaskbar)
						                   TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
				                   }
				                   else
				                   {
					                   if (args.Succeed)
						                   taskDialog.Text = donePrompt;
					                   if (owned)
					                   {
						                   cancelButton.Text = "OK";
						                   cancelButton.Enabled = true;
					                   }
				                   }
			                   };
			operation.ExceptionHandling.ExceptionOccurred += (sender, args) =>
			                                                 {
				                                                 if (exceptionPrompt.IsNullOrEmpty() && owned)
				                                                 {
					                                                 taskDialog.Close();
					                                                 if (updateTaskbar)
						                                                 TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Error);
				                                                 }
				                                                 else
				                                                 {
					                                                 taskDialog.Text = exceptionPrompt;
					                                                 taskDialog.DetailsExpandedText = args.Exception.GetBaseException().Message;
					                                                 taskDialog.ProgressBar.State = TaskDialogProgressBarState.Error;
					                                                 if (updateTaskbar)
						                                                 TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Error);
				                                                 }
			                                                 };
			taskDialog.Opened += delegate
			                     {
				                     if (owned && !cancelable)
					                     cancelButton.Enabled = false;
				                     operation.Start();
			                     };
			taskDialog.Show();
		}
	}
}