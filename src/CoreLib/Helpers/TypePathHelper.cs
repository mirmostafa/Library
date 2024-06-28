namespace Library.Helpers;

public readonly record struct TypeData(string Name, string? NameSpace, IEnumerable<TypeData> Generics, bool IsNullable) : IEquatable<TypeData>
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
}

public static class TypePathHelper
{
    internal static TypeData ParseFullPath([DisallowNull] in string fullPath)
    {
        TypeData result = default;
        if (!fullPath.Contains('<'))
        {
            if (fullPath.StartsWith("System.Nullable`1[["))
            {
                var buffer = string.Concat(fullPath["System.Nullable`1[[".Length..fullPath.IndexOf(',')], "?");
                result = ParseFullPath(buffer);
            }
            else if (fullPath.Contains("`1[["))
            {
                var indexOfGeneric = fullPath.IndexOf("`1[[");

                var mainPart = fullPath[..indexOfGeneric];
                var mainPartParseResult = ParseFullPath(mainPart);

                var generic = fullPath[(indexOfGeneric + "`1[[".Length)..fullPath.IndexOf(',')];
                var genericParseResult = ParseFullPath(generic);

                var isNullable = fullPath.EndsWith('?');

                result = mainPartParseResult with { Generics = [genericParseResult], IsNullable = isNullable };
            }
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
                var mainTypeParseResult = ParseFullPath(mainType);
                var genericTypeParseResult = ParseFullPath(genericType);
                result = mainTypeParseResult with { Generics = [genericTypeParseResult], IsNullable = isNullable };
            }
        }

        return result;
    }
}