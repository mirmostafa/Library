using System.Diagnostics;
using System.Reflection;

using Library.Exceptions;
using Library.Results;
using Library.Validations;

namespace Library.Coding;

/// <summary>
/// 🤏 Little code snippets to do little works.
/// </summary>
[DebuggerStepThrough]
[StackTraceHidden]
public static class CodeHelper
{
    /// <summary>
    /// Breaks code execution.
    /// </summary>
    [DoesNotReturn]
    public static void Break() =>
        BreakException.Throw();

    /// <summary>
    /// Breaks code execution.
    /// </summary>
    [DoesNotReturn]
    public static T Break<T>() =>
        throw new BreakException();

    /// <summary>
    /// Executes the given action and returns a Result object indicating success or failure.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <returns>A Result object indicating success or failure.</returns>
    public static Result CatchResult([DisallowNull] in Action action)
    {
        Check.MustBeArgumentNotNull(action);
        try
        {
            action();
            return Result.Success;
        }
        catch (Exception ex)
        {
            return Result.CreateFailure(ex.GetBaseException().Message, ex);
        }
    }

    /// <summary>
    /// Executes the specified action and returns a <see cref="Result"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="action">The action to execute.</param>
    /// <param name="defaultResult">The default result to return if an exception is thrown.</param>
    /// <returns>A <see cref="ResultTResult"/>.</returns>
    public static Result<TResult> CatchResult<TResult>([DisallowNull] in Func<TResult> action, TResult? defaultResult = default)
    {
        Check.MustBeArgumentNotNull(action);
        try
        {
            return Result<TResult>.CreateSuccess(action());
        }
        catch (Exception ex)
        {
            return Result<TResult?>.CreateFailure(ex.GetBaseException().Message, ex, defaultResult)!;
        }
    }

    /// <summary>
    /// Executes a function asynchronously and returns a Result object with the result of the
    /// function or an error message.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="func">The function to execute.</param>
    /// <param name="defaultResult">The default result to return in case of an error.</param>
    /// <returns>A Result object with the result of the function or an error message.</returns>
    public static async Task<Result<TResult?>> CatchResultAsync<TResult>(Func<Task<TResult>> func, TResult? defaultResult = default)
    {
        Check.MustBeArgumentNotNull(func);
        try
        {
            var result = await func();
            return Result<TResult?>.CreateSuccess(result);
        }
        catch (Exception ex)
        {
            return Result<TResult?>.CreateFailure(ex.GetBaseException().Message, defaultResult);
        }
    }

    /// <summary>
    /// Executes a function asynchronously and returns a Result object with the result of the
    /// function or an error message.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="func">The function to execute.</param>
    /// <param name="defaultResult">The default result to return in case of an error.</param>
    /// <returns>A Result object with the result of the function or an error message.</returns>
    public static async Task<Result> CatchResultAsync(Func<Task<Result>> func)
    {
        Check.MustBeArgumentNotNull(func);
        try
        {
            var result = await func();
            return result;
        }
        catch (Exception ex)
        {
            return Result.CreateFailure(ex);
        }
    }

    public static async Task<Result> CatchResultAsync(Func<Task> func)
    {
        Check.MustBeArgumentNotNull(func);
        try
        {
            await func();
            return Result.Success;
        }
        catch (Exception ex)
        {
            return Result.CreateFailure(ex);
        }
    }

    public static Func<TResult2> Compose<TArgs, TResult2>(TArgs args, Func<TArgs, TResult2> func)
    {
        var create = () => args;
        Check.MustBeArgumentNotNull(create);
        Check.MustBeArgumentNotNull(func);
        return [DebuggerStepThrough] () => func(create());
    }

    /// <summary>
    /// Combines two functions into one.
    /// </summary>
    /// <typeparam name="TArgs">The type of the first function's return value.</typeparam>
    /// <typeparam name="TResult2">The type of the second function's return value.</typeparam>
    /// <param name="create">The first function.</param>
    /// <param name="func">The second function.</param>
    /// <returns>A function that combines the two functions.</returns>
    public static Func<TResult2> Compose<TArgs, TResult2>([DisallowNull] this Func<TArgs> create, Func<TArgs, TResult2> func)
    {
        Check.MustBeArgumentNotNull(create);
        Check.MustBeArgumentNotNull(func);
        return [DebuggerStepThrough] () => func(create());
    }

