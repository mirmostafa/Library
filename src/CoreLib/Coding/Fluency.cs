﻿using Library.Exceptions.Validations;
using Library.Interfaces;

namespace Library.Coding;

public record struct Fluency<T>(T Value) : IEquatable<T>, IConvertible<Fluency<T>, T>, IConvertible<T, Fluency<T>>
{
    public bool Equals(T? other)
        => this.Value?.Equals(other) ?? (other is null);
    public override string? ToString()
        =>
        this.Value?.ToString() ?? base.ToString();
    public override int GetHashCode()
        => this.Value?.GetHashCode() ?? base.GetHashCode();

    public static implicit operator T(Fluency<T> fluency)
        => fluency.Value;
    public static implicit operator Fluency<T>(T value)
        => new(value);

    public T ConvertTo()
        => this.Value;
    public static Fluency<T> ConvertFrom(T other)
        => new(other);

    public static Fluency<T> From(T other)
        => new(other);

    Fluency<T> IConvertible<T, Fluency<T>>.ConvertTo()
        => new(this.Value);

    public static T ConvertFrom(Fluency<T> other)
        => other.Value;

    public T GetValue()
        => this.Value;
    public T GetValue([NotNullWhen(true)] bool checkNotNull)
        => checkNotNull && this.Value is null ? throw new NullValueValidationException() : this.Value;
}

public interface ivalidationResult
{
    static invalid Invalid { get; } = new();
    static valid Valid { get; } = new();
}

public record struct valid : ivalidationResult { public static readonly valid Result = ivalidationResult.Valid; }
public record struct invalid : ivalidationResult { public static readonly invalid Result = ivalidationResult.Invalid; }