using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Mohammad.Collections.Generic;
using Mohammad.Helpers;

namespace Mohammad.Data.SqlServer
{
    public static class SqlStatementBuilder
    {
        public static string CreateSelect(string tablename, params string[] columns)
        {
            if (tablename == null)
                throw new ArgumentNullException(nameof(tablename));
            return columns.Length == 0 || columns[0] == "*" ? $"SELECT * FROM {tablename}" : $"SELECT {columns.Merge(", ")} FROM {tablename}";
        }

        public static string CreateSelect(Type tableEntity)
            => CreateSelect(InternalServices.Refactor(tableEntity, false).TableName, InternalServices.Refactor(tableEntity, false).ColumnNames.ToArray());

        public static string CreateSelect<TTableEntity>() => CreateSelect(typeof(TTableEntity));

        private static string CreateInsertValueCore(string tableName, IEnumerable<PairValue<string, object>> keyValues)
        {
            var pairs = keyValues as IList<PairValue<string, object>> ?? keyValues.ToList();
            return string.Format("INSERT INTO {1}{0}            ({2}){0}   VALUES   ({3})",
                Environment.NewLine,
                tableName,
                pairs.Select(kv => kv.Value1).Merge(", "),
                pairs.Select(kv => kv.Value2?.ToString() ?? "NULL").Merge(", "));
        }

        public static string CreateInsertValue(string tableName, params PairValue<string, object>[] keyValues)
            => CreateInsertValueCore(tableName, keyValues.ToList());

        public static string CreateInsertValue(string tableName, IEnumerable<PairValue<string, object>> keyValues) => CreateInsertValueCore(tableName, keyValues);

        public static string CreateInsertValue<TTableEntity>(string tableName, TTableEntity entity)
            => CreateInsertValueCore(tableName, InternalServices.Refactor(entity, false).Columns.ToArray());

        public static string CreateInsertValue<TTableEntity>(TTableEntity entity, params string[] columnNames)
        {
            var schema = InternalServices.Refactor(entity);
            return CreateInsertValueCore(schema.TableName,
                columnNames.Length != 0 ? schema.Columns.Where(col => columnNames.Contains(col.Value1.Remove("[").Remove("]"))) : schema.Columns.ToArray());
        }

        public static string CreateUpdate<TTableEntity>(TTableEntity entity, bool useLike = false, IEnumerable<string> updateColumns = null,
            IEnumerable<string> whereColumns = null)
        {
            var schema = InternalServices.Refactor(entity);
            var result = new StringBuilder($"UPDATE {schema.TableName}");
            var whereCols = schema.Columns.Where(c => whereColumns != null && whereColumns.Contains(c.Value1.Remove("[").Remove("]"))).ToList();

            if (updateColumns == null)
                updateColumns = entity.GetType().GetProperties().Select(p => p.Name);
            if (whereColumns != null)
                updateColumns = updateColumns.Except(whereColumns);
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

        private static string CreateWhereClause(IEnumerable<PairValue<string, object>> whereCols, bool useLike)
        {
            whereCols = whereCols.Where(col => col != null && (col.Value1 != null || !col.Value1.ToString().IsNullOrEmpty()));
            var columns = whereCols as PairValue<string, object>[] ?? whereCols.ToArray();
            if (!columns.Any())
                return string.Empty;
            var result = new StringBuilder();
            var firstColumn = columns.First();
            result.AppendLine();
            result.AppendFormat(" WHERE {0} {2} {1}", CreateColName(firstColumn.Value1), CreateValue(firstColumn.Value2), useLike ? "LIKE" : "=");

            foreach (var column in columns.Skip(1))
                result.AppendFormat(" AND {0} {2} {1}", CreateColName(column.Value1), CreateValue(column.Value2), useLike ? "LIKE" : "=");
            return result.ToString();
        }

        private static string CreateValue(object value) => value != null ? $"\'{value.ToString().Remove("\'").Remove("\'")}\'" : null;
        private static string CreateColName(object value) => value != null ? $"[{value.ToString().Remove("[").Remove("]")}]" : null;

        public static string CreateUpdate(string tableName, string column, string value, string whereClause)
        {
            var result = $"UPDATE {tableName} SET [{column}] = N'{value}'";
            if (!whereClause.IsNullOrEmpty())
                result = string.Concat(result, " WHERE ", whereClause);
            return result;
        }

        public static string CreateExecuteStoredProcedure(string spName, Action<List<SqlParameter>> fillParams = null)
        {
            var cmdText = new StringBuilder($"Exec [{spName}]");
            if (fillParams == null)
                return cmdText.ToString();
            var parameters = new List<SqlParameter>();
            fillParams(parameters);
            for (var index = 0; index < parameters.Count; index++)
            {
                var parameter = parameters[index];
                cmdText.Append(string.Format("\t{2}{0} = '{1}'", parameter.ParameterName, parameter.Value, Environment.NewLine));
                if (index != parameters.Count - 1)
                    cmdText.Append(", ");
            }
            return cmdText.ToString();
        }

        public static string CreateDelete(string tableName, bool useLike = false, IEnumerable<PairValue<string, object>> whereColumns = null)
            => $"DELECT FROM {tableName}{CreateWhereClause(whereColumns, useLike)}";

        public static string CreateDelete<TEntity>(TEntity entity) => CreateDelete(typeof(TEntity).Name, entity);

        public static string CreateDelete<TEntity>(string tableName, TEntity entity)
            => CreateDelete(tableName, false, typeof(TEntity).GetProperties().Select(p => new PairValue<string, object>(p.Name, p.GetValue(entity, null))));
    }
}