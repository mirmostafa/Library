using Library.DesignPatterns.Markers;

namespace Library.CodeGeneration.Models;

[Immutable]
public readonly struct TypePath : IEquatable<TypePath>
{
    public TypePath(in string? name, in string? nameSpace = null)
        => (this.Name, this.NameSpace) = SplitTypePath(Validate(name), nameSpace);

    private static string? Validate(string? name)
        => name?.Contains('`') is null or false ? name : name[..name.IndexOf('`')];

    public void Deconstruct(out string? name, out string? nameSpace)
        => (name, nameSpace) = (this.Name, this.NameSpace);

    public static implicit operator string?(in TypePath typeInfo)
        => typeInfo.ToString();
    public static implicit operator TypePath(in string? typeInfo)
        => new(typeInfo);
    public IList<TypePath> GenericTypes { get; } = new List<TypePath>();

    public string? Name { get; }
    public string? NameSpace { get; }
    public string FullPath
    {
        get
        {
            if (this.Name.IsNullOrEmpty())
            {
                return string.Empty;
            }
            string result;
            if (this.NameSpace.IsNullOrEmpty())
            {
                result = this.Name;
            }
            else
            {
                result = $"{this.NameSpace}.{this.Name}";
            }
            if (this.GenericTypes.Any())
            {
                result = $"{result}<{StringHelper.Merge(this.GenericTypes.Select(x => x.FullPath), ',')}>";
            }
            return result;
        }
    }

    public static (string? Name, string? NameSpace) SplitTypePath(in string? typePath)
    {
        if (typePath.IsNullOrEmpty())
        {
            return default;
        }

        var dotLastIndex = typePath.LastIndexOf('.');
        if (dotLastIndex == -1)
        {
            return (typePath, null);
        }
        else
        {
            return (typePath[(dotLastIndex + 1)..], typePath[..dotLastIndex]);
        }
    }

    public static (string? Name, string? NameSpace) SplitTypePath(in string? name, in string? nameSpace = null)
    {
        if (string.IsNullOrEmpty(nameSpace))
        {
            return SplitTypePath(name);
        }

        if (nameSpace!.EndsWith("."))
        {
            return SplitTypePath($"{nameSpace}{name}");
        }

        return SplitTypePath($"{nameSpace}.{name}");
    }

    public TypePath AddGenericType(TypePath typePath)
    {
        this.GenericTypes.Add(typePath);
        return this;
    }

    public override string ToString()
        => this.FullPath;

    public override int GetHashCode()
        => this.Name?.GetHashCode() ?? 0 + this.NameSpace?.GetHashCode() ?? 0;
    public bool Equals(TypePath? other)
        => (this.Name, this.NameSpace) == (other?.Name, other?.NameSpace);
    public override bool Equals(object? obj)
        => obj is TypePath path && this.Equals(path);
    public bool Equals(TypePath other)
        => (this.Name, this.NameSpace) == (other.Name, other.NameSpace);

    public static bool operator ==(TypePath left, TypePath right)
        => left.Equals(right);
    public static bool operator !=(TypePath left, TypePath right)
        => !(left == right);

    public static TypePath New(in string? name, in string? nameSpace = null)
        => new(name, nameSpace);
    public static TypePath New((string? Name, string? NameSpace) typePath)
        => new(typePath.Name, typePath.NameSpace);
    public static TypePath New(in Type? type)
        => new(type?.FullName);

    public static TypePath New<TType>()
        => new(typeof(TType).FullName);

}
