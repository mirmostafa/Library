﻿using Library;
using Library.Logging;
using Library.Results;
using Library.Threading.MultistepProgress;
using Library.Validations;

namespace Library.Threading.MultistepProgress;

public sealed class TaskRunner<TArg> : ILoggerContainer
{
    private readonly CancellationToken? _cancellationToken;
    private readonly string _name;
    private readonly List<Func<TArg?, Task<TArg?>>> _taskList;
    private bool _continueOnException;
    private bool _isRunning;
    private Action<(TaskRunner<TArg> TaskRunner, int TaskIndex, int TaskCount)>? _onCancellationRequested;
    private Action<(TaskRunner<TArg> TaskRunner, int TaskIndex, int TaskCount, Result<TArg?>)>? _onEnded;
    private Action<(TaskRunner<TArg> TaskRunner, int TaskIndex, int TaskCount, Exception exception)>? _onExceptionOccurred;
    private Action<(TaskRunner<TArg> TaskRunner, int TaskIndex, int TaskCount)>? _onStarting;
    private Action<(TaskRunner<TArg> TaskRunner, int TaskIndex, int TaskCount)>? _onStepped;
    private Action<(TaskRunner<TArg> TaskRunner, int TaskIndex, int TaskCount)>? _onStepping;
    private Func<Task<TArg?>> _startWith = null!;

    public TaskRunner(ILogger? logger = null, string name = null, CancellationToken? cancellationToken = default)
    {
        this._taskList = new();
        this._cancellationToken = cancellationToken;
        this.Logger = logger ?? ILogger.Empty;
        this._name = name;
    }

    public ILogger Logger { get; }

    public static TaskRunner<TArg> New(ILogger? logger = null, string name = null, CancellationToken? cancellationToken = default)
        => new(logger, name, cancellationToken);

    public TaskRunner<TArg?> ContinueOnException(bool continueOnException = true)
    {
        _ = Check.MustBe(!this._isRunning).ThrowOnFail(this);
        this._continueOnException = continueOnException;
        return this;
    }

    public string GetName() => this._name;

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

    public TaskRunner<TArg> OnExceptionOccurred(Action<(TaskRunner<TArg> TaskRunner, int TaskIndex, int TaskCount, Exception exception)> handler)
    {
        _ = Check.MustBe(!this._isRunning).ThrowOnFail(this);
        this._onExceptionOccurred = handler;
        return this;
    }

    public TaskRunner<TArg> OnStarting(Action<(TaskRunner<TArg> TaskRunner, int TaskIndex, int TaskCount)> handler)
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

    public async Task<Result<TArg?>> RunAsync()
    {
        _ = Check.MustBe(!this._isRunning).ThrowOnFail(this);

        var count = this._taskList.Count + 1;
        var index = 0;
        var token = this._cancellationToken;
        Result<TArg?> result = null!;

        this._isRunning = true;
        this._onStarting?.Invoke((this, 0, count));
        var arg = await this._startWith();
        for (; index < this._taskList.Count; index++)
        {
            if (checkCancellationRequested(token))
            {
                this._onCancellationRequested?.Invoke((this, index, count));
                result = fail(arg, new OperationCanceledException());
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
                this._onExceptionOccurred?.Invoke((this, index, count, ex));
                if (!this._continueOnException)
                {
                    result = fail(arg, ex);
                    break;
                }
            }
        }
        this._isRunning = false;
        result ??= Result<TArg?>.CreateSuccess(arg);
        this._onEnded?.Invoke((this, index, count, result));
        this._isRunning = false;
        return result;

        static bool checkCancellationRequested(CancellationToken? token)
            => token is not null and { CanBeCanceled: true } and { IsCancellationRequested: true };

        static Result<TArg> fail(TArg? arg, Exception ex)
            => Result<TArg>.CreateFail(value: arg, error: ex);
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

    public override string ToString()
        => _name.IsNullOrEmpty() ? base.ToString() : _name;
}