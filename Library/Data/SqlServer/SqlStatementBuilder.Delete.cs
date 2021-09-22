using System.Diagnostics.CodeAnalysis;
using Library.Data.SqlServer.Builders.Bases;

namespace Library.Data.SqlServer;

public partial class SqlStatementBuilder
{
    public static IDeleteStatement Delete([DisallowNull] string tableName)
        => new DeleteStatement { TableName = tableName.ArgumentNotNull(nameof(tableName)) };
    public static IDeleteStatement Delete()
        => new DeleteStatement();
    
    public static IDeleteStatement From([DisallowNull] this IDeleteStatement statement, [DisallowNull] string tableName)
        => statement.ArgumentNotNull(nameof(statement)).Fluent(() => statement.TableName = tableName.ArgumentNotNull(nameof(tableName)));

    public static IDeleteStatement Where([DisallowNull] this IDeleteStatement statement, string? whereClause)
        => statement.ArgumentNotNull(nameof(statement)).Fluent(() => statement.WhereClause = whereClause);
    
    public static string Build([DisallowNull] this IDeleteStatement statement, string indent = "    ")
    {
        Check.IfNotNull(statement.TableName, nameof(statement.TableName));
        var result = new StringBuilder($"DELETE FROM {AddBrackets(statement.TableName)}");
        if (!statement.WhereClause.IsNullOrEmpty())
        {
            _ = AddClause($"WHERE {statement.WhereClause}", indent, result);
        }
        return result.ToString();
    }

    private struct DeleteStatement : IDeleteStatement
    {
        public string TableName { get; set; }
        public string? WhereClause { get; set; }
    }
}
