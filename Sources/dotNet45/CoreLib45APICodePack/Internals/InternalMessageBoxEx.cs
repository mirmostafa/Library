using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Dialogs;
using Mohammad.Helpers;

namespace Mohammad.Internals
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class InternalMessageBoxEx
    {
        protected static TaskDialog GetTaskDialog(string instructionText = null, string text = null, string caption = null,
            TaskDialogStandardIcon icon = TaskDialogStandardIcon.None, TaskDialogStandardButtons buttons = TaskDialogStandardButtons.None,
            string detailsExpandedLabel = null, string detailsExpandedText = null, bool cancelable = true, string detailsCollapsedLabel = null,
            bool detailsExpanded = false, bool? footerCheckBoxChecked = null, string footerCheckBoxText = null,
            TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None, string footerText = null, bool hyperlinksEnabled = true,
            TaskDialogStartupLocation startupLocation = TaskDialogStartupLocation.CenterOwner,
            TaskDialogProgressBarState progressBarState = TaskDialogProgressBarState.None, int progressbarMinValue = 0, int progressbarMaxValue = 0,
            int? progressbarCurrValue = null, Action<TaskDialog> onOpened = null, TimeSpan timeout = default(TimeSpan), Action timeoutAction = null,
            TaskDialogResult timeoutDialogResult = TaskDialogResult.Close, IntPtr hWnd = default(IntPtr), params TaskDialogControl[] controls)
        {
            using (new EnableThemingInScope(true))
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
                                 OwnerWindowHandle = hWnd
                             };
                if (controls != null && controls.Length > 0)
                    result.Controls.AddMany(controls);
                else
                    result.StandardButtons = buttons;
                if (progressBarState != TaskDialogProgressBarState.None || progressbarMaxValue > 0)
                    result.ProgressBar = new TaskDialogProgressBar(progressbarMinValue, progressbarMaxValue, progressbarCurrValue ?? progressbarMinValue)
                                         {
                                             State =
                                                 progressBarState ==
                                                 TaskDialogProgressBarState
                                                     .None
                                                     ? TaskDialogProgressBarState
                                                         .Normal
                                                     : progressBarState
                                         };

                result.Opened += (e1, e2) => CodeHelper.Catch(() =>
                {
                    result.Icon = icon;
                    if (!result.FooterText.IsNullOrEmpty())
                        result.FooterIcon = footerIcon;
                    onOpened?.Invoke(result);
                });
                if (timeout != default(TimeSpan))
                {
                    var closed = false;
                    const int steps = 100;
                    var timeoutMilliseconds = timeout.TotalMilliseconds;
                    var progress = new TaskDialogProgressBar(0, timeoutMilliseconds.ToInt(), 0);
                    result.Controls.Add(progress);
                    result.Closing += (_, __) => { closed = true; };
                    result.Opened += (_, __) => Task.Factory.StartNew(() =>
                    {
                        var current = 0;
                        var sw = new Stopwatch();
                        sw.Start();
                        while (!closed && sw.ElapsedMilliseconds < timeoutMilliseconds)
                        {
                            if (!footerText.IsNullOrEmpty() && footerText.Contains("{0}"))
                                result.FooterText = string.Format(footerText, Convert.ToInt32(timeoutMilliseconds - current) / 1000);
                            current = current + steps + 5 >= timeoutMilliseconds ? (int) timeoutMilliseconds : current + steps + 5;
                            progress.Value = current;
                            Thread.Sleep(steps);
                        }
                        sw.Stop();
                        if (closed)
                            return;
                        progress.Value = progress.Maximum;
                        if (timeoutAction != null)
                            timeoutAction();
                        else
                            result.Close(timeoutDialogResult);
                    });
                }
                return result;
            }
        }
    }
}