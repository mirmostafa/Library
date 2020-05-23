using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using Mohammad.Collections.Specialized;
using Mohammad.EventsArgs;
using Mohammad.Exceptions;
using Mohammad.Helpers;
using Mohammad.Internals;
using Mohammad.Threading.Tasks;
using Mohammad.Wpf.Helpers;
using Mohammad.Wpf.Windows.Media;
using static Mohammad.Helpers.CodeHelper;
using ApplicationHelper = Mohammad.Helpers.ApplicationHelper;

#pragma warning disable 4014

namespace Mohammad.Wpf.Windows.Dialogs
{
    public sealed class MsgBoxEx : InternalMessageBoxEx
    {
        public static Window DefaultWindow { get; set; }
        public static event EventHandler<ItemActedEventArgs<Window>> WindowRequired;

        /// <summary>
        /// </summary>
        /// <param name="action">
        ///     TaskDialog: Hosting Dialog, Func&lt;bool&gt;: isCancellationRequested, Func&lt;bool&gt;:
        ///     isBackgroundWorking
        /// </param>
        /// <param name="maximum"></param>
        /// <param name="caption"></param>
        /// <param name="instructionText"></param>
        /// <param name="initializingText"></param>
        /// <param name="detailsExpandedText"></param>
        /// <param name="isCancallable"></param>
        /// <param name="supportsBackgroundWorking"></param>
        /// <param name="footerIcon"></param>
        /// <param name="footerText"></param>
        /// <param name="cancelButtonText"></param>
        /// <param name="backgroundButtonText"></param>
        /// <param name="cancellingPromptText"></param>
        /// <param name="runInTask"></param>
        /// <param name="showOkButton"></param>
        /// <param name="enableOkButtonOnDone"></param>
        public static TaskDialog GetTaskDialog(Action<TaskDialog, Func<bool>, Func<bool>> action, int maximum = 100, string caption = null,
            string instructionText = null, string initializingText = null, string detailsExpandedText = null, bool isCancallable = false,
            bool supportsBackgroundWorking = false, TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None, string footerText = null,
            string cancelButtonText = "Cancel", string backgroundButtonText = "Background", string cancellingPromptText = "Cancelling...", bool runInTask = true,
            bool showOkButton = true, bool enableOkButtonOnDone = true)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var isCancellationRequested = false;
            var isBackgroundWorking = false;

            TaskDialogButton cancellationRequestedTaskDialogButton = null;
            TaskDialogButton inBackgroundTaskDialogButton = null;
            TaskDialogButton okButton = null;

            var controls = new List<TaskDialogControl>();
            if (supportsBackgroundWorking)
            {
                inBackgroundTaskDialogButton = new TaskDialogButton("inBackground", backgroundButtonText) {Default = true};
                inBackgroundTaskDialogButton.Click += (s, _) =>
                {
                    isBackgroundWorking = true;
                    s.As<TaskDialogButton>().HostingDialog.As<TaskDialog>().Close();
                };
                controls.Add(inBackgroundTaskDialogButton);
            }
            if (isCancallable)
            {
                cancellationRequestedTaskDialogButton = new TaskDialogButton("cancellationRequested", cancelButtonText);
                cancellationRequestedTaskDialogButton.Click += (s, __) =>
                {
                    isCancellationRequested = true;
                    var btn = s.As<TaskDialogButton>();
                    if (cancellingPromptText != null)
                    {
                        btn.HostingDialog.As<TaskDialog>().Text = cancellingPromptText;
                        btn.HostingDialog.As<TaskDialog>().ProgressBar.State = TaskDialogProgressBarState.Paused;
                    }
                    btn.Enabled = false;
                };
                controls.Add(cancellationRequestedTaskDialogButton);
            }

