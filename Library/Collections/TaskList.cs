namespace Library.Collections;
public sealed class TaskList : List<Task>, IDisposable
{
    private readonly CancellationTokenSource _CancellationTokenSource;
    private bool _DisposedValue;

    public TaskList() => this._CancellationTokenSource = new();
    public static TaskList New() => new();

    public TaskList(IEnumerable<Task> tasks, CancellationTokenSource cancellationTokenSource)
        : base(tasks)
        => this._CancellationTokenSource = cancellationTokenSource;
    public TaskList(IEnumerable<Task> items)
        : base(items)
        => this._CancellationTokenSource = new();
    public TaskList(int capacity)
        : base(capacity)
        => this._CancellationTokenSource = new();
    public bool IsCancellationRequested => this._CancellationTokenSource.IsCancellationRequested;

    public bool WaitAll(TimeSpan timeout)
        => Task.WaitAll(this.ToArray(), timeout);

    public TaskList WaitAll()
    {
        Task.WaitAll(this.ToArray(), this._CancellationTokenSource.Token);
        return this;
    }

    public async Task<TaskList> WaitAllAsync() => await Task.Run(() => this.WaitAll());

    public TaskList WaitAny()
    {
        _ = Task.WaitAny(this.ToArray());
        return this;
    }

    public async Task<TaskList> WaitAnyAsync() => await Task.Run(() => this.WaitAny());

    public TaskList Run(Action action)
    {
        this.Add(Task.Run(action.ArgumentNotNull(), this._CancellationTokenSource.Token));
        return this;
    }

    public TaskList Run(Func<Task> action)
    {
        this.Add(action.ArgumentNotNull()());
        return this;
    }
    public async Task<TaskList> RunAsync(Func<Task> action)
    {
        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        var task = action();
        await task;
        this.Add(task);
        return this;
    }

    public async Task<TaskList> RunAsync(Action action) => await Task.Run(() => this.Run(action));

    public TaskList Run(params Action[] actions)
    {
        foreach (var action in actions)
        {
            _ = this.Run(action);
        }
        return this;
    }

    public TaskList Run(IEnumerable<Action> actions) => this.Run(actions.ToArray());

    public void CancelAll() => this._CancellationTokenSource.Cancel();

    private void Dispose(bool disposing)
    {
        if (!this._DisposedValue)
        {
            if (disposing)
            {
                this._CancellationTokenSource.Dispose();
            }
            this._DisposedValue = true;
        }
    }

    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public new int IndexOf(Task item) => throw new NotImplementedException();
    public new void Insert(int index, Task item) => throw new NotImplementedException();
    public new void RemoveAt(int index) => throw new NotImplementedException();
    public new void Add(Task item) => throw new NotImplementedException();
    public new void Clear() => throw new NotImplementedException();
    public new bool Contains(Task item) => throw new NotImplementedException();
    public new void CopyTo(Task[] array, int arrayIndex) => throw new NotImplementedException();
    public new bool Remove(Task item) => throw new NotImplementedException();
}