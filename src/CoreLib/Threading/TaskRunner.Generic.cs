using Library.Results;
using Library.Validations;

namespace Library.Threading;

public sealed class TaskRunner<TState> : TaskRunnerBase<TaskRunner<TState?>, Result<TState?>>
{
    private readonly List<Func<TState, CancellationToken, Task<TState>>> _funcList = [];
    private readonly Func<CancellationToken, Task<TState>> _start;

    private TaskRunner([DisallowNull] Func<CancellationToken, Task<TState>> start) =>
        this._start = start.ArgumentNotNull();

    public static TaskRunner<TState> StartWith(Func<CancellationToken, Task<TState>> start) =>
        new(start);

    public static TaskRunner<TState> StartWith(Func<Task<TState>> start) =>
        StartWith(c => start());

    public static TaskRunner<TState> StartWith(Func<TState> start) =>
        StartWith(c => Task.FromResult(start()));

    public static TaskRunner<TState> StartWith(TState state) =>
        StartWith(c => Task.FromResult(state));

    public TaskRunner<TState> Then([DisallowNull] Func<TState, CancellationToken, Task<TState>> func)
    {
        this.CheckIfNotRunning();

        Checker.MustBeArgumentNotNull(func);
        this._funcList.Add(func);
        return this;
    }

    public TaskRunner<TState> Then([DisallowNull] Func<TState, CancellationToken, Task> func) =>
        this.Then(new Func<TState, CancellationToken, Task<TState>>(async (x, t) =>
        {
            if (!t.IsCancellationRequested)
            {
                await func(x, t);
            }

            return x;
        }));

    public TaskRunner<TState> Then(Func<TState, Task<TState>> func) =>
        this.Then(new Func<TState, CancellationToken, Task<TState>>((x, _) => func(x)));

    public TaskRunner<TState> Then(Func<TState, Task> func) =>
        this.Then(new Func<TState, CancellationToken, Task<TState>>(async (x, t) =>
        {
            if (!t.IsCancellationRequested)
            {
                await func(x);
            }

            return x;
        }));

    public TaskRunner<TState> Then(Func<Task> func) =>
        this.Then(new Func<TState, CancellationToken, Task<TState>>(async (x, t) =>
        {
            if (!t.IsCancellationRequested)
            {
                await func();
            }

            return x;
        }));

    public TaskRunner<TState> Then(Func<CancellationToken, Task> func) =>
        this.Then(new Func<TState, CancellationToken, Task<TState>>(async (x, t) =>
        {
            if (!t.IsCancellationRequested)
            {
                await func(t);
            }

            return x;
        }));

    public TaskRunner<TState> Then(Action<TState> func) =>
        this.Then(new Func<TState, CancellationToken, Task<TState>>((x, t) =>
        {
            if (!t.IsCancellationRequested)
            {
                func(x);
            }

            return Task.FromResult(x);
        }));

    public TaskRunner<TState> Then(Func<TState, TState> func) =>
        this.Then(new Func<TState, CancellationToken, Task<TState>>((x, t) => Task.FromResult(func(x))));

    public TaskRunner<TState> Then(Action func) =>
        this.Then(new Func<TState, CancellationToken, Task<TState>>((x, t) =>
        {
            if (!t.IsCancellationRequested)
            {
                func();
            }

            return Task.FromResult(x);
        }));

    protected override Result<TState?> GetErrorResult(Exception exception) =>
        Result<TState?>.CreateFailure(exception);

    protected override async Task<Result<TState?>> OnRunningAsync(CancellationToken token)
    {
        var state = await this._start(token);
        foreach (var func in this._funcList.Compact())
        {
            if (token.IsCancellationRequested)
            {
                return Result<TState?>.CreateFailure(new OperationCanceledException(token), state);
            }

            state = await func(state, token);
            if (state is Result<TState?> r and { IsSucceed: false })
            {
                return r;
            }
        }
        return state is Result<TState?> result
            ? result
            : Result<TState?>.CreateSuccess(state);
    }
}