    /// <summary> Compose a Func<Result<TResult1>> and a Func<TResult1, Result<TResult2>> into a
    /// Func<Result<TResult2>>. </summary> <typeparam name="TArgs">The type of the first
    /// result.</typeparam> <typeparam name="TResult2">The type of the second result.</typeparam>
    /// <param name="create">The Func<Result<TResult1>> to compose.</param> <param name="func">The
    /// Func<TResult1, Result<TResult2>> to compose.</param> <param name="onFail">The
    /// Func<Result<TResult1>, Result<TResult2>> to invoke if the first result is a failure.</param>
    /// <returns>A Func<Result<TResult2>> composed of the two functions.</returns>
    public static Func<Result<TResult2>> Compose<TArgs, TResult2>([DisallowNull] this Func<Result<TArgs>> create, Func<TArgs, Result<TResult2>> func, Func<Result<TArgs>, Result<TResult2>>? onFail = null)
    {
        Check.MustBeArgumentNotNull(create);
        Check.MustBeArgumentNotNull(func);
        return [DebuggerStepThrough] () =>
        {
            var result = create();
            return result.IsSucceed
                ? func(result.Value)
                : onFail?.Invoke(result) ?? Result<TResult2>.From(result, default!);
        };
    }

    /// <summary>
    /// Compose a function with a given argument.
    /// </summary>
    /// <typeparam name="TArgs">The type of the result of the first function.</typeparam>
    /// <typeparam name="TResult2">The type of the result of the composed function.</typeparam>
    /// <typeparam name="TArg">The type of the argument of the composed function.</typeparam>
    /// <param name="create">The first function.</param>
    /// <param name="func">The composed function.</param>
    /// <param name="arg">The argument of the composed function.</param>
    /// <returns>The composed function.</returns>
    public static Func<TResult2> Compose<TArgs, TResult2, TArg>([DisallowNull] this Func<TArgs> create, Func<TArgs, TArg, TResult2> func, TArg arg)
    {
        Check.MustBeArgumentNotNull(create);
        Check.MustBeArgumentNotNull(func);
        return [DebuggerStepThrough] () => func(create(), arg);
    }

    /// <summary>
    /// Combines two functions and an action into a single function.
    /// </summary>
    /// <typeparam name="TResult1">The type of the first result.</typeparam>
    /// <typeparam name="TResult2">The type of the second result.</typeparam>
    /// <typeparam name="TArg">The type of the argument.</typeparam>
    /// <param name="create">The first function.</param>
    /// <param name="func">The second function.</param>
    /// <param name="action">The action.</param>
    /// <param name="arg">The argument.</param>
    /// <returns>A function that combines the two functions and the action.</returns>
    public static Func<TResult2> Compose<TResult1, TResult2, TArg>([DisallowNull] this Func<TResult1> create, Func<TResult1, TResult2> func, Action<TArg> action, TArg arg)
    {
        Check.MustBeArgumentNotNull(create);
        Check.MustBeArgumentNotNull(func);
        return [DebuggerStepThrough] () =>
        {
            action?.Invoke(arg);
            return func(create());
        };
    }

    /// <summary>
    /// Creates a new function that composes a given function with an action that takes an argument.
    /// </summary>
    /// <typeparam name="TResult1">The type of the result of the created function.</typeparam>
    /// <typeparam name="TArg">The type of the argument of the action.</typeparam>
    /// <param name="create">The function to compose.</param>
    /// <param name="action">The action to compose.</param>
    /// <param name="arg">The argument of the action.</param>
    /// <returns>A new function that composes the given function with the action.</returns>
    public static Func<TResult1> Compose<TResult1, TArg>([DisallowNull] this Func<TResult1> create, Action<TResult1, TArg> action, TArg arg)
    {
        Check.MustBeArgumentNotNull(create);
        return [DebuggerStepThrough] () =>
        {
            var result = create();
            action?.Invoke(result, arg);
            return result;
        };
    }

    /// <summary>
    /// Combines two functions into one, where the first function is executed and then the second one.
    /// </summary>
    /// <typeparam name="TResult1">The type of the result of the first function.</typeparam>
    /// <typeparam name="TArg">The type of the argument of the second function.</typeparam>
    /// <param name="create">The first function.</param>
    /// <param name="action">The second function.</param>
    /// <param name="arg">The argument of the second function.</param>
    /// <returns>A combined function.</returns>
    public static Func<TResult1> Compose<TResult1, TArg>([DisallowNull] this Func<TResult1> create, Action<TArg> action, TArg arg)
    {
        Check.MustBeArgumentNotNull(create);
        return [DebuggerStepThrough] () =>
        {
            var result = create();
            action?.Invoke(arg);
            return result;
        };
    }

