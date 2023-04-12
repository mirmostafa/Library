using System.Diagnostics;

using Library.Exceptions.Validations;

namespace Library.Coding;

[DebuggerStepThrough]
[StackTraceHidden]
public record struct Fluency<T>(T Value) : IEquatable<T>//, IConvertible<Fluency<T>, T>, IConvertible<T, Fluency<T>>
{
    public readonly bool Equals(T? other)
        => this.Value?.Equals(other) ?? (other is null);

    public override readonly string? ToString()
        => this.Value?.ToString() ?? base.ToString();

    public override readonly int GetHashCode()
        => this.Value?.GetHashCode() ?? base.GetHashCode();

    public static implicit operator T(Fluency<T> fluency)
        => fluency.Value;
    public static implicit operator Fluency<T>(T value)
        => new(value);

    [Obsolete("Subject to remove", true)]
    public readonly T ConvertTo()
        => this.Value;

    public static Fluency<T> New(T other)
        => new(other);


    [Obsolete("Subject to remove", true)]
    public static T From(Fluency<T> other)
        => other.Value;

    public readonly T GetValue()
        => this.Value;
    public readonly T GetValue([NotNullWhen(true)] bool checkNotNull)
        => checkNotNull && this.Value is null ? throw new NullValueValidationException() : this.Value;
}