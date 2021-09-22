using System.Diagnostics.CodeAnalysis;
using Library.Coding;
using Library.Data.SqlServer.Builders.Bases;

namespace Library.Data.SqlServer;

public static partial class SqlStatementBuilder
{
    public static IUpdateStatement Update() => new UpdateStatement();
    public static IUpdateStatement Update([DisallowNull] string tableName)
        => new UpdateStatement { TableName = tableName.ArgumentNotNull(nameof(tableName)) };
    public static IUpdateStatement Table([DisallowNull] this IUpdateStatement statement, [DisallowNull] string tableName)
        => statement.Fluent(statement.ArgumentNotNull(nameof(statement)).TableName = tableName.ArgumentNotNull(nameof(tableName)));
    public static IUpdateStatement Set([DisallowNull] this IUpdateStatement statement, string column, object value)
    {
        statement.ArgumentNotNull(nameof(statement)).ColumnsValue.Add(column.ArgumentNotNull(nameof(column)), value);
        return statement;
    }
    public static IUpdateStatement Set([DisallowNull] this IUpdateStatement statement, params (string Column, object Value)[] columnsValue)
    {
        nameof(statement).IfArgumentNotNull(nameof(statement));
        columnsValue.ForEach(cv => statement.Set(cv.Column, cv.Value)).Apply();
        return statement;
    }
    public static IUpdateStatement Set([DisallowNull] this IUpdateStatement statement, IEnumerable<(string Column, object Value)> columnsValue)
    {
        nameof(statement).IfArgumentNotNull(nameof(statement));
        columnsValue.ForEach(cv => statement.Set(cv.Column, cv.Value));
        return statement;
    }

    public static IUpdateStatement Where([DisallowNull] this IUpdateStatement statement, string? whereClause)
        => statement.Fluent(statement.ArgumentNotNull(nameof(statement)).WhereClause = whereClause);

    public static string Build([DisallowNull] this IUpdateStatement statement, string indent = "    ")
    {
        statement.TableName.IfNotNull(nameof(statement.TableName));
        Check.IfAny(statement.ColumnsValue, nameof(statement.ColumnsValue));
        var result = new StringBuilder($"Update {AddBrackets(statement.TableName)}");
        AddClause($"SET ({statement.ColumnsValue.Select(cv => AddBrackets(cv.Key)).Merge(", ")})", indent, result);
        AddClause($"VALUES ({statement.ColumnsValue.Select(cv => FormatValue(cv.Value)).Merge(", ")})", indent, result);
        if (!statement.WhereClause.IsNullOrEmpty())
        {
            AddClause($"WHERE {statement.WhereClause}", indent, result);
        }

        return result.ToString();
    }

    private class UpdateStatement : IUpdateStatement
    {
        public string TableName { get; set; }
        public Dictionary<string, object> ColumnsValue { get; } = new();
        public string? WhereClause { get; set; }
    }
}
