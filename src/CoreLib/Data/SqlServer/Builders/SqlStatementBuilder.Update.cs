using Library.Data.SqlServer.Builders.Bases;
using Library.Validations;

namespace Library.Data.SqlServer;

public static partial class SqlStatementBuilder
{
    public static IUpdateStatement Update() => new UpdateStatement();
    public static IUpdateStatement Update([DisallowNull] string tableName)
        => new UpdateStatement { TableName = tableName.ArgumentNotNull(nameof(tableName)) };
    public static IUpdateStatement Table([DisallowNull] this IUpdateStatement statement, [DisallowNull] string tableName)
        => statement.Fluent(statement.ArgumentNotNull().TableName = tableName.ArgumentNotNull()).GetValue();
    public static IUpdateStatement Set([DisallowNull] this IUpdateStatement statement, string column, object value)
    {
        statement.ArgumentNotNull(nameof(statement)).ColumnsValue.Add(column.ArgumentNotNull(nameof(column)), value);
        return statement;
    }
    public static IUpdateStatement Set([DisallowNull] this IUpdateStatement statement, params (string Column, object Value)[] columnsValue)
    {
        _ = nameof(statement).ArgumentNotNull(nameof(statement));
        _ = columnsValue.ForEachItem(cv => statement.Set(cv.Column, cv.Value)).Build();
        return statement;
    }
    public static IUpdateStatement Set([DisallowNull] this IUpdateStatement statement, IEnumerable<(string Column, object Value)> columnsValue)
    {
        _ = nameof(statement).ArgumentNotNull(nameof(statement));
        _ = columnsValue.ForEachItem(cv => statement.Set(cv.Column, cv.Value));
        return statement;
    }

    public static IUpdateStatement Where([DisallowNull] this IUpdateStatement statement, string? whereClause)
        => statement.Fluent(statement.ArgumentNotNull().WhereClause = whereClause).GetValue();

    public static string Build([DisallowNull] this IUpdateStatement statement, string indent = "    ")
    {
        _ = statement.TableName.NotNull(nameof(statement.TableName));
        Check.IfHasAny(statement.ColumnsValue, nameof(statement.ColumnsValue));
        var result = new StringBuilder($"Update {AddBrackets(statement.TableName)}");
        _ = AddClause($"SET ({statement.ColumnsValue.Select(cv => AddBrackets(cv.Key)).Merge(", ")})", indent, result);
        _ = AddClause($"VALUES ({statement.ColumnsValue.Select(cv => FormatValue(cv.Value)).Merge(", ")})", indent, result);
        if (!statement.WhereClause.IsNullOrEmpty())
        {
            _ = AddClause($"WHERE {statement.WhereClause}", indent, result);
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
