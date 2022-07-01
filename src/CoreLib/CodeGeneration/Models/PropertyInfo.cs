using System.CodeDom;

namespace Library.CodeGeneration.Models;

public struct PropertyInfo : IMemberInfo, IEquatable<PropertyInfo>
{
    public PropertyInfo(
        in string type,
        in string name,
        in MemberAttributes? accessModifier = null,
        in PropertyAccessor? getter = null,
        in PropertyAccessor? setter = null)
    {
        this.Name = name;
        this.Type = type;
        this.AccessModifier = accessModifier;
        this.Getter = getter;
        this.Setter = setter;
        this.HasBackingField = false;
        this.Value = null;
        this.Comment = null;
        this.InitCode = null;
        this.IsNullable = false;
        this.BackingFieldName = null;
        this.ShouldCreateBackingField = null;
    }

    public MemberAttributes? AccessModifier { get; init; }
    public string Type { get; init; }
    public bool IsNullable { get; init; }
    public string Name { get; init; }
    public object? Value { get; set; }

    public List<string> Attributes { get; } = new List<string>();
    public PropertyAccessor? Setter { get; init; }
    public PropertyAccessor? Getter { get; init; }
    public string? InitCode { get; init; }

    public bool HasBackingField { get; init; }
    public string? BackingFieldName { get; init; }
    public bool? ShouldCreateBackingField { get; init; }

    public string? Comment { get; init; }

    public override bool Equals(object? obj) => obj is PropertyInfo info && this.Equals(info);
    public bool Equals(PropertyInfo other) => this.Name == other.Name;
    public override int GetHashCode() => HashCode.Combine(this.Name);

    public static bool operator ==(PropertyInfo left, PropertyInfo right) => left.Equals(right);
    public static bool operator !=(PropertyInfo left, PropertyInfo right) => !(left == right);
}
