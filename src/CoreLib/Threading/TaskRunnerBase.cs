using System.Diagnostics;

using Library.DesignPatterns.Markers;
using Library.Results;

namespace Library.Threading;

[Fluent]
[DebuggerStepThrough, StackTraceHidden]
public abstract class TaskRunnerBase<TSelf, TResult>
    where TSelf : TaskRunnerBase<TSelf, TResult>
    where TResult : ResultBase
{
    private Action<TResult>? _onEnded;
    private Action<Exception>? _onException;
    private bool _isRunning;

    public bool IsRunning { get; private set; }

    public TSelf OnEnded(Action<TResult>? action) =>
        this.Me().Fluent(this._onEnded = action);

    public TSelf OnException(Action<Exception>? onException)
    {
        this._onException = onException;
        return this.Me();
    }

    public async Task<TResult> RunAsync(CancellationToken token = default)
    {
        TResult result = default!;
        try
        {
            this.IsRunning = true;
            result = await this.OnRunningAsync(token);
        }
        catch (Exception ex)
        {
            this._onException?.Invoke(ex);
            result = this.GetErrorResult(ex);
        }
        finally
        {
            this.IsRunning = false;
            this._onEnded?.Invoke(result);
        }
        return result;
    }

    protected abstract TResult GetErrorResult(Exception exception);

    protected abstract Task<TResult> OnRunningAsync(CancellationToken token);

    protected virtual TSelf Me() =>
        (TSelf)this;
}