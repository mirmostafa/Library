using System.CodeDom;

namespace Library.CodeGeneration.Models;

public struct PropertyInfo(
    in string type,
    in string name,
    in MemberAttributes? accessModifier = null,
    in PropertyAccessor? getter = null,
    in PropertyAccessor? setter = null) : IMemberInfo, IEquatable<PropertyInfo>
{
    public MemberAttributes? AccessModifier { get; init; } = accessModifier;
    public List<string> Attributes { get; } = new List<string>();
    public string? BackingFieldName { get; init; } = null;
    public string? Comment { get; init; } = null;
    public PropertyAccessor? Getter { get; init; } = getter;
    public bool HasBackingField { get; init; } = false;
    public string? InitCode { get; init; } = null;
    public bool IsNullable { get; init; } = false;
    public string Name { get; init; } = name;
    public PropertyAccessor? Setter { get; init; } = setter;
    public bool? ShouldCreateBackingField { get; init; } = null;
    public string Type { get; init; } = type;
    public object? Value { get; set; } = null;

    public static bool operator !=(PropertyInfo left, PropertyInfo right)
        => !(left == right);

    public static bool operator ==(PropertyInfo left, PropertyInfo right)
        => left.Equals(right);

    public override bool Equals(object? obj)
        => obj is PropertyInfo info && this.Equals(info);

    public readonly bool Equals(PropertyInfo other)
        => this.Name == other.Name;

    public override int GetHashCode()
        => HashCode.Combine(this.Name);
}