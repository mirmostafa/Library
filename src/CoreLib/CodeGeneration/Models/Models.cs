using System.CodeDom;

using Library.DesignPatterns.Markers;

namespace Library.CodeGeneration.Models;

[Immutable]
public readonly struct FieldInfo(
        in TypePath type,
        in string name,
        in string? comment = null,
        in MemberAttributes? accessModifier = null,
        in bool isReadOnly = false,
        in bool isPartial = false) : IMemberInfo
{
    public MemberAttributes? AccessModifier { get; } = accessModifier;
    public string? Comment { get; } = comment;
    public bool IsPartial { get; } = isPartial;
    public bool IsReadOnly { get; } = isReadOnly;
    public string Name { get; } = name;
    public TypePath Type { get; } = type;
}

[Immutable]
public readonly struct MethodArgument(in TypePath type, in string name)
{
    public string Name { get; } = name;
    public TypePath Type { get; } = type;

    public void Deconstruct(out TypePath type, out string name) =>
        (type, name) = (this.Type, this.Name);
}

[Immutable]
public readonly struct PropertyInfo(
    in string type,
    in string name,
    in MemberAttributes? accessModifier = null,
    in PropertyAccessor? getter = null,
    in PropertyAccessor? setter = null) : IMemberInfo
{
    public MemberAttributes? AccessModifier { get; init; } = accessModifier;
    public List<string> Attributes { get; } = [];
    public string? BackingFieldName { get; init; } = null;
    public string? Comment { get; init; } = null;
    public PropertyAccessor? Getter { get; init; } = getter;
    public bool HasBackingField { get; init; } = false;
    public string? InitCode { get; init; } = null;
    public bool IsNullable { get; init; } = false;
    public string Name { get; init; } = name;
    public PropertyAccessor? Setter { get; init; } = setter;
    public TypePath Type { get; init; } = type;
}

public interface IMemberInfo
{
    string Name { get; }
}

[Immutable]
public readonly struct PropertyAccessor(in bool has = true, in bool? isPrivate = null, in string? code = null)
{
    public string? Code { get; } = code;
    public bool Has { get; } = has;
    public bool? IsPrivate { get; } = isPrivate;

    public void Destruct(out bool has, out bool? isPrivate, out string? code) =>
        (has, isPrivate, code) = (this.Has, this.IsPrivate, this.Code);
}