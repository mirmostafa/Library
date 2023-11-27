using Library.Data.SqlServer.Builders.Bases;
using Library.Validations;

namespace Library.Data.SqlServer;

public partial class SqlStatementBuilder
{
    public static string Build([DisallowNull] this IInsertStatement statement, string indent = "    ")
    {
        Check.MutBeNotNull(statement?.TableName);
        Check.MustHaveAny(statement.Columns);

        var result = new StringBuilder($"INSERT INTO {AddBrackets(statement.TableName)}");
        _ = AddClause($"({statement.Columns.Select(x => AddBrackets(x.Key)).Merge(", ")})", indent, result);
        _ = AddClause($"VALUES ({statement.Columns.Select(x => FormatValue(x.Value)).Merge(", ")})", indent, result);
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

    public static IInsertStatement ReturnId([DisallowNull] this IInsertStatement statement, bool returnId = true)
        => statement.With(x => x.ReturnId = returnId);

    public static IInsertStatement Values([DisallowNull] this IInsertStatement statement, params (string ColumnName, object Value)[] values) =>
            statement.With(x => x.Columns.AddRange(values.Select(x => new KeyValuePair<string, object>(x.ColumnName, x.Value))));

    public static IInsertStatement Values([DisallowNull] this IInsertStatement statement, IEnumerable<(string ColumnName, string Value)> value) =>
        statement.Values(value.ToArray());

    private struct InsertStatement : IInsertStatement
    {
        public InsertStatement()
        {
        }

        public IDictionary<string, object> Columns { get; } = new Dictionary<string, object>();
        public bool ReturnId { get; set; }
        public string? Schema { get; set; }
        public string TableName { get; set; } = null!;
    }
}