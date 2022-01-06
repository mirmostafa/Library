using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization;
using Library.DesignPatterns.ExceptionHandlingPattern;
using Library.Exceptions;
using Library.Validations;

namespace Library.Coding;

[DebuggerStepThrough]
[StackTraceHidden]
public static class CodeHelper
{
    /// <summary>
    ///     Breaks code execution.
    /// </summary>
    [DoesNotReturn]
    public static void Break() =>
        Throw<BreakException>();

    public static async Task<TResult> Async<TResult>(Func<TResult> action, CancellationToken cancellationToken = default) =>
        await Task.Run(action, cancellationToken);

    public static async Task<TResult> Async<TResult>(TResult result) =>
        await Task.FromResult(result);

    public static async Task<TResult> Async<TResult>(Func<CancellationToken, TResult> action, CancellationToken cancellationToken = default) =>
        await Task.Run(() => action.ArgumentNotNull(nameof(action))(cancellationToken), cancellationToken);

    public static Exception? Catch(
        in Action tryMethod,
        in Action<Exception>? catchMethod = null,
        in Action? finallyMethod = null,
        in ExceptionHandling? handling = null,
        in bool throwException = false)
    {
        tryMethod.ArgumentNotNull(nameof(tryMethod));

        handling?.Reset();
        try
        {
            tryMethod();
            return null;
        }
        catch (Exception ex)
        {
            catchMethod?.Invoke(ex);
            handling?.HandleException(ex);
            if (throwException)
            {
                throw;
            }

            return ex;
        }
        finally
        {
            finallyMethod?.Invoke();
        }
    }

    public static void Catch(in Action action, in ExceptionHandling exceptionHandling) =>
        Catch(action, handling: exceptionHandling);

    /// <summary>
    ///     Catches the exceptions in specific function.
    /// </summary>
    /// <typeparam name="TResult"> The type of the result. </typeparam>
    /// <param name="tryAction"> The try action. </param>
    /// <param name="catchAction"> The catch action. </param>
    /// <param name="exception"> The exception. </param>
    /// <param name="finallyAction"> The finally action. </param>
    /// <param name="handling"> The handling. </param>
    /// <param name="throwException"> if set to <c> true </c> [throw exception]. </param>
    /// <returns> </returns>
    /// <exception cref="ArgumentNullException"> tryAction or catchAction </exception>
    public static (TResult? Result, Exception? Exception) CatchFunc<TResult>(
        in Func<TResult> tryAction,
        in Func<Exception, TResult>? catchAction = null,
        in Action? finallyAction = null,
        in ExceptionHandling? handling = null,
        bool throwException = false)
    {
        Check.IfArgumentNotNull(tryAction, nameof(tryAction));

        handling?.Reset();
        try
        {
            return (tryAction(), null);
        }
        catch (Exception ex)
        {
            var result = catchAction is not null ? catchAction(ex) : default;
            handling?.HandleException(ex);
            if (throwException)
            {
                throw;
            }

            return (result, ex);
        }
        finally
        {
            finallyAction?.Invoke();
        }
    }

    public static (TResult? Result, Exception? Exception) CatchFunc<TResult>(in Func<TResult> action)
    {
        try
        {
            return (action.ArgumentNotNull(nameof(action))(), null);
        }
        catch (Exception ex)
        {
            return (default, ex);
        }
    }

    public static (TResult Result, Exception? Exception) CatchFunc<TResult>(in Func<TResult> action, in TResult defaultValue)
    {
        try
        {
            return (action.ArgumentNotNull(nameof(action))(), null);
        }
        catch (Exception ex)
        {
            return (defaultValue, ex);
        }
    }

    public static TResult? Catch<TResult, TException>(Func<TResult> action)
        where TException : Exception
    {
        try
        {
            return action.ArgumentNotNull(nameof(action))();
        }
        catch (TException)
        {
            return default;
        }
    }

    public static TResult? CatchFunc<TResult, TException>(in Func<TResult> action, in Predicate<TException> predicate)
        where TException : Exception
    {
        try
        {
            return action.ArgumentNotNull(nameof(action))();
        }
        catch (TException ex) when (predicate.ArgumentNotNull(nameof(predicate))(ex))
        {
            return default;
        }
    }
    public static async Task<Exception?> CatchAsync(Action action)
    {
        try
        {
            await Task.Factory.StartNew(action);
            return null;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    /// <summary>
    ///     Gets the caller method.
    /// </summary>
    /// <param name="index"> The index. </param>
    /// <returns> </returns>
    public static MethodBase? GetCallerMethod(in int index = 2) =>
        new StackTrace(true).GetFrame(index)?.GetMethod();

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
    public static string? GetCallerMethodName(in int index = 2) =>
        GetCallerMethod(index)?.Name;

    /// <summary>
    ///     Gets the current method.
    /// </summary>
    /// <returns> </returns>
    public static MethodBase? GetCurrentMethod() =>
        GetCallerMethod();

    /// <summary>
    ///     Gets the delegate.
    /// </summary>
    /// <typeparam name="TType"> The type of the type. </typeparam>
    /// <typeparam name="TDelegate"> The type of the delegate. </typeparam>
    /// <param name="methodName"> Name of the method. </param>
    /// <returns> </returns>
    /// <exception cref="InvalidOperationException"> </exception>
    public static TDelegate GetDelegate<TType, TDelegate>(in string methodName) =>
        (TDelegate)(ISerializable)Delegate.CreateDelegate(typeof(TDelegate), typeof(TType).GetMethod(methodName) ?? Throw<MethodInfo>(new Exceptions.InvalidOperationException()));

    /// <summary>
    ///     Determines whether the specified try function has exception.
    /// </summary>
    /// <param name="tryFunc"> The try function. </param>
    /// <returns>
    ///     <c> true </c> if the specified tryFunc has exception; otherwise, <c> false </c>.
    /// </returns>
    public static bool HasException(in Action tryFunc) =>
        Catch(tryFunc) is not null;

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

    [DoesNotReturn]
    public static void Throw<TException>() where TException : Exception, new() =>
        ExceptionDispatchInfo.Throw(new TException());

    [DoesNotReturn]
    public static T Throw<T>(in Exception exception) =>
        throw exception;

    [DoesNotReturn]
    public static void Throw(in Exception exception) =>
        ExceptionDispatchInfo.Throw(exception);

    [DoesNotReturn]
    public static T Throw<T>(in Func<Exception> getException) =>
        throw getException();

    [DoesNotReturn]
    public static void Throw(in Func<Exception> getException) =>
        ExceptionDispatchInfo.Throw(getException());

    /// <summary>
    ///     Disposes the specified disposable object.
    /// </summary>
    /// <typeparam name="TDisposable"> The type of the disposable. </typeparam>
    /// <param name="disposable"> The disposable. </param>
    /// <param name="action"> The action. </param>
    public static void Dispose<TDisposable>(in TDisposable disposable, in Action<TDisposable>? action = null)
        where TDisposable : IDisposable
        => ObjectHelper.Dispose(disposable, action);

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
        => ObjectHelper.Dispose(disposable, action);

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
        => ObjectHelper.Dispose(disposable, result);


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
        => ObjectHelper.Dispose(disposable, action);
}