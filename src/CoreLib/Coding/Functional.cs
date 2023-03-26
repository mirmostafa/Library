using System.Diagnostics;
using System.Runtime.ExceptionServices;

using Library.DesignPatterns.ExceptionHandlingPattern;
using Library.Results;
using Library.Validations;

namespace Library.Coding;

/// <summary>
/// C# statements in functional way. (nothing more)
/// </summary>
//! C# statements in functional way. (nothing more)
[DebuggerStepThrough]
[StackTraceHidden]
public static class Functional
{
    public static Task<TResult> Async<TResult>(Func<TResult> action, CancellationToken cancellationToken = default)
        => Task.Run(action, cancellationToken);

    public static Exception? Catch(
        in Action tryMethod,
        in Action<Exception>? catchMethod = null,
        in Action? finallyMethod = null,
        in ExceptionHandling? handling = null,
        in bool throwException = false)
    {
        _ = tryMethod.ArgumentNotNull(nameof(tryMethod));

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
        => Catch(action, handling: exceptionHandling);

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

    public static async Task<Result> CatchAsync(Func<Task> action)
    {
        try
        {
            await action();
            return Result.CreateSuccess();
        }
        catch (Exception ex)
        {
            return Result.CreateFailure(error: ex);
        }
    }

    public static async Task<Result<TResult?>> CatchAsync<TResult>(Func<Task<TResult?>> action)
    {
        try
        {
            return Result<TResult?>.CreateSuccess(await action());
        }
        catch (Exception ex)
        {
            return Result<TResult?>.CreateFailure(error: ex);
        }
    }

    /// <summary>
    /// Catches the exceptions in specific function.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="tryAction">     The try action.</param>
    /// <param name="catchAction">   The catch action.</param>
    /// <param name="exception">     The exception.</param>
    /// <param name="finallyAction"> The finally action.</param>
    /// <param name="handling">      The handling.</param>
    /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">tryAction or catchAction</exception>
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

    public static void Finally(Action action, Action done)
    {
        try
        {
            action?.Invoke();
        }
        finally
        {
            done?.Invoke();
        }
    }

    public static TInstance For<TInstance, Item>(in TInstance @this, IEnumerable<Item>? items, in Action<(Item Item, int Index)> action, Func<int, int>? getNext = null)
    {
        var act = action;
        For(items, (Item item, int index) => act((item, index)), getNext);
        return @this;
    }

    public static TInstance For<TInstance, Item>(in TInstance @this, IEnumerable<Item>? items, in Action<Item, int> action, Func<int, int>? getNext = null)
    {
        var act = action;
        For(items, (Item item, int index) => act(item, index), getNext);
        return @this;
    }

    public static void For<Item>(IEnumerable<Item>? items, in Action<(Item Item, int Index)> action, Func<int, int>? getNext = null)
    {
        var act = action;
        For(items, (Item item, int index) => act((item, index)), getNext);
    }

    public static void For<Item>(IEnumerable<Item>? items, in Action<Item, int> action, Func<int, int>? getNext = null)
    {
        if (action is not null)
        {
            getNext ??= i => ++i;
            if (items is Item[] arr)
            {
                for (var i = 0; i < arr.Length; i = getNext(i))
                {
                    action(arr[i], i);
                }
            }
            else if (items is IList<Item> list)
            {
                for (var i = 0; i < list.Count; i = getNext(i))
                {
                    action(list[i], i);
                }
            }
            else if (items is not null)
            {
                for (var i = 0; i < items.Count(); i = getNext(i))
                {
                    action(items.ElementAt(i), i);
                }
            }
        }
    }

    public static TInstance ForEach<TInstance, Item>(in TInstance @this, IEnumerable<Item>? items, in Action<Item> action)
    {
        if (items is not null && action is not null)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }

        return @this;
    }

    public static void If(Func<bool> b, in Action ifTrue, in Action? ifFalse = null)
    {
        if (b() == true)
        {
            ifTrue?.Invoke();
        }
        else
        {
            ifFalse?.Invoke();
        }
    }

    public static void If(bool b, in Action ifTrue, in Action? ifFalse = null)
    {
        if (b == true)
        {
            ifTrue?.Invoke();
        }
        else
        {
            ifFalse?.Invoke();
        }
    }

    public static bool IfFalse(this bool b, in Action ifFalse)
    {
        if (b is false)
        {
            ifFalse?.Invoke();
        }
        return b;
    }

    public static T IfFalse<T>(this bool b, in Func<T> ifFalse, in T defaultValue = default!)
        => b is false ? ifFalse.Invoke() : defaultValue;

    public static bool IfTrue(this bool b, in Action ifTrue)
    {
        if (b is true)
        {
            ifTrue?.Invoke();
        }
        return b;
    }

    public static T IfTrue<T>(this bool b, in Func<T> ifTrue, in T defaultValue = default!)
        => b is true ? ifTrue.Invoke() : defaultValue;

    public static List<TItem> List<TItem>(params TItem[] items)
    {
        var result = new List<TItem>();
        result.AddRange(items);
        return result;
    }

    public static void Lock(object? lockObject, Action action)
            => _ = Lock(lockObject, ()
            =>
            {
                action.ArgumentNotNull(nameof(action))();
                return true;
            });

    public static TResult Lock<TResult>(object? lockObject, in Func<TResult> action)
    {
        lock (lockObject ?? CodeHelper.GetCallerMethod()!.DeclaringType!)
        {
            return action.ArgumentNotNull()();
        }
    }

    /// <summary>
    /// Creates a new instance of TType.
    /// </summary>
    /// <typeparam name="T">The type of the type.</typeparam>
    /// <returns></returns>
    public static T New<T>()
        where T : class, new()
        => new();

    public static T New<T>(params object[] args)
        => Activator.CreateInstance(typeof(T), args).NotNull().To<T>();

    [DoesNotReturn]
    public static void Throw<TException>() where TException : Exception, new()
        => ExceptionDispatchInfo.Throw(new TException());

    [DoesNotReturn]
    public static T Throw<T>(in Exception exception)
        => throw exception;

    [DoesNotReturn]
    public static void Throw(in Exception exception)
        => ExceptionDispatchInfo.Throw(exception);

    [DoesNotReturn]
    public static T Throw<T>(in Func<Exception> getException)
        => throw getException();

    [DoesNotReturn]
    public static void Throw(in Func<Exception> getException)
        => ExceptionDispatchInfo.Throw(getException());

    public static Action ToAction(Action action)
        => action;

    public static Func<TResult> ToFunc<TResult>(Func<TResult> action)
        => action;

    public static void Using<TDisposable>(Func<TDisposable> getItem, Action<TDisposable> action)
                where TDisposable : IDisposable
    {
        using var item = getItem();
        action(item);
    }

    public static TResult Using<TDisposable, TResult>(Func<TDisposable> getItem, Func<TDisposable, TResult> action)
        where TDisposable : IDisposable
    {
        using var item = getItem();
        return action(item);
    }

    public static IEnumerable<TResult> While<TResult>(Func<bool> predicate, Func<TResult> action, Action? onIterationDone = null)
    {
        Check.IfArgumentNotNull(predicate);
        Check.IfArgumentNotNull(action);

        while (predicate())
        {
            yield return action();
        }

        onIterationDone?.Invoke();
    }

    public static void While(in Func<bool> predicate, in Action? action = null)
    {
        Check.IfArgumentNotNull(predicate);

        while (predicate())
        {
            action?.Invoke();
        }
    }
}