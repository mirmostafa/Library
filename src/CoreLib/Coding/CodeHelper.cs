using System.Diagnostics;
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
    /// Catches the result.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <returns></returns>
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

    public static Result<TResult> CatchResult<TResult>([DisallowNull] in Func<TResult> action, TResult? defaultResult = default)
    {
        Check.IfArgumentNotNull(action);
        try
        {
            return Result<TResult>.CreateSuccess(action());
        }
        catch (Exception ex)
        {
            return Result<TResult?>.CreateFail(ex.GetBaseException().Message, ex, defaultResult)!;
        }
    }

    /// <summary>
    /// Catches the result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="func">         The function.</param>
    /// <param name="defaultResult">The default result.</param>
    /// <returns></returns>
    public static async Task<Result<TResult?>> CatchResult<TResult>(this Func<Task<TResult>> func, TResult? defaultResult = default)
    {
        try
        {
            var result = await func();
            return Result<TResult?>.CreateSuccess(result);
        }
        catch (Exception ex)
        {
            return Result<TResult?>.CreateFail(ex.GetBaseException().Message, defaultResult);
        }
    }

    public static Func<TResult2> Compose<TResult1, TResult2>([DisallowNull] this Func<TResult1> create, Func<TResult1, TResult2> func)
    {
        Check.IfArgumentNotNull(create);
        Check.IfArgumentNotNull(func);
        return [DebuggerStepThrough] () => func(create());
    }

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

    public static Func<TResult2> Compose<TResult1, TResult2, TArg>([DisallowNull] this Func<TResult1> create, Func<TResult1, TArg, TResult2> func, TArg arg)
    {
        Check.IfArgumentNotNull(create);
        Check.IfArgumentNotNull(func);
        return [DebuggerStepThrough] () => func(create(), arg);
    }

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
    /// Gets the caller method.
    /// </summary>
    /// <param name="index">          The index.</param>
    /// <param name="parsePrevIfNull">if set to <c>true</c> [parse previous if null].</param>
    /// <returns></returns>
    public static MethodBase? GetCallerMethod(in int index, bool parsePrevIfNull)
    {
        var stackTrace = new StackTrace(true);
        var i = index;
        if (stackTrace.GetFrame(i) is not null || parsePrevIfNull is false)
        {
            return stackTrace.GetFrame(i)?.GetMethod();
        }

        while (stackTrace.GetFrame(--i) is null)
        {
        }

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
    /// Gets the delegate.
    /// </summary>
    /// <typeparam name="TType">The type of the type.</typeparam>
    /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
    /// <param name="methodName">Name of the method.</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
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
    /// Throws the on error.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="action">      The action.</param>
    /// <param name="getException">The get exception.</param>
    /// <returns></returns>
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

    public static Task ToVoidAsync<TValue>(this Task<TValue> task)
        => task;

    public static Result TryInvoke(this Action action)
    {
        Check.IfArgumentNotNull(action);
        try
        {
            action();
            return Result.Success;
        }
        catch(Exception ex)
        {
            return Result.CreateFailure(ex);
        }
    }

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

    public static TInstance With<TInstance>(this TInstance instance, in Action<TInstance>? action)
        => instance.Fluent(action);
<<<<<<< HEAD:src/CoreLib/Coding/CodeHelpers.cs
    public static async Task<TInstance> WithAsync<TInstance>(this Task<TInstance> instanceAsync, Action<TInstance>? action)
    {
        var result = await instanceAsync;
        action?.Invoke(result);
        return result;
    }
=======
>>>>>>> 5681c22d35b75924ec27a5dedd109df5172c66f5:src/CoreLib/Coding/CodeHelper.cs

    public static TInstance With<TInstance>(this TInstance instance, in Action? action)
        => instance.Fluent(action);

    public static TInstance With<TInstance>(this TInstance instance, in object? obj)
        => instance.Fluent(obj);
}

public record TryWithRecord<TValue>(Result<TValue> Result);