            if (showOkButton || !isCancallable && !supportsBackgroundWorking)
            {
                okButton = new TaskDialogButton("fakeButton", "OK");
                okButton.Click += (s, __) => { s.As<TaskDialogButton>().HostingDialog.As<TaskDialog>().Close(TaskDialogResult.Ok); };
                controls.Add(okButton);
            }
            return GetTaskDialog(instructionText,
                initializingText,
                caption ?? ApplicationHelper.ApplicationTitle,
                progressbarMaxValue: maximum,
                controls: controls.ToArray(),
                cancelable: false,
                footerText: footerText,
                footerIcon: footerIcon,
                detailsExpandedText: detailsExpandedText,
                onOpened: dlg =>
                {
                    if (okButton != null)
                        okButton.Enabled = false;
                    if (runInTask)
                    {
                        Async.Run(() => action(dlg, () => isCancellationRequested, () => isBackgroundWorking)).ContinueWith(t =>
                        {
                            if (enableOkButtonOnDone)
                                if (okButton != null)
                                    okButton.Enabled = true;
                            if (cancellationRequestedTaskDialogButton != null)
                                cancellationRequestedTaskDialogButton.Enabled = false;
                            if (inBackgroundTaskDialogButton != null)
                                inBackgroundTaskDialogButton.Enabled = false;
                        });
                    }
                    else
                    {
                        action(dlg, () => isCancellationRequested, () => isBackgroundWorking);
                        if (enableOkButtonOnDone)
                            if (okButton != null)
                                okButton.Enabled = true;
                        if (cancellationRequestedTaskDialogButton != null)
                            cancellationRequestedTaskDialogButton.Enabled = false;
                        if (inBackgroundTaskDialogButton != null)
                            inBackgroundTaskDialogButton.Enabled = false;
                    }
                });
        }

        public static TaskDialog GetTaskDialog(string instructionText = null, string text = null, string caption = null,
            TaskDialogStandardIcon icon = TaskDialogStandardIcon.None, TaskDialogStandardButtons buttons = TaskDialogStandardButtons.None,
            string detailsExpandedLabel = null, string detailsExpandedText = null, bool cancelable = true, string detailsCollapsedLabel = null,
            bool detailsExpanded = false, bool? footerCheckBoxChecked = null, string footerCheckBoxText = null,
            TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None, string footerText = null, bool hyperlinksEnabled = true,
            TaskDialogStartupLocation? startupLocation = null, TaskDialogProgressBarState progressBarState = TaskDialogProgressBarState.None,
            int progressbarMinValue = 0, int progressbarMaxValue = 0, int? progressbarCurrValue = null, Action<TaskDialog> onOpened = null,
            TimeSpan timeout = default(TimeSpan), Action timeoutAction = null, TaskDialogResult timeoutDialogResult = TaskDialogResult.Close, Window window = null,
            params TaskDialogControl[] controls)
        {
            var hWnd = IntPtr.Zero;
            WindowRequired.Raise(null, new ItemActedEventArgs<Window>(window));
            if (window == null)
                if ((window = DefaultWindow) == null)
                    window = Application.Current.MainWindow;
            if (startupLocation == null)
                startupLocation = window.WindowState == WindowState.Minimized ? TaskDialogStartupLocation.CenterScreen : TaskDialogStartupLocation.CenterOwner;
            if (hWnd == IntPtr.Zero)
                hWnd = window.GetHandle();
            return GetTaskDialog(instructionText,
                text,
                caption,
                icon,
                buttons,
                detailsExpandedLabel,
                detailsExpandedText,
                cancelable,
                detailsCollapsedLabel,
                detailsExpanded,
                footerCheckBoxChecked,
                footerCheckBoxText,
                footerIcon,
                footerText,
                hyperlinksEnabled,
                startupLocation.Value,
                progressBarState,
                progressbarMinValue,
                progressbarMaxValue,
                progressbarCurrValue,
                onOpened,
                timeout,
                timeoutAction,
                timeoutDialogResult,
                hWnd,
                controls);
        }

