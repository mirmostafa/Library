namespace Library.Helpers;

public static class SqlHelper
{
    public static Type SqlTypeToNetType(string typeName) =>
        typeName?.ToLower() switch
        {
            "smallint" => typeof(short),
            "int" => typeof(int),
            "bigint" => typeof(long),
            "datetime" or "datetime2" => typeof(DateTime),
            "varchar" or "nvarchar" => typeof(string),
            "bit" => typeof(bool),
            null => throw new NotSupportedException(),
            _ => throw new NotImplementedException(),
        };
}