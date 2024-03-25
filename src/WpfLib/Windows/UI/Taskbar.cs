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
    private static Taskbar? _mainWindow;

    public Taskbar(Window window)
    {
        this.TaskbarItem = window.TaskbarItemInfo ??= new();
        this.ProgressBar = new(this);
    }

    public static Taskbar MainWindow => _mainWindow ??= new(Application.Current.MainWindow);

    public string? Description { get => this.TaskbarItem.Description; set => this.TaskbarItem.Description = value; }
    public TaskbarProgressBar ProgressBar { get; }
    internal TaskbarItemInfo TaskbarItem { get; }
}

public sealed class TaskbarProgressBar : FluentClass<TaskbarProgressBar>
{
    private readonly Taskbar _taskbar;

    internal TaskbarProgressBar(Taskbar taskbar) =>
        this._taskbar = taskbar;

    public TaskbarProgressBar SetState(TaskbarProgressState value) =>
        this.Do(() => this._taskbar.TaskbarItem.ProgressState = EnumHelper.Convert<TaskbarItemProgressState>(value));

    public TaskbarProgressBar SetValue(double value, double max = 100) =>
        this.Do(() => this._taskbar.TaskbarItem.ProgressValue = value * 1 / max);
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