    /// <summary>
    /// Compose a Func with an Action and a Func to get the argument for the Action.
    /// </summary>
    /// <typeparam name="TArgs">The type of the result of the Func.</typeparam>
    /// <typeparam name="TResult">The type of the argument for the Action.</typeparam>
    /// <param name="create">The Func to compose.</param>
    /// <param name="action">The Action to compose.</param>
    /// <param name="getResult">The Func to get the argument for the Action.</param>
    /// <returns>A composed Func.</returns>
    public static Func<TArgs> Compose<TArgs, TResult>([DisallowNull] this Func<TArgs> create, Action<TResult> action, Func<TArgs, TResult> getResult)
    {
        Check.MustBeArgumentNotNull(create);
        return [DebuggerStepThrough] () =>
        {
            var result = create();
            action?.Invoke(getResult(result));
            return result;
        };
    }

    /// <summary>
    /// Compose a function with an action and a getArg function.
    /// </summary>
    /// <typeparam name="TResult1">The type of the result of the create function.</typeparam>
    /// <typeparam name="TArg">The type of the argument of the action.</typeparam>
    /// <param name="create">The function to be composed.</param>
    /// <param name="action">The action to be composed.</param>
    /// <param name="getArg">The getArg function to be composed.</param>
    /// <returns>The composed function.</returns>
    public static Func<TResult1> Compose<TResult1, TArg>([DisallowNull] this Func<TResult1> create, Action<TResult1, TArg> action, Func<TResult1, TArg> getArg)
    {
        Check.MustBeArgumentNotNull(create);
        return [DebuggerStepThrough] () =>
        {
            var result = create();
            action?.Invoke(result, getArg(result));
            return result;
        };
    }

    /// <summary>
    /// Creates a new function that composes two functions together.
    /// </summary>
    /// <typeparam name="TResult1">The type of the first result.</typeparam>
    /// <typeparam name="TResult2">The type of the second result.</typeparam>
    /// <typeparam name="TArg">The type of the argument.</typeparam>
    /// <param name="create">The first function.</param>
    /// <param name="func">The second function.</param>
    /// <param name="getArg">The argument for the second function.</param>
    /// <returns>A new function that composes the two functions.</returns>
    public static Func<TResult2> Compose<TResult1, TResult2, TArg>([DisallowNull] this Func<TResult1> create, Func<TResult1, TArg, TResult2> func, Func<TResult1, TArg> getArg)
    {
        Check.MustBeArgumentNotNull(create);
        Check.MustBeArgumentNotNull(func);
        return [DebuggerStepThrough] () =>
        {
            var start = create();
            return func(start, getArg(start));
        };
    }

    /// <summary> Compose a Func<TResult1> with a Func<TResult1, TResult2> and an Action<TArg> and a
    /// Func<TResult1, TArg> </summary> <typeparam name="TResult1">The type of the result of the
    /// first Func</typeparam> <typeparam name="TResult2">The type of the result of the second
    /// Func</typeparam> <typeparam name="TArg">The type of the argument of the Action</typeparam>
    /// <param name="create">The first Func</param> <param name="func">The second Func</param>
    /// <param name="action">The Action</param> <param name="getArg">The Func to get the argument of
    /// the Action</param> <returns>A Func<TResult2></returns> <exception
    /// cref="ArgumentNullException">Thrown when create is null</exception>
    public static Func<TResult2> Compose<TResult1, TResult2, TArg>([DisallowNull] this Func<TResult1> create, Func<TResult1, TResult2> func, Action<TArg> action, Func<TResult1, TArg> getArg)
    {
        Check.MustBeArgumentNotNull(create);
        Check.MustBeArgumentNotNull(func);
        return [DebuggerStepThrough] () =>
        {
            var start = create();
            action?.Invoke(getArg(start));
            return func(start);
        };
    }

    /// <summary>
    /// Disposes the specified disposable object.
    /// </summary>
    /// <typeparam name="TDisposable">The type of the disposable.</typeparam>
    /// <param name="disposable">The disposable.</param>
    /// <param name="action">The action.</param>
    public static void Dispose<TDisposable>(in TDisposable disposable, in Action<TDisposable>? action = null) where TDisposable : IDisposable
        => Dispose(disposable, action);

    /// <summary>
    /// Disposes the specified disposable object.
    /// </summary>
    /// <typeparam name="TDisposable">The type of the disposable.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="disposable">The disposable.</param>
    /// <param name="action">The action.</param>
    /// <returns></returns>
    public static TResult Dispose<TDisposable, TResult>(in TDisposable disposable, in Func<TDisposable, TResult> action) where TDisposable : IDisposable
        => Dispose(disposable, action);

