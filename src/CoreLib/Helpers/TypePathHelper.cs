namespace Library.Helpers;

public readonly record struct TypeData(string Name, string? NameSpace, IEnumerable<TypeData> Generics, bool IsNullable);

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

        return result;
    }
}