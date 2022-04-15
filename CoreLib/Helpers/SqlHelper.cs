namespace Library.Helpers;

public static class SqlHelper
{
    internal static Type SqlTypeToNetType(string typeName) =>
        typeName?.ToLower() switch
        {
            "int" => typeof(int),
            "bitint" => typeof(long),
            "datetime" or "datetime2" => typeof(DateTime),
            "varchar" or "nvarchar" => typeof(string),
            "bit" => typeof(bool),
            null => throw new NotImplementedException(),
            _ => throw new NotImplementedException(),
        };
}