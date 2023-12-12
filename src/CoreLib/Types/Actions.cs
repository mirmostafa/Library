namespace Library.Types;

public static class Actions
{
    public static Action Empty() =>
        () => { };

    public static Action<T> Empty<T>() =>
        t => { };

    public static Func<bool> True() =>
        () => true;

    public static void Void()
    { }
}

public static class Funcs
{
    [return: MaybeNull]
    public static Func<TResult?> Empty<TResult>() =>
        () => default;

    [return: MaybeNull]
    public static Func<T?> SelfEmpty<T>() =>
        () => default;
}