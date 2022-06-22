using Library.Data.SqlServer.Builders;
using Library.Data.SqlServer.Builders.Bases;
using Library.Validations;

namespace Library.Data.SqlServer;

public static partial class SqlStatementBuilder
{
    public static ISelectStatement Select<TEntity>()
    {
        var table = EntityModelConverterHelper.GetTableInfo<TEntity>();
        var result = Select()
                        .Columns(table.Columns.OrderBy(c => c.Order).Select(c => c.Name))
                        .From(table.Name);
        return result;
    }

    public static ISelectStatement Select([DisallowNull] string tableName)
        => new SelectStatement { TableName = tableName.ArgumentNotNull(nameof(tableName)) };

    public static ISelectStatement Select()
        => new SelectStatement();

    public static ISelectStatement From([DisallowNull] this ISelectStatement statement, [DisallowNull] string tableName)
        => statement.ArgumentNotNull(nameof(statement)).Fluent(() => statement.TableName = tableName.ArgumentNotNull(nameof(tableName)));

    public static ISelectStatement AllColumns([DisallowNull] this ISelectStatement statement)
        => statement.Star();
    public static ISelectStatement Star([DisallowNull] this ISelectStatement statement)
        => statement.ArgumentNotNull(nameof(statement)).Fluent(statement.Columns.Clear);
    public static ISelectStatement Columns([DisallowNull] this ISelectStatement statement, params string[] columns)
        => statement.ArgumentNotNull(nameof(statement)).Fluent(() => statement.Columns.ClearAndAddRange(columns.Compact()));
    public static ISelectStatement Columns([DisallowNull] this ISelectStatement statement, IEnumerable<string> columns)
    {
        Check.IfArgumentNotNull(statement, nameof(statement));
        statement.Columns.Clear();
        return statement.AddColumns(columns);
    }
    public static ISelectStatement AddColumn([DisallowNull] this ISelectStatement statement, params string[] columns)
        => statement.ArgumentNotNull(nameof(statement)).Fluent(() => columns.Compact().ForEach(c => statement.Columns.Add(c)).Build());
    public static ISelectStatement AddColumns([DisallowNull] this ISelectStatement statement, IEnumerable<string> columns)
        => statement.AddColumn(columns.ToArray());

    public static ISelectStatement Where([DisallowNull] this ISelectStatement statement, string? whereClause)
        => statement.ArgumentNotNull(nameof(statement)).Fluent(() => statement.WhereClause = whereClause);

    public static ISelectStatement OrderBy([DisallowNull] this ISelectStatement statement, string? column)
        => statement.ArgumentNotNull(nameof(statement)).Fluent(() => statement.OrderByColumn = column);
    public static ISelectStatement Ascending([DisallowNull] this ISelectStatement statement)
        => statement.ArgumentNotNull(nameof(statement)).Fluent(() => statement.OrderByDirection = OrderByDirection.Ascending);
    public static ISelectStatement Descending([DisallowNull] this ISelectStatement statement)
        => statement.ArgumentNotNull(nameof(statement)).Fluent(() => statement.OrderByDirection = OrderByDirection.Descending);
    public static ISelectStatement ClearOrdering([DisallowNull] this ISelectStatement statement)
        => statement.ArgumentNotNull(nameof(statement)).Fluent(() =>
        {
            statement.OrderByColumn = null;
            statement.OrderByDirection = OrderByDirection.None;
        });

    public static string Build([DisallowNull] this ISelectStatement statement, string indent = "    ")
    {
        Check.IfNotNull(statement.TableName, nameof(statement.TableName));
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
        _ = AddClause($"FROM { AddBrackets(statement.TableName)}", indent, result);
        if (!statement.WhereClause.IsNullOrEmpty())
        {
            _ = AddClause($"WHERE {statement.WhereClause}", indent, result);
        }
        if (statement.OrderByDirection != OrderByDirection.None)
        {
            _ = AddClause($"ORDER BY {AddBrackets(statement.OrderByColumn.NotNull(nameof(statement.OrderByColumn)))}", indent, result);
            result = statement.OrderByDirection switch
            {
                OrderByDirection.Ascending => result.Append(" ASC"),
                OrderByDirection.Descending => result.Append(" DESC"),
                _ => throw new NotImplementedException(),
            };
        }
        return result.ToString();
    }

    private struct SelectStatement : ISelectStatement
    {
        public SelectStatement()
        {
            this.TableName = string.Empty;
            this.WhereClause = this.OrderBy = this.OrderByColumn = null;
        }
        public string TableName { get; set; }
        public string? WhereClause { get; set; }
        public string? OrderBy { get; set; }
        public List<string> Columns { get; } = new();
        public string? OrderByColumn { get; set; }
        public OrderByDirection OrderByDirection { get; set; } = OrderByDirection.None;
    }
}
