using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

using Library.Validations;

namespace Library.Helpers;

[DebuggerStepThrough]
[StackTraceHidden]
public static class FluencyHelper
{
    public static async Task<TInstance> Async<TInstance>(this Fluency<TInstance> instance, Func<Task> funcAsync)
    {
        await funcAsync.ArgumentNotNull()();
        return instance.GetValue();
    }

    public static Fluency<TInstance> Fluent<TInstance>(this TInstance o) =>
        new(o);

    public static Fluency<TInstance> Fluent<TInstance>(this TInstance instance, in Action? action)
    {
        action?.Invoke();
        return instance;
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fluency<TInstance> Fluent<TInstance>(this TInstance instance, in object? obj) =>
        instance;

    public static Fluency<TInstance> Fluent<TInstance>(this TInstance instance, in Action<TInstance>? action)
    {
        action?.Invoke(instance);
        return instance;
    }

    public static Fluency<TInstance> Fluent<TInstance>(in TInstance instance, in Action? action = null)
    {
        action?.Invoke();
        return instance;
    }

    public static Fluency<TInstance> Fluent<TInstance>(this TInstance instance, in Func<TInstance> func) =>
        func.ArgumentNotNull().Invoke();

    public static Fluency<TInstance> Fluent<TInstance>(this TInstance instance, in Func<TInstance, TInstance> func) =>
        func.ArgumentNotNull().Invoke(instance);

    public static async Task<Fluency<TResult>> Fluent<TFuncResult, TResult>(this object @this, Func<Task<TFuncResult>> funcAsync, Func<TFuncResult, TResult> action) =>
        action.ArgumentNotNull()(await funcAsync.ArgumentNotNull()());

    public static async Task<Fluency<TResult>> Fluent<TResult>(this object @this, Func<Task<TResult>> funcAsync, Action action)
    {
        var result = await funcAsync.ArgumentNotNull()();
        action.ArgumentNotNull()();
        return new(result);
    }

    public static Fluency<TInstance> If<TInstance>([DisallowNull] this Fluency<TInstance> @this, bool b, in Func<TInstance, TInstance> ifTrue, in Func<TInstance, TInstance> ifFalse)
    {
        if (b is true)
        {
            if (ifTrue is not null)
            {
                return new(ifTrue.ArgumentNotNull()(@this.GetValue()));
            }
        }
        else
        {
            if (ifFalse is not null)
            {
                return new(ifFalse.ArgumentNotNull()(@this.GetValue()));
            }
        }
        return @this;
    }

    public static Fluency<TInstance> IfTrue<TInstance>(this Fluency<TInstance> @this, bool b, in Action ifTrue)
    {
        if (b)
        {
            ifTrue?.Invoke();
        }

        return @this;
    }

    public static Fluency<TInstance> IfTrue<TInstance>([DisallowNull] this Fluency<TInstance> @this, bool b, in Func<TInstance, TInstance> ifTrue)
        => b is true ? new(ifTrue.NotNull()(@this.GetValue())) : @this;

    public static Fluency<TInstance> IfTrue<TInstance>(this Fluency<TInstance> @this, bool b, in Action<TInstance> ifTrue)
    {
        if (b is true)
        {
            ifTrue?.Invoke(@this.GetValue());
        }
        return @this;
    }

    public static Fluency<TList> Items<TList, TItem>(this Fluency<TList> list, in Action<TItem> action)
        where TList : IEnumerable<TItem>
    {
        foreach (var item in list.GetValue())
        {
            action.ArgumentNotNull()(item);
        }
        return list;
    }

    public static Fluency<TList> Items<TList, TItem>(this Fluency<TList> list, in Func<TItem, TItem> action, Func<IEnumerable<TItem>, TList> convert)
        where TList : IEnumerable<TItem> =>
        //return new Fluency<TList>(convert(iterate(list.Value, action)));
        new(convert.ArgumentNotNull()(list.GetValue().Select(action)));

    public static (TInstance Instance, TResult Result) Result<TInstance, TResult>(this Fluency<TInstance> instance, in Func<TResult> func) =>
        (instance.GetValue(), func.Invoke());

    public static Fluency<TInstance> With<TInstance>(this Fluency<TInstance> instance, in Action action)
    {
        action?.Invoke();
        return instance;
    }

    public static Fluency<TInstance> With<TInstance>(this Fluency<TInstance> instance, in Action<TInstance>? action)
    {
        action?.Invoke(instance);
        return instance;
    }

    public static Fluency<TResult> With<TResult>(this Fluency<TResult> instance, in Func<TResult, TResult>? func) =>
        func is null ? instance : new Fluency<TResult>(func.ArgumentNotNull()(instance.GetValue()));

    public static Fluency<TResult> WithNew<TInstance, TResult>(this Fluency<TInstance> instance, in Func<TInstance, TResult> action) =>
        action.ArgumentNotNull()(instance);
}