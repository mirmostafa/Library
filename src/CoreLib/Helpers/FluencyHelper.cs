using System.Diagnostics;

using Library.Validations;

namespace Library.Helpers;

[DebuggerStepThrough]
public static class FluencyHelper
{
    public static async Task<TInstance> Async<TInstance>(this Fluency<TInstance> instance, Func<Task> funcAsync)
    {
        await funcAsync();
        return instance.Value;
    }

    public static Fluency<TInstance> Fluent<TInstance>(this TInstance o)
        => new(o);

    public static Fluency<TInstance> Fluent<TInstance>(this TInstance instance, in Action action)
    {
        action?.Invoke();
        return instance;
    }

    public static Fluency<TInstance> Fluent<TInstance>(this TInstance instance, in object? obj)
        => instance;

    public static Fluency<TInstance> Fluent<TInstance>(this TInstance instance, in Action<TInstance> action)
    {
        action(instance);
        return instance;
    }

    public static Fluency<TInstance> Fluent<TInstance>(in TInstance instance, in Action? action = null)
    {
        action?.Invoke();
        return instance;
    }

    public static Fluency<TInstance> Fluent<TInstance>(this TInstance instance, in Func<TInstance> func)
        => func.Invoke();

    public static Fluency<TInstance> Fluent<TInstance>(this TInstance instance, in Func<TInstance, TInstance> func)
        => func.Invoke(instance);

    public static async Task<Fluency<TResult>> FluentAsync<TFuncResult, TResult>(this object @this, Func<Task<TFuncResult>> funcAsync, Func<TFuncResult, TResult> action)
        => action(await funcAsync());

    public static Fluency<TInstance> If<TInstance>([DisallowNull] this Fluency<TInstance> @this, bool b, in Func<TInstance, TInstance> ifTrue, in Func<TInstance, TInstance> ifFalse)
        => b is true ? new(ifTrue.NotNull()(@this.Value)) : new(ifFalse.NotNull()(@this.Value));

    public static Fluency<TInstance> IfTrue<TInstance>(this Fluency<TInstance> @this, bool b, in Action ifTrue)
    {
        if (b)
        {
            ifTrue?.Invoke();
        }

        return @this;
    }

    public static Fluency<TInstance> IfTrue<TInstance>([DisallowNull] this Fluency<TInstance> @this, bool b, in Func<TInstance, TInstance> ifTrue)
        => b is true ? new(ifTrue.NotNull()(@this.Value)) : @this;

    public static Fluency<TInstance> IfTrue<TInstance>(this Fluency<TInstance> @this, bool b, in Action<TInstance> ifTrue)
    {
        if (b is true)
        {
            ifTrue?.Invoke(@this.Value);
        }
        return @this;
    }

    public static (TInstance Instance, TResult Result) Result<TInstance, TResult>(this Fluency<TInstance> instance, in Func<TResult> func)
        => (instance.Value, func.Invoke());

    public static Fluency<TInstance> Then<TInstance>(this Fluency<TInstance> instance, in Action action)
    {
        action?.Invoke();
        return instance;
    }

    public static Fluency<TResult> Then<TInstance, TResult>(this Fluency<TInstance> instance, in Action action, in TResult result)
    {
        action?.Invoke();
        return result;
    }

    public static Fluency<TInstance> Then<TInstance>(this Fluency<TInstance> instance, in Action<TInstance> action)
    {
        action(instance);
        return instance;
    }

    public static Fluency<TInstance> Then<TInstance>(this Fluency<TInstance> instance, in Func<TInstance, TInstance> action) 
        => action(instance);

    //! Not Object Oriented !
    ////public static Fluency<TResult> Then<TInstance, TResult>(this Fluency<TInstance> instance, in TResult result)
    ////    => result;
    ////public static Fluency<TResult> Then<TInstance, TResult>(this Fluency<TInstance> instance, in Func<TResult> result)
    ////    => result();
}