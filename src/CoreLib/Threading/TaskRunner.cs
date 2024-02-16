using System.Diagnostics;

using Library.Exceptions;
using Library.Results;

namespace Library.Threading;

//[DebuggerStepThrough, StackTraceHidden]
public sealed class TaskRunner : TaskRunnerBase<TaskRunner, Result>
{
    private readonly List<Func<CancellationToken, Task>> _funcList = new();
    private readonly Func<CancellationToken, Task> _start;

    private TaskRunner([DisallowNull] Func<CancellationToken, Task> start, IEnumerable<Func<CancellationToken, Task>>? funcs = null)
    {
        Checker.MustBeArgumentNotNull(start);
        this._start = start;
        if (funcs?.Any() ?? false)
        {
            this._funcList.AddRange(funcs);
        }
    }

    public static TaskRunner StartWith(Func<CancellationToken, Task> start) =>
        new(start);

    public static TaskRunner StartWith(Func<Task> start) =>
        StartWith(c => start());

    public static TaskRunner StartWith(Action start) =>
        StartWith(c => start.ToAsync(c));

    // Auxiliary method
    public static TaskRunner<TState> StartWith<TState>(TState state) =>
        TaskRunner<TState>.StartWith(state);

    public TaskRunner Then([DisallowNull] Func<CancellationToken, Task> func)
    {
        CheckIfNotRunning();

        Checker.MustBeArgumentNotNull(func);
        this._funcList.Add(func);
        return this;
    }

    public TaskRunner Then(Func<Task> func) =>
        this.Then(new Func<CancellationToken, Task>(_ => func()));

    public TaskRunner Then(Action func) =>
        this.Then(new Func<CancellationToken, Task>([DebuggerStepThrough] (_) =>
        {
            func();
            return Task.CompletedTask;
        }));

    protected override Result GetErrorResult(Exception exception) =>
        Result.CreateFailure(exception);

    protected override async Task<Result> OnRunningAsync(CancellationToken token)
    {
        await this._start(token);
        foreach (var func in this._funcList.Compact())
        {
            if (token.IsCancellationRequested)
            {
                return Result.CreateFailure(new OperationCanceledException(token));
            }

            await func(token);
        }
        return Result.CreateSuccess();
    }
}