        public static TaskDialogResult Show(string instructionText = null, string text = null, string caption = null,
            TaskDialogStandardIcon icon = TaskDialogStandardIcon.None, TaskDialogStandardButtons buttons = TaskDialogStandardButtons.None,
            string detailsExpandedLabel = null, string detailsExpandedText = null, bool cancelable = true, string detailsCollapsedLabel = null,
            bool detailsExpanded = false, bool? footerCheckBoxChecked = null, string footerCheckBoxText = null,
            TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None, string footerText = null, bool hyperlinksEnabled = true,
            TaskDialogStartupLocation? startupLocation = null, TaskDialogProgressBarState progressBarState = TaskDialogProgressBarState.None,
            int progressbarMinValue = 0, int progressbarMaxValue = 0, int? progressbarCurrValue = null, Action<TaskDialog> onOpened = null,
            TimeSpan timeout = default(TimeSpan), Action timeoutAction = null, TaskDialogResult timeoutDialogResult = TaskDialogResult.Close, Window window = null,
            params TaskDialogControl[] controls)
        {
            var wnd = window ?? DefaultWindow;
            using (
                var dialog = GetTaskDialog(instructionText,
                    text,
                    caption,
                    icon,
                    buttons,
                    detailsExpandedLabel,
                    detailsExpandedText,
                    cancelable,
                    detailsCollapsedLabel,
                    detailsExpanded,
                    footerCheckBoxChecked,
                    footerCheckBoxText,
                    footerIcon,
                    footerText,
                    hyperlinksEnabled,
                    startupLocation,
                    progressBarState,
                    progressbarMinValue,
                    progressbarMaxValue,
                    progressbarCurrValue,
                    onOpened,
                    timeout,
                    timeoutAction,
                    timeoutDialogResult,
                    wnd,
                    controls))
            {
                double opacity = 1;
                if (wnd != null)
                {
                    opacity = wnd.Opacity;
                    Catch(() => Animations.FadeOut(wnd, .75));
                }
                try
                {
                    var result = dialog.Show();
                    return result;
                }
                finally
                {
                    if (wnd != null)
                        Catch(() => Animations.FadeIn(wnd, opacity));
                }
            }
        }

        public static void Inform(string instructionText = null, string text = null, string caption = null, string detailsExpandedLabel = null,
            string detailsExpandedText = null, bool cancelable = false, string detailsCollapsedLabel = null, bool detailsExpanded = false,
            bool? footerCheckBoxChecked = false, string footerCheckBoxText = null, TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None,
            string footerText = null, bool hyperlinksEnabled = true, TaskDialogStartupLocation? startupLocation = null,
            TaskDialogProgressBarState progressBarState = TaskDialogProgressBarState.None, int progressbarMinValue = 0, int progressbarMaxValue = 0,
            int? progressbarCurrValue = null, Action<TaskDialog> onOpened = null, TimeSpan timeout = default(TimeSpan), Action timeoutAction = null,
            Window window = null, params TaskDialogControl[] controls)
        {
            Show(instructionText,
                text,
                caption,
                TaskDialogStandardIcon.Information,
                TaskDialogStandardButtons.Ok,
                detailsExpandedLabel,
                detailsExpandedText,
                cancelable,
                detailsCollapsedLabel,
                detailsExpanded,
                footerCheckBoxChecked,
                footerCheckBoxText,
                footerIcon,
                footerText,
                hyperlinksEnabled,
                startupLocation,
                progressBarState,
                progressbarMinValue,
                progressbarMaxValue,
                progressbarCurrValue,
                onOpened,
                timeout,
                timeoutAction,
                TaskDialogResult.Close,
                window,
                controls);
        }

        public static void Warn(string instructionText = null, string text = null, string caption = null, string detailsExpandedLabel = null,
            string detailsExpandedText = null, bool cancelable = false, string detailsCollapsedLabel = null, bool detailsExpanded = false,
            bool? footerCheckBoxChecked = false, string footerCheckBoxText = null, TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None,
            string footerText = null, bool hyperlinksEnabled = true, TaskDialogStartupLocation? startupLocation = null,
            TaskDialogProgressBarState progressBarState = TaskDialogProgressBarState.None, int progressbarMinValue = 0, int progressbarMaxValue = 0,
            int? progressbarCurrValue = null, Action<TaskDialog> onOpened = null, TimeSpan timeout = default(TimeSpan), Action timeoutAction = null,
            Window window = null, params TaskDialogControl[] controls)
        {
            Show(instructionText,
                text,
                caption,
                TaskDialogStandardIcon.Warning,
                TaskDialogStandardButtons.Ok,
                detailsExpandedLabel,
                detailsExpandedText,
                cancelable,
                detailsCollapsedLabel,
                detailsExpanded,
                footerCheckBoxChecked,
                footerCheckBoxText,
                footerIcon,
                footerText,
                hyperlinksEnabled,
                startupLocation,
                progressBarState,
                progressbarMinValue,
                progressbarMaxValue,
                progressbarCurrValue,
                onOpened,
                timeout,
                timeoutAction,
                TaskDialogResult.Close,
                window,
                controls);
        }

