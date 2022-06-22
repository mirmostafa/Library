using Microsoft.WindowsAPICodePack.Dialogs;

namespace Library.Wpf.Helpers;

public static class TaskDialogHelper
{
    public static void Set(this TaskDialog dialog,
        string? instruction = null,
        string? text = null,
        string? details = null,
        string? caption = null,
        string? footerText = null,
        int? prograssValue = null,
        int? prograssMax = null,
        bool? cancellable = null)
    {
        if (dialog is null)
        {
            return;
        }

        Application.Current.RunInUiThread(() =>
        {
            if (cancellable is { } value)
            {
                dialog.Cancelable = value;
            }

            if (caption is not null)
            {
                dialog.Caption = caption;
            }

            if (details is not null)
            {
                dialog.DetailsExpandedText = details;
            }

            if (footerText is not null)
            {
                dialog.FooterText = footerText;
            }

            if (instruction is not null)
            {
                dialog.InstructionText = instruction;
            }

            if (prograssValue is not null)
            {
                if (dialog.ProgressBar is not null)
                {
                    dialog.ProgressBar.Value = prograssValue.Value;
                }
            }

            if (prograssMax is not null)
            {
                if (dialog.ProgressBar is not null)
                {
                    dialog.ProgressBar.Maximum = prograssMax.Value;
                }
            }

            if (text is not null)
            {
                dialog.Text = text;
            }
        });
    }

    public static bool IsCancelled(this TaskDialog dialog, Func<bool> isCancelled)
    {
        if (isCancelled is null)
        {
            return false;
        }

        if (!isCancelled())
        {
            return false;
        }

        if (dialog?.ProgressBar is not null)
        {
            dialog.ProgressBar.State = TaskDialogProgressBarState.Paused;
        }

        return true;
    }
}
