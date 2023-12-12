﻿using Library.Data.SqlServer.Builders.Bases;
using Library.Validations;

namespace Library.Data.SqlServer;

public static partial class SqlStatementBuilder
{
    public static string Build([DisallowNull] this IUpdateStatement statement, string indent = "    ")
    {
        Check.MutBeNotNull(statement?.TableName);
        Check.MustBe(statement.Values?.Count > 0);

        var result = new StringBuilder($"UPDATE {AddBrackets(statement.TableName)} ");

        Func<object, string> format = statement.ForceFormatValues ? FormatValue : cv => cv?.ToString() ?? DBNull.Value.ToString();
        var keyValues = statement.Values.Select(kv => $"{AddBrackets(kv.Key)} = {format(kv.Value)}").Merge(", ");
        _ = AddClause($"SET {keyValues}", indent, result);

        if (!statement.WhereClause.IsNullOrEmpty())
        {
            _ = AddClause($"WHERE {statement.WhereClause}", indent, result);
        }

        if (statement.ReturnId)
        {
            _ = result.AppendLine(";").Append("SELECT SCOPE_IDENTITY();");
        }
        return result.ToString();
    }

    public static IUpdateStatement Set([DisallowNull] this IUpdateStatement statement, string column, object value)
    {
        statement.ArgumentNotNull().Values.Add(column.ArgumentNotNull(), value);
        return statement;
    }

    public static IUpdateStatement Set([DisallowNull] this IUpdateStatement statement, params (string Column, object Value)[] columnsValue)
    {
        _ = statement.ArgumentNotNull();
        _ = columnsValue.Iterate(cv => statement.Set(cv.Column, cv.Value)).Build();
        return statement;
    }

    public static IUpdateStatement Set([DisallowNull] this IUpdateStatement statement, IEnumerable<(string Column, object Value)> columnsValue)
    {
        _ = statement.ArgumentNotNull();
        _ = columnsValue.Iterate(cv => statement.Set(cv.Column, cv.Value)).Build();
        return statement;
    }

    public static IUpdateStatement Table([DisallowNull] this IUpdateStatement statement, [DisallowNull] string tableName)
        => statement.Fluent(statement.ArgumentNotNull().TableName = tableName.ArgumentNotNull()).GetValue();

    public static IUpdateStatement Update() =>
        new UpdateStatement();

    public static IUpdateStatement Update([DisallowNull] string tableName)
        => new UpdateStatement { TableName = tableName.ArgumentNotNull() };

    public static IUpdateStatement Where([DisallowNull] this IUpdateStatement statement, string? whereClause)
        => statement.Fluent(statement.ArgumentNotNull().WhereClause = whereClause).GetValue();

    private class UpdateStatement : IUpdateStatement
    {
        public IDictionary<string, object> Values { get; } = new Dictionary<string, object>();
        public bool ForceFormatValues { get; set; } = true;
        public bool ReturnId { get; set; }
        public string? Schema { get; set; }
        public string TableName { get; set; } = null!;
        public string? WhereClause { get; set; }
    }
}