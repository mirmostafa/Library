using System;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Mohammad.Wpf.Helpers
{
    public static class TaskDialogHelper
    {
        public static void Set(this TaskDialog dialog, string instruction = null, string text = null, string details = null, string caption = null,
            string footerText = null, int? prograssValue = null, int? prograssMax = null, bool? cancellable = null)
        {
            if (dialog == null)
                return;
            Application.Current.RunInUiThread(() =>
            {
                if (cancellable != null)
                    dialog.Cancelable = cancellable.Value;
                if (caption != null)
                    dialog.Caption = caption;
                if (details != null)
                    dialog.DetailsExpandedText = details;
                if (footerText != null)
                    dialog.FooterText = footerText;
                if (instruction != null)
                    dialog.InstructionText = instruction;
                if (prograssValue != null)
                    if (dialog.ProgressBar != null)
                        dialog.ProgressBar.Value = prograssValue.Value;
                if (prograssMax != null)
                    if (dialog.ProgressBar != null)
                        dialog.ProgressBar.Maximum = prograssMax.Value;
                if (text != null)
                    dialog.Text = text;
            });
        }

        public static bool IsCancelled(this TaskDialog dialog, Func<bool> isCancelled)
        {
            if (isCancelled == null)
                return false;
            if (!isCancelled())
                return false;
            if (dialog?.ProgressBar != null)
                dialog.ProgressBar.State = TaskDialogProgressBarState.Paused;
            return true;
        }
    }
}