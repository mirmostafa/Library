using Library.Logging;
using Library.Results;
using Library.Validations;

namespace Library.MultistepProgress;

public sealed class TaskRunner<TArg> : ILoggerContainer
{
    private readonly CancellationToken? _cancellationToken;
    private readonly List<Func<TArg?, Task<TArg?>>> _taskList;
    private bool _continueOnException;
    private bool _isRunning;
    private Action<(TaskRunner<TArg> TaskRunner, int TaskIndex, int TaskCount)>? _onCancellationRequested;
    private Action<(TaskRunner<TArg> TaskRunner, int TaskIndex, int TaskCount, Result<TArg?>)>? _onEnded;
    private Action<(TaskRunner<TArg> TaskRunner, Exception exception, int TaskIndex, int TaskCount)>? _onExceptionOccurred;
    private Action<(TaskRunner<TArg> TaskRunner, int TaskCount)>? _onStarting;
    private Action<(TaskRunner<TArg> TaskRunner, int TaskIndex, int TaskCount)>? _onStepped;
    private Action<(TaskRunner<TArg> TaskRunner, int TaskIndex, int TaskCount)>? _onStepping;
    private Func<Task<TArg?>> _startWith = null!;

    public TaskRunner(ILogger? logger = null, CancellationToken? cancellationToken = default)
    {
        this._taskList = new();
        this._cancellationToken = cancellationToken;
        this.Logger = logger ?? ILogger.Empty;
    }

    public ILogger Logger { get; }

    public static TaskRunner<TArg> New(ILogger? logger = null, CancellationToken? cancellationToken = default)
            => new(logger, cancellationToken);

    public TaskRunner<TArg?> ContinueOnException(bool continueOnException = true)
    {
        _ = Check.MustBe(!this._isRunning).ThrowOnFail(this);
        this._continueOnException = continueOnException;
        return this;
    }

    public TaskRunner<TArg> OnCancellationRequested(Action<(TaskRunner<TArg> TaskRunner, int TaskIndex, int TaskCount)> handler)
    {
        _ = Check.MustBe(!this._isRunning).ThrowOnFail(this);
        this._onCancellationRequested = handler;
        return this;
    }

    public TaskRunner<TArg> OnEnded(Action<(TaskRunner<TArg> TaskRunner, int TaskIndex, int TaskCount, Result<TArg?>)> handler)
    {
        _ = Check.MustBe(!this._isRunning).ThrowOnFail(this);
        this._onEnded = handler;
        return this;
    }

    public TaskRunner<TArg> OnExceptionOccurred(Action<(TaskRunner<TArg> TaskRunner, Exception exception, int TaskIndex, int TaskCount)> handler)
    {
        _ = Check.MustBe(!this._isRunning).ThrowOnFail(this);
        this._onExceptionOccurred = handler;
        return this;
    }

    public TaskRunner<TArg> OnStarting(Action<(TaskRunner<TArg> TaskRunner, int TaskCount)> handler)
    {
        _ = Check.MustBe(!this._isRunning).ThrowOnFail(this);
        this._onStarting = handler;
        return this;
    }

    public TaskRunner<TArg> OnStepped(Action<(TaskRunner<TArg> TaskRunner, int TaskIndex, int TaskCount)> handler)
    {
        _ = Check.MustBe(!this._isRunning).ThrowOnFail(this);
        this._onStepped = handler;
        return this;
    }

    public TaskRunner<TArg> OnStepping(Action<(TaskRunner<TArg> TaskRunner, int TaskIndex, int TaskCount)> handler)
    {
        _ = Check.MustBe(!this._isRunning).ThrowOnFail(this);
        this._onStepping = handler;
        return this;
    }

    /// <summary>
    /// Runs the asynchronous.
    /// </summary>
    /// <returns></returns>
    public async Task<Result<TArg?>> RunAsync()
    {
        _ = Check.MustBe(!this._isRunning).ThrowOnFail(this);

        var count = this._taskList.Count + 1;
        var index = 0;
        var token = this._cancellationToken;
        Result<TArg?> result = null!;

        this._isRunning = true;
        this._onStarting?.Invoke((this, count));
        var arg = await this._startWith();
        for (; index < this._taskList.Count; index++)
        {
            if (token is not null and { CanBeCanceled: true } and { IsCancellationRequested: true })
            {
                this._onCancellationRequested?.Invoke((this, index, count));
                result = Result<TArg>.CreateFail(value: arg, error: new OperationCanceledException());
                break;
            }
            var task = this._taskList[index];
            try
            {
                this._onStepping?.Invoke((this, index, count));
                arg = await task(arg);
                this._onStepped?.Invoke((this, index, count));
            }
            catch (Exception ex)
            {
                this._onExceptionOccurred?.Invoke((this, ex, index, count));
                if (!this._continueOnException)
                {
                    result = Result<TArg>.CreateFail(value: arg, error: ex);
                    break;
                }
            }
        }
        this._isRunning = false;
        result ??= Result<TArg?>.CreateSuccess(arg);
        this._onEnded?.Invoke((this, index, count, result));
        this._isRunning = false;
        return result;
    }

    public TaskRunner<TArg?> StartWith(TArg? arg)
        => this.StartWith(() => Task.FromResult(arg));

    public TaskRunner<TArg?> StartWith(Func<Task<TArg?>> getArg)
    {
        _ = Check.MustBe(!this._isRunning).ThrowOnFail(this);
        this._startWith = getArg;
        return this;
    }

    public TaskRunner<TArg?> Then(Func<TArg?, Task<TArg?>> task)
    {
        this._taskList.Add(task);
        return this;
    }
}