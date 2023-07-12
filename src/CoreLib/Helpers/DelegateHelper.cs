namespace Library.Helpers;

public static class DelegateHelper
{
    /// <summary>
    /// Creates an empty Action delegate.
    /// </summary>
    /// <returns>An empty Action delegate.</returns>
    public static Action Empty()
            => () => { };

    /// <summary>
    /// Creates an empty Action delegate of type T.
    /// </summary>
    /// <returns>An empty Action delegate of type T.</returns>
    public static Action<T> Empty<T>()
            => t => { };

    /// <summary> Extension method to convert a Func<T> to a Func<Task<T>>. </summary>
    public static Func<Task<T>> ToAsync<T>(this Func<T> func)
            => () => Task.FromResult(func());

    /// <summary>
    /// Converts a synchronous Func to an asynchronous Func.
    /// </summary>
    /// <typeparam name="TInput">The type of the input parameter of the Func.</typeparam>
    /// <typeparam name="TOutput">The type of the output of the Func.</typeparam>
    /// <param name="func">The synchronous Func to convert.</param>
    /// <returns>An asynchronous Func.</returns>
    public static Func<TInput, Task<TOutput>> ToAsync<TInput, TOutput>(this Func<TInput, TOutput> func) =>
        input => Task.FromResult(func(input));

    public static Task ToAsync(this Action action) =>
        new(action);
    public static Task ToAsync(this Action action, CancellationToken token) =>
        new(action, token);
}