using Library.Validations;

namespace Library.Collections;

/// <summary>
/// Represents a list of tasks that can be cancelled and provides methods to run, wait for, and dispose of tasks.
/// </summary>
public sealed class TaskList : FluentListBase<Task, TaskList>, IDisposable, IEnumerable<Task>
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private bool _disposedValue;

    public TaskList(List<Task> list) : base(list)
        => this._cancellationTokenSource = new CancellationTokenSource();

    public TaskList(IEnumerable<Task> list) : base(list)
        => this._cancellationTokenSource = new CancellationTokenSource();

    public TaskList(int capacity) : base(capacity)
        => this._cancellationTokenSource = new CancellationTokenSource();

    public TaskList()
        => this._cancellationTokenSource = new CancellationTokenSource();

    public TaskList([DisallowNull] CancellationTokenSource cancellationTokenSource)
        => this._cancellationTokenSource = cancellationTokenSource.ArgumentNotNull();

    public bool IsCancellationRequested => this.CancellationTokenSource.IsCancellationRequested;

    private CancellationTokenSource CancellationTokenSource => this.This()._cancellationTokenSource;

    public static TaskList New()
        => new();

    public void CancelAll()
        => this.CancellationTokenSource.Cancel();

    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public TaskList Run(Action action)
        => this.Add(Task.Run(action.ArgumentNotNull(nameof(action)), this.CancellationTokenSource.Token));

    public TaskList Run(Func<Task> action)
        => this.This().Add(action.ArgumentNotNull(nameof(action))());

    public TaskList WaitAll(TimeSpan timeout)
    {
        _ = Task.WaitAll(this.This().ToArray(), timeout);
        return this;
    }

    public TaskList WaitAll()
    {
        Task.WaitAll(this.ToArray(), this.CancellationTokenSource.Token);
        return this;
    }

    public async Task<TaskList> WaitAllAsync()
    {
        await Task.Run(() => Task.WaitAll(this.ToArray(), this.CancellationTokenSource.Token));
        return this;
    }

    public TaskList WaitAny()
    {
        _ = Task.WaitAny(this.This().ToArray());
        return this;
    }

    public async Task<TaskList> WaitAnyAsync()
        => await Task.Run(() => this.This().WaitAny());

    public async Task WhenAllAsync()
        => await TaskHelper.WhenAllAsync(this.This());

    private void Dispose(bool disposing)
    {
        if (!this._disposedValue)
        {
            if (disposing)
            {
                this.CancellationTokenSource.Dispose();
            }
            this._disposedValue = true;
        }
    }

    private TaskList This()
        => this.ThrowIfDisposed(this._disposedValue);
}