using System.Diagnostics;

using Library.Exceptions.Validations;

namespace Library.Coding;

[DebuggerStepThrough]
[StackTraceHidden]
public readonly struct Fluency<T>(T value) : IEquatable<T>//, IConvertible<Fluency<T>, T>, IConvertible<T, Fluency<T>>
, IEquatable<Fluency<T>>
{
    private readonly T _value = value;

    public static implicit operator Fluency<T>(T value)
        => new(value);

    public static implicit operator T(Fluency<T> fluency)
        => fluency._value;

    public static bool operator !=(Fluency<T> left, Fluency<T> right) =>
        !(left == right);

    public static bool operator ==(Fluency<T> left, Fluency<T> right) =>
        left.Equals(right);

    public readonly bool Equals(T? other) =>
        this._value?.Equals(other) ?? (other is null);

    public override bool Equals(object? obj) =>
        obj is Fluency<T> f && f.Equals(this);

    public bool Equals(Fluency<T> other) =>
        other._value?.Equals(this._value) ?? (this._value == null);

    public override readonly int GetHashCode() =>
        this._value?.GetHashCode() ?? base.GetHashCode();

    public readonly T GetValue() =>
        this._value;

    public readonly T GetValue([NotNullWhen(true)] bool checkNotNull) =>
        checkNotNull && this._value is null ? throw new NullValueValidationException() : this._value;

    public override readonly string? ToString() =>
        this._value?.ToString() ?? base.ToString();
}