    /// <summary>
    /// Disposes the specified disposable object.
    /// </summary>
    /// <typeparam name="TDisposable">The type of the disposable.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="disposable">The disposable.</param>
    /// <param name="result">The result.</param>
    /// <returns></returns>
    public static TResult Dispose<TDisposable, TResult>(in TDisposable disposable, in TResult result) where TDisposable : IDisposable
        => Dispose(disposable, result);

    /// <summary>
    /// Disposes the specified disposable object.
    /// </summary>
    /// <typeparam name="TDisposable">The type of the disposable.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="disposable">The disposable.</param>
    /// <param name="action">The action.</param>
    /// <returns></returns>
    public static TResult Dispose<TDisposable, TResult>(in TDisposable disposable, in Func<TResult> action) where TDisposable : IDisposable
        => Dispose(disposable, action);

    /// <summary>
    /// Disposes the specified disposable object.
    /// </summary>
    /// <typeparam name="TDisposable">The type of the disposable.</typeparam>
    /// <param name="disposable">The disposable.</param>
    /// <param name="action">The action.</param>
    public static void Dispose<TDisposable>(this TDisposable disposable, in Action<TDisposable>? action = null)
        where TDisposable : IDisposable
    {
        try
        {
            action?.Invoke(disposable);
        }
        finally
        {
            disposable?.Dispose();
        }
    }

    /// <summary>
    /// Disposes the specified disposable object.
    /// </summary>
    /// <typeparam name="TDisposable">The type of the disposable.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="disposable">The disposable.</param>
    /// <param name="action">The action.</param>
    /// <returns></returns>
    public static TResult Dispose<TDisposable, TResult>(this TDisposable disposable, in Func<TDisposable, TResult> action)
        where TDisposable : IDisposable
    {
        Check.MustBeArgumentNotNull(action);
        try
        {
            return action(disposable);
        }
        finally
        {
            disposable?.Dispose();
        }
    }

    /// <summary>
    /// Disposes the specified disposable object.
    /// </summary>
    /// <typeparam name="TDisposable">The type of the disposable.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="disposable">The disposable.</param>
    /// <param name="result">The result.</param>
    /// <returns></returns>
    public static TResult Dispose<TDisposable, TResult>(this TDisposable disposable, in TResult result)
        where TDisposable : IDisposable
    {
        try
        {
            return result;
        }
        finally
        {
            disposable?.Dispose();
        }
    }

    /// <summary>
    /// Disposes the specified disposable object.
    /// </summary>
    /// <typeparam name="TDisposable">The type of the disposable.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="disposable">The disposable.</param>
    /// <param name="action">The action.</param>
    /// <returns></returns>
    public static TResult Dispose<TDisposable, TResult>(this TDisposable disposable, in Func<TResult> action)
        where TDisposable : IDisposable
    {
        Check.MustBeArgumentNotNull(action);
        try
        {
            return action();
        }
        finally
        {
            disposable?.Dispose();
        }
    }

    /// <summary>
    /// Gets the caller method.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns></returns>
    public static MethodBase? GetCallerMethod(in int index = 2)
        => new StackTrace(true).GetFrame(index)?.GetMethod();

    /// <summary>
    /// Gets the caller method from the stack trace.
    /// </summary>
    /// <param name="index">The index of the stack frame.</param>
    /// <param name="parsePrevIfNull">
    /// Whether to parse the previous frame if the current one is null.
    /// </param>
    /// <returns>The method base of the caller.</returns>
    public static MethodBase? GetCallerMethod(in int index, bool parsePrevIfNull)
    {
        // Create a new StackTrace object
        var stackTrace = new StackTrace(true);
        // Set the index to the parameter passed in
        var i = index;
        // If the frame at the index is not null, or parsePrevIfNull is false, return the method at
        // the index
        if (stackTrace.GetFrame(i) is not null || parsePrevIfNull is false)
        {
            return stackTrace.GetFrame(i)?.GetMethod();
        }

        // If the frame at the index is null, decrement the index until a non-null frame is found
        while (stackTrace.GetFrame(--i) is null)
        {
            if (i == 0)
            {
                break;
            }
        }

        // Return the method at the non-null frame
        return stackTrace.GetFrame(i)?.GetMethod();
    }

    /// <summary>
    /// Gets the name of the caller method.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns></returns>
    public static string? GetCallerMethodName(in int index = 2)
        => GetCallerMethod(index)?.Name;

