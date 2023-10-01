using System.Windows.Shell;

using Library.DesignPatterns.Markers;

namespace Library.Wpf.Windows.UI;

public enum TaskbarProgressState
{
    /// <summary>
    /// No progress indicator is displayed in the taskbar button.
    /// </summary>
    None = 0,

    /// <summary>
    /// A pulsing green indicator is displayed in the taskbar button.
    /// </summary>
    Indeterminate = 1,

    /// <summary>
    /// A green progress indicator is displayed in the taskbar button.
    /// </summary>
    Normal = 2,

    /// <summary>
    /// A red progress indicator is displayed in the taskbar button.
    /// </summary>
    Error = 3,

    /// <summary>
    /// A yellow progress indicator is displayed in the taskbar button.
    /// </summary>
    Paused = 4
}

[Immutable]
public sealed class Taskbar
{
    public Taskbar(Window window)
    {
        this.Item = window.TaskbarItemInfo ??= new();
        this.ProgressBar = new(this);
    }

    public string? Description { get => this.Item.Description; set => this.Item.Description = value; }
    public TaskbarProgressBar ProgressBar { get; }
    internal TaskbarItemInfo Item { get; }

    public Taskbar HideProgressBar() =>
        this.Do(x => x.ProgressState = TaskbarItemProgressState.None);

    public Taskbar SetProgressBarToError() =>
            this.Do(x => x.ProgressState = TaskbarItemProgressState.Error);

    public Taskbar SetProgressBarToIndeterminate() =>
        this.Do(x => x.ProgressState = TaskbarItemProgressState.Indeterminate);

    public Taskbar SetProgressBarToNormal() =>
        this.Do(x => x.ProgressState = TaskbarItemProgressState.Normal);

    public Taskbar SetProgressBarToPaused() =>
        this.Do(x => x.ProgressState = TaskbarItemProgressState.Paused);

    public Taskbar SetProgressBarValue(double value, double max = 100) =>
        this.Do(x => x.ProgressValue = value * 1 / max);

    private Taskbar Do(Action<TaskbarItemInfo> action)
    {
        action(this.Item);
        return this;
    }
}

public sealed class TaskbarProgressBar : FluentClass<TaskbarProgressBar>
{
    private readonly Taskbar _taskbar;

    internal TaskbarProgressBar(Taskbar taskbar) => this._taskbar = taskbar;

    public TaskbarProgressBar SetState(TaskbarProgressState state) =>
        this.Do(() =>
        {
            this._taskbar.Item.ProgressState = EnumHelper.Convert<TaskbarItemProgressState>(state);
        });
}

public abstract class FluentClass<TSelf>
    where TSelf : FluentClass<TSelf>
{
    protected TSelf Do(Action action)
    {
        action();
        return (TSelf)this;
    }
}