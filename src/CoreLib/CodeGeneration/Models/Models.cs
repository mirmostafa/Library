using Library.DesignPatterns.Markers;

namespace Library.CodeGeneration.Models;

[Immutable]
public readonly struct ConstructorArgument(in TypePath type, in string name, in string? dataMemberName = null, in bool isProperty = false)
{
    public string? DataMemberName { get; } = dataMemberName;
    public bool IsProperty { get; } = isProperty;
    public string Name { get; } = name;
    public TypePath Type { get; } = type;
}

[Immutable]
public readonly struct GenerateCodeResult(in Code? main, in Code? partial)
{
    public Code? Main { get; } = main;
    public Code? Partial { get; } = partial;

    public void Deconstruct(out Code? main, out Code? partial)
        => (main, partial) = (this.Main, this.Partial);
}

[Immutable]
public readonly struct GenerateCodesParameters(in bool generateMainCode = false, in bool generatePartialCode = true, in bool generateUiCode = false)
{
    public bool GenerateMainCode { get; } = generateMainCode;
    public bool GeneratePartialCode { get; } = generatePartialCode;
    public bool GenerateUiCode { get; } = generateUiCode;
}

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
            => EqualityComparer<TypePath>.Default.Equals(this.Type, other.Type) && EqualityComparer<string>.Default.Equals(this.Name, other.Name);

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

public static class ModelsExtensions
{
    public static void Deconstruct(this MethodArgument argument, out TypePath type, out string name)
        => (type, name) = (argument.Type, argument.Name);
}