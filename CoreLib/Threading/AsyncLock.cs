using Library.Validations;

namespace Library.Threading;

public sealed class AsyncLock : IAsyncLock, IDisposable
{
    private readonly SemaphoreSlim _Lock;

    public AsyncLock()
    {
        this._Lock = new SemaphoreSlim(1);
        this.Id = Guid.NewGuid();
    }

    public Guid Id { get; }

    public void Dispose()
        => this._Lock.Dispose();

    public async Task LockAsync(Func<Task> action)
    {
        Check.IfArgumentNotNull(action, nameof(action));
        await this._Lock.WaitAsync();
        try
        {
            await action();
        }
        finally
        {
            _ = this._Lock.Release();
        }
    }

    public async Task<TResult> LockAsync<TResult>(Func<Task<TResult>> action)
    {
        Check.IfArgumentNotNull(action, nameof(action));
        await this._Lock.WaitAsync();
        try
        {
            return await action();
        }
        finally
        {
            _ = this._Lock.Release();
        }
    }
}
