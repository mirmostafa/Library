namespace Library.Helpers;

public static class SqlHelper
{
    /// <summary>
    /// Converts a SQL type name to its corresponding .NET type.
    /// </summary>
    /// <param name="typeName">The SQL type name to be converted.</param>
    /// <returns>The corresponding .NET Type for the given SQL type name.</returns>
    public static Type SqlTypeToNetType(string typeName) =>
        typeName?.ToLower() switch
        {
            "smallint" => typeof(short), // Map "smallint" to short.
            "int" => typeof(int), // Map "int" to int.
            "bigint" => typeof(long), // Map "bigint" to long.
            "datetime" or "datetime2" => typeof(DateTime), // Map "datetime" or "datetime2" to DateTime.
            "varchar" or "nvarchar" => typeof(string), // Map "varchar" or "nvarchar" to string.
            "bit" => typeof(bool), // Map "bit" to bool.
            null => throw new NotSupportedException(), // Throw an exception if typeName is null.
            _ => throw new NotImplementedException(), // Throw an exception for any other typeName.
        };
}