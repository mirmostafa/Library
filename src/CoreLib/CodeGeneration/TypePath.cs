using System.Globalization;

using Library.CodeGeneration.Models;
using Library.DesignPatterns.Markers;
using Library.Validations;

using TypeData = (string Name, string NameSpace, System.Collections.Generic.IEnumerable<Library.CodeGeneration.TypePath> Generics, bool IsNullable);

namespace Library.CodeGeneration;

[Immutable]
public sealed class TypePath([DisallowNull] in string fullPath, in IEnumerable<string>? generics = null, bool? isNullable = null) : IEquatable<TypePath>
{
    private readonly TypeData _data = Parse(fullPath, generics, isNullable);
    private string? _fullName;
    private string? _fullPath;

    [NotNull]
    public string FullName => this._fullName ??= this.GetFullName();

    [NotNull]
    public string FullPath => this._fullPath ??= this.GetFullPath();

    [NotNull]
    public IEnumerable<TypePath> Generics => this._data.Generics;

    public bool IsGeneric => this._data.Generics.Any();

    public bool IsNullable => this._data.IsNullable;

    [NotNull]
    public string Name => this._data.Name;

    public string NameSpace => this._data.NameSpace;

    public static string Combine(in string? part1, params string?[] parts) =>
        StringHelper.Merge(EnumerableHelper.Iterate(part1).AddRangeImmuted(parts).Compact().Select(x => x.Trim('.')), '.');

    [return: NotNullIfNotNull(nameof(typePath))]
    public static string? GetName(in string? typePath) =>
        typePath == null ? null : Parse(typePath).Name;

    [return: NotNullIfNotNull(nameof(typePath))]
    public static string? GetNameSpace(in string? typePath) =>
        typePath == null ? null : Parse(typePath).NameSpace;

    [return: NotNull]
    public static IEnumerable<string>? GetNameSpaces(in string? typePath) =>
        typePath == null ? Enumerable.Empty<string>() : New(typePath).GetNameSpaces();

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
    public static TypePath New([DisallowNull] in string fullPath, in IEnumerable<string>? generics = null, bool? isNullable = null) =>
        new(fullPath, generics, isNullable);

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

    public static (string Name, string NameSpace) ToKeyword(string name, string nameSpace) =>
        nameSpace == "System"
            ? (name switch
            {
                nameof(String) => ("string", ""),
                nameof(Byte) => ("byte", ""),
                nameof(SByte) => ("sbyte", ""),
                nameof(Char) => ("char", ""),
                nameof(Boolean) => ("bool", ""),
                nameof(UInt32) => ("uint", ""),
                nameof(IntPtr) => ("nint", ""),
                nameof(UIntPtr) => ("nuint", ""),
                nameof(Int16) => ("short", ""),
                nameof(UInt16) => ("ushort", ""),
                nameof(Int32) => ("int", ""),
                nameof(Int64) => ("long", ""),
                nameof(Single) => ("float", ""),
                nameof(Decimal) => ("decimal", ""),
                nameof(Double) => ("double", ""),
                _ => (name, nameSpace),
            })
            : (name, nameSpace);

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

    public (string Name, string NameSpace) ToKeyword() =>
        ToKeyword(this.Name, this.NameSpace);

    [return: NotNull]
    public override string ToString() =>
        this.GetFullPath();

    [return: NotNull]
    public TypePath ToTypePath() =>
        new(this.FullName);

    private static TypeData Parse(in string typePath, in IEnumerable<string>? generics = null, bool? isNullable = null)
    {
        // Validation checks
        Check.MustBeArgumentNotNull(typePath);
        Check.MustBe(generics?.All(x => !x.IsNullOrEmpty()) ?? true, () => "Generic types cannot be null or empty.");

        // Initializations
        var typePathBuffer = typePath;
        var gens = new List<string>();

        // Take care of nullability.
        if (isNullable is { } nullable)
        {
            typePathBuffer = typePathBuffer.TrimEnd('?');
            if (nullable)
            {
                typePathBuffer = typePathBuffer.AddEnd('?');
            }
        }

        // Nullability checks
        var nullability = typePathBuffer.EndsWith('?');

        // Find Generics
        if (typePathBuffer.Contains('<', StringComparison.Ordinal))
        {
            var indexOfGenSymbol = typePathBuffer.IndexOf('<', StringComparison.Ordinal);
            var gen = typePathBuffer[indexOfGenSymbol..].Trim('<').Trim('>');
            typePathBuffer = typePathBuffer[..indexOfGenSymbol];
            gens.AddRange(gen.Split(',').Select(x => x.Trim()));
        }
        if (typePathBuffer.Contains('`', StringComparison.Ordinal))
        {
            typePathBuffer = typePathBuffer.Remove(typePathBuffer.IndexOf('`', StringComparison.Ordinal), 2);
        }
        if (generics?.Any() ?? false)
        {
            gens.AddRange(generics);
        }

        // Retrieve name and namespace
        var lastIndexOfDot = typePathBuffer.LastIndexOf('.');
        (var name, var nameSpace) = lastIndexOfDot > 0
            ? (typePathBuffer[(lastIndexOfDot + 1)..], typePathBuffer[..lastIndexOfDot])
            : (typePathBuffer, string.Empty);

        var genTypes = gens.Select(x => new TypePath(x));

        // To be more friendly, let's be kind and use C# keywords.
        //! CodeCOM makes a mistake.
        //x (name, nameSpace) = ToKeyword(name, nameSpace);
        return (name, nameSpace, genTypes, nullability);
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