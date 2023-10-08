using Library.CodeGeneration.Models;
using Library.DesignPatterns.Markers;

using GenericTypeInfo = (Library.CodeGeneration.TypePath Type, string? Constraints);

namespace Library.CodeGeneration;

[Immutable]
public sealed class TypePath : IEquatable<TypePath>
{
    public TypePath(in string? name, in string? nameSpace = null) =>
        (this.Name, this.NameSpace) = SplitTypePath(Validate(name), nameSpace);

    public string FullPath
    {
        get
        {
            if (this.Name.IsNullOrEmpty())
            {
                return string.Empty;
            }
            var result = this.NameSpace.IsNullOrEmpty() ? this.Name : $"{this.NameSpace}.{this.Name}";
            if (this.GenericTypes.Any())
            {
                //UNDONE: Must be cheecked.
                result = $"{result}<{StringHelper.Merge(this.GenericTypes.Select(x => x.Type.FullPath), ',')}>";
            }
            return result;
        }
    }

    public ISet<GenericTypeInfo> GenericTypes { get; } = new HashSet<GenericTypeInfo>();
    public string? Name { get; }
    public string? NameSpace { get; }

    public static string Combine(string part1, params string[] parts) =>
        new StringBuilder(part1).AppendAll(parts, part => $".{part.Trim('.')}").ToString();

    [return: NotNullIfNotNull(nameof(typeInfo))]
    public static implicit operator string?(in TypePath? typeInfo) =>
        typeInfo?.ToString();
    [return: NotNullIfNotNull(nameof(typeInfo))]
    public static implicit operator TypePath?(in string? typeInfo) =>
        typeInfo.IsNullOrEmpty() ? null : new(typeInfo);

    public static TypePath New(in string? name, in string? nameSpace = null) =>
        new(name, nameSpace);

    public static TypePath New(in Type? type) =>
        new(type?.FullName);
    public static bool operator !=(TypePath left, TypePath right) =>
        !(left == right);

    public static bool operator ==(TypePath left, TypePath right) =>
        left.Equals(right);

    public static (string? Name, string? NameSpace) SplitTypePath(in string? typePath)
    {
        if (typePath.IsNullOrEmpty())
        {
            return default;
        }
        var buffer = typePath.Contains('<') ? typePath[..typePath.IndexOf('<')] : typePath;
        var lastIndexOfDot = buffer.LastIndexOf('.');
        var result = lastIndexOfDot == -1
            ? (buffer, null)
            : (buffer[(lastIndexOfDot + 1)..], buffer[..lastIndexOfDot]);
        return result;
    }
    //========
    //        var dotLastIndex = typePath.LastIndexOf('.');
    //        return dotLastIndex == -1 ? ((string? Name, string? NameSpace))(typePath, null) : ((string? Name, string? NameSpace))(typePath[(dotLastIndex + 1)..], typePath[..dotLastIndex]);
    //    }

    public static (string? Name, string? NameSpace) SplitTypePath(in string? name, in string? nameSpace) =>
        string.IsNullOrEmpty(nameSpace)
            ? SplitTypePath(name)
            : nameSpace.EndsWith('.') ? SplitTypePath($"{nameSpace}{name}") : SplitTypePath($"{nameSpace}.{name}");

    public TypePath AddGenericType(in GenericTypeInfo genericType)
    {
        _ = this.GenericTypes.Add(genericType);
        return this;
    }
    public TypePath AddGenericType(string genericType)
    {
        _ = this.GenericTypes.Add((genericType, null));
        return this;
    }

    public static TypePath New<TType>()
        => new(typeof(Type).Name, typeof(Type).Namespace);

    public void Deconstruct(out string? name, out string? nameSpace) =>
        (name, nameSpace) = (this.Name, this.NameSpace);

    public bool Equals(TypePath? other) =>
        (this.Name, this.NameSpace) == (other?.Name, other?.NameSpace);

    public override bool Equals(object? obj) =>
        obj is TypePath path && this.Equals(path);

    public override int GetHashCode() =>
        HashCode.Combine(this.Name?.GetHashCode() ?? 0, this.NameSpace?.GetHashCode() ?? 0);
    public override string ToString() =>
        this.FullPath;

    private static string? Validate(string? name) =>
        name?.Contains('`') is null or false ? name : name[..name.IndexOf('`')];

    [return: NotNullIfNotNull(nameof(typePath))]
    public static string? GetName(string? typePath) =>
        typePath == null ? null : SplitTypePath(typePath).Name;

    [return: NotNullIfNotNull(nameof(typePath))]
    public static string? GetNameSpace(string? typePath) =>
        typePath == null ? null : SplitTypePath(typePath).NameSpace;
}