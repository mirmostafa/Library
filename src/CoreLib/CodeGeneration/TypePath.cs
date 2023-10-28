using System.Globalization;

using Library.CodeGeneration.Models;
using Library.DesignPatterns.Markers;
using Library.Validations;

using TypeData = (string Name, string? NameSpace, System.Collections.Generic.IEnumerable<Library.CodeGeneration.TypePath> Generics);

namespace Library.CodeGeneration;

[Immutable]
public sealed class TypePath([DisallowNull] in string fullPath, in IEnumerable<string>? generics = null) : IEquatable<TypePath>
{
    private readonly TypeData _data = Parse(fullPath, generics);
    private string? _fullName;
    private string? _fullPath;

    [NotNull]
    public string FullName => this._fullName ??= this.GetFullName();

    [NotNull]
    public string FullPath => this._fullPath ??= this.GetFullPath();

    [NotNull]
    public IEnumerable<TypePath> Generics => this._data.Generics;

    public bool IsGeneric => this._data.Generics.Any();

    [NotNull]
    public string Name => this._data.Name;

    public string? NameSpace => this._data.NameSpace;

    public static string Combine(in string? part1, params string?[] parts) =>
        StringHelper.Merge(EnumerableHelper.Iterate(part1).AddRangeImmuted(parts).Compact().Select(x => x.Trim('.')), '.');

    [return: NotNullIfNotNull(nameof(typePath))]
    public static string? GetName(in string? typePath) =>
        typePath == null ? null : Parse(typePath).Name;

    [return: NotNullIfNotNull(nameof(typePath))]
    public static string? GetNameSpace(in string? typePath) =>
        typePath == null ? null : Parse(typePath).NameSpace;

    [return: NotNullIfNotNull(nameof(typeInfo))]
    public static implicit operator string?(in TypePath? typeInfo) =>
        typeInfo?.FullPath;

    [return: NotNullIfNotNull(nameof(typeInfo))]
    public static implicit operator TypePath?(in string? typeInfo) =>
        typeInfo.IsNullOrEmpty() ? null : new(typeInfo);

    [return: NotNullIfNotNull(nameof(typeInfo))]
    public static implicit operator TypePath?(in Type? typeInfo) =>
        typeInfo == null ? null : New(typeInfo);

    [return: NotNull]
    public static TypePath New([DisallowNull] in string fullPath, in IEnumerable<string>? generics = null) =>
        new(fullPath, generics);

    [return: NotNull]
    public static TypePath New([DisallowNull] in TypePath typePath) =>
        new(typePath.ArgumentNotNull().FullPath);

    [return: NotNull]
    public static TypePath New<T>(in IEnumerable<string>? generics = null) =>
        new(typeof(T).FullName!, generics);

    [return: NotNull]
    public static TypePath New([DisallowNull] in Type type, in IEnumerable<string>? generics = null) =>
        new(type.ArgumentNotNull().FullName!, generics);

    [return: NotNull]
    public static TypePath New(in string? name, in string? nameSpace, params string[] generics) =>
        new(Combine(nameSpace, name), generics);

    public static bool operator !=(in TypePath? left, in TypePath? right) =>
        !(left == right);

    public static bool operator ==(in TypePath? left, in TypePath? right) =>
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
        this.FullName.GetHashCode(StringComparison.Ordinal);

    [return: NotNull]
    public IEnumerable<string> GetNameSpaces()
    {
        return iterate().Compact().Distinct();

        IEnumerable<string> iterate()
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
    }

    [return: NotNull]
    public override string ToString() =>
        this.GetFullPath();

    [return: NotNull]
    public TypePath ToTypePath() =>
        new(this.FullName);

    private static TypeData Parse(in string typePath, in IEnumerable<string>? generics = null)
    {
        Check.MustBeArgumentNotNull(typePath);
        Check.MustBe(generics?.All(x => !x.IsNullOrEmpty()) ?? true, () => "Generic types cannot be null or empty.");

        var temp = typePath;
        var gens = new List<string>();
        // Find Generics
        if (temp.Contains('<', StringComparison.Ordinal))
        {
            var indexOfGenSymbol = temp.IndexOf('<', StringComparison.Ordinal);
            var gen = temp[indexOfGenSymbol..].Trim('<').Trim('>');
            temp = temp[..indexOfGenSymbol];
            gens.AddRange(gen.Split(',').Select(x => x.Trim()));
        }
        if (temp.Contains('`', StringComparison.Ordinal))
        {
            temp = temp.Remove(temp.IndexOf('`', StringComparison.Ordinal), 2);
        }

        if (generics?.Any() ?? false)
        {
            gens.AddRange(generics);
        }

        // Retrieve name and namespace
        var lastIndexOfDot = temp.LastIndexOf('.');
        (var name, var nameSpace) = lastIndexOfDot > 0
            ? (temp[(lastIndexOfDot + 1)..], temp[..lastIndexOfDot])
            : (temp, string.Empty);

        var genTypes = gens.Select(x => new TypePath(x));
        if (nameSpace == "System")
        {
            (name, nameSpace) = name switch
            {
                "String" => ("string", ""),
                "Boolean" => ("bool", ""),
                "Int32" => ("int", ""),
                "Int64" => ("long", ""),
                "Single" => ("float", ""),
                _ => (name, nameSpace),
            };
        }

        return (name, nameSpace, genTypes);
    }

    [return: NotNull]
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

    [return: NotNull]
    private string GetFullPath()
    {
        var buffer = new StringBuilder();
        if (!this.NameSpace.IsNullOrEmpty())
        {
            _ = buffer.Append(CultureInfo.CurrentCulture, $"{this.NameSpace}.");
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
}