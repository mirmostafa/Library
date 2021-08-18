#pragma warning disable CA1031 // Do not catch general exception types
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using Library.DesignPatterns.ExceptionHandlingPattern;
using Library.Exceptions;

namespace Library.Coding;
public static class CodeHelper
{
    public static async Task Async() => await Task.CompletedTask;

    public static async Task<TResult> Async<TResult>(Func<TResult> action, CancellationToken cancellationToken = default)
        => await Task.Run(action, cancellationToken);

    public static async Task<TResult> Async<TResult>(Func<CancellationToken, TResult> action, CancellationToken cancellationToken = default)
        => await Task.Run(() => action.ArgumentNotNull(nameof(action))(cancellationToken), cancellationToken);

    /// <summary>
    ///     Breaks code execution.
    /// </summary>
    public static void Break() => throw new BreakException();

    public static Exception? Catch(in Action tryMethod,
        in Action<Exception>? catchMethod = null,
        in Action? finallyMethod = null,
        in ExceptionHandling? handling = null,
        bool throwException = false)
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
    public static TResult? CatchFunc<TResult>(in Func<TResult> tryAction,
        in Func<Exception, TResult> catchAction,
        out Exception? exception,
        in Action? finallyAction = null,
        in ExceptionHandling? handling = null,
        bool throwException = false)
    {
        tryAction.IfArgumentNotNull(nameof(tryAction));
        catchAction.IfArgumentNotNull(nameof(catchAction));

        handling?.Reset();
        exception = null;
        try
        {
            return tryAction();
        }
        catch (Exception ex)
        {
            var result = catchAction(ex);
            handling?.HandleException(ex);
            if (throwException)
            {
                throw;
            }

            exception = ex;
            return result;
        }
        finally
        {
            finallyAction?.Invoke();
        }
    }

    /// <summary>
    ///     Catches the exceptions the function.
    /// </summary>
    /// <typeparam name="TResult"> The type of the result. </typeparam>
    /// <param name="tryAction"> The try action. </param>
    /// <param name="exception"> The exception. </param>
    /// <param name="defaultResult"> The default result. </param>
    /// <param name="finallyAction"> The finally action. </param>
    /// <param name="handling"> The handling. </param>
    /// <param name="throwException"> if set to <c> true </c> [throw exception]. </param>
    /// <returns> </returns>
    public static TResult? CatchFunc<TResult>(in Func<TResult> tryAction,
        out Exception? exception,
        TResult? defaultResult = default,
        in Action? finallyAction = null,
        in ExceptionHandling? handling = null,
        bool throwException = false)
    {
        var result = CatchFunc(tryAction,
                ex => defaultResult,
                out var buffer,
                finallyAction,
                handling,
                throwException);
        exception = buffer;
        return result;
    }

    public static (TResult? Result, Exception? Exception) Catch<TResult>(Func<TResult> action)
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

    public static TResult? Catch<TResult, TException>(Func<TResult> action, Predicate<TException> predicate)
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
    public static string? GetCallerMethodName(in int index = 2) => GetCallerMethod(index)?.Name;

    /// <summary>
    ///     Gets the current method.
    /// </summary>
    /// <returns> </returns>
    public static MethodBase? GetCurrentMethod() => GetCallerMethod();

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

    public static Action<T> EmptyIntAction<T>() => x => { };
    public static Func<T, T> SelfIntFunc<T>() => x => x;
    public static Action EmptyAction => () => { };
    public static Func<Task> EmptyActionTask => () => Task.CompletedTask;
    public static Func<Task> GetAction(Func<Task> func) => func;
    public static Func<Task> ReturnsTask => async () => await Task.CompletedTask;
}
#pragma warning restore CA1031 // Do not catch general exception types
