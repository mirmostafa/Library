using System.Collections;
using System.Globalization;

using Library.CodeGeneration.Models;
using Library.DesignPatterns.Markers;
using Library.Validations;

namespace Library.CodeGeneration;

//[DebuggerStepThrough, StackTraceHidden]
[Immutable]
public sealed class TypePath : IEquatable<TypePath>
{
    private static readonly Dictionary<Type, string> _primitiveTypes = new()
    {
        { typeof(int), "int" },
        { typeof(long), "long" },
        { typeof(byte), "byte" },
        { typeof(char), "char" },
        { typeof(bool), "bool" },
        { typeof(uint), "uint" },
        { typeof(nint), "nint" },
        { typeof(float), "float" },
        { typeof(nuint), "nuint" },
        { typeof(short), "short" },
        { typeof(sbyte), "sbyte" },
        { typeof(double), "double" },
        { typeof(string), "string" },
        { typeof(ushort), "ushort" },
        { typeof(decimal), "decimal" },
    };

    private readonly TypeData _data;
    private string? _fullName;
    private string? _fullPath;

    public TypePath([DisallowNull] in string fullPath, in IEnumerable<string>? generics = null, bool isNullable = false)
        : this(Parse(fullPath, generics, isNullable))
    {
    }

    private TypePath(TypeData typeData)
        => this._data = typeData;

    [NotNull]
    public string FullName => this._fullName ??= this.GetFullName();

    [NotNull]
    public string FullPath => this._fullPath ??= this.GetFullPath();

    [NotNull]
    public IEnumerable<TypePath> Generics => this._data.Generics.Select(x => new TypePath(x));

    public bool IsGeneric => this._data.Generics.Any();

    public bool IsNullable => this._data.IsNullable;

    [NotNull]
    public string Name => this._data.Name;

    public string NameSpace => this._data.NameSpace ?? string.Empty;

    public static string AsKeyword(in string nameOrFullName)
    {
        var buffer = nameOrFullName;
        _primitiveTypes.ForEach(x => buffer = buffer.Replace($"System.{x.Key.Name}", x.Value).Replace($"{x.Key.Name}", x.Value));
        return buffer;
    }

    public static string Combine(in string? part1, params string?[] parts)
            => StringHelper.Merge(EnumerableHelper.AsEnumerable(part1).AddRangeImmuted(parts).Compact().Select(x => x.Trim('.')), '.');

    [return: NotNullIfNotNull(nameof(typePath))]
    public static string? GetName(in string? typePath)
        => typePath == null ? null : Parse(typePath).Name;

    [return: NotNullIfNotNull(nameof(typePath))]
    public static string? GetNameSpace(in string? typePath)
        => typePath == null ? null : Parse(typePath).NameSpace ?? string.Empty;

    [return: NotNull]
    public static string GetNameSpace<T>()
    {
        var path = typeof(T).FullName;
        return path.IsNullOrEmpty() ? string.Empty : Parse(path).NameSpace ?? string.Empty;
    }

    [return: NotNull]
    public static IEnumerable<string>? GetNameSpaces(in string? typePath)
        => typePath == null ? [] : New(typePath).GetNameSpaces();

    [return: NotNullIfNotNull(nameof(typeInfo))]
    public static implicit operator string?(in TypePath? typeInfo)
        => typeInfo?.FullPath;

    [return: NotNullIfNotNull(nameof(typeInfo))]
    public static implicit operator TypePath?(in string? typeInfo)
        => typeInfo.IsNullOrEmpty() ? null : new(typeInfo);

    [return: NotNullIfNotNull(nameof(typeInfo))]
    public static implicit operator TypePath?(in Type? typeInfo)
        => typeInfo == null ? null : New(typeInfo);

    [return: NotNull]
    public static TypePath New([DisallowNull] in string name, in string? nameSpace)
        => new(Combine(nameSpace, name));

    [return: NotNull]
    public static TypePath New([DisallowNull] in string name, in string? nameSpace, in IEnumerable<string>? generics)
        => new(Combine(nameSpace, name), generics);

