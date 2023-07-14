using System.Diagnostics;

using Library.DesignPatterns.Markers;
using Library.Exceptions;
using Library.Results;
using Library.Validations;

namespace Library.Threading.MultistepProgress;

[Fluent]
[DebuggerStepThrough]
[StackTraceHidden]
public sealed class TaskRunner<TArg> : TaskRunnerBase<TaskRunner<TArg?>, Result<TArg?>>
{
    private readonly List<Func<TArg, CancellationToken, Task<TArg>>> _funcList = new();
    private readonly Func<CancellationToken, Task<TArg>> _start;

    private TaskRunner([DisallowNull] Func<CancellationToken, Task<TArg>> start)
    {
        Check.IfArgumentNotNull(start);
        this._start = start;
    }

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
        Check.If(this.IsRunning, () => new CommonException());
        Check.IfArgumentNotNull(func);
        this._funcList.Add(func);
        return this;
    }

    public TaskRunner<TArg> Then([DisallowNull] Func<TArg, CancellationToken, Task> func) =>
        this.Then(new Func<TArg, CancellationToken, Task<TArg>>(async (x, t) =>
        {
            await func(x, t);
            return x;
        }));

    public TaskRunner<TArg> Then(Func<TArg?, Task<TArg>> func) =>
        this.Then(new Func<TArg, CancellationToken, Task<TArg>>((x, _) => func(x)));

    public TaskRunner<TArg> Then(Func<TArg?, Task> func) =>
        this.Then(new Func<TArg, CancellationToken, Task<TArg>>(async (x, _) =>
        {
            await func(x);
            return x;
        }));

    public TaskRunner<TArg> Then(Func<Task> func) =>
        this.Then(new Func<TArg, CancellationToken, Task<TArg>>(async (x, _) =>
        {
            await func();
            return x;
        }));

    public TaskRunner<TArg> Then(Action<TArg> func) =>
        this.Then(new Func<TArg, CancellationToken, Task<TArg>>((x, t) =>
        {
            func(x);
            return Task.FromResult(x);
        }));

    public TaskRunner<TArg> Then(Action func) =>
        this.Then(new Func<TArg, CancellationToken, Task<TArg>>((x, t) =>
        {
            func();
            return Task.FromResult(x);
        }));

    protected override Result<TArg?> GetErrorResult(Exception exception) =>
        Result<TArg?>.CreateFailure(exception);

    protected override async Task<Result<TArg?>> OnRuningAsync(CancellationToken token)
    {
        var state = await this._start(token);
        foreach (var func in this._funcList.Compact())
        {
            if (token.IsCancellationRequested)
            {
                Throw<OperationCancelException>();
            }

            state = await func(state, token);
        }
        return Result<TArg?>.CreateSuccess(state);
    }
}