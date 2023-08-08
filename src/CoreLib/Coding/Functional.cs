using System.Diagnostics;

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
    /// <summary>
    /// Executes the specified try method and catches any exceptions that occur.
    /// </summary>
    /// <param name="tryMethod">The try method.</param>
    /// <param name="catchMethod">The catch method.</param>
    /// <param name="finallyMethod">The finally method.</param>
    /// <param name="handling">The exception handling.</param>
    /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
    /// <returns>The exception that was caught, or null if no exception was caught.</returns>
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
    /// Executes the specified action and returns the result or an exception.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="action">The action to execute.</param>
    /// <returns>A tuple containing the result or an exception.</returns>
    public static (TResult? Result, Exception? Exception) CatchFunc<TResult>(in Func<TResult> action)
    {
        Check.MustBeArgumentNotNull(action);
        try
        {
            return (action(), null);
        }
        catch (Exception ex)
        {
            return (default, ex);
        }
    }

    /// <summary>
    /// Executes the specified action and returns the result or a default value if an exception is thrown.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="action">The action to execute.</param>
    /// <param name="defaultValue">The default value to return if an exception is thrown.</param>
    /// <returns>
    /// A tuple containing the result of the action or the default value and the exception if one
    /// was thrown.
    /// </returns>
    public static (TResult Result, Exception? Exception) CatchFunc<TResult>(in Func<TResult> action, in TResult defaultValue)
    {
        Check.MustBeArgumentNotNull(action);
        try
        {
            return (action(), null);
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
            return action.ArgumentNotNull()();
        }
        catch (TException ex) when (predicate.ArgumentNotNull()(ex))
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

    /// <summary>
    /// Simulated
    /// <code>for</code>
    /// keyword.
    /// </summary>
    /// <typeparam name="Item">The type of the items in the IEnumerable.</typeparam>
    /// <param name="items">The IEnumerable of items.</param>
    /// <param name="action">The action to execute for each item.</param>
    /// <param name="getNext">A function to get the next index.</param>
    public static void For<Item>(IEnumerable<Item>? items, in Action<Item, int> action, Func<int, int>? getNext = null)
    {
        //If the action is null, return
        if (action is null || items is not null)
        {
            return;
        }

        //Set the getNext variable to increment the index
        getNext ??= i => ++i;

        //If the items are an array, loop through the array and execute the action
        if (items is Item[] arr)
        {
            for (var i = 0; i < arr.Length; i = getNext(i))
            {
                action(arr[i], i);
            }
        }
        //If the items are a list, loop through the list and execute the action
        else if (items is IList<Item> list)
        {
            for (var i = 0; i < list.Count; i = getNext(i))
            {
                action(list[i], i);
            }
        }
        //If the items are not null, loop through the items and execute the action
        else if (items is not null)
        {
            var enumerator = items.GetEnumerator();
            var index = 0;
            while (enumerator.MoveNext())
            {
                action(enumerator.Current, index);
                index = getNext(index);
            }
        }
    }

    public static TInstance ForEach<TInstance, Item>(in TInstance @this, IEnumerable<Item>? items, in Action<Item> action)
    {
        if (items is null || action is null)
        {
            return @this;
        }

        foreach (var item in items)
        {
            action(item);
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
    /// Creates a new instance of the generic type T.
    /// </summary>
    /// <typeparam name="T">The type of the instance to create.</typeparam>
    /// <returns>A new instance of the generic type T.</returns>
    public static T New<T>()
            where T : class, new()
            => new();

    public static T New<T>(params object[] args)
        => Activator.CreateInstance(typeof(T), args).NotNull().Cast().To<T>();

    /// <summary>
    /// Throws a new instance of the specified exception type.
    /// </summary>
    [DoesNotReturn]
    [DebuggerHidden]
    [StackTraceHidden]
    [DebuggerStepThrough]
    public static void Throw<TException>() where TException : Exception, new()
        => throw new TException();

    /// <summary>
    /// Throws the specified exception.
    /// </summary>
    /// <typeparam name="TFakeResult">The type of the return value.</typeparam>
    /// <param name="exception">The exception to throw.</param>
    /// <returns>This method does not return.</returns>
    [DoesNotReturn]
    public static TFakeResult Throw<TFakeResult>(in Exception exception) =>
        throw exception;

    /// <summary>
    /// Throws the specified exception.
    /// </summary>
    /// <param name="exception">The exception to throw.</param>
    [DoesNotReturn]
    public static void Throw(in Exception exception) =>
        throw exception;

    /// <summary>
    /// Throws an exception using the provided function.
    /// </summary>
    [DoesNotReturn]
    public static T Throw<T>(in Func<Exception> getException) =>
        throw getException();

    /// <summary>
    /// Throws an exception using the provided Func.
    /// </summary>
    [DoesNotReturn]
    public static void Throw(in Func<Exception> getException) =>
        throw getException();

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
        Check.MustBeArgumentNotNull(predicate);
        Check.MustBeArgumentNotNull(action);

        while (predicate())
        {
            yield return action();
        }

        onIterationDone?.Invoke();
    }

    public static void While(in Func<bool> predicate, in Action? action = null)
    {
        Check.MustBeArgumentNotNull(predicate);

        while (predicate())
        {
            action?.Invoke();
        }
    }
}