    [return: NotNull]
    public static TypePath New([DisallowNull] in string name, in string? nameSpace, in IEnumerable<string>? generics, bool isNullable)
        => new(Combine(nameSpace, name), generics, isNullable);

    [return: NotNull]
    public static TypePath New([DisallowNull] in string fullPath)
        => new(fullPath);

    [return: NotNull]
    public static TypePath New([DisallowNull] in string fullPath, in IEnumerable<string>? generics)
        => new(fullPath, generics);

    [return: NotNull]
    public static TypePath New([DisallowNull] in string fullPath, bool isNullable)
        => new(fullPath, null, isNullable);

    [return: NotNull]
    public static TypePath New([DisallowNull] in string fullPath, in IEnumerable<string>? generics, bool isNullable)
        => new(fullPath, generics, isNullable);

    [return: NotNull]
    public static TypePath New([DisallowNull] in Type type)
        => new(type?.FullName!);

    [return: NotNull]
    public static TypePath New([DisallowNull] in Type type, in IEnumerable<Type>? generics)
        => new(type?.FullName!, generics?.Select(x => x.Name == "Nullable`1" ? $"{x.GenericTypeArguments[0].FullName}?" : x.FullName!));

    [return: NotNull]
    public static TypePath New([DisallowNull] in Type type, in IEnumerable<Type>? generics, bool isNullable)
        => new(type?.FullName!, generics?.Select(x => x.FullName!), isNullable);

    [return: NotNull]
    public static TypePath New<T>()
        => new(typeof(T).FullName!);

    [return: NotNull]
    public static TypePath New<T>(in IEnumerable<string>? generics)
        => new(typeof(T).FullName!, generics);

    [return: NotNull]
    public static TypePath New<T>(in IEnumerable<string>? generics, bool isNullable)
        => new(typeof(T).FullName!, generics, isNullable);

    [return: NotNull]
    public static TypePath NewEnumerable(in TypePath generic)
        => New(typeof(IEnumerable<>).FullName!, [generic]);

    [return: NotNull]
    public static TypePath NewEnumerable()
        => New<IEnumerable>();

    [return: NotNull]
    public static TypePath NewTask()
        => New<Task>();

    [return: NotNull]
    public static TypePath NewTask(in TypePath generic)
        => New(typeof(Task<>).FullName!, [generic.FullName]);

    [return: NotNull]
    public static TypePath NewTask(in TypePath generic, bool isNullable)
        => New(typeof(Task<>).FullName!, [generic.FullName], isNullable);

    public static bool operator !=(in TypePath? left, in TypePath? right)
        => !(left == right);

    public static bool operator ==(in TypePath? left, in TypePath? right)
        => left?.Equals(right) ?? (right is null);

    public string AsKeyword()
        => AsKeyword(this.FullName);

    public void Deconstruct(out string? name, out string? nameSpace)
        => (name, nameSpace) = (this.Name, this.NameSpace);

    public void Deconstruct(out string? name, out string? nameSpace, out IEnumerable<TypePath> generics)
        => (name, nameSpace, generics) = (this.Name, this.NameSpace, this.Generics);

    public bool Equals(TypePath? other)
        => (this.Name, this.NameSpace) == (other?.Name, other?.NameSpace);

    public override bool Equals(object? obj)
        => obj is TypePath path && this.Equals(path);

    public override int GetHashCode()
        => this.FullName.GetHashCode(StringComparison.Ordinal);

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
    public override string ToString()
        => this.FullName;

    public TypePath WithNullable(bool isNullable)
    {
        if (this.IsNullable == isNullable)
        {
            return this;
        }

        var fullPath = this.GetFullPath().Trim('?');
        if (isNullable)
        {
            fullPath = fullPath.AddEnd('?');
        }
        return new(fullPath);
    }

