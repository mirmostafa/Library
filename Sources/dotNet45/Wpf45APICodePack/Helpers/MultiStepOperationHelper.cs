using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Taskbar;
using Mohammad.Helpers;
using Mohammad.ProgressiveOperations;
using Mohammad.Wpf.Windows;
using Mohammad.Wpf.Windows.Dialogs;
using TaskbarProgressBarState = Mohammad.Wpf.Windows.TaskbarProgressBarState;

namespace Mohammad.Wpf.Helpers
{
    public static class MultiStepOperationHelper
    {
        public static void ShowProgress(this MultiStepOperation operation, Window owner = null, TaskDialog taskDialog = null, string instructionText = null,
            bool cancelable = true, string cancelButtonText = "Cancel", string donePrompt = null, string exceptionPrompt = null, Windows7Tools windows7 = null)
        {
            var owned = false;
            TaskDialogButton cancelButton = null;

            operation.MainOperationStarted += (_, __) =>
            {
                if (taskDialog != null)
                    return;
                owned = true;
                cancelButton = new TaskDialogButton("CancelButton", cancelButtonText);
                taskDialog = MsgBoxEx.GetTaskDialog(window: owner,
                    instructionText: instructionText,
                    progressbarMaxValue: operation.MainStepsCount.ToInt(),
                    cancelable: cancelable,
                    detailsExpandedText: "No more info",
                    controls: cancelButton);
                var dialog = taskDialog;
                cancelButton.Click += delegate
                {
                    operation.Cancel();
                    dialog.Close();
                    if (windows7 != null)
                        windows7.Taskbar.ProgressBar.State = TaskbarProgressBarState.NoProgress;
                };
                var dialog1 = taskDialog;
                taskDialog.Opened += delegate
                {
                    dialog1.Icon = TaskDialogStandardIcon.Information;
                    if (owned && !cancelable)
                        cancelButton.Enabled = false;
                };
                taskDialog.Show();
                taskDialog = null;
            };
            operation.MainOperationStepIncreasing += (sender, e) =>
            {
                if (windows7 != null)
                    TaskbarManager.Instance.SetProgressValue(e.Step.ToInt(), e.Max.ToInt() - 1);

                if (taskDialog == null)
                    return;
                taskDialog.ProgressBar.Value = e.Step.ToInt();
                CodeHelper.Catch(() => taskDialog.Text = (e.Log ?? string.Empty).ToString());
            };
            operation.MainOperationEnded += (sender, args) =>
            {
                if (donePrompt.IsNullOrEmpty() && owned)
                {
                    taskDialog?.Close();
                    if (windows7 != null)
                        windows7.Taskbar.ProgressBar.State = TaskbarProgressBarState.NoProgress;
                }
                else
                {
                    if (args.IsSucceed)
                        if (taskDialog != null)
                            taskDialog.Text = donePrompt;
                    if (!owned)
                        return;
                    cancelButton.Text = "OK";
                    cancelButton.Enabled = true;
                }
            };
            operation.CurrentOperationStepIncreasing += (_, e) =>
            {
                if (e.Log == null)
                    return;
                if (taskDialog != null)
                    taskDialog.DetailsExpandedText = e.Log.ToString();
            };
            operation.ExceptionHandling.ExceptionOccurred += (sender, args) =>
            {
                if (exceptionPrompt.IsNullOrEmpty() && owned)
                {
                    taskDialog?.Close();
                    if (windows7 != null)
                        windows7.Taskbar.ProgressBar.State = TaskbarProgressBarState.Error;
                }
                else
                {
                    if (taskDialog != null)
                    {
                        taskDialog.Text = exceptionPrompt;
                        taskDialog.DetailsExpandedText = args.Exception.GetBaseException().Message;
                        taskDialog.ProgressBar.State = TaskDialogProgressBarState.Error;
                    }
                    if (windows7 != null)
                        windows7.Taskbar.ProgressBar.State = TaskbarProgressBarState.Error;
                }
            };
            operation.Start();
        }
    }
}