using Library.DesignPatterns.Markers;

namespace Library.CodeGeneration.Models;


[Immutable]
public readonly struct MethodArgument(in TypePath type, in string name) : IEquatable<MethodArgument>
{
    public string Name { get; } = name;
    public TypePath Type { get; } = type;

    public static bool operator !=(MethodArgument left, MethodArgument right)
        => !(left == right);

    public static bool operator ==(MethodArgument left, MethodArgument right)
        => left.Equals(right);

    public void Deconstruct(out TypePath type, out string name)
        => (type, name) = (this.Type, this.Name);

    public readonly bool Equals(MethodArgument other)
        => other.GetHashCode() == this.GetHashCode();

    public override bool Equals(object? obj)
        => obj is MethodArgument argument && this.Equals(argument);

    public override int GetHashCode()
        => HashCode.Combine(this.Name.GetHashCode(), this.Type.GetHashCode());
}

[Immutable]
public readonly struct PropertyAccessor(in bool has = true, in bool? isPrivate = null, in string? code = null)
{
    public string? Code { get; } = code;
    public bool Has { get; } = has;
    public bool? IsPrivate { get; } = isPrivate;
}