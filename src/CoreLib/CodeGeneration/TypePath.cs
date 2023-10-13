using Library.CodeGeneration.Models;
using Library.DesignPatterns.Markers;
using Library.Validations;

using TypeData = (string Name, string? NameSpace, System.Collections.Generic.IEnumerable<Library.CodeGeneration.TypePath> Generics);

namespace Library.CodeGeneration;

[Immutable]
public sealed class TypePath([DisallowNull] in string fullPath, IEnumerable<string>? generics = null) : IEquatable<TypePath>
{
    private readonly TypeData _data = Parse(fullPath.ArgumentNotNull(), generics);
    private string? _fullPath;
    private string? _fullName;

    [NotNull]
    public IEnumerable<TypePath> Generics => this._data.Generics;
    public bool IsGeneric => this._data.Generics.Any();
    [NotNull]
    public string Name => this._data.Name;
    public string? NameSpace => this._data.NameSpace;
    [NotNull]
    public string FullPath => this._fullPath ??= this.GetFullPath();
    [NotNull]
    public string FullName => this._fullName ??= this.GetFullName();

    public static string Combine(string? part1, params string?[] parts) =>
        StringHelper.Merge(parts.AddImmuted(part1).Compact(), '.');

    [return: NotNullIfNotNull(nameof(typePath))]
    public static string? GetName(string? typePath) =>
        typePath == null ? null : Parse(typePath).Name;

    [return: NotNullIfNotNull(nameof(typePath))]
    public static string? GetNameSpace(string? typePath) =>
        typePath == null ? null : Parse(typePath).NameSpace;

    [return: NotNullIfNotNull(nameof(typeInfo))]
    public static implicit operator string?(in TypePath? typeInfo) =>
        typeInfo?.FullPath;

    [return: NotNullIfNotNull(nameof(typeInfo))]
    public static implicit operator TypePath?(in string? typeInfo) =>
        typeInfo.IsNullOrEmpty() ? null : new(typeInfo);

    public static bool operator !=(TypePath left, TypePath right) =>
        !(left == right);

    public static bool operator ==(TypePath left, TypePath right) =>
        left?.Equals(right) ?? (right is null);

    public void Deconstruct(out string? name, out string? nameSpace) =>
        (name, nameSpace) = (this.Name, this.NameSpace);

    public void Deconstruct(out string? name, out string? nameSpace, out IEnumerable<TypePath> generics) =>
        (name, nameSpace, generics) = (this.Name, this.NameSpace, this.Generics);

    public bool Equals(TypePath? other) =>
        (this.Name, this.NameSpace) == (other?.Name, other?.NameSpace);

    public override bool Equals(object? obj) =>
        obj is TypePath path && this.Equals(path);

    public override int GetHashCode() =>
        HashCode.Combine(this.Name?.GetHashCode() ?? 0, this.NameSpace?.GetHashCode() ?? 0);

    public IEnumerable<string> GetNameSpaces()
    {
        if (!this.NameSpace.IsNullOrEmpty())
        {
            yield return this.NameSpace;
        }

        foreach (var generic in this.Generics)
        {
            foreach (var genericNamespace in generic.GetNameSpaces())
            {
                yield return genericNamespace;
            }
        }
    }

    public override string ToString() =>
        this.GetFullPath();

    private string GetFullPath()
    {
        var buffer = new StringBuilder();
        if (!this.NameSpace.IsNullOrEmpty())
        {
            _ = buffer.Append($"{this.NameSpace}.");
        }
        _ = buffer.Append(this.Name);
        if (this.Generics.Any())
        {
            _ = buffer.Append('<')
                .Append(this.Generics.Select(x => x.GetFullPath())!.Merge(", "))
                .Append('>');
        }
        return buffer.ToString();
    }

    private string GetFullName()
    {
        var buffer = new StringBuilder();
        _ = buffer.Append(this.Name);
        if (this.Generics.Any())
        {
            _ = buffer.Append('<')
                .Append(this.Generics.Select(x => x.GetFullName())!.Merge(", "))
                .Append('>');
        }
        return buffer.ToString();
    }

    private static TypeData Parse(string typePath, IEnumerable<string>? generics = null)
    {
        var temp = typePath;
        var gens = new List<string>();
        // Find Generics
        if (temp.Contains('<'))
        {
            var indexOfGenSymbol = temp.IndexOf('<');
            var gen = temp[indexOfGenSymbol..].Trim('<').Trim('>');
            gens.AddRange(gen.Split(',').Select(x => x.Trim()));
            if (generics?.Any() ?? false)
            {
                gens.AddRange(generics);
            }
            temp = temp[..indexOfGenSymbol];
        }

        // Retrieve name and namespace
        var lastIndexOfDot = temp.LastIndexOf('.');
        (var name, var nameSpace) = lastIndexOfDot > 0
            ? (temp[(lastIndexOfDot + 1)..], temp[..lastIndexOfDot])
            : (temp, string.Empty);
        var genTypes = gens.Select(x => new TypePath(x));

        return (name, nameSpace, genTypes);
    }

    public static TypePath? ToTypePath() => throw new NotImplementedException();

    public static TypePath New<T>(IEnumerable<string>? generics = null) =>
        new(typeof(T).FullName, generics);
    public static TypePath New(Type type, IEnumerable<string>? generics = null) =>
        new(type?.FullName, generics);
    public static TypePath New(TypePath typePath) =>
        new(typePath?.FullPath);
    public static TypePath New(in string? name, in string? nameSpace, params string[] generics) =>
        new(Combine(name, nameSpace), generics);
}