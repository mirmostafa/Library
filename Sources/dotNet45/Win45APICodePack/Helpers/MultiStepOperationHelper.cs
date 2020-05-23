using System.Windows.Forms;
using Library45.Win.Dialogs;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Taskbar;
using Mohammad.Helpers;
using Mohammad.ProgressiveOperations;

namespace Library45.Win.Helpers
{
    public static class MultiStepOperationHelper
    {
        public static void ShowProgress(this MultiStepOperation operation, Form owner = null, TaskDialog taskDialog = null, string instructionText = null,
            bool cancelable = true, string cancelButtonText = "Cancel", string donePrompt = null, string exceptionPrompt = null, bool updateTaskbar = true)
        {
            var owned = false;
            TaskDialogButton cancelButton = null;

            operation.MainOperationStarted += (_, __) =>
            {
                if (taskDialog == null)
                {
                    owned = true;
                    cancelButton = new TaskDialogButton("CancelButton", cancelButtonText);
                    taskDialog = MsgBoxEx.GetTaskDialog(form: owner,
                        instructionText: instructionText,
                        progressbarMaxValue: operation.MainStepsCount.ToInt() - 1,
                        cancelable: cancelable,
                        controls: cancelButton);
                    cancelButton.Click += (___, ____) =>
                    {
                        taskDialog.Close();
                        if (updateTaskbar)
                            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
                    };
                }
            };
            operation.MainOperationStepIncreasing += (sender, e) => taskDialog.Text = (e.Log ?? string.Empty).ToString();
            operation.MainOperationStepIncreased += (sender, e) =>
            {
                if (updateTaskbar)
                    TaskbarManager.Instance.SetProgressValue(e.Step.ToInt(), e.Max.ToInt() - 1);
                taskDialog.ProgressBar.Value = e.Step.ToInt();
            };
            operation.MainOperationEnded += (sender, args) =>
            {
                if (donePrompt.IsNullOrEmpty() && owned)
                {
                    taskDialog.Close();
                    if (updateTaskbar)
                        TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
                }
                else
                {
                    if (args.IsSucceed)
                        if (taskDialog != null)
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
                    taskDialog?.Close();
                    if (updateTaskbar)
                        TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Error);
                }
                else
                {
                    if (taskDialog != null)
                    {
                        taskDialog.Text = exceptionPrompt;
                        taskDialog.DetailsExpandedText = args.Exception.GetBaseException().Message;
                        taskDialog.ProgressBar.State = TaskDialogProgressBarState.Error;
                    }
                    if (updateTaskbar)
                        TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Error);
                }
            };
            if (taskDialog != null)
            {
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
}