    /// <summary>
    /// Gets the current method.
    /// </summary>
    /// <returns></returns>
    public static MethodBase? GetCurrentMethod()
        => GetCallerMethod();

    /// <summary>
    /// Determines whether the specified try function has exception.
    /// </summary>
    /// <param name="tryFunc">The try function.</param>
    /// <returns><c>true</c> if the specified tryFunc has exception; otherwise, <c>false</c>.</returns>
    public static bool HasException(in Action tryFunc)
        => Catch(tryFunc) is not null;

    /// <summary>
    /// Executes the specified action and throws an exception if an error occurs.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="action">The action to execute.</param>
    /// <param name="getException">A function to get the exception to throw.</param>
    /// <returns>The result of the action.</returns>
    public static TResult ThrowOnError<TResult>(in Func<TResult> action, Func<Exception, Exception>? getException = null)
    {
        Check.MustBeArgumentNotNull(action);
        try
        {
            return action();
        }
        catch (Exception ex)
        {
            if (getException is not null)
            {
                Throw(() => getException(ex));
            }

            throw;
        }
    }

    /// <summary>
    /// Extension method to convert a Task of type TValue to a Task of type Void.
    /// </summary>
    public static Task ToVoidAsync<TValue>(this Task<TValue> task) =>
        task;

    public static bool TryNotNull<T>(T? o, [NotNullIfNotNull(nameof(o))] out T result)
    {
        result = o!;
        return result != null;
    }

    public static Result<TValue> TryResult<TValue>(Func<Result<TValue>> method, Func<TValue> getDefaultValue) =>
                                                                                                                                                InnerCall(method, getDefaultValue);

    public static Result<IEnumerable<TValue>> TryResult<TValue>(Func<Result<IEnumerable<TValue>>> method) =>
            InnerCall(method, Enumerable.Empty<TValue>);

    public static Result<IEnumerable<TValue>> TryResult<TValue, TArg>(Func<TArg, Result<IEnumerable<TValue>>> method, TArg arg) =>
        InnerCall(method, arg, Enumerable.Empty<TValue>);

    public static Result<IEnumerable<TValue>> TryResult<TValue, TArg>(Func<TArg, IEnumerable<TValue>> method, TArg arg) =>
        InnerCall(method, arg, Enumerable.Empty<TValue>);

    public static Result<TResult> TryResult<TResult, TArg>(Func<TArg, TResult> method, TArg arg, Func<TResult> getDefaultValue) =>
        InnerCall(method, arg, getDefaultValue);

    /// <summary>
    /// Invokes an action on an instance and returns the instance.
    /// </summary>
    /// <param name="instance">The instance to invoke the action on.</param>
    /// <param name="action">The action to invoke.</param>
    /// <returns>The instance.</returns>
    public static TInstance With<TInstance>(this TInstance instance, in Action<TInstance>? action) =>
        instance.Fluent(action);

    /// <summary>
    /// Executes an action on a task instance and returns the result.
    /// </summary>
    /// <param name="instanceAsync">The task instance.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns>The result of the task instance.</returns>
    public static async Task<TInstance> WithAsync<TInstance>(this Task<TInstance> instanceAsync, Action<TInstance>? action)
    {
        var result = await instanceAsync;
        action?.Invoke(result);
        return result;
    }

    private static Result<TResult> InnerCall<TResult>(Func<Result<TResult>> method, Func<TResult> getDefaultValue)
    {
        Check.MustBeArgumentNotNull(method);
        try
        {
            var result = method();
            return result;
        }
        catch (Exception ex)
        {
            return Result<TResult>.CreateFailure(ex, getDefaultValue.ArgumentNotNull()());
        }
    }

    private static Result<TResult> InnerCall<TResult, TArg>(Func<TArg, Result<TResult>> method, TArg arg, Func<TResult> getDefaultValue)
    {
        Check.MustBeArgumentNotNull(method);
        try
        {
            var result = method(arg);
            return result;
        }
        catch (Exception ex)
        {
            return Result<TResult>.CreateFailure(ex, getDefaultValue.ArgumentNotNull()());
        }
    }

    private static Result<TResult> InnerCall<TResult, TArg>(Func<TArg, TResult> method, TArg arg, Func<TResult> getDefaultValue)
    {
        Check.MustBeArgumentNotNull(method);
        try
        {
            var result = method(arg);
            return result;
        }
        catch (Exception ex)
        {
            return Result<TResult>.CreateFailure(ex, getDefaultValue.ArgumentNotNull()());
        }
    }
}