        public static TaskDialogResult AskWithWarn(string instructionText = null, string text = null, string caption = null, string detailsExpandedLabel = null,
            string detailsExpandedText = null, bool cancelable = false, string detailsCollapsedLabel = null, bool detailsExpanded = false,
            bool? footerCheckBoxChecked = false, string footerCheckBoxText = null, TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None,
            string footerText = null, bool hyperlinksEnabled = true, TaskDialogStartupLocation? startupLocation = null,
            TaskDialogProgressBarState progressBarState = TaskDialogProgressBarState.None, int progressbarMinValue = 0, int progressbarMaxValue = 0,
            int? progressbarCurrValue = null, Action<TaskDialog> onOpened = null, TimeSpan timeout = default(TimeSpan), Action timeoutAction = null,
            TaskDialogResult timeoutDialogResult = TaskDialogResult.Close, Window window = null, params TaskDialogControl[] controls)
        {
            return Show(instructionText,
                text,
                caption,
                TaskDialogStandardIcon.Warning,
                TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No,
                detailsExpandedLabel,
                detailsExpandedText,
                cancelable,
                detailsCollapsedLabel,
                detailsExpanded,
                footerCheckBoxChecked,
                footerCheckBoxText,
                footerIcon,
                footerText,
                hyperlinksEnabled,
                startupLocation,
                progressBarState,
                progressbarMinValue,
                progressbarMaxValue,
                progressbarCurrValue,
                onOpened,
                timeout,
                timeoutAction,
                timeoutDialogResult,
                window,
                controls);
        }

        public static TaskDialogResult Ask(string instructionText = null, string text = null, string caption = null, string detailsExpandedLabel = null,
            string detailsExpandedText = null, bool cancelable = false, string detailsCollapsedLabel = null, bool detailsExpanded = false,
            bool? footerCheckBoxChecked = false, string footerCheckBoxText = null, TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None,
            string footerText = null, bool hyperlinksEnabled = true, TaskDialogStartupLocation? startupLocation = null,
            TaskDialogProgressBarState progressBarState = TaskDialogProgressBarState.None, int progressbarMinValue = 0, int progressbarMaxValue = 0,
            int? progressbarCurrValue = null, Action<TaskDialog> onOpened = null, TimeSpan timeout = default(TimeSpan), Action timeoutAction = null,
            TaskDialogResult timeoutDialogResult = TaskDialogResult.Close, Window window = null, params TaskDialogControl[] controls)
        {
            return Show(instructionText,
                text,
                caption,
                TaskDialogStandardIcon.Information,
                TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No,
                detailsExpandedLabel,
                detailsExpandedText,
                cancelable,
                detailsCollapsedLabel,
                detailsExpanded,
                footerCheckBoxChecked,
                footerCheckBoxText,
                footerIcon,
                footerText,
                hyperlinksEnabled,
                startupLocation,
                progressBarState,
                progressbarMinValue,
                progressbarMaxValue,
                progressbarCurrValue,
                onOpened,
                timeout,
                timeoutAction,
                timeoutDialogResult,
                window,
                controls);
        }

