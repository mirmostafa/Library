using System.Data.SqlClient;

namespace Library.Data.SqlServer;

//[Obsolete($"Use {typeof(Builders.SqlStatementBuilder)}, instead.")]
[Obsolete]
public static class SqlStatementBuilder
{
    public static string BracketClose = "]";
    public static string BracketOpen = "[";

    public static string CreateDelete(string tableName, bool useLike = false, IEnumerable<(string Value1, object Value2)> whereColumns = null) =>
        $"DELETE FROM {tableName}{CreateWhereClause(whereColumns, useLike)}";

    public static string CreateDelete<TEntity>(TEntity entity) => CreateDelete(typeof(TEntity).Name, entity);

    /// <summary>
    /// Creates the delete.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="tableName">Name of the table.</param>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    public static string CreateDelete<TEntity>(string tableName, TEntity entity)
        => CreateDelete(tableName, false, typeof(TEntity).GetProperties().Select(p => (p.Name, p.GetValue(entity, null))));

    public static string CreateExecuteStoredProcedure(string spName, Action<List<SqlParameter>> fillParams = null)
    {
        var cmdText = new StringBuilder($"Exec [{spName}]");
        if (fillParams == null)
        {
            return cmdText.ToString();
        }

        var parameters = new List<SqlParameter>();
        fillParams(parameters);
        for (var index = 0; index < parameters.Count; index++)
        {
            var parameter = parameters[index];
            _ = cmdText.Append($"\t{Environment.NewLine}{parameter.ParameterName} = '{parameter.Value}'");
            if (index != parameters.Count - 1)
            {
                cmdText.Append(", ");
            }
        }

        return cmdText.ToString();
    }

    public static string CreateInsertValue(string tableName, params (string Value1, object Value2)[] keyValues) =>
        CreateInsertValueCore(tableName, keyValues.ToList());

    public static string CreateInsertValue(string tableName, IEnumerable<(string Value1, object Value2)> keyValues) =>
        CreateInsertValueCore(tableName, keyValues);

    public static string CreateInsertValue<TTableEntity>(string tableName, TTableEntity entity) =>
        CreateInsertValueCore(tableName, InternalServices.Refactor(entity, false).Columns.ToArray());

    public static string CreateInsertValue<TTableEntity>(TTableEntity entity, params string[] columnNames)
    {
        var schema = InternalServices.Refactor(entity);
        return CreateInsertValueCore(schema.TableName,
            columnNames.Length != 0
                ? schema.Columns.Where(col => columnNames.Contains(col.Value1.Remove("[").Remove("]")))
                : schema.Columns.ToArray());
    }

    public static string CreateSelect(string tablename, params string[] columns)
    {
        if (tablename == null)
        {
            throw new ArgumentNullException(nameof(tablename));
        }

        var formattedTableName = tablename.SplitMerge('.', BracketOpen, $"{BracketClose}.").RemoveFromEnd(1);
        return columns.Length == 0 || columns[0] == "*"
            ? $"SELECT * FROM {formattedTableName}"
            : $"SELECT {columns.Select(c => $"{BracketOpen}{c}{BracketClose}").Merge(", ")} FROM {formattedTableName}";
    }

    public static string CreateSelect(Type tableEntity) => CreateSelect(InternalServices.Refactor(tableEntity, false).TableName,
        InternalServices.Refactor(tableEntity, false).ColumnNames.ToArray());

    public static string CreateSelect<TTableEntity>() => CreateSelect(typeof(TTableEntity));

    public static string CreateUpdate<TTableEntity>(TTableEntity entity,
        bool useLike = false,
        IEnumerable<string> updateColumns = null,
        IEnumerable<string> whereColumns = null)
    {
        var schema = InternalServices.Refactor(entity);
        var result = new StringBuilder($"UPDATE {schema.TableName}");
        var whereCols = schema.Columns.Where(c => whereColumns?.Contains(c.Value1.Remove("[").Remove("]")) == true).ToList();

        if (updateColumns == null)
        {
            updateColumns = entity.GetType().GetProperties().Select(p => p.Name);
        }

        if (whereColumns != null)
        {
            updateColumns = updateColumns.Except(whereColumns);
        }

        var updateCols = schema.Columns.Where(c => updateColumns.Contains(c.Value1.Remove("[").Remove("]"))).ToList();

        var firstColumn = updateCols.First();
        result.AppendFormat("{2}   SET {0} = {1}", firstColumn.Value1, firstColumn.Value2, Environment.NewLine);

        for (var i = 1; i < updateCols.Count; i++)
        {
            var column = updateCols[i];
            result.AppendFormat(" ,{0} = {1}", column.Value1, column.Value2);
        }

        result.Append(CreateWhereClause(whereCols, useLike));
        return result.ToString();
    }

    public static string CreateUpdate(string tableName, string column, string value, string whereClause)
    {
        var result = $"UPDATE {tableName} SET [{column}] = N'{value}'";
        if (!whereClause.IsNullOrEmpty())
        {
            result = string.Concat(result, " WHERE ", whereClause);
        }

        return result;
    }

    private static string CreateColName(object value) => value != null ? $"[{value.ToString().Remove("[").Remove("]")}]" : null;

    private static string CreateInsertValueCore(string tableName, IEnumerable<(string Value1, object Value2)> keyValues)
    {
        var pairs = keyValues as IList<(string Value1, object Value2)> ?? keyValues.ToList();
        return $@"INSERT INTO {tableName}{Environment.NewLine} ({pairs.Select(kv => kv.Value1).Merge(", ")}){Environment.NewLine}
                        VALUES   ({pairs.Select(kv => kv.Value2?.ToString() ?? "NULL").Merge(", ")})";
    }

    private static string CreateValue(object value) => value != null ? $"\'{value.ToString().Remove("\'").Remove("\'")}\'" : null;

    private static string CreateWhereClause(IEnumerable<(string Value1, object Value2)> whereCols, bool useLike)
    {
        whereCols = whereCols.Where(col => col.Value1?.ToString().IsNullOrEmpty() == false);
        var columns = whereCols as (string Value1, object Value2)[] ?? whereCols.ToArray();
        if (!columns.Any())
        {
            return string.Empty;
        }

        var result = new StringBuilder();
        var firstColumn = columns.First();
        result.AppendLine();
        result.AppendFormat(" WHERE {0} {2} {1}", CreateColName(firstColumn.Value1), CreateValue(firstColumn.Value2), useLike ? "LIKE" : "=");

        foreach (var column in columns.Skip(1))
        {
            result.AppendFormat(" AND {0} {2} {1}", CreateColName(column.Value1), CreateValue(column.Value2), useLike ? "LIKE" : "=");
        }

        return result.ToString();
    }
}
