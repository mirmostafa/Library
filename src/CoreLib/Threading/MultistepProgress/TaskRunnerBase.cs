using System.Diagnostics;

using Library.DesignPatterns.Markers;
using Library.Exceptions;
using Library.Results;

namespace Library.Threading.MultistepProgress;

[DebuggerStepThrough, Fluent, StackTraceHidden]
public abstract class TaskRunnerBase<TSelf, TResult>
    where TSelf : TaskRunnerBase<TSelf, TResult>
    where TResult : ResultBase
{
    private Action<TResult>? _onEnded;
    private Action<Exception>? _onException;

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
            result = await this.OnRuningAsync(token);
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

    protected abstract Task<TResult> OnRuningAsync(CancellationToken token);

    private TSelf Me() =>
        (TSelf)this;
}