        public static TaskDialogResult AskWithCancel(string instructionText = null, string text = null, string caption = null, string detailsExpandedLabel = null,
            string detailsExpandedText = null, bool cancelable = false, string detailsCollapsedLabel = null, bool detailsExpanded = false,
            bool? footerCheckBoxChecked = false, string footerCheckBoxText = null, TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None,
            string footerText = null, bool hyperlinksEnabled = true, TaskDialogStartupLocation? startupLocation = null,
            TaskDialogProgressBarState progressBarState = TaskDialogProgressBarState.None, int progressbarMinValue = 0, int progressbarMaxValue = 0,
            int? progressbarCurrValue = null, Action<TaskDialog> onOpened = null, TimeSpan timeout = default(TimeSpan), Action timeoutAction = null,
            TaskDialogResult timeoutDialogResult = TaskDialogResult.Close, Window window = null, params TaskDialogControl[] controls)
        {
            return Show(instructionText,
                text,
                caption,
                TaskDialogStandardIcon.Information,
                TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No | TaskDialogStandardButtons.Cancel,
                detailsExpandedLabel,
                detailsExpandedText,
                cancelable,
                detailsCollapsedLabel,
                detailsExpanded,
                footerCheckBoxChecked,
                footerCheckBoxText,
                footerIcon,
                footerText,
                hyperlinksEnabled,
                startupLocation,
                progressBarState,
                progressbarMinValue,
                progressbarMaxValue,
                progressbarCurrValue,
                onOpened,
                timeout,
                timeoutAction,
                timeoutDialogResult,
                window,
                controls);
        }

        public static void Error(string instructionText = null, string text = null, string caption = null, string detailsExpandedLabel = null,
            string detailsExpandedText = null, bool cancelable = false, string detailsCollapsedLabel = null, bool detailsExpanded = false,
            bool? footerCheckBoxChecked = false, string footerCheckBoxText = null, TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None,
            string footerText = null, bool hyperlinksEnabled = true, TaskDialogStartupLocation? startupLocation = null,
            TaskDialogProgressBarState progressBarState = TaskDialogProgressBarState.None, int progressbarMinValue = 0, int progressbarMaxValue = 0,
            int? progressbarCurrValue = null, Action<TaskDialog> onOpened = null, Action timeoutAction = null, TimeSpan timeout = default(TimeSpan),
            Window window = null, params TaskDialogControl[] controls)
        {
            Show(instructionText,
                text,
                caption,
                TaskDialogStandardIcon.Error,
                TaskDialogStandardButtons.Ok,
                detailsExpandedLabel,
                detailsExpandedText,
                cancelable,
                detailsCollapsedLabel,
                detailsExpanded,
                footerCheckBoxChecked,
                footerCheckBoxText,
                footerIcon,
                footerText,
                hyperlinksEnabled,
                startupLocation,
                progressBarState,
                progressbarMinValue,
                progressbarMaxValue,
                progressbarCurrValue,
                onOpened,
                timeout,
                timeoutAction,
                TaskDialogResult.Close,
                window,
                controls);
        }

        public static void Exception(Exception ex, string instructionText = null, string caption = null, string detailsExpandedLabel = null,
            string detailsExpandedText = null, bool cancelable = false, string detailsCollapsedLabel = null, bool detailsExpanded = false,
            bool? footerCheckBoxChecked = false, string footerCheckBoxText = null, TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None,
            string footerText = null, bool hyperlinksEnabled = true, TaskDialogStartupLocation? startupLocation = null,
            TaskDialogProgressBarState progressBarState = TaskDialogProgressBarState.None, int progressbarMinValue = 0, int progressbarMaxValue = 0,
            int? progressbarCurrValue = null, Action<TaskDialog> onOpened = null, TimeSpan timeout = default(TimeSpan), Action timeoutAction = null,
            Window window = null, params TaskDialogControl[] controls)
        {
            var message = ex.Message;
            var innerMessage = ex.GetBaseException().Message;
            Error(instructionText,
                message,
                caption,
                detailsExpandedLabel,
                innerMessage == message ? string.Empty : innerMessage,
                cancelable,
                detailsCollapsedLabel,
                detailsExpanded,
                footerCheckBoxChecked,
                footerCheckBoxText,
                footerIcon,
                footerText,
                hyperlinksEnabled,
                startupLocation,
                progressBarState,
                progressbarMinValue,
                progressbarMaxValue,
                progressbarCurrValue,
                onOpened,
                timeoutAction,
                timeout,
                ex.Data["Owner"].As<Window>() ?? window,
                controls);
        }

