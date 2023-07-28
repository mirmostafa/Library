using Library.DesignPatterns.Markers;

namespace Library.CodeGeneration.Models;

[Immutable]
public readonly struct TypePath : IEquatable<TypePath>
{
    public TypePath(in string? name, in string? nameSpace = null)
        => (this.Name, this.NameSpace) = SplitTypePath(Validate(name), nameSpace);

    public string FullPath
    {
        get
        {
            if (this.Name.IsNullOrEmpty())
            {
                return string.Empty;
            }
            var result = this.NameSpace.IsNullOrEmpty() ? this.Name : $"{this.NameSpace}.{this.Name}";
            if (this.GenericTypes.Count != 0)
            {
                result = $"{result}<{StringHelper.Merge(this.GenericTypes.Select(x => x.FullPath), ',')}>";
            }
            return result;
        }
    }

    public IList<TypePath> GenericTypes { get; } = new List<TypePath>();

    public string? Name { get; }

    public string? NameSpace { get; }

    public static string Combine(string part1, params string[] parts)
    {
        var result = new StringBuilder(part1);
        foreach (var part in parts)
        {
            _ = result.Append(part.EndsWith(".") ? part : string.Concat(part, "."));
        }
        return result.ToString();
    }

    public static implicit operator string?(in TypePath typeInfo)
        => typeInfo.ToString();

    public static implicit operator TypePath(in string? typeInfo)
        => new(typeInfo);

    public static TypePath New(in string? name, in string? nameSpace = null)
        => new(name, nameSpace);

    public static TypePath New((string? Name, string? NameSpace) typePath)
        => new(typePath.Name, typePath.NameSpace);

    public static TypePath New(in Type? type)
        => new(type?.FullName);

    public static TypePath New<TType>()
        => new(typeof(TType).FullName);

    public static bool operator !=(TypePath left, TypePath right)
        => !(left == right);

    public static bool operator ==(TypePath left, TypePath right)
        => left.Equals(right);

    public static (string? Name, string? NameSpace) SplitTypePath(in string? typePath)
    {
        if (typePath.IsNullOrEmpty())
        {
            return default;
        }

        var dotLastIndex = typePath.LastIndexOf('.');
        return dotLastIndex == -1 ? ((string? Name, string? NameSpace))(typePath, null) : ((string? Name, string? NameSpace))(typePath[(dotLastIndex + 1)..], typePath[..dotLastIndex]);
    }

    public static (string? Name, string? NameSpace) SplitTypePath(in string? name, in string? nameSpace = null)
        => nameSpace.IsNullOrEmpty()
            ? SplitTypePath(name)
            : nameSpace!.EndsWith(".") ? SplitTypePath($"{nameSpace}{name}") : SplitTypePath($"{nameSpace}.{name}");

    public TypePath AddGenericType(TypePath typePath)
    {
        this.GenericTypes.Add(typePath);
        return this;
    }

    public void Deconstruct(out string? name, out string? nameSpace)
        => (name, nameSpace) = (this.Name, this.NameSpace);

    public bool Equals(TypePath? other)
        => (this.Name, this.NameSpace) == (other?.Name, other?.NameSpace);

    public override bool Equals(object? obj)
        => obj is TypePath path && this.Equals(path);

    public bool Equals(TypePath other)
        => (this.Name, this.NameSpace) == (other.Name, other.NameSpace);

    public override int GetHashCode()
        => this.Name?.GetHashCode() ?? 0 + this.NameSpace?.GetHashCode() ?? 0;

    public override string ToString()
        => this.FullPath;

    private static string? Validate(string? name)
        => name?.Contains('`') is null or false ? name : name[..name.IndexOf('`')];
}