using Library.Collections;
using Library.EventsArgs;
using Library.Exceptions;
using Library.Wpf.Internals;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Library.Wpf.Dialogs;

public sealed class MsgBox2 : InternalMessageBox2
{
    public static Window? DefaultWindow { get; set; }

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
    public static TaskDialog GetTaskDialog(Action<TaskDialog, Func<bool>, Func<bool>> action!!,
        int maximum = 100,
        string? caption = null,
        string? instructionText = null,
        string? initializingText = null,
        string? detailsExpandedText = null,
        bool isCancallable = false,
        bool supportsBackgroundWorking = false,
        TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None,
        string? footerText = null,
        string? cancelButtonText = "Cancel",
        string? backgroundButtonText = "Background",
        string? cancellingPromptText = "Cancelling...",
        bool runInTask = true,
        bool showOkButton = true,
        bool enableOkButtonOnDone = true)
    {
        var isCancellationRequested = false;
        var isBackgroundWorking = false;

        TaskDialogButton? cancellationRequestedTaskDialogButton = null;
        TaskDialogButton? inBackgroundTaskDialogButton = null;
        TaskDialogButton? okButton = null;

        var controls = new List<TaskDialogControl>();
        if (supportsBackgroundWorking)
        {
            inBackgroundTaskDialogButton = new TaskDialogButton("inBackground", backgroundButtonText) { Default = true };
            inBackgroundTaskDialogButton.Click += (s, _) =>
            {
                isBackgroundWorking = true;
                s.As<TaskDialogButton>()?.HostingDialog.As<TaskDialog>()?.Close();
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
                if (cancellingPromptText is not null)
                {
                    var taskDialog = btn!.HostingDialog!.As<TaskDialog>();
                    taskDialog!.Text = cancellingPromptText;
                    taskDialog!.ProgressBar.State = TaskDialogProgressBarState.Paused;
                }

                btn!.Enabled = false;
            };
            controls.Add(cancellationRequestedTaskDialogButton);
        }

        if (showOkButton || (!isCancallable && !supportsBackgroundWorking))
        {
            okButton = new TaskDialogButton("fakeButton", "OK");
            okButton.Click += (s, __) => s.As<TaskDialogButton>()!.HostingDialog.As<TaskDialog>()!.Close(TaskDialogResult.Ok);
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
                if (okButton is not null)
                {
                    okButton.Enabled = false;
                }

                if (runInTask)
                {
                    _ = Task.Run(() => action(dlg, () => isCancellationRequested, () => isBackgroundWorking))
                            .ContinueWith(t =>
                            {
                                if (enableOkButtonOnDone)
                                {
                                    if (okButton is not null)
                                    {
                                        okButton.Enabled = true;
                                    }
                                }

                                if (cancellationRequestedTaskDialogButton is not null)
                                {
                                    cancellationRequestedTaskDialogButton.Enabled = false;
                                }

                                if (inBackgroundTaskDialogButton is not null)
                                {
                                    inBackgroundTaskDialogButton.Enabled = false;
                                }
                            }, TaskScheduler.Default);
                }
                else
                {
                    action(dlg, () => isCancellationRequested, () => isBackgroundWorking);
                    if (enableOkButtonOnDone)
                    {
                        if (okButton is not null)
                        {
                            okButton.Enabled = true;
                        }
                    }

                    if (cancellationRequestedTaskDialogButton is not null)
                    {
                        cancellationRequestedTaskDialogButton.Enabled = false;
                    }

                    if (inBackgroundTaskDialogButton is not null)
                    {
                        inBackgroundTaskDialogButton.Enabled = false;
                    }
                }
            });
    }

    public static TaskDialog GetTaskDialog(string? instructionText = null,
        string? text = null,
        string? caption = null,
        TaskDialogStandardIcon icon = TaskDialogStandardIcon.None,
        TaskDialogStandardButtons buttons = TaskDialogStandardButtons.None,
        string? detailsExpandedLabel = null,
        string? detailsExpandedText = null,
        bool cancelable = true,
        string? detailsCollapsedLabel = null,
        bool detailsExpanded = false,
        bool? footerCheckBoxChecked = null,
        string? footerCheckBoxText = null,
        TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None,
        string? footerText = null,
        bool hyperlinksEnabled = true,
        TaskDialogStartupLocation? startupLocation = null,
        TaskDialogProgressBarState progressBarState = TaskDialogProgressBarState.None,
        int progressbarMinValue = 0,
        int progressbarMaxValue = 0,
        int? progressbarCurrValue = null,
        Action<TaskDialog>? onOpened = null,
        TimeSpan timeout = default,
        Action? timeoutAction = null,
        TaskDialogResult timeoutDialogResult = TaskDialogResult.Close,
        Window? window = null,
        params TaskDialogControl[] controls)
    {
        var hWnd = IntPtr.Zero;
        WindowRequired?.Invoke(null, new ItemActedEventArgs<Window>(window!));
        if (window is null)
        {
            if ((window = DefaultWindow) is null)
            {
                window = Application.Current.MainWindow;
            }
        }

        if (startupLocation is null)
        {
            startupLocation = window.WindowState == WindowState.Minimized ? TaskDialogStartupLocation.CenterScreen : TaskDialogStartupLocation.CenterOwner;
        }

        if (hWnd == IntPtr.Zero)
        {
            hWnd = window.GetHandle();
        }

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

    public static TaskDialogResult Show(string? instructionText = null,
        string? text = null,
        string? caption = null,
        TaskDialogStandardIcon icon = TaskDialogStandardIcon.None,
        TaskDialogStandardButtons buttons = TaskDialogStandardButtons.None,
        string? detailsExpandedLabel = null,
        string? detailsExpandedText = null,
        bool cancelable = true,
        string? detailsCollapsedLabel = null,
        bool detailsExpanded = false,
        bool? footerCheckBoxChecked = null,
        string? footerCheckBoxText = null,
        TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None,
        string? footerText = null,
        bool hyperlinksEnabled = true,
        TaskDialogStartupLocation? startupLocation = null,
        TaskDialogProgressBarState progressBarState = TaskDialogProgressBarState.None,
        int progressbarMinValue = 0,
        int progressbarMaxValue = 0,
        int? progressbarCurrValue = null,
        Action<TaskDialog>? onOpened = null,
        TimeSpan timeout = default,
        Action? timeoutAction = null,
        TaskDialogResult timeoutDialogResult = TaskDialogResult.Close,
        Window? window = null,
        params TaskDialogControl[] controls)
    {
        var wnd = window ?? DefaultWindow;
        using var dialog = GetTaskDialog(instructionText,
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
                controls);
        double opacity = 1;
        if (wnd is not null)
        {
            opacity = wnd.Opacity;
            _ = Catch(() => Animations.FadeOut(wnd, .75));
        }

        try
        {
            var result = dialog.Show();
            return result;
        }
        finally
        {
            if (wnd is not null)
            {
                _ = Catch(() => Animations.FadeIn(wnd, opacity));
            }
        }
    }

    public static void Inform(string? instructionText = null,
        string? text = null,
        string? caption = null,
        string? detailsExpandedLabel = null,
        string? detailsExpandedText = null,
        bool cancelable = false,
        string? detailsCollapsedLabel = null,
        bool detailsExpanded = false,
        bool? footerCheckBoxChecked = false,
        string? footerCheckBoxText = null,
        TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None,
        string? footerText = null,
        bool hyperlinksEnabled = true,
        TaskDialogStartupLocation? startupLocation = null,
        TaskDialogProgressBarState progressBarState = TaskDialogProgressBarState.None,
        int progressbarMinValue = 0,
        int progressbarMaxValue = 0,
        int? progressbarCurrValue = null,
        Action<TaskDialog>? onOpened = null,
        TimeSpan timeout = default,
        Action? timeoutAction = null,
        Window? window = null,
        params TaskDialogControl[] controls) => Show(instructionText,
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

    public static void Warn(string? instructionText = null,
        string? text = null,
        string? caption = null,
        string? detailsExpandedLabel = null,
        string? detailsExpandedText = null,
        bool cancelable = false,
        string? detailsCollapsedLabel = null,
        bool detailsExpanded = false,
        bool? footerCheckBoxChecked = false,
        string? footerCheckBoxText = null,
        TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None,
        string? footerText = null,
        bool hyperlinksEnabled = true,
        TaskDialogStartupLocation? startupLocation = null,
        TaskDialogProgressBarState progressBarState = TaskDialogProgressBarState.None,
        int progressbarMinValue = 0,
        int progressbarMaxValue = 0,
        int? progressbarCurrValue = null,
        Action<TaskDialog>? onOpened = null,
        TimeSpan timeout = default,
        Action? timeoutAction = null,
        Window? window = null,
        params TaskDialogControl[] controls) => Show(instructionText,
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

    public static TaskDialogResult AskWithWarn(string? instructionText = null,
        string? text = null,
        string? caption = null,
        string? detailsExpandedLabel = null,
        string? detailsExpandedText = null,
        bool cancelable = false,
        string? detailsCollapsedLabel = null,
        bool detailsExpanded = false,
        bool? footerCheckBoxChecked = false,
        string? footerCheckBoxText = null,
        TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None,
        string? footerText = null,
        bool hyperlinksEnabled = true,
        TaskDialogStartupLocation? startupLocation = null,
        TaskDialogProgressBarState progressBarState = TaskDialogProgressBarState.None,
        int progressbarMinValue = 0,
        int progressbarMaxValue = 0,
        int? progressbarCurrValue = null,
        Action<TaskDialog>? onOpened = null,
        TimeSpan timeout = default,
        Action? timeoutAction = null,
        TaskDialogResult timeoutDialogResult = TaskDialogResult.Close,
        Window? window = null,
        params TaskDialogControl[] controls)
        => Show(instructionText,
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

    public static TaskDialogResult Ask(string? instructionText = null,
        string? text = null,
        string? caption = null,
        string? detailsExpandedLabel = null,
        string? detailsExpandedText = null,
        bool cancelable = false,
        string? detailsCollapsedLabel = null,
        bool detailsExpanded = false,
        bool? footerCheckBoxChecked = false,
        string? footerCheckBoxText = null,
        TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None,
        string? footerText = null,
        bool hyperlinksEnabled = true,
        TaskDialogStartupLocation? startupLocation = null,
        TaskDialogProgressBarState progressBarState = TaskDialogProgressBarState.None,
        int progressbarMinValue = 0,
        int progressbarMaxValue = 0,
        int? progressbarCurrValue = null,
        Action<TaskDialog>? onOpened = null,
        TimeSpan timeout = default,
        Action? timeoutAction = null,
        TaskDialogResult timeoutDialogResult = TaskDialogResult.Close,
        Window? window = null,
        params TaskDialogControl[] controls)
        => Show(instructionText,
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

    public static TaskDialogResult AskWithCancel(string? instructionText = null,
        string? text = null,
        string? caption = null,
        string? detailsExpandedLabel = null,
        string? detailsExpandedText = null,
        bool cancelable = false,
        string? detailsCollapsedLabel = null,
        bool detailsExpanded = false,
        bool? footerCheckBoxChecked = false,
        string? footerCheckBoxText = null,
        TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None,
        string? footerText = null,
        bool hyperlinksEnabled = true,
        TaskDialogStartupLocation? startupLocation = null,
        TaskDialogProgressBarState progressBarState = TaskDialogProgressBarState.None,
        int progressbarMinValue = 0,
        int progressbarMaxValue = 0,
        int? progressbarCurrValue = null,
        Action<TaskDialog>? onOpened = null,
        TimeSpan timeout = default,
        Action? timeoutAction = null,
        TaskDialogResult timeoutDialogResult = TaskDialogResult.Close,
        Window? window = null,
        params TaskDialogControl[] controls)
        => Show(instructionText,
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

    public static TaskDialogResult Error(string? instructionText = null,
        string? text = null,
        string? caption = null,
        string? detailsExpandedLabel = null,
        string? detailsExpandedText = null,
        bool cancelable = false,
        string? detailsCollapsedLabel = null,
        bool detailsExpanded = false,
        bool? footerCheckBoxChecked = false,
        string? footerCheckBoxText = null,
        TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None,
        string? footerText = null,
        bool hyperlinksEnabled = true,
        TaskDialogStartupLocation? startupLocation = null,
        TaskDialogProgressBarState progressBarState = TaskDialogProgressBarState.None,
        int progressbarMinValue = 0,
        int progressbarMaxValue = 0,
        int? progressbarCurrValue = null,
        Action<TaskDialog>? onOpened = null,
        Action? timeoutAction = null,
        TimeSpan timeout = default,
        Window? window = null,
        params TaskDialogControl[] controls) => Show(instructionText,
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

    public static TaskDialogResult Exception(Exception ex,
        string? instructionText = null,
        string? caption = null,
        string? detailsExpandedLabel = null,
        string? detailsExpandedText = null,
        bool cancelable = false,
        string? detailsCollapsedLabel = null,
        bool detailsExpanded = false,
        bool? footerCheckBoxChecked = false,
        string? footerCheckBoxText = null,
        TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None,
        string? footerText = null,
        bool hyperlinksEnabled = true,
        TaskDialogStartupLocation? startupLocation = null,
        TaskDialogProgressBarState progressBarState = TaskDialogProgressBarState.None,
        int progressbarMinValue = 0,
        int progressbarMaxValue = 0,
        int? progressbarCurrValue = null,
        Action<TaskDialog>? onOpened = null,
        TimeSpan timeout = default,
        Action? timeoutAction = null,
        Window? window = null,
        params TaskDialogControl[] controls)
    {
        _ = ex.ArgumentNotNull(nameof(ex));
        var message = ex.Message;
        var innerMessage = ex.GetBaseException().Message;
        return Error(instructionText,
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

    public static TaskDialogResult Exception(IException ex,
        string? caption = null,
        string? detailsExpandedLabel = null,
        string? detailsExpandedText = null,
        bool cancelable = false,
        string? detailsCollapsedLabel = null,
        bool detailsExpanded = false,
        bool? footerCheckBoxChecked = false,
        string? footerCheckBoxText = null,
        TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None,
        string? footerText = null,
        bool hyperlinksEnabled = true,
        TaskDialogStartupLocation? startupLocation = null,
        TaskDialogProgressBarState progressBarState = TaskDialogProgressBarState.None,
        int progressbarMinValue = 0,
        int progressbarMaxValue = 0,
        int? progressbarCurrValue = null,
        Action<TaskDialog>? onOpened = null,
        TimeSpan timeout = default,
        Action? timeoutAction = null,
        Window? window = null,
        params TaskDialogControl[] controls)
    {
        _ = ex.ArgumentNotNull();
        var message = ex.Message;
        var innerMessage = ex.GetBaseException()?.Message;
        return Error(ex.Instruction,
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
    public static TaskDialogResult ShowProgress(Action<TaskDialog> action,
        int maximum = 100,
        string? caption = null,
        string? instructionText = null,
        string? initializingText = null,
        string? detailsExpandedText = null,
        TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None,
        string? footerText = null,
        bool runInTask = true,
        bool enableOkButtonOnDone = true,
        bool showOkButton = true)
        => action is null
            ? throw new ArgumentNullException(nameof(action))
            : ShowProgress((dlg, _, __) => action(dlg),
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
    public static TaskDialogResult ShowProgress(Action<TaskDialog, Func<bool>> action,
        int maximum = 100,
        string? caption = null,
        string? instructionText = null,
        string? initializingText = null,
        string? detailsExpandedText = null,
        bool isCancallable = false,
        TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None,
        string? footerText = null,
        string? cancelButtonText = "Cancel",
        string? cancellingPromptText = "Cancelling...",
        bool runInTask = true,
        bool showOkButton = true,
        bool enableOkButtonOnDone = true)
        => action is null
            ? throw new ArgumentNullException(nameof(action))
            : ShowProgress((dlg, isCancellationRequested, isBackgroundWorking) => action(dlg, isCancellationRequested),
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
    public static TaskDialogResult ShowProgress(Action<TaskDialog, Func<bool>, Func<bool>> action,
        int maximum = 100,
        string? caption = null,
        string? instructionText = null,
        string? initializingText = null,
        string? detailsExpandedText = null,
        bool isCancallable = false,
        bool supportsBackgroundWorking = false,
        TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None,
        string? footerText = null,
        string? cancelButtonText = "Cancel",
        string? backgroundButtonText = "Background",
        string? cancellingPromptText = "Cancelling...",
        bool runInTask = false,
        bool showOkButton = false,
        bool enableOkButtonOnDone = true)
    {
        using var dlg = GetTaskDialog(action,
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
                enableOkButtonOnDone);
        return dlg.Show();
    }

    public static TaskDialogResult ShowProgress<TItem>(IEnumerable<TItem> items!!,
        Action<TaskDialog, TItem> onIterating!!,
        Action<int>? onEachIterated = null,
        Action<TaskDialog>? onIterationEnded = null,
        int? maximum = null,
        string? caption = null,
        string? instructionText = null,
        string? initializingText = null,
        string? detailsExpandedText = null,
        bool isCancallable = false,
        bool supportsBackgroundWorking = false,
        TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None,
        string? footerText = null,
        string? cancelButtonText = "Cancel",
        string? backgroundButtonText = "Background",
        string? cancellingPromptText = "Cancelling...",
        bool runInTask = true,
        bool showOkButton = true,
        bool enableOkButtonOnDone = true,
        bool runIterationAsParallel = false)
    {
        Action<TaskDialog, Func<bool>, Func<bool>> action = runIterationAsParallel
            ? ((dlg, isCancelled, isInBackground) =>
            {
                var tasks = TaskList.New();
                bool cancelled(TaskDialog dialog, Func<bool> isCancelledFunc) => tasks.IsCancellationRequested || dialog.IsCancelled(isCancelledFunc);
                var index = 0;
                foreach (var item in items)
                {
                    if (dlg.IsCancelled(isCancelled))
                    {
                        break;
                    }

                    _ = tasks.Run(() =>
                    {
                        if (!dlg.IsCancelled(isCancelled))
                        {
                            onIterating(dlg, item);
                        }
                    });
                    _ = Catch(() => dlg.Set(prograssValue: ++index));
                    onEachIterated?.Invoke(index);
                    if (cancelled(dlg, isCancelled))
                    {
                        break;
                    }
                }

                _ = tasks?.WaitAll();
                onIterationEnded?.Invoke(dlg);
            })
            : ((dlg, isCancelled, isInBackground) =>
            {
                bool cancelled(TaskDialog dialog, Func<bool> isCancelledFunc) => dialog.IsCancelled(isCancelledFunc);
                var index = 0;
                foreach (var item in items)
                {
                    if (dlg.IsCancelled(isCancelled))
                    {
                        break;
                    }

                    onIterating(dlg, item);
                    _ = Catch(() => dlg.Set(prograssValue: ++index));
                    onEachIterated?.Invoke(index);
                    if (cancelled(dlg, isCancelled))
                    {
                        break;
                    }
                }

                onIterationEnded?.Invoke(dlg);
            });
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

    public static TaskDialogResult ShowProgress<TItem>(IEnumerable<TItem> items!!,
        Action<TaskDialog, Func<bool>, Func<bool>, TItem> onIterating!!,
        Action<int>? onEachIterated = null,
        Action<TaskDialog>? onIterationEnded = null,
        int? maximum = null,
        string? caption = null,
        string? instructionText = null,
        string? initializingText = null,
        string? detailsExpandedText = null,
        bool isCancallable = false,
        bool supportsBackgroundWorking = false,
        TaskDialogStandardIcon footerIcon = TaskDialogStandardIcon.None,
        string? footerText = null,
        string? cancelButtonText = "Cancel",
        string? backgroundButtonText = "Background",
        string? cancellingPromptText = "Cancelling...",
        bool runInTask = true,
        bool showOkButton = true,
        bool enableOkButtonOnDone = true,
        bool runIterationAsParallel = false)
    {
        Action<TaskDialog, Func<bool>, Func<bool>> action = runIterationAsParallel
            ? ((dlg, isCancelled, isInBackground) =>
            {
                var tasks = TaskList.New();

                static bool cancelled(TaskDialog dialog, Func<bool> isCancelledFunc, TaskList? tasks) => tasks?.IsCancellationRequested ?? (false || dialog.IsCancelled(isCancelledFunc));
                var index = 0;
                foreach (var item in items)
                {
                    if (dlg.IsCancelled(isCancelled))
                    {
                        break;
                    }

                    _ = tasks.Run(() =>
                      {
                          if (dlg.IsCancelled(isCancelled))
                          {
                              return;
                          }

                          onIterating(dlg, isCancelled, isInBackground, item);
                      });
                    _ = Catch(() => dlg.Set(prograssValue: ++index));
                    onEachIterated?.Invoke(index);
                    if (cancelled(dlg, isCancelled, tasks))
                    {
                        break;
                    }
                }

                _ = tasks.WaitAll();
                onIterationEnded?.Invoke(dlg);
            })
            : ((dlg, isCancelled, isInBackground) =>
            {
                static bool cancelled(TaskDialog dialog, Func<bool> isCancelledFunc) => dialog.IsCancelled(isCancelledFunc);
                var index = 0;
                foreach (var item in items)
                {
                    if (dlg.IsCancelled(isCancelled))
                    {
                        break;
                    }

                    onIterating(dlg, isCancelled, isInBackground, item);
                    _ = Catch(() => dlg.Set(prograssValue: ++index));
                    onEachIterated?.Invoke(index);
                    if (cancelled(dlg, isCancelled))
                    {
                        break;
                    }
                }

                onIterationEnded?.Invoke(dlg);
            });
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

    public static IEnumerable<TaskDialogControl> ToButtons(params ButtonInfo[] buttons)
    {
        foreach (var button in buttons.Compact())
        {
            var control = button.Name.IsNullOrEmpty()
                                        ? new TaskDialogButton() { Text = button.Text }
                                        : new TaskDialogButton(button.Name, button.Text);
            control.Click += button.OnClick;
            yield return control;
        }
    }

    public static event EventHandler<ItemActedEventArgs<Window>>? WindowRequired;
}

public record ButtonInfo(string Text, EventHandler OnClick, string? Name = null);