        public static void Exception(IException ex, string caption = null, string detailsExpandedLabel = null, string detailsExpandedText = null,
            bool cancelable = false, string detailsCollapsedLabel = null, bool detailsExpanded = false, bool? footerCheckBoxChecked = false,
            string footerCheckBoxText = null, TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None, string footerText = null,
            bool hyperlinksEnabled = true, TaskDialogStartupLocation? startupLocation = null,
            TaskDialogProgressBarState progressBarState = TaskDialogProgressBarState.None, int progressbarMinValue = 0, int progressbarMaxValue = 0,
            int? progressbarCurrValue = null, Action<TaskDialog> onOpened = null, TimeSpan timeout = default(TimeSpan), Action timeoutAction = null,
            Window window = null, params TaskDialogControl[] controls)
        {
            var message = ex.Message;
            var innerMessage = ex.GetBaseException().Message;
            Error(ex.Instruction,
                message,
                caption,
                detailsExpandedLabel,
                innerMessage == message ? string.Empty : innerMessage,
                cancelable,
                detailsCollapsedLabel,
                detailsExpanded,
                footerCheckBoxChecked,
                footerCheckBoxText,
                footerIcon,
                footerText,
                hyperlinksEnabled,
                startupLocation,
                progressBarState,
                progressbarMinValue,
                progressbarMaxValue,
                progressbarCurrValue,
                onOpened,
                timeoutAction,
                timeout,
                ex.Owner.As<Window>() ?? window,
                controls);
        }