    private static TypeData Parse(in string typePath, in IEnumerable<string>? generics = null, bool isNullable = false)
    {
        // Validation checks
        Check.MustBeArgumentNotNull(typePath);
        Check.MustBe(generics?.All(x => !x.IsNullOrEmpty()) ?? true, () => "Generic types cannot be null or empty.");
        Check.MustBe(generics?.Any() != true || (!typePath.ContainsAny(["<", ">"]) && !typePath.Contains(TypeData.CLR_GENERIC_SYMBOL))
            , () => $"{nameof(generics)} cannot have items while {nameof(typePath)} is nullable.");

        var result = TypeData.Parse(typePath, generics, isNullable);

        return result;
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
        if (this.IsNullable)
        {
            _ = buffer.Append('?');
        }

        var result = buffer.ToString();
        return result;
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
        if (this.IsNullable)
        {
            _ = buffer.Append('?');
        }

        var result = buffer.ToString();
        return result;
    }

    internal readonly record struct TypeData(string Name, string? NameSpace, IEnumerable<TypeData> Generics, bool IsNullable) : IEquatable<TypeData>
    {
        public bool Equals(TypeData other)
            => this.GetHashCode() == other.GetHashCode();

        public override int GetHashCode()
        {
            var result = HashCode.Combine(this.Name, this.NameSpace, this.IsNullable);
            foreach (var generic in this.Generics)
            {
                result = HashCode.Combine(result, generic.GetHashCode());
            }
            return result;
        }

        internal const string CLR_GENERIC_SYMBOL = "`1[[";
        internal static TypeData Parse([DisallowNull] in string fullPath)
        {
            TypeData result = default;

            // Not Generic
            if (!fullPath.Contains('<'))
            {
                // Is CLR Nullable?
                if (isClrNullable(fullPath))
                {
                    // Retrieve the generic parameter
                    var buffer = string.Concat(fullPath["System.Nullable`1[[".Length..fullPath.IndexOf(',')], "?");
                    result = Parse(buffer);
                }
                // Not nullable, but CLR generic
                else if (isClrGeneric(fullPath))
                {
                    // So complicated type
                    var parts = fullPath.Split(CLR_GENERIC_SYMBOL);
                    parts[^1] = parts[^1].With(x => x[..x.IndexOf(',')]);

                    var buffer = Parse(parts[^1]);

                    parts[..^1].ForEachReverse(item =>
                    {
                        if (!item.StartsWith("System.Nullable"))
                        {
                            buffer = Parse(item) with { Generics = [buffer], IsNullable = false };
                        }
                        else
                        {
                            buffer = buffer with { IsNullable = true };
                        }
                    });
                    result = buffer;
                }
                // Not CLR nullable, not CLR generic
                else
                {
                    var isNullable = fullPath.EndsWith('?');
                    var buffer = fullPath.TrimEnd('?');
                    var parts = buffer.Split('.');
                    var name = parts.Last();
                    result = parts.Length == 1
                        ? new TypeData(name, null, [], isNullable)
                        : new TypeData(name, parts.Take(parts.Length - 1).Merge('.'), [], isNullable);
                }
            }
            else
            {
                if (!fullPath.Contains(','))
                {
                    var isNullable = fullPath.EndsWith('?');
                    var beginningOfGeneric = fullPath.IndexOf('<');
                    var mainType = fullPath[..beginningOfGeneric];
                    var genericType = fullPath[(beginningOfGeneric + 1)..fullPath.LastIndexOf('>')];
                    var mainTypeParseResult = Parse(mainType);
                    var genericTypeParseResult = Parse(genericType);
                    result = mainTypeParseResult with { Generics = [genericTypeParseResult], IsNullable = isNullable };
                }
            }

            return result;

            static bool isClrNullable(string s)
                => s?.StartsWith($"System.Nullable{CLR_GENERIC_SYMBOL}") is true;
            static bool isClrGeneric(string s)
                => s?.Contains(CLR_GENERIC_SYMBOL) is true;
        }

        internal static TypeData Parse([DisallowNull] in string fullType, IEnumerable<string>? generics, bool isNullable)
        {
            Checker.MustBeArgumentNotNull(fullType);
            Checker.MustBe(generics?.All(x => !x.IsNullOrEmpty()) ?? true, () => "Generic types cannot be null or empty.");

            var fullPath = generics?.Any() ?? false
                ? $"{fullType}<{string.Join(',', generics)}>{(isNullable ? "?" : "")}"
                : $"{fullType}{(isNullable ? "?" : "")}";

            var result = Parse(fullPath);
            return result;
        }
    }
}