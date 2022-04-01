using Library.Validations;

namespace Library.Collections;
public sealed class TaskList : FluentListBase<Task, TaskList>, IDisposable, IEnumerable<Task>
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private bool _disposedValue;

    public TaskList(List<Task> list) : base(list) =>
        this._cancellationTokenSource = new CancellationTokenSource();
    public TaskList(IEnumerable<Task> list) : base(list) =>
        this._cancellationTokenSource = new CancellationTokenSource();
    public TaskList(int capacity) : base(capacity) =>
        this._cancellationTokenSource = new CancellationTokenSource();
    public TaskList() =>
        this._cancellationTokenSource = new CancellationTokenSource();
    public TaskList([DisallowNull] CancellationTokenSource cancellationTokenSource) =>
        this._cancellationTokenSource = cancellationTokenSource.ArgumentNotNull();

    public static TaskList New() =>
        new();

    public bool IsCancellationRequested => this.CancellationTokenSource.IsCancellationRequested;
    private CancellationTokenSource CancellationTokenSource => this.This()._cancellationTokenSource;

    private TaskList This() =>
        this.ThrowIfDisposed(this._disposedValue);

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

    public async Task WhenAllAsync() =>
        await Task.WhenAll(this.This());

    public TaskList WaitAny()
    {
        _ = Task.WaitAny(this.This().ToArray());
        return this;
    }

    public async Task<TaskList> WaitAnyAsync()
        => await Task.Run(() => this.This().WaitAny());

    public TaskList Run(Action action)
        => this.Add(Task.Run(action.ArgumentNotNull(nameof(action)), this.CancellationTokenSource.Token));

    public TaskList Run(Func<Task> action)
        => this.This().Add(action.ArgumentNotNull(nameof(action))());

    //public async Task<TaskList> RunAsync(Func<Task> action)
    //{
    //    Check.ThrowIfDisposed(this, this._disposedValue);
    //    var task = action.ArgumentNotNull(nameof(action))();
    //    await task;
    //    return this.Add(task);
    //}

    //public async Task<TaskList> RunAsync(Action action)
    //    => await Task.Run(() => this.Run(action));

    public TaskList Run(params Action[] actions)
    {
        foreach (var action in actions)
        {
            _ = this.This().Run(action);
        }
        return this.This();
    }

    public TaskList Run(IEnumerable<Action> actions)
        => this.This().Run(actions.ToArray());

    public void CancelAll() =>
        this.CancellationTokenSource.Cancel();

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

    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}