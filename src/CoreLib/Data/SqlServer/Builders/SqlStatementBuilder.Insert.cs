using Library.Data.SqlServer.Builders.Bases;
using Library.Validations;

namespace Library.Data.SqlServer;

public partial class SqlStatementBuilder
{
    public static string Build([DisallowNull] this IInsertStatement statement, string indent = "    ")
    {
        Check.MutBeNotNull(statement.TableName);

        var result = new StringBuilder($"INSERT INTO {AddBrackets(statement.TableName)}");
        return AddClause($", VALUES( {statement.Values.Merge(", ", (x, i) => FormatValue(x, i, statement))} )", indent, result).ToString();
    }

    private static string FormatValue(string value, int index, IInsertStatement statement) =>
        statement.Columns[index].IsString ? $"'{value}'" : $"{value}";
}