        /// <summary>
        /// </summary>
        /// <param name="action">
        ///     TaskDialog: Hosting Dialog, Func&lt;bool&gt;: isCancellationRequested, Func&lt;bool&gt;:
        ///     isBackgroundWorking
        /// </param>
        /// <param name="maximum"></param>
        /// <param name="caption"></param>
        /// <param name="instructionText"></param>
        /// <param name="initializingText"></param>
        /// <param name="detailsExpandedText"></param>
        /// <param name="footerIcon"></param>
        /// <param name="footerText"></param>
        /// <param name="runInTask"></param>
        /// <param name="enableOkButtonOnDone"></param>
        /// <param name="showOkButton"></param>
        public static TaskDialogResult ShowProgress(Action<TaskDialog> action, int maximum = 100, string caption = null, string instructionText = null,
            string initializingText = null, string detailsExpandedText = null, TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None,
            string footerText = null, bool runInTask = true, bool enableOkButtonOnDone = true, bool showOkButton = true)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            return ShowProgress((dlg, _, __) => action(dlg),
                maximum,
                caption,
                instructionText,
                initializingText,
                detailsExpandedText,
                false,
                footerIcon: footerIcon,
                footerText: footerText,
                runInTask: runInTask,
                showOkButton: showOkButton,
                enableOkButtonOnDone: enableOkButtonOnDone);
        }

        /// <summary>
        /// </summary>
        /// <param name="action">
        ///     TaskDialog: Hosting Dialog, Func&lt;bool&gt;: isCancellationRequested, Func&lt;bool&gt;:
        ///     isBackgroundWorking
        /// </param>
        /// <param name="maximum"></param>
        /// <param name="caption"></param>
        /// <param name="instructionText"></param>
        /// <param name="initializingText"></param>
        /// <param name="detailsExpandedText"></param>
        /// <param name="isCancallable"></param>
        /// <param name="footerIcon"></param>
        /// <param name="footerText"></param>
        /// <param name="cancelButtonText"></param>
        /// <param name="cancellingPromptText"></param>
        /// <param name="runInTask"></param>
        /// <param name="showOkButton"></param>
        /// <param name="enableOkButtonOnDone"></param>
        public static TaskDialogResult ShowProgress(Action<TaskDialog, Func<bool>> action, int maximum = 100, string caption = null, string instructionText = null,
            string initializingText = null, string detailsExpandedText = null, bool isCancallable = false,
            TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None, string footerText = null, string cancelButtonText = "Cancel",
            string cancellingPromptText = "Cancelling...", bool runInTask = true, bool showOkButton = true, bool enableOkButtonOnDone = true)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            return ShowProgress((dlg, isCancellationRequested, isBackgroundWorking) => action(dlg, isCancellationRequested),
                maximum,
                caption,
                instructionText,
                initializingText,
                detailsExpandedText,
                isCancallable,
                false,
                footerIcon,
                footerText,
                cancelButtonText,
                cancellingPromptText: cancellingPromptText,
                runInTask: runInTask,
                showOkButton: showOkButton,
                enableOkButtonOnDone: enableOkButtonOnDone);
        }

        /// <summary>
        /// </summary>
        /// <param name="action">
        ///     TaskDialog: Hosting Dialog, Func&lt;bool&gt;: isCancellationRequested, Func&lt;bool&gt;:
        ///     isBackgroundWorking
        /// </param>
        /// <param name="maximum"></param>
        /// <param name="caption"></param>
        /// <param name="instructionText"></param>
        /// <param name="initializingText"></param>
        /// <param name="detailsExpandedText"></param>
        /// <param name="isCancallable"></param>
        /// <param name="supportsBackgroundWorking"></param>
        /// <param name="footerIcon"></param>
        /// <param name="footerText"></param>
        /// <param name="cancelButtonText"></param>
        /// <param name="backgroundButtonText"></param>
        /// <param name="cancellingPromptText"></param>
        /// <param name="runInTask"></param>
        /// <param name="showOkButton"></param>
        /// <param name="enableOkButtonOnDone"></param>
        public static TaskDialogResult ShowProgress(Action<TaskDialog, Func<bool>, Func<bool>> action, int maximum = 100, string caption = null,
            string instructionText = null, string initializingText = null, string detailsExpandedText = null, bool isCancallable = false,
            bool supportsBackgroundWorking = false, TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None, string footerText = null,
            string cancelButtonText = "Cancel", string backgroundButtonText = "Background", string cancellingPromptText = "Cancelling...", bool runInTask = true,
            bool showOkButton = true, bool enableOkButtonOnDone = true)
        {
            using (
                var dlg = GetTaskDialog(action,
                    maximum,
                    caption,
                    instructionText,
                    initializingText,
                    detailsExpandedText,
                    isCancallable,
                    supportsBackgroundWorking,
                    footerIcon,
                    footerText,
                    cancelButtonText,
                    backgroundButtonText,
                    cancellingPromptText,
                    runInTask,
                    showOkButton,
                    enableOkButtonOnDone))
                return dlg.Show();
        }

        public static TaskDialogResult ShowProgress<TItem>(IEnumerable<TItem> items, Action<TaskDialog, TItem> onIterating, Action<int> onEachIterated = null,
            Action<TaskDialog> onIterationEnded = null, int? maximum = null, string caption = null, string instructionText = null, string initializingText = null,
            string detailsExpandedText = null, bool isCancallable = false, bool supportsBackgroundWorking = false,
            TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None, string footerText = null, string cancelButtonText = "Cancel",
            string backgroundButtonText = "Background", string cancellingPromptText = "Cancelling...", bool runInTask = true, bool showOkButton = true,
            bool enableOkButtonOnDone = true, bool runIterationAsParallel = false)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            if (onIterating == null)
                throw new ArgumentNullException(nameof(onIterating));

            Action<TaskDialog, Func<bool>, Func<bool>> action;
            if (runIterationAsParallel)
                action = (dlg, isCancelled, isInBackground) =>
                {
                    var tasks = new TaskList();
                    Func<TaskDialog, Func<bool>, bool> cancelled = (dialog, isCancelledFunc) => tasks.IsCancellationRequested || dialog.IsCancelled(isCancelledFunc);
                    var index = 0;
                    foreach (var item in items)
                    {
                        if (dlg.IsCancelled(isCancelled))
                            break;
                        tasks.Run(() =>
                        {
                            if (dlg.IsCancelled(isCancelled))
                                return;
                            onIterating(dlg, item);
                        });
                        Catch(() => dlg.Set(prograssValue: ++index));
                        onEachIterated?.Invoke(index);
                        if (cancelled(dlg, isCancelled))
                            break;
                    }
                    tasks.WaitAll();
                    onIterationEnded?.Invoke(dlg);
                };
            else
                action = (dlg, isCancelled, isInBackground) =>
                {
                    Func<TaskDialog, Func<bool>, bool> cancelled = (dialog, isCancelledFunc) => dialog.IsCancelled(isCancelledFunc);
                    var index = 0;
                    foreach (var item in items)
                    {
                        if (dlg.IsCancelled(isCancelled))
                            break;
                        onIterating(dlg, item);
                        Catch(() => dlg.Set(prograssValue: ++index));
                        onEachIterated?.Invoke(index);
                        if (cancelled(dlg, isCancelled))
                            break;
                    }
                    onIterationEnded?.Invoke(dlg);
                };

            return ShowProgress(action,
                maximum ?? items.Count(),
                caption,
                instructionText,
                initializingText,
                detailsExpandedText,
                isCancallable,
                supportsBackgroundWorking,
                footerIcon,
                footerText,
                cancelButtonText,
                backgroundButtonText,
                cancellingPromptText,
                runInTask,
                showOkButton,
                enableOkButtonOnDone);
        }

        public static TaskDialogResult ShowProgress<TItem>(IEnumerable<TItem> items, Action<TaskDialog, Func<bool>, Func<bool>, TItem> onIterating,
            Action<int> onEachIterated = null, Action<TaskDialog> onIterationEnded = null, int? maximum = null, string caption = null, string instructionText = null,
            string initializingText = null, string detailsExpandedText = null, bool isCancallable = false, bool supportsBackgroundWorking = false,
            TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None, string footerText = null, string cancelButtonText = "Cancel",
            string backgroundButtonText = "Background", string cancellingPromptText = "Cancelling...", bool runInTask = true, bool showOkButton = true,
            bool enableOkButtonOnDone = true, bool runIterationAsParallel = false)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            if (onIterating == null)
                throw new ArgumentNullException(nameof(onIterating));

            Action<TaskDialog, Func<bool>, Func<bool>> action;
            if (runIterationAsParallel)
                action = (dlg, isCancelled, isInBackground) =>
                {
                    var tasks = new TaskList();
                    Func<TaskDialog, Func<bool>, bool> cancelled = (dialog, isCancelledFunc) => tasks.IsCancellationRequested || dialog.IsCancelled(isCancelledFunc);
                    var index = 0;
                    foreach (var item in items)
                    {
                        if (dlg.IsCancelled(isCancelled))
                            break;
                        tasks.Run(() =>
                        {
                            if (dlg.IsCancelled(isCancelled))
                                return;
                            onIterating(dlg, isCancelled, isInBackground, item);
                        });
                        Catch(() => dlg.Set(prograssValue: ++index));
                        onEachIterated?.Invoke(index);
                        if (cancelled(dlg, isCancelled))
                            break;
                    }
                    tasks.WaitAll();
                    onIterationEnded?.Invoke(dlg);
                };
            else
                action = (dlg, isCancelled, isInBackground) =>
                {
                    Func<TaskDialog, Func<bool>, bool> cancelled = (dialog, isCancelledFunc) => dialog.IsCancelled(isCancelledFunc);
                    var index = 0;
                    foreach (var item in items)
                    {
                        if (dlg.IsCancelled(isCancelled))
                            break;
                        onIterating(dlg, isCancelled, isInBackground, item);
                        Catch(() => dlg.Set(prograssValue: ++index));
                        onEachIterated?.Invoke(index);
                        if (cancelled(dlg, isCancelled))
                            break;
                    }
                    onIterationEnded?.Invoke(dlg);
                };

            return ShowProgress(action,
                maximum ?? items.Count(),
                caption,
                instructionText,
                initializingText,
                detailsExpandedText,
                isCancallable,
                supportsBackgroundWorking,
                footerIcon,
                footerText,
                cancelButtonText,
                backgroundButtonText,
                cancellingPromptText,
                runInTask,
                showOkButton,
                enableOkButtonOnDone);
        }
    }
}