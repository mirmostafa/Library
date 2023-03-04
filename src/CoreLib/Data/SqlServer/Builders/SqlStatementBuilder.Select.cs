using Library.Data.SqlServer;
using Library.Data.SqlServer.Builders.Bases;
using Library.DesignPatterns.Markers;
using Library.Validations;

namespace Library.Data.SqlServer;

public static partial class SqlStatementBuilder
{
    public static ISelectStatement AddColumn([DisallowNull] this ISelectStatement statement, params string[] columns)
        => statement.ArgumentNotNull().Fluent(() => columns.Compact().ForEach(c => statement.Columns.Add(c)).Build()).GetValue();

    public static ISelectStatement AddColumns([DisallowNull] this ISelectStatement statement, IEnumerable<string> columns)
        => statement.AddColumn(columns.ToArray());

    public static ISelectStatement AllColumns([DisallowNull] this ISelectStatement statement)
        => statement.Star();

    public static ISelectStatement Ascending([DisallowNull] this ISelectStatement statement)
        => statement.ArgumentNotNull().Fluent(() => statement.OrderByDirection = OrderByDirection.Ascending).GetValue();

    public static string Build([DisallowNull] this ISelectStatement statement, string indent = "    ")
    {
        Check.NotNull(statement.TableName, nameof(statement.TableName));
        var result = new StringBuilder("SELECT");
        if (statement.Columns.Any())
        {
            foreach (var column in statement.Columns)
            {
                _ = result.Append($" {AddBrackets(column)},");
            }
            _ = result.Remove(result.Length - 1, 1);
        }
        else
        {
            _ = result.Append(" *");
        }
        _ = AddClause($"FROM {AddBrackets(statement.TableName)}", indent, result);
        if (!statement.WhereClause.IsNullOrEmpty())
        {
            _ = AddClause($"WHERE {statement.WhereClause}", indent, result);
        }
        if (statement.OrderByDirection != OrderByDirection.None)
        {
            _ = AddClause($"ORDER BY {AddBrackets(statement.OrderByColumn.NotNull())}", indent, result);
            _ = result.Append(statement.OrderByDirection switch
            {
                OrderByDirection.Ascending => " ASC",
                OrderByDirection.Descending => " DESC",
                _ => throw new NotImplementedException(),
            });
        }
        return result.ToString();
    }

    public static ISelectStatement ClearOrdering([DisallowNull] this ISelectStatement statement)
        => statement.ArgumentNotNull().Fluent(() =>
        {
            statement.OrderByColumn = null;
            statement.OrderByDirection = OrderByDirection.None;
        }).GetValue();

    public static ISelectStatement Columns([DisallowNull] this ISelectStatement statement, params string[] columns)
        => statement.ArgumentNotNull().Fluent(() => statement.Columns.ClearAndAddRange(columns.Compact())).GetValue();

    public static ISelectStatement Columns([DisallowNull] this ISelectStatement statement, IEnumerable<string> columns)
    {
        Check.IfArgumentNotNull(statement, nameof(statement));
        statement.Columns.Clear();
        return statement.AddColumns(columns);
    }

    public static ISelectStatement Descending([DisallowNull] this ISelectStatement statement)
        => statement.ArgumentNotNull().Fluent(() => statement.OrderByDirection = OrderByDirection.Descending).GetValue();

    public static ISelectStatement From([DisallowNull] this ISelectStatement statement, [DisallowNull] string tableName)
        => statement.ArgumentNotNull().Fluent(() => statement.TableName = tableName.ArgumentNotNull()).GetValue();

    public static ISelectStatement OrderBy([DisallowNull] this ISelectStatement statement, string? column)
        => statement.ArgumentNotNull().Fluent(() => statement.OrderByColumn = column).GetValue();

    public static ISelectStatement OrderByDescending([DisallowNull] this ISelectStatement statement, string? column)
        => OrderBy(statement, column).Descending();

    public static ISelectStatement Select<TEntity>()
    {
        var table = SqlEntity.GetTableInfo<TEntity>();
        var result = Select()
                        .Columns(table.Columns.Select(c => c.Name))
                        .From(table.Name);
        return result;
    }

    public static ISelectStatement Select([DisallowNull] string tableName)
        => new SelectStatement { TableName = tableName.ArgumentNotNull() };

    public static ISelectStatement Select()
        => new SelectStatement();

    public static ISelectStatement Star([DisallowNull] this ISelectStatement statement)
        => statement.ArgumentNotNull().Fluent(statement.Columns.Clear).GetValue();

    public static ISelectStatement Where([DisallowNull] this ISelectStatement statement, string? whereClause)
        => statement.ArgumentNotNull().Fluent(() => statement.WhereClause = whereClause).GetValue();

    private struct SelectStatement : ISelectStatement
    {
        public SelectStatement()
        {
            this.TableName = string.Empty;
            this.WhereClause = this.OrderBy = this.OrderByColumn = null;
        }

        public List<string> Columns { get; } = new();

        public string? OrderBy { get; set; }

        public string? OrderByColumn { get; set; }

        public OrderByDirection OrderByDirection { get; set; } = OrderByDirection.None;

        public string TableName { get; set; }

        public string? WhereClause { get; set; }
    }
}