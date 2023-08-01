using Library.Validations;

namespace Library.Threading;

public sealed class AsyncLock : IAsyncLock, IDisposable
{
    private readonly SemaphoreSlim _lock = new(1);

    public Guid Id { get; } = Guid.NewGuid();

    public void Dispose()
        => this._lock.Dispose();

    public async Task LockAsync(Func<Task> action)
    {
        Check.MustBeArgumentNotNull(action, nameof(action));
        await this._lock.WaitAsync();
        try
        {
            await action();
        }
        finally
        {
            _ = this._lock.Release();
        }
    }

    public async Task<TResult> LockAsync<TResult>(Func<Task<TResult>> action)
    {
        Check.MustBeArgumentNotNull(action, nameof(action));
        await this._lock.WaitAsync();
        try
        {
            return await action();
        }
        finally
        {
            _ = this._lock.Release();
        }
    }
}