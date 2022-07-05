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
    public static Task Async(Action action, CancellationToken cancellationToken = default)
        => Task.Run(action, cancellationToken);

    public static Task<TResult> Async<TResult>(TResult result)
        => Task.FromResult(result);

    public static Task<TResult> Async<TResult>(Func<CancellationToken, TResult> action, CancellationToken cancellationToken = default)
        => Task.Run(() => action.ArgumentNotNull(nameof(action))(cancellationToken), cancellationToken);

    /// <summary>
    ///     Breaks code execution.
    /// </summary>
    [DoesNotReturn]
    public static void Break()
        => BreakException.Throw();

    public static Result CatchResult([DisallowNull] in Action action)
    {
        Check.IfArgumentNotNull(action);
        try
        {
            action();
            return Result.Success;
        }
        catch (Exception ex)
        {
            return Result.CreateFail(ex.GetBaseException().Message);
        }
    }

    public static Func<TResult> Compose<TResult>([DisallowNull] this Func<TResult> create, [DisallowNull] params Func<TResult, TResult>[] funcs)
    {
        Check.IfArgumentNotNull(create);
        Check.IfArgumentNotNull(funcs);
        var result = () =>
        {
            var value = create();
            foreach (var func in funcs)
            {
                value = func(value);
            }
            return value;
        };
        return result;
    }

    /// <summary>
    ///     Disposes the specified disposable object.
    /// </summary>
    /// <typeparam name="TDisposable"> The type of the disposable. </typeparam>
    /// <param name="disposable"> The disposable. </param>
    /// <param name="action"> The action. </param>
    public static void Dispose<TDisposable>(in TDisposable disposable, in Action<TDisposable>? action = null)
        where TDisposable : IDisposable
        => Dispose(disposable, action);

    /// <summary>
    ///     Disposes the specified disposable object.
    /// </summary>
    /// <typeparam name="TDisposable"> The type of the disposable. </typeparam>
    /// <typeparam name="TResult"> The type of the result. </typeparam>
    /// <param name="disposable"> The disposable. </param>
    /// <param name="action"> The action. </param>
    /// <returns> </returns>
    public static TResult Dispose<TDisposable, TResult>(in TDisposable disposable, in Func<TDisposable, TResult> action)
        where TDisposable : IDisposable
        => Dispose(disposable, action);

    /// <summary>
    ///     Disposes the specified disposable object.
    /// </summary>
    /// <typeparam name="TDisposable"> The type of the disposable. </typeparam>
    /// <typeparam name="TResult"> The type of the result. </typeparam>
    /// <param name="disposable"> The disposable. </param>
    /// <param name="result"> The result. </param>
    /// <returns> </returns>
    public static TResult Dispose<TDisposable, TResult>(in TDisposable disposable, in TResult result)
        where TDisposable : IDisposable
        => Dispose(disposable, result);

    /// <summary>
    ///     Disposes the specified disposable object.
    /// </summary>
    /// <typeparam name="TDisposable"> The type of the disposable. </typeparam>
    /// <typeparam name="TResult"> The type of the result. </typeparam>
    /// <param name="disposable"> The disposable. </param>
    /// <param name="action"> The action. </param>
    /// <returns> </returns>
    public static TResult Dispose<TDisposable, TResult>(in TDisposable disposable, in Func<TResult> action)
        where TDisposable : IDisposable
        => Dispose(disposable, action);

    /// <summary>
    ///     Disposes the specified disposable object.
    /// </summary>
    /// <typeparam name="TDisposable"> The type of the disposable. </typeparam>
    /// <param name="disposable"> The disposable. </param>
    /// <param name="action"> The action. </param>
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
    ///     Disposes the specified disposable object.
    /// </summary>
    /// <typeparam name="TDisposable"> The type of the disposable. </typeparam>
    /// <typeparam name="TResult"> The type of the result. </typeparam>
    /// <param name="disposable"> The disposable. </param>
    /// <param name="action"> The action. </param>
    /// <returns> </returns>
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
    ///     Disposes the specified disposable object.
    /// </summary>
    /// <typeparam name="TDisposable"> The type of the disposable. </typeparam>
    /// <typeparam name="TResult"> The type of the result. </typeparam>
    /// <param name="disposable"> The disposable. </param>
    /// <param name="result"> The result. </param>
    /// <returns> </returns>
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
    ///     Disposes the specified disposable object.
    /// </summary>
    /// <typeparam name="TDisposable"> The type of the disposable. </typeparam>
    /// <typeparam name="TResult"> The type of the result. </typeparam>
    /// <param name="disposable"> The disposable. </param>
    /// <param name="action"> The action. </param>
    /// <returns> </returns>
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
    ///     Gets the caller method.
    /// </summary>
    /// <param name="index"> The index. </param>
    /// <returns> </returns>
    public static MethodBase? GetCallerMethod(in int index = 2)
        => new StackTrace(true).GetFrame(index)?.GetMethod();

    /// <summary>
    ///     Gets the caller method.
    /// </summary>
    /// <param name="index"> The index. </param>
    /// <param name="parsePrevIfNull"> if set to <c> true </c> [parse previous if null]. </param>
    /// <returns> </returns>
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
    ///     Gets the name of the caller method.
    /// </summary>
    /// <param name="index"> The index. </param>
    /// <returns> </returns>
    public static string? GetCallerMethodName(in int index = 2)
        => GetCallerMethod(index)?.Name;

    /// <summary>
    ///     Gets the current method.
    /// </summary>
    /// <returns> </returns>
    public static MethodBase? GetCurrentMethod()
        => GetCallerMethod();

    /// <summary>
    ///     Gets the delegate.
    /// </summary>
    /// <typeparam name="TType"> The type of the type. </typeparam>
    /// <typeparam name="TDelegate"> The type of the delegate. </typeparam>
    /// <param name="methodName"> Name of the method. </param>
    /// <returns> </returns>
    /// <exception cref="InvalidOperationException"> </exception>
    public static TDelegate GetDelegate<TType, TDelegate>(in string methodName)
        => (TDelegate)(ISerializable)Delegate.CreateDelegate(typeof(TDelegate), typeof(TType).GetMethod(methodName) ?? Throw<MethodInfo>(new Exceptions.InvalidOperationException()));

    public static Func<TResult> GetFunc<TArg, TResult>(Func<TArg, TResult> func, TArg arg)
        => () => func(arg);

    public static Func<TResult> GetFunc<TArg1, TArg2, TResult>(Func<TArg1, TArg2, TResult> func, TArg1 arg1, TArg2 arg2)
        => () => func(arg1, arg2);

    public static Func<TResult> GetFunc<TArg1, TArg2, TArg3, TResult>(Func<TArg1, TArg2, TArg3, TResult> func, TArg1 arg1, TArg2 arg2, TArg3 arg3)
        => () => func(arg1, arg2, arg3);

    /// <summary>
    ///     Determines whether the specified try function has exception.
    /// </summary>
    /// <param name="tryFunc"> The try function. </param>
    /// <returns>
    ///     <c> true </c> if the specified tryFunc has exception; otherwise, <c> false </c>.
    /// </returns>
    public static bool HasException(in Action tryFunc)
        => Catch(tryFunc) is not null;

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

    public static TInstance With<TInstance>(this TInstance instance, [DisallowNull] Action<TInstance> action)
    {
        action.ArgumentNotNull(nameof(action))(instance);
        return instance;
    }
}