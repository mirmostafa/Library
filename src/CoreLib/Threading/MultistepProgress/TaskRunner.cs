using System.Diagnostics;

using Library.DesignPatterns.Markers;
using Library.Exceptions;
using Library.Results;
using Library.Validations;

namespace Library.Threading.MultistepProgress;

[Fluent]
[DebuggerStepThrough]
[StackTraceHidden]
public sealed class TaskRunner<TArg>
{
    private readonly List<Func<TArg, CancellationToken, Task<TArg>>> _funcList = new();
    private readonly Func<CancellationToken, Task<TArg>> _start;
    private bool _isRunning;
    private Action<Result<TArg?>>? _onEnded;

    private TaskRunner([DisallowNull] Func<CancellationToken, Task<TArg>> start, IEnumerable<Func<TArg, CancellationToken, Task<TArg>>>? funcs = null)
    {
        Check.IfArgumentNotNull(start);
        this._start = start;
        if (funcs?.Any() ?? false)
        {
            this._funcList.AddRange(funcs);
        }
    }

    public static TaskRunner<TArg> StartWith(Func<CancellationToken, Task<TArg>> start) =>
        new(start);

    public static TaskRunner<TArg> StartWith(Func<Task<TArg>> start) =>
        StartWith(c => start());

    public static TaskRunner<TArg> StartWith(Func<TArg> start) =>
        StartWith(c => Task.FromResult(start()));

    public static TaskRunner<TArg> StartWith(TArg state) =>
        StartWith(c => Task.FromResult(state));

    public TaskRunner<TArg> OnEnded(Action<Result<TArg?>>? action) =>
        this.Fluent(this._onEnded = action);

    public async Task<Result<TArg?>> RunAsync(CancellationToken token = default)
    {
        this._isRunning = true;
        TArg? state = default;
        Result<TArg?> result = default!;
        try
        {
            state = await this._start(token);
            foreach (var func in this._funcList.Compact())
            {
                if (token.IsCancellationRequested)
                {
                    Throw<OperationCancelException>();
                }

                state = await func(state, token);
            }
            result = Result<TArg?>.CreateSuccess(state);
        }
        catch (Exception ex)
        {
            result = Result<TArg?>.CreateFailure(ex, state);
        }
        finally
        {
            this._isRunning = false;
            this._onEnded?.Invoke(result);
        }
        return result;
    }

    public TaskRunner<TArg> Then([DisallowNull] Func<TArg, CancellationToken, Task<TArg>> func)
    {
        Check.If(this._isRunning, () => new CommonException());
        Check.IfArgumentNotNull(func);
        this._funcList.Add(func);
        return this;
    }

    public TaskRunner<TArg> Then(Func<TArg, Task<TArg>> func) =>
        this.Then(new Func<TArg, CancellationToken, Task<TArg>>((x, _) => func(x)));

    public TaskRunner<TArg> Then(Func<TArg, Task> func) =>
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
}

[Fluent]
[DebuggerStepThrough]
[StackTraceHidden]
public sealed class TaskRunner
{
    private readonly List<Func<CancellationToken, Task>> _funcList = new();
    private readonly Func<CancellationToken, Task> _start;
    private bool _isRunning;
    private Action<Result>? _onEnded;

    private TaskRunner([DisallowNull] Func<CancellationToken, Task> start, IEnumerable<Func<CancellationToken, Task>>? funcs = null)
    {
        Check.IfArgumentNotNull(start);
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

    public TaskRunner OnEnded(Action<Result>? action) =>
        this.Fluent(this._onEnded = action);

    public async Task<Result> RunAsync(CancellationToken token = default)
    {
        this._isRunning = true;
        Result result = default!;
        try
        {
            await this._start(token);
            foreach (var func in this._funcList.Compact())
            {
                if (token.IsCancellationRequested)
                {
                    Throw<OperationCancelException>();
                }

                await func(token);
            }
            result = Result.CreateSuccess();
        }
        catch (Exception ex)
        {
            result = Result.CreateFailure(ex);
        }
        finally
        {
            this._isRunning = false;
            this._onEnded?.Invoke(result);
        }
        return result;
    }

    public TaskRunner Then([DisallowNull] Func<CancellationToken, Task> func)
    {
        Check.If(this._isRunning, () => new CommonException());
        Check.IfArgumentNotNull(func);
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
}