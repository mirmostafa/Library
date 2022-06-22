namespace Library.Threading;

public interface IAsyncLock
{
    Task LockAsync(Func<Task> action);
    Task<TResult> LockAsync<TResult>(Func<Task<TResult>> action);
}
