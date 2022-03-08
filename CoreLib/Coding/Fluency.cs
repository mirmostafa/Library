namespace Library.Coding;

public record struct Fluency<T>(T Value) : IEquatable<T>
{
    public bool Equals(T? other) => this.Value?.Equals(other) ?? (other is null);
    public override string? ToString() => this.Value?.ToString() ?? base.ToString();
    public override int GetHashCode() => this.Value?.GetHashCode() ?? base.GetHashCode();

    public static implicit operator T(Fluency<T> fluency) => fluency.Value;
    public static implicit operator Fluency<T>(T value) => new(value);
    public T ToValue() => this.Value;
    public static Fluency<T> FromValue(T value) => new(value);
}

public static class FluencyHelper
{
    public static Fluency<T?> Fluent<T>(this T? o) =>
        new(o);

    public static Fluency<TInstance> IfTrue<TInstance>(this Fluency<TInstance> @this, bool b, in Action ifTrue)
    {
        if (b)
        {
            ifTrue?.Invoke();
        }

        return @this;
    }
    public static Fluency<TInstance> IfTrue<TInstance>(this Fluency<TInstance> @this, bool b, in Func<TInstance, TInstance> ifTrue) =>
        b is true ? new(ifTrue(@this.Value)) : @this;
    public static Fluency<TInstance> IfTrue<TInstance>(this Fluency<TInstance> @this, bool b, in Action<TInstance> ifTrue)
    {
        if (b is true)
        {
            ifTrue?.Invoke(@this.Value);
        }
        return @this;
    }

    public static async Task<TInstance> Async<TInstance>(this Fluency<TInstance> instance, Func<Task> funcAsync)
    {
        await funcAsync();
        return instance.Value;
    }

    public static TInstance Fluent<TInstance>(this TInstance instance, in Action action)
    {
        action?.Invoke();
        return instance;
    }
    public static TInstance Fluent<TInstance>(this TInstance instance, in object? obj) =>
        instance;
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
    public static TInstance Fluent<TInstance>(this TInstance instance, in Func<TInstance> func) =>
        func.Invoke();
    public static TInstance Fluent<TInstance>(this TInstance instance, in Func<TInstance, TInstance> func) =>
        func.Invoke(instance);
    public static (TInstance Instance, TResult Result) Result<TInstance, TResult>(this Fluency<TInstance> instance, in Func<TResult> func) =>
        (instance.Value, func.Invoke());
}