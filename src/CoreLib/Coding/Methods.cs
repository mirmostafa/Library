using System.Diagnostics;

namespace Library.Coding;

[DebuggerStepThrough]
[StackTraceHidden]
public static class Methods
{
    public static Func<Task> Async => async ()
        => await Task.CompletedTask;

    public static Action Empty
        => () => { };

    public static Func<Task> EmptyTask
        => () => Task.CompletedTask;

    public static Action<T> EmptyArg<T>()
        => x => { };

    public static Func<T, T> Self<T>()
        => x => x;

    public static Func<T, Task<T>> SelfTask<T>()
        => t => Task.FromResult(t);

    public static Func<Task> ToFunc(Func<Task> func)
        => func;
}