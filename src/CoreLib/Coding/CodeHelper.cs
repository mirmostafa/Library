﻿using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;

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
    public static void Break()
         => BreakException.Throw();

    /// <summary>
    /// Breaks code execution.
    /// </summary>
    [DoesNotReturn]
    public static T Break<T>()
        => throw new BreakException();

    /// <summary>
    /// Executes the given action and returns a Result object indicating success or failure.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <returns>A Result object indicating success or failure.</returns>
    public static Result CatchResult([DisallowNull] in Action action)
    {
        Check.IfArgumentNotNull(action, nameof(action));
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
        Check.IfArgumentNotNull(action);
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
    /// Executes a function asynchronously and returns a Result object with the result of the function or an error message.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="func">The function to execute.</param>
    /// <param name="defaultResult">The default result to return in case of an error.</param>
    /// <returns>A Result object with the result of the function or an error message.</returns>
    public static async Task<Result<TResult?>> CatchResultAsync<TResult>(Func<Task<TResult>> func, TResult? defaultResult = default)
    {
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
    /// Combines two functions into one.
    /// </summary>
    /// <typeparam name="TResult1">The type of the first function's return value.</typeparam>
    /// <typeparam name="TResult2">The type of the second function's return value.</typeparam>
    /// <param name="create">The first function.</param>
    /// <param name="func">The second function.</param>
    /// <returns>A function that combines the two functions.</returns>
    public static Func<TResult2> Compose<TResult1, TResult2>([DisallowNull] this Func<TResult1> create, Func<TResult1, TResult2> func)
    {
        Check.IfArgumentNotNull(create);
        Check.IfArgumentNotNull(func);
        return [DebuggerStepThrough] () => func(create());
    }

    /// <summary>
    /// Compose a Func<Result<TResult1>> and a Func<TResult1, Result<TResult2>> into a Func<Result<TResult2>>.
    /// </summary>
    /// <typeparam name="TResult1">The type of the first result.</typeparam>
    /// <typeparam name="TResult2">The type of the second result.</typeparam>
    /// <param name="create">The Func<Result<TResult1>> to compose.</param>
    /// <param name="func">The Func<TResult1, Result<TResult2>> to compose.</param>
    /// <param name="onFail">The Func<Result<TResult1>, Result<TResult2>> to invoke if the first result is a failure.</param>
    /// <returns>A Func<Result<TResult2>> composed of the two functions.</returns>
    public static Func<Result<TResult2>> Compose<TResult1, TResult2>([DisallowNull] this Func<Result<TResult1>> create, Func<TResult1, Result<TResult2>> func, Func<Result<TResult1>, Result<TResult2>>? onFail = null)
    {
        Check.IfArgumentNotNull(create);
        Check.IfArgumentNotNull(func);
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
    /// <typeparam name="TResult1">The type of the result of the first function.</typeparam>
    /// <typeparam name="TResult2">The type of the result of the composed function.</typeparam>
    /// <typeparam name="TArg">The type of the argument of the composed function.</typeparam>
    /// <param name="create">The first function.</param>
    /// <param name="func">The composed function.</param>
    /// <param name="arg">The argument of the composed function.</param>
    /// <returns>The composed function.</returns>
    public static Func<TResult2> Compose<TResult1, TResult2, TArg>([DisallowNull] this Func<TResult1> create, Func<TResult1, TArg, TResult2> func, TArg arg)
    {
        Check.IfArgumentNotNull(create);
        Check.IfArgumentNotNull(func);
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
        Check.IfArgumentNotNull(create);
        Check.IfArgumentNotNull(func);
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
        Check.IfArgumentNotNull(create);
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
        Check.IfArgumentNotNull(create);
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
    /// <typeparam name="TResult1">The type of the result of the Func.</typeparam>
    /// <typeparam name="TArg">The type of the argument for the Action.</typeparam>
    /// <param name="create">The Func to compose.</param>
    /// <param name="action">The Action to compose.</param>
    /// <param name="getArg">The Func to get the argument for the Action.</param>
    /// <returns>A composed Func.</returns>
    public static Func<TResult1> Compose<TResult1, TArg>([DisallowNull] this Func<TResult1> create, Action<TArg> action, Func<TResult1, TArg> getArg)
    {
        Check.IfArgumentNotNull(create);
        return [DebuggerStepThrough] () =>
        {
            var result = create();
            action?.Invoke(getArg(result));
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
        Check.IfArgumentNotNull(create);
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
        Check.IfArgumentNotNull(create);
        Check.IfArgumentNotNull(func);
        return [DebuggerStepThrough] () =>
        {
            var start = create();
            return func(start, getArg(start));
        };
    }

    /// <summary>
    /// Compose a Func<TResult1> with a Func<TResult1, TResult2> and an Action<TArg> and a Func<TResult1, TArg>
    /// </summary>
    /// <typeparam name="TResult1">The type of the result of the first Func</typeparam>
    /// <typeparam name="TResult2">The type of the result of the second Func</typeparam>
    /// <typeparam name="TArg">The type of the argument of the Action</typeparam>
    /// <param name="create">The first Func</param>
    /// <param name="func">The second Func</param>
    /// <param name="action">The Action</param>
    /// <param name="getArg">The Func to get the argument of the Action</param>
    /// <returns>A Func<TResult2></returns>
    /// <exception cref="ArgumentNullException">Thrown when create is null</exception>
    public static Func<TResult2> Compose<TResult1, TResult2, TArg>([DisallowNull] this Func<TResult1> create, Func<TResult1, TResult2> func, Action<TArg> action, Func<TResult1, TArg> getArg)
    {
        Check.IfArgumentNotNull(create);
        Check.IfArgumentNotNull(func);
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
    /// <param name="action">    The action.</param>
    public static void Dispose<TDisposable>(in TDisposable disposable, in Action<TDisposable>? action = null) where TDisposable : IDisposable
        => Dispose(disposable, action);

    /// <summary>
    /// Disposes the specified disposable object.
    /// </summary>
    /// <typeparam name="TDisposable">The type of the disposable.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="disposable">The disposable.</param>
    /// <param name="action">    The action.</param>
    /// <returns></returns>
    public static TResult Dispose<TDisposable, TResult>(in TDisposable disposable, in Func<TDisposable, TResult> action) where TDisposable : IDisposable
        => Dispose(disposable, action);

    /// <summary>
    /// Disposes the specified disposable object.
    /// </summary>
    /// <typeparam name="TDisposable">The type of the disposable.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="disposable">The disposable.</param>
    /// <param name="result">    The result.</param>
    /// <returns></returns>
    public static TResult Dispose<TDisposable, TResult>(in TDisposable disposable, in TResult result) where TDisposable : IDisposable
        => Dispose(disposable, result);

    /// <summary>
    /// Disposes the specified disposable object.
    /// </summary>
    /// <typeparam name="TDisposable">The type of the disposable.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="disposable">The disposable.</param>
    /// <param name="action">    The action.</param>
    /// <returns></returns>
    public static TResult Dispose<TDisposable, TResult>(in TDisposable disposable, in Func<TResult> action) where TDisposable : IDisposable
        => Dispose(disposable, action);

    /// <summary>
    /// Disposes the specified disposable object.
    /// </summary>
    /// <typeparam name="TDisposable">The type of the disposable.</typeparam>
    /// <param name="disposable">The disposable.</param>
    /// <param name="action">    The action.</param>
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
    /// <param name="action">    The action.</param>
    /// <returns></returns>
    public static TResult Dispose<TDisposable, TResult>(this TDisposable disposable, in Func<TDisposable, TResult> action)
        where TDisposable : IDisposable
    {
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
    /// <param name="result">    The result.</param>
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
    /// <param name="action">    The action.</param>
    /// <returns></returns>
    public static TResult Dispose<TDisposable, TResult>(this TDisposable disposable, in Func<TResult> action)
        where TDisposable : IDisposable
    {
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
    /// <param name="parsePrevIfNull">Whether to parse the previous frame if the current one is null.</param>
    /// <returns>The method base of the caller.</returns>
    public static MethodBase? GetCallerMethod(in int index, bool parsePrevIfNull)
    {
        // Create a new StackTrace object
        var stackTrace = new StackTrace(true);
        // Set the index to the parameter passed in
        var i = index;
        // If the frame at the index is not null, or parsePrevIfNull is false, return the method at the index
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
    /// Creates a delegate of type TDelegate from the method name of type TType.
    /// </summary>
    /// <typeparam name="TType">The type of the method.</typeparam>
    /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
    /// <param name="methodName">The name of the method.</param>
    /// <returns>A delegate of type TDelegate.</returns>
    public static TDelegate GetDelegate<TType, TDelegate>(in string methodName)
            => (TDelegate)(ISerializable)Delegate.CreateDelegate(typeof(TDelegate),
                typeof(TType).GetMethod(methodName) ?? Throw<MethodInfo>(new Exceptions.InvalidOperationException()));

    /// <summary>
    /// Gets the function.
    /// </summary>
    /// <typeparam name="TArg">The type of the argument.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="func">The function.</param>
    /// <param name="arg"> The argument.</param>
    /// <returns></returns>
    public static Func<TResult> GetFunc<TArg, TResult>(Func<TArg, TResult> func, TArg arg)
        => () => func(arg);

    /// <summary>
    /// Gets the function.
    /// </summary>
    /// <typeparam name="TArg1">The type of the arg1.</typeparam>
    /// <typeparam name="TArg2">The type of the arg2.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="func">The function.</param>
    /// <param name="arg1">The arg1.</param>
    /// <param name="arg2">The arg2.</param>
    /// <returns></returns>
    public static Func<TResult> GetFunc<TArg1, TArg2, TResult>(Func<TArg1, TArg2, TResult> func, TArg1 arg1, TArg2 arg2)
        => () => func(arg1, arg2);

    /// <summary>
    /// Gets the function.
    /// </summary>
    /// <typeparam name="TArg1">The type of the arg1.</typeparam>
    /// <typeparam name="TArg2">The type of the arg2.</typeparam>
    /// <typeparam name="TArg3">The type of the arg3.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="func">The function.</param>
    /// <param name="arg1">The arg1.</param>
    /// <param name="arg2">The arg2.</param>
    /// <param name="arg3">The arg3.</param>
    /// <returns></returns>
    public static Func<TResult> GetFunc<TArg1, TArg2, TArg3, TResult>(Func<TArg1, TArg2, TArg3, TResult> func, TArg1 arg1, TArg2 arg2, TArg3 arg3)
        => () => func(arg1, arg2, arg3);

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
        Check.IfArgumentNotNull(action, nameof(action));
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
    public static Task ToVoidAsync<TValue>(this Task<TValue> task)
        => task;

    /// <summary>
    /// Tries to invoke the given action and returns a Result object indicating success or failure.
    /// </summary>
    /// <param name="action">The action to invoke.</param>
    /// <returns>A Result object indicating success or failure.</returns>
    public static Result TryInvoke(this Action action)
    {
        Check.IfArgumentNotNull(action);
        try
        {
            action();
            return Result.Success;
        }
        catch (Exception ex)
        {
            return Result.CreateFailure(ex);
        }
    }

    /// <summary>
    /// Tries to invoke the specified action and returns a <see cref="ResultTValue"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="action">The action.</param>
    /// <returns>A <see cref="ResultTValue"/>.</returns>
    public static Result<TValue?> TryInvoke<TValue>(this Func<TValue> action)
    {
        Check.IfArgumentNotNull(action);
        try
        {
            return Result<TValue>.CreateFailure(action());
        }
        catch (Exception ex)
        {
            return Result<TValue>.CreateFailure(ex);
        }
    }

    /// <summary>
    /// Invokes an action on an instance and returns the instance.
    /// </summary>
    /// <param name="instance">The instance to invoke the action on.</param>
    /// <param name="action">The action to invoke.</param>
    /// <returns>The instance.</returns>
    public static TInstance With<TInstance>(this TInstance instance, in Action<TInstance>? action)
        => instance.Fluent(action);

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

    /// <summary>
    /// Executes an asynchronous action on an instance of type TInstance and returns the instance.
    /// </summary>
    /// <param name="actionAsync">The asynchronous action to execute.</param>
    /// <returns>The instance of type TInstance.</returns>
    public static async Task<TInstance> WithAsync<TInstance>(this TInstance instance, Func<TInstance, Task>? actionAsync)
    {
        if (actionAsync != null)
        {
            await actionAsync(instance);
        }

        return instance;
    }
}

public record TryWithRecord<TValue>(Result<TValue> Result);