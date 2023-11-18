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