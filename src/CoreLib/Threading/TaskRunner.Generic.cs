using Library.Exceptions;
using Library.Results;
using Library.Validations;

namespace Library.Threading;

public sealed class TaskRunner<TArg> : TaskRunnerBase<TaskRunner<TArg?>, Result<TArg?>>
{
    private readonly List<Func<TArg, CancellationToken, Task<TArg>>> _funcList = new();
    private readonly Func<CancellationToken, Task<TArg>> _start;

    private TaskRunner([DisallowNull] Func<CancellationToken, Task<TArg>> start) =>
        this._start = start.ArgumentNotNull();

    public static TaskRunner<TArg> StartWith(Func<CancellationToken, Task<TArg>> start) =>
        new(start);

    public static TaskRunner<TArg> StartWith(Func<Task<TArg>> start) =>
        StartWith(c => start());

    public static TaskRunner<TArg> StartWith(Func<TArg> start) =>
        StartWith(c => Task.FromResult(start()));

    public static TaskRunner<TArg> StartWith(TArg state) =>
        StartWith(c => Task.FromResult(state));

    public TaskRunner<TArg> Then([DisallowNull] Func<TArg, CancellationToken, Task<TArg>> func)
    {
        Checker.MustBe(!this.IsRunning, () => new CommonException());
        Checker.MustBeArgumentNotNull(func);
        this._funcList.Add(func);
        return this;
    }

    public TaskRunner<TArg> Then([DisallowNull] Func<TArg, CancellationToken, Task> func) =>
        this.Then(new Func<TArg, CancellationToken, Task<TArg>>(async (x, t) =>
        {
            if (!t.IsCancellationRequested)
            {
                await func(x, t);
            }

            return x;
        }));

    public TaskRunner<TArg> Then(Func<TArg, Task<TArg>> func) =>
        this.Then(new Func<TArg, CancellationToken, Task<TArg>>((x, _) => func(x)));

    public TaskRunner<TArg> Then(Func<TArg, Task> func) =>
        this.Then(new Func<TArg, CancellationToken, Task<TArg>>(async (x, t) =>
        {
            if (!t.IsCancellationRequested)
            {
                await func(x);
            }

            return x;
        }));

    public TaskRunner<TArg> Then(Func<Task> func) =>
        this.Then(new Func<TArg, CancellationToken, Task<TArg>>(async (x, t) =>
        {
            if (!t.IsCancellationRequested)
            {
                await func();
            }

            return x;
        }));

    public TaskRunner<TArg> Then(Func<CancellationToken, Task> func) =>
        this.Then(new Func<TArg, CancellationToken, Task<TArg>>(async (x, t) =>
        {
            if (!t.IsCancellationRequested)
            {
                await func(t);
            }

            return x;
        }));

    public TaskRunner<TArg> Then(Action<TArg> func) =>
        this.Then(new Func<TArg, CancellationToken, Task<TArg>>((x, t) =>
        {
            if (!t.IsCancellationRequested)
            {
                func(x);
            }

            return Task.FromResult(x);
        }));

    public TaskRunner<TArg> Then(Func<TArg, TArg> func) =>
        this.Then(new Func<TArg, CancellationToken, Task<TArg>>((x, t) => Task.FromResult(func(x))));

    public TaskRunner<TArg> Then(Action func) =>
        this.Then(new Func<TArg, CancellationToken, Task<TArg>>((x, t) =>
        {
            if (!t.IsCancellationRequested)
            {
                func();
            }

            return Task.FromResult(x);
        }));

    protected override Result<TArg?> GetErrorResult(Exception exception) =>
        Result<TArg?>.CreateFailure(exception);

    protected override async Task<Result<TArg?>> OnRunningAsync(CancellationToken token)
    {
        var state = await this._start(token);
        foreach (var func in this._funcList.Compact())
        {
            if (token.IsCancellationRequested)
            {
                return Result<TArg?>.CreateFailure(new OperationCanceledException(token), state);
            }

            state = await func(state, token);
        }
        return Result<TArg?>.CreateSuccess(state);
    }
}