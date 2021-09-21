namespace Library.Coding.Models;

public sealed class TypePath : IEquatable<TypePath>
{
    public TypePath(in string? name, in string? nameSpace = null) => (this.Name, this.NameSpace) = SplitTypePath(name, nameSpace);
    public TypePath((string? name, string? nameSpace) typePath) : this(typePath.name, typePath.nameSpace) { }
    public void Deconstruct(out string? name, out string? nameSpace) => (name, nameSpace) = (this.Name, this.NameSpace);

    public static implicit operator string?(in TypePath typeInfo) => typeInfo.ToString();
    public static implicit operator TypePath(in string? typeInfo) => new(typeInfo);

    public string? Name { get; }
    public string? NameSpace { get; }
    public string FullPath
        => (string.IsNullOrEmpty(this.Name)
                ? string.Empty
                : string.IsNullOrEmpty(this.NameSpace)
                    ? this.Name
                    : $"{this.NameSpace}.{this.Name}")!;

    public static (string? Name, string? NameSpace) SplitTypePath(in string? typePath)
    {
        if (string.IsNullOrEmpty(typePath))
        {
            return default;
        }

        var dotLastIndex = typePath!.LastIndexOf('.');
        return dotLastIndex == -1 ? (typePath, null) : (typePath.Substring(dotLastIndex), typePath.Substring(dotLastIndex - 1));
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

    public bool Equals(TypePath? other) => (this.Name, this.NameSpace) == (other?.Name, other?.NameSpace);
    public override string ToString() => this.FullPath;

    public override bool Equals(object? obj) => this.Equals(obj as TypePath);

    public override int GetHashCode() => this.Name?.GetHashCode() ?? 0 + this.NameSpace?.GetHashCode() ?? 0;
}
