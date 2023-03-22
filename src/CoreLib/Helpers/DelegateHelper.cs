namespace Library.Helpers;

public static class DelegateHelper
{
    public static Func<Task<T>> ToAsync<T>(this Func<T> func)
        => () => Task.FromResult(func());

    public static Func<TInput, Task<TOutput>> ToAsync<TInput, TOutput>(this Func<TInput, TOutput> func)
        => (TInput) => Task.FromResult(func(TInput));

    public static Action Empty()
        => () => { };
    public static Action<T> Empty<T>()
        => t => { };
}