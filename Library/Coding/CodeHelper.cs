using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using Library.DesignPatterns.ExceptionHandlingPattern;
using Library.Exceptions;
using Library.Validations;

namespace Library.Coding;

public static class CodeHelper
{
    /// <summary>
    ///     Breaks code execution.
    /// </summary>
    public static void Break()
        => throw new BreakException();

    public static async Task Async()
        => await Task.CompletedTask;

    public static async Task<TResult> Async<TResult>(Func<TResult> action, CancellationToken cancellationToken = default)
        => await Task.Run(action, cancellationToken);

    public static async Task<TResult> Async<TResult>(Func<CancellationToken, TResult> action, CancellationToken cancellationToken = default)
        => await Task.Run(() => action.ArgumentNotNull(nameof(action))(cancellationToken), cancellationToken);

    public static Exception? Catch(
        in Action tryMethod,
        in Action<Exception>? catchMethod = null,
        in Action? finallyMethod = null,
        in ExceptionHandling? handling = null,
        in bool throwException = false)
    {
        tryMethod.IfArgumentNotNull(nameof(tryMethod));

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

    public static void Catch(in Action action, in ExceptionHandling exceptionHandling)
    {
        try
        {
            action?.Invoke();
        }
        catch (Exception ex)
        {
            exceptionHandling?.HandleException(ex);
        }
    }

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
        tryAction.IfArgumentNotNull(nameof(tryAction));

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

    public static TResult? CatchFunc<TResult, TException>(Func<TResult> action, Predicate<TException> predicate)
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
    public static TDelegate GetDelegate<TType, TDelegate>(in string methodName) =>
        (TDelegate)(ISerializable)Delegate.CreateDelegate(typeof(TDelegate), typeof(TType).GetMethod(methodName) ?? throw new InvalidOperationException());

    /// <summary>
    ///     Determines whether the specified try function has exception.
    /// </summary>
    /// <param name="tryFunc"> The try function. </param>
    /// <returns>
    ///     <c> true </c> if the specified try function has exception; otherwise, <c> false </c>.
    /// </returns>
    public static bool HasException(in Action tryFunc)
        => Catch(tryFunc) is not null;
    
    public static TResult Throw<TResult>(Func<TResult> action, Func<Exception, Exception>? getException)
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
                throw getException(ex);
            }

            throw;
        }
    }
}

public static class Methods
{
    public static Action<T> EmptyArg<T>() => x => { };
    public static Func<T, T> Self<T>() => x => x;
    public static Action Empty => () => { };
    public static Func<Task> EmptyTask => () => Task.CompletedTask;
    public static Func<Task> ToFunc(Func<Task> func) => func;
    public static Func<Task> Async => async () => await Task.CompletedTask;
}