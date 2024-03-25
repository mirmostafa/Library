using System.Diagnostics;

namespace Library.Coding;

/// <summary>
/// A static class containing methods for creating and manipulating functions.
/// </summary>
/// <returns>
/// A static class containing methods for creating and manipulating functions.
/// </returns>
[DebuggerStepThrough]
[StackTraceHidden]
public static class Methods
{
    /// <summary>
    /// Async method that returns a Task object.
    /// </summary>
    public static Func<Task> Async
        => async () => await Task.CompletedTask;

    /// <summary>
    /// Returns an empty Action delegate.
    /// </summary>
    /// <returns>An empty Action delegate.</returns>
    public static Action Empty
        => () => { };

    /// <summary>
    /// Returns an empty Task object.
    /// </summary>
    public static Func<Task> EmptyTask
        => () => Task.CompletedTask;

    /// <summary>
    /// Creates an empty Action delegate of type T.
    /// </summary>
    /// <returns>An empty Action delegate of type T.</returns>
    public static Action<T> EmptyArg<T>()
        => x => { };

    /// <summary>
    /// Creates a function that returns its input.
    /// </summary>
    /// <typeparam name="T">The type of the input and output of the function.</typeparam>
    /// <returns>A function that returns its input.</returns>
    public static Func<T, T> Self<T>()
        => x => x;

    /// <summary>
    /// Creates a Task that returns the given value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <returns>A Task that returns the given value.</returns>
    public static Func<T, Task<T>> SelfTask<T>()
        => t => Task.FromResult(t);

    public static Func<Task> ToFunc(Func<Task> func)
        => func;
}