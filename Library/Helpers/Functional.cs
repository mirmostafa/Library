namespace Library.Helpers;
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

    public static (TInstance Instance, TResult Result) FluentByResult<TInstance, TResult>(this TInstance instance, in Func<TResult> action)
        => (instance, action.Invoke());

    public static async Task<TInstance> FluentAsync<TInstance>(this TInstance instance, Func<Task> action)
    {
        await action.Invoke();
        return instance;
    }

    public static TInstance Fluent<TInstance>(in TInstance instance, in Action<TInstance>? action = null)
    {
        action?.Invoke(instance);
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

    public static void Lock(this object? lockObject, Action action) => _ = Lock(lockObject, () =>
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

    public static IEnumerable<TResult> While<TResult>(Func<bool> predicate, Func<TResult> action, Action? onIterationDone = null)
    {
        Check.IfArgumentNotNull(predicate, nameof(predicate));
        Check.IfArgumentNotNull(action, nameof(action));

        while (predicate())
        {
            yield return action();
        }

        onIterationDone?.Invoke();
    }

    public static void While(in Func<bool> predicate, in Action? action = null)
    {
        Check.IfArgumentNotNull(predicate, nameof(predicate));

        while (predicate())
        {
            action?.Invoke();
        }
    }
}
