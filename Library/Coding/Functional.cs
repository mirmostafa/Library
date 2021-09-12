﻿namespace Library.Coding;
public static class Functional
{
    public static bool IfTrue(this bool b, in Action ifTrue)
    {
        if (b is true)
        {
            ifTrue?.Invoke();
        }
        return b;
    }

    public static TInstance IfTrue<TInstance>(this TInstance @this, bool b, in Action ifTrue)
    {
        if (b is true)
        {
            ifTrue?.Invoke();
        }
        return @this;
    }
    public static TInstance If<TInstance>(this TInstance @this, bool b, in Action ifTrue, in Action ifFalse)
    {
        if (b is true)
        {
            ifTrue?.Invoke();
        }
        else
        {
            ifFalse?.Invoke();
        }

        return @this;
    }
    public static TInstance IfTrue<TInstance>(this TInstance @this, bool b, in Action<TInstance> ifTrue)
    {
        if (b is true)
        {
            ifTrue?.Invoke(@this);
        }
        return @this;
    }
    public static TInstance IfTrue<TInstance>(this TInstance @this, bool b, in Func<TInstance, TInstance> ifTrue)
        => b is true ? ifTrue(@this) : @this;

    public static T? IfTrue<T>(this bool b, in Func<T> ifTrue, in T? defaultValue = default) => b is true ? ifTrue.Invoke() : defaultValue;

    public static bool IfFalse(this bool b, in Action ifFalse)
    {
        if (b is false)
        {
            ifFalse?.Invoke();
        }
        return b;
    }
    public static T? IfFalse<T>(this bool b, in Func<T> ifFalse, in T? defaultValue = default) => b is false ? ifFalse.Invoke() : defaultValue;

    public static TInstance Fluent<TInstance>(this TInstance instance, in Action action)
    {
        action?.Invoke();
        return instance;
    }
    public static TInstance Fluent<TInstance>(this TInstance instance, in object? obj) => instance;

    public static TInstance Fluent<TInstance>(in TInstance instance, in Action<TInstance> action)
    {
        action(instance);
        return instance;
    }

    public static TInstance Fluent<TInstance>(in TInstance instance, in Action? action = null)
    {
        action?.Invoke();
        return instance;
    }

    public static TInstance Fluent<TInstance>(this TInstance instance, in Func<TInstance> func) => func.Invoke();
    public static TInstance Fluent<TInstance>(this TInstance instance, in Func<TInstance, TInstance> func) => func.Invoke(instance);

    public static (TInstance Instance, TResult Result) FluentByResult<TInstance, TResult>(this TInstance instance, in Func<TResult> func) => (instance, func.Invoke());
    public static (TInstance Instance, TResult Result) FluentByResult<TInstance, TResult>(this TInstance instance, in Func<TInstance, TResult> action) => (instance, action.Invoke(instance));

    public static async Task<TInstance> FluentAsync<TInstance>(this TInstance instance, Func<Task> action)
    {
        await action.Invoke();
        return instance;
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

    public static void Lock(this object? lockObject, Action action) => _ = lockObject.Lock(() =>
       {
           action.ArgumentNotNull()();
           return true;
       });

    public static TResult Lock<TResult>(this object? lockObject, in Func<TResult> action)
    {
        lock (lockObject ?? GetCallerMethod()!.DeclaringType!)
        {
            return action.ArgumentNotNull()();
        }
    }

    public static T New<T>() where T : new() => new();
    
    /// <summary>
    ///     Creates a new instance of TType.
    /// </summary>
    /// <typeparam name="T"> The type of the type. </typeparam>
    /// <returns> </returns>
    public static T? New<T>()
        where T : class
    {
        var ctor = typeof(T).GetConstructor(EnumerableHelper.EmptyArray<Type>());
        return ctor is not null ? (T)ctor.Invoke(null) : default;
    }

    /// <summary>
    ///     Creates a new instance in object o.
    /// </summary>
    /// <typeparam name="T"> The type of the type. </typeparam>
    /// <param name="type"> The type. </param>
    /// <returns> </returns>
    public static T? New<T>(in Type type)
        where T : class => (T?)type.GetConstructor(EnumerableHelper.EmptyArray<Type>())?.Invoke(null);

    /// <summary>
    ///     Creates an new instance of TType.
    /// </summary>
    /// <typeparam name="T"> The type of the type. </typeparam>
    /// <param name="types"> The types. </param>
    /// <param name="args"> The constructor's arguments. </param>
    /// <returns> </returns>
    public static T? New<T>(in Type[] types, in object?[] args)
        where T : class
    {
        var constructorInfo = typeof(T).GetConstructor(types);
        return constructorInfo is not null ? (T)constructorInfo.Invoke(args) : null;
    }

    public static IEnumerable<TResult> While<TResult>(Func<bool> predicate, Func<TResult> action, Action? onIterationDone = null)
    {
        predicate.IfArgumentNotNull(nameof(predicate));
        action.IfArgumentNotNull(nameof(action));

        while (predicate())
        {
            yield return action();
        }

        onIterationDone?.Invoke();
    }

    public static void While(in Func<bool> predicate, in Action? action = null)
    {
        predicate.IfArgumentNotNull(nameof(predicate));

        while (predicate())
        {
            action?.Invoke();
        }
    }
}
