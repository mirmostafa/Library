using Library.Data.SqlServer.Builders.Bases;
using Library.Validations;

namespace Library.Data.SqlServer;

public partial class SqlStatementBuilder
{
    public static string Build([DisallowNull] this IInsertStatement statement, string indent = "    ")
    {
        Check.MutBeNotNull(statement?.TableName);
        Check.MustHaveAny(statement.Values);

        var result = new StringBuilder($"INSERT INTO {AddBrackets(statement.TableName)}");
        _ = AddClause($"({statement.Values.Select(x => AddBrackets(x.Key)).Merge(", ")})", indent, result);
        if (!statement.ForceFormatValues)
        {
            _ = AddClause($"VALUES ({statement.Values.Select(x => x.Value?.ToString() ?? DBNull.Value.ToString()).Merge(", ")})", indent, result);
        }
        else
        {
            _ = AddClause($"VALUES ({statement.Values.Select(x => FormatValue(x.Value)).Merge(", ")})", indent, result);
        }

        if (statement.ReturnId)
        {
            _ = result.AppendLine(";").Append("SELECT SCOPE_IDENTITY();");
        }

        return result.Build();
    }
    public static IInsertStatement Insert() =>
        new InsertStatement();

    public static IInsertStatement Into([DisallowNull] this IInsertStatement statement, string tableName) =>
        statement.With(x => x.TableName = tableName);

    public static IInsertStatement ReturnId([DisallowNull] this IInsertStatement statement, bool returnId = true) =>
        statement.With(x => x.ReturnId = returnId);

    public static IInsertStatement Values([DisallowNull] this IInsertStatement statement, params (string ColumnName, object? Value)[] values) =>
        statement.With(x => x.Values(values.Select(x => new KeyValuePair<string, object?>(x.ColumnName, x.Value))));

    public static IInsertStatement Values([DisallowNull] this IInsertStatement statement, IEnumerable<(string ColumnName, object? Value)> values) =>
        statement.Values(values.ToArray());

    public static IInsertStatement Values([DisallowNull] this IInsertStatement statement, IEnumerable<KeyValuePair<string, object?>> values) =>
        statement.Values(values.ToArray());

    public static IInsertStatement Values([DisallowNull] this IInsertStatement statement, params KeyValuePair<string, object?>[] values) =>
        statement.With(x => x.Values.AddRange(values));

    private struct InsertStatement : IInsertStatement
    {
        public InsertStatement()
        {
        }

        public bool ForceFormatValues { get; set; } = true;
        public bool ReturnId { get; set; }
        public string? Schema { get; set; }
        public string TableName { get; set; } = null!;
        public IDictionary<string, object?> Values { get; } = new Dictionary<string, object?>();
    }
}