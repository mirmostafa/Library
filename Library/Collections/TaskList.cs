using Library.Validations;

namespace Library.Collections;
public sealed class TaskList : FluentListBase<Task, TaskList>, IDisposable, IEnumerable<Task>
{
    private readonly CancellationTokenSource _CancellationTokenSource;
    private bool _disposedValue;

    public TaskList(List<Task> list) : base(list)
        => this._CancellationTokenSource = new CancellationTokenSource();

    public TaskList(IEnumerable<Task> list) : base(list)
        => this._CancellationTokenSource = new CancellationTokenSource();

    public TaskList(int capacity) : base(capacity)
        => this._CancellationTokenSource = new CancellationTokenSource();

    public TaskList()
        => this._CancellationTokenSource = new CancellationTokenSource();

    public static TaskList New() => new();

    public bool IsCancellationRequested => this._CancellationTokenSource.IsCancellationRequested;

    public bool WaitAll(TimeSpan timeout)
        => Task.WaitAll(this.ToArray(), timeout);

    public TaskList WaitAll()
    {
        Task.WaitAll(this.ToArray(), this._CancellationTokenSource.Token);
        return this;
    }

    public async Task WaitAllAsync()
        => await Task.WhenAll(this);

    public TaskList WaitAny()
    {
        _ = Task.WaitAny(this.ToArray());
        return this;
    }

    public async Task<TaskList> WaitAnyAsync()
        => await Task.Run(() => this.WaitAny());

    public TaskList Run(Action action)
        => this.Add(Task.Run(action.ArgumentNotNull(), this._CancellationTokenSource.Token));

    public TaskList Run(Func<Task> action)
        => this.Add(action.ArgumentNotNull()());

    public async Task<TaskList> RunAsync(Func<Task> action)
    {
        var task = action.ArgumentNotNull()();
        await task;
        return this.Add(task);
    }

    public async Task<TaskList> RunAsync(Action action)
        => await Task.Run(() => this.Run(action));

    public TaskList Run(params Action[] actions)
    {
        foreach (var action in actions)
        {
            _ = this.Run(action);
        }
        return this;
    }

    public TaskList Run(IEnumerable<Action> actions)
        => this.Run(actions.ToArray());

    public void CancelAll()
        => this._CancellationTokenSource.Cancel();

    private void Dispose(bool disposing)
    {
        if (!this._disposedValue)
        {
            if (disposing)
            {
                this._CancellationTokenSource.Dispose();
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