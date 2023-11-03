﻿using System.CodeDom;
using System.Collections.ObjectModel;

using Library.DesignPatterns.Markers;

namespace Library.CodeGeneration.Models;

public interface IMemberInfo
{
    string Name { get; }
}

[Immutable]
public sealed class PropertyInfo(
    in string type,
    in string name,
    in MemberAttributes? accessModifier = null,
    in PropertyAccessor? getter = null,
    in PropertyAccessor? setter = null) : IMemberInfo
{
    public PropertyInfo()
        : this(string.Empty, string.Empty)
    {
    }

    public MemberAttributes? AccessModifier { get; init; } = accessModifier;
    public Collection<string> Attributes { get; } = [];
    public string? BackingFieldName { get; init; }
    public string? Comment { get; init; }
    public PropertyAccessor? Getter { get; init; } = getter;
    public bool HasBackingField { get; init; }
    public string? InitCode { get; init; }
    public bool IsNullable { get; init; }
    public string Name { get; init; } = name;
    public PropertyAccessor? Setter { get; init; } = setter;
    public TypePath Type { get; init; } = type;
}

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

    public static bool operator !=(FieldInfo left, FieldInfo right)
    {
        return !(left == right);
    }

    public static bool operator ==(FieldInfo left, FieldInfo right)
    {
        return left.Equals(right);
    }

    public override bool Equals(object obj)
    {
        throw new NotImplementedException();
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}

[Immutable]
public readonly struct MethodArgument(in TypePath type, in string name)
{
    public string Name { get; } = name;
    public TypePath Type { get; } = type;

    public static bool operator !=(MethodArgument left, MethodArgument right)
    {
        return !(left == right);
    }

    public static bool operator ==(MethodArgument left, MethodArgument right)
    {
        return left.Equals(right);
    }

    public void Deconstruct(out TypePath type, out string name) =>
                (type, name) = (this.Type, this.Name);

    public override bool Equals(object obj)
    {
        throw new NotImplementedException();
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}

[Immutable]
public readonly struct PropertyAccessor(in bool has = true, in bool? isPrivate = null, in string? code = null)
{
    public string? Code { get; } = code;
    public bool Has { get; } = has;
    public bool? IsPrivate { get; } = isPrivate;

    public static bool operator !=(PropertyAccessor left, PropertyAccessor right)
    {
        return !(left == right);
    }

    public static bool operator ==(PropertyAccessor left, PropertyAccessor right)
    {
        return left.Equals(right);
    }

    public void Destruct(out bool has, out bool? isPrivate, out string? code) =>
                (has, isPrivate, code) = (this.Has, this.IsPrivate, this.Code);

    public override bool Equals(object obj)
    {
        throw new NotImplementedException();
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}