﻿using Library.Data.SqlServer.Builders.Bases;
using Library.Validations;

namespace Library.Data.SqlServer;

public partial class SqlStatementBuilder
{
    public static IDeleteStatement Delete([DisallowNull] string tableName)
        => new DeleteStatement { TableName = tableName.ArgumentNotNull() };
    public static IDeleteStatement Delete()
        => new DeleteStatement();

    public static IDeleteStatement From([DisallowNull] this IDeleteStatement statement, [DisallowNull] string tableName)
        => statement.ArgumentNotNull().Fluent(() => statement.TableName = tableName.ArgumentNotNull()).Build();

    public static IDeleteStatement Where([DisallowNull] this IDeleteStatement statement, string? whereClause)
        => statement.ArgumentNotNull().Fluent(() => statement.WhereClause = whereClause).Build();

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
