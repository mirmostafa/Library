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
        const string CLR_GENERIC_SYMBOL = "`1[[";

        TypeData result = default;
        // Not Generic
        if (!fullPath.Contains('<'))
        {
            // Is CLR Nullable?
            if (isClrNullable(fullPath))
            {
                // Retrieve the generic parameter
                var buffer = string.Concat(fullPath["System.Nullable`1[[".Length..fullPath.IndexOf(',')], "?");
                result = ParseFullPath(buffer);
            }
            // Not nullable, but CLR generic
            else if (isClrGeneric(fullPath))
            {
                // So complicated type
                var parts = fullPath.Split(CLR_GENERIC_SYMBOL);
                parts[^1] = parts[^1].With(x => x[..x.IndexOf(',')]);

                var buffer = ParseFullPath(parts[^1]);
                var isNullable = false;

                parts[..^1].ForEachReverse(item =>
                {
                    buffer = ParseFullPath(item) with { Generics = [buffer], IsNullable = isNullable };
                    isNullable = item.StartsWith("System.Nullable");
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
                var mainTypeParseResult = ParseFullPath(mainType);
                var genericTypeParseResult = ParseFullPath(genericType);
                result = mainTypeParseResult with { Generics = [genericTypeParseResult], IsNullable = isNullable };
            }
        }

        return result;

        static bool isClrNullable(string s)
            => s?.StartsWith($"System.Nullable{CLR_GENERIC_SYMBOL}") is true;
        static bool isClrGeneric(string s)
            => s?.Contains(CLR_GENERIC_SYMBOL) is true;
    }
}