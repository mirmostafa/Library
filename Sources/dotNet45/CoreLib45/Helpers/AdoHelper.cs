#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mohammad.Helpers.CodeHelper;

#endregion

namespace Mohammad.Helpers
{
    /// <summary>
    ///     A utility to do some common tasks about ADO
    ///     arguments
    /// </summary>
    public static partial class AdoHelper
    {
        /// <summary>
        ///     Selects the specified table.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="table"> The table. </param>
        /// <param name="columnTitle"> The column title. </param>
        /// <param name="convertor"> The converter. </param>
        /// <returns> </returns>
        [Obsolete("Please use Sql, instead.", true)]
        public static IEnumerable<T> Select<T>(this DataTable table, string columnTitle, Func<object, T> convertor) =>
            table.Select().Select(row => convertor(row[columnTitle]));

        /// <summary>
        ///     Selects the specified table.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="table"> The table. </param>
        /// <param name="columnTitle"> The column title. </param>
        /// <param name="predicate"> The predicate. </param>
        /// <returns> </returns>
        [Obsolete("Please use Sql, instead.", true)]
        public static IEnumerable<T> Select<T>(this DataTable table, string columnTitle, Predicate<object> predicate)
        {
            return table.Select().Where(row => predicate?.Invoke(row) != false).Select(row => row[columnTitle]).Cast<T>();
        }

        /// <summary>
        ///     Selects the specified table.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="table"> The table. </param>
        /// <param name="columnTitle"> The column title. </param>
        /// <param name="convertor"> The converter. </param>
        /// <param name="predicate"> The predicate. </param>
        /// <returns> </returns>
        [Obsolete("Please use Sql, instead.", true)]
        public static IEnumerable<T> Select<T>(this DataTable table, string columnTitle, Func<object, T> convertor, Predicate<object> predicate)
        {
            return table.Select().Where(row => predicate?.Invoke(row) != false).Select(row => convertor(row[columnTitle]));
        }

        [Obsolete("Please use Sql, instead.", true)]
        public static IEnumerable<T> Select<T>(this DataTable table)
            where T : new()
        {
            var type        = typeof(T);
            var properties  = type.GetProperties();
            var columnNames = table.Columns.Cast<DataColumn>().Select(col => col.ColumnName.ToLowerInvariant());
            foreach (var row in table.Select())
            {
                var t    = new T();
                var row1 = row;
                foreach (var property in properties.Where(property => columnNames.Contains(property.Name.ToLowerInvariant()))
                                                   .Where(property => row1[property.Name] != null && row1[property.Name] != DBNull.Value))
                    property.SetValue(t, row[property.Name], new object[] { });
                yield return t;
            }
        }

        [Obsolete("Please use Sql, instead.", true)]
        public static IEnumerable<T> SelectTable<T>(this SqlConnection connection, string tableName)
            where T : new()
        {
            return connection.Execute(cmd => cmd.ExecuteReader(CommandBehavior.CloseConnection).Select<T>(), $"SELECT * FROM [{tableName}]");
        }

        [Obsolete("Please use Sql, instead.", true)]
        public static IEnumerable<T> ExecuteReader<T>(this SqlConnection connection, string query, Action<SqlParameterCollection> fillParams = null)
            where T : new()
        {
            using (var command = connection.CreateCommand(query, fillParams))
            {
                connection.Open();
                return command.ExecuteReader(CommandBehavior.CloseConnection).Select<T>();
            }
        }

        public static bool CanConnect(this SqlConnection conn) => conn.TryConnect() == null;

        public static Exception CheckConnectionString(string connectionString)
        {
            using (var conn = new SqlConnection(connectionString)) return conn.TryConnect();
        }

        public static T CheckDbNull<T>(this SqlDataReader reader, string columnName, T defaultValue, Func<object, T> converter) => ObjectHelper.CheckDbNull(
                                                                                                                                                            reader[columnName],
                                                                                                                                                            defaultValue,
                                                                                                                                                            converter);

        public static SqlCommand CreateCommand(this SqlConnection connection, string commandText, Action<SqlParameterCollection> fillParams = null)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            var result = connection.CreateCommand();
            result.CommandText    = commandText;
            result.CommandTimeout = connection.ConnectionTimeout;
            fillParams?.Invoke(result.Parameters);
            return result;
        }

        public static void EnsureClosed(this SqlConnection connection, Action<SqlConnection> action, bool openConnection = false)
        {
            connection.EnsureClosed(c =>
                                    {
                                        action(c);
                                        return true;
                                    },
                                    openConnection);
        }

        public static void EnsureClosed(this SqlConnection connection, Action action, bool openConnection = false)
        {
            connection.EnsureClosed(c =>
                                    {
                                        action();
                                        return true;
                                    },
                                    openConnection);
        }

        public static TResult EnsureClosed<TResult>(this SqlConnection connection, Func<TResult> action, bool openConnection = false)
        {
            return connection.EnsureClosed(c => action(), openConnection);
        }

        public static TResult EnsureClosed<TResult>(this SqlConnection connection, Func<SqlConnection, TResult> action, bool openConnection = false)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            try
            {
                if (openConnection)
                    connection.Open();
                return action(connection);
            }
            finally
            {
                connection.Close();
            }
        }

        public static async Task<TResult> EnsureClosedAsync<TResult>(this SqlConnection connection, Func<SqlConnection, Task<TResult>> actionAsync, bool openConnection = false)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (actionAsync == null)
                throw new ArgumentNullException(nameof(actionAsync));
            try
            {
                if (openConnection)
                    connection.Open();
                return await actionAsync(connection);
            }
            finally
            {
                connection.Close();
            }
        }

        public static TResult Execute<TResult>(this SqlConnection             connection, Func<SqlCommand, TResult> execute, string sql,
                                               Action<SqlParameterCollection> fillParams = null)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
            if (sql == null)
                throw new ArgumentNullException(nameof(sql));
            using (var command = connection.CreateCommand(sql, fillParams)) return connection.EnsureClosed(conn => execute(command), true);
        }

        public static async Task<TResult> ExecuteAsync<TResult>(this SqlConnection             connection, Func<SqlCommand, Task<TResult>> executeAsync, string sql,
                                               Action<SqlParameterCollection> fillParams = null)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (executeAsync == null)
                throw new ArgumentNullException(nameof(executeAsync));
            if (sql == null)
                throw new ArgumentNullException(nameof(sql));
            using (var command = connection.CreateCommand(sql, fillParams)) return await connection.EnsureClosedAsync(conn => executeAsync(command), true);
        }

        public static int ExecuteNonQuery(this SqlConnection connection, string sql, Action<SqlParameterCollection> fillParams = null)
        {
            return connection.Execute(cmd => cmd.ExecuteNonQuery(), sql, fillParams);
        }

        public static SqlDataReader ExecuteReader(this SqlConnection connection, string sql, Action<SqlParameterCollection> fillParams = null
                                                , CommandBehavior    behavior = CommandBehavior.Default)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (sql == null)
                throw new ArgumentNullException(nameof(sql));
            var command = connection.CreateCommand(sql, fillParams);
            connection.StateChange += (_, e) =>
            {
                if (e.CurrentState == ConnectionState.Closed)
                    command?.Dispose();
            };
            connection.Open();
            return command.ExecuteReader(behavior);
        }

        public static async Task<SqlDataReader> ExecuteReaderAsync(this SqlConnection connection, string sql, Action<SqlParameterCollection> fillParams = null
                                                , CommandBehavior    behavior = CommandBehavior.Default)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (sql == null)
                throw new ArgumentNullException(nameof(sql));
            var command = connection.CreateCommand(sql, fillParams);
            connection.StateChange += (_, e) =>
            {
                if (e.CurrentState == ConnectionState.Closed)
                    command?.Dispose();
            };
            connection.Open();
            return await command.ExecuteReaderAsync(behavior);
        }

        public static object ExecuteScalar(this SqlConnection connection, string sql, Action<SqlParameterCollection> fillParams = null)
        {
            return connection.Execute(cmd => cmd.ExecuteScalar(), sql, fillParams);
        }

        public static async  Task<object> ExecuteScalarAsync(this SqlConnection connection, string sql, Action<SqlParameterCollection> fillParams = null)
        {
            return await connection.ExecuteAsync(cmd =>cmd.ExecuteScalarAsync(), sql, fillParams);
        }

        public static object ExecuteStoredProcedure(this SqlConnection connection, string spName, Action<SqlParameterCollection> fillParams = null,
                                                    Action<string>     logger = null)
        {
            object result = null;
            using (var cmd = connection.CreateCommand())
            {
                var cmdText = new StringBuilder($"Exec [{spName}]");
                if (fillParams != null)
                {
                    fillParams(cmd.Parameters);
                    for (var index = 0; index < cmd.Parameters.Count; index++)
                    {
                        var parameter = cmd.Parameters[index];
                        cmdText.Append(string.Format("\t{2}{0} = '{1}'", parameter.ParameterName, parameter.Value, Environment.NewLine));
                        if (index != cmd.Parameters.Count - 1)
                            cmdText.Append(", ");
                    }
                }

                logger?.Invoke(cmdText.ToString());
                ExecuteTransactional(connection,
                                     trans =>
                                     {
                                         cmd.Transaction = trans;
                                         cmd.CommandText = cmdText.ToString();
                                         result          = cmd.ExecuteScalar();
                                     });
            }

            return result;
        }

        public static void ExecuteTransactional(this SqlConnection             connection, string sql, Action<SqlCommand> executor,
                                                Action<SqlParameterCollection> fillParams = null)
        {
            if (sql == null)
                throw new ArgumentNullException(nameof(sql));
            if (executor == null)
                throw new ArgumentNullException(nameof(executor));

            ExecuteTransactional(connection,
                                 transaction =>
                                 {
                                     using (var command = new SqlCommand(sql, connection, transaction))
                                     {
                                         fillParams?.Invoke(command.Parameters);
                                         executor(command);
                                     }
                                 });
        }

        public static void ExecuteTransactional(this SqlConnection connection, Action executionBlock)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (executionBlock == null)
                throw new ArgumentNullException(nameof(executionBlock));
            ExecuteTransactional(connection, tran => executionBlock());
        }

        public static void ExecuteTransactional(this SqlConnection connection, Action<SqlTransaction> executionBlock)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (executionBlock == null)
                throw new ArgumentNullException(nameof(executionBlock));
            var leaveOpen = connection.State == ConnectionState.Open;
            if (!leaveOpen)
                connection.Open();
            var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                executionBlock(transaction);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                if (!leaveOpen)
                    connection.Close();
            }
        }

        public static T Field<T>(this IDataReader reader, string columnName, Converter<object, T> converter) =>
            converter(reader[columnName]);

        public static T Field<T>(this DataRow row, string columnName, Converter<object, T> converter) =>
            converter(row.Field<object>(columnName));

        public static DataSet FillDataSet(this SqlConnection connection, string sql)
        {
            var result = new DataSet();
            using (var da = new SqlDataAdapter(sql, connection)) da.Fill(result);

            return result;
        }

        public static DataTable FillDataTable(this SqlConnection connection, string sql, Action<SqlParameterCollection> fillParams = null)
        {
            var result = new DataTable();
            using (var command = connection.CreateCommand(sql, fillParams)) EnsureClosed(connection, () => result.Load(command.ExecuteReader()), true);

            return result;
        }

        /// <summary>
        ///     Returns the first row data in specific column of the specified table.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="table"> The table. </param>
        /// <param name="columnTitle"> The column title. </param>
        /// <returns> </returns>
        public static T FirstCol<T>(this DataTable table, string columnTitle) => FirstCol(table, columnTitle, default(T));

        /// <summary>
        ///     Returns the first row data in specific column of the specified table in string format.
        /// </summary>
        /// <param name="table"> The table. </param>
        /// <param name="columnTitle"> The column title. </param>
        /// <returns> </returns>
        public static string FirstCol(this DataTable table, string columnTitle) => FirstCol(table, columnTitle, obj => obj.ToString(), string.Empty);

        /// <summary>
        ///     Returns the first row data in specific column of the specified table.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="table"> The table. </param>
        /// <param name="columnTitle"> The column title. </param>
        /// <param name="defaultValue"> The default value. </param>
        /// <returns> </returns>
        public static T FirstCol<T>(this DataTable table, string columnTitle, T defaultValue) => table.Select(columnTitle, defaultValue).First();

        /// <summary>
        ///     Returns the first row data in specific column of the specified table.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="table"> The table. </param>
        /// <param name="columnTitle"> The column title. </param>
        /// <param name="convertor"> The converter. </param>
        /// <returns> </returns>
        public static T FirstCol<T>(this DataTable table, string columnTitle, Converter<object, T> convertor) =>
            table.Select(columnTitle, convertor).First();

        /// <summary>
        ///     first the specified table according to de given value.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="table"> The table. </param>
        /// <param name="columnTitle"> The column title. </param>
        /// <param name="convertor"> The converter. </param>
        /// <param name="defaultValue"> The default value. </param>
        /// <returns> </returns>
        public static T FirstCol<T>(this DataTable table, string columnTitle, Converter<object, T> convertor, T defaultValue) => table
                                                                                                                                .Select(
                                                                                                                                        columnTitle, convertor,
                                                                                                                                        defaultValue).First();

        /// <summary>
        ///     Gets the column data.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="rows"> The rows. </param>
        /// <param name="columnName"> Name of the column. </param>
        /// <returns> </returns>
        public static IEnumerable<T> GetColumnData<T>(this DataRowCollection rows, string columnName)
            where T : class
        {
            return rows.Cast<DataRow>().Where(row => row != null).Select(row => row[columnName] as T);
        }

        /// <summary>
        ///     Gets the column data.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="rows"> The rows. </param>
        /// <param name="columnIndex"> Index of the column. </param>
        /// <returns> </returns>
        public static IEnumerable<T> GetColumnData<T>(this DataRowCollection rows, int columnIndex = 0)
            where T : class
        {
            return rows.Cast<DataRow>().Where(row => row != null).Select(row => row[columnIndex] as T);
        }

        /// <summary>
        ///     Gets the columns data.
        /// </summary>
        /// <param name="row"> The row. </param>
        /// <returns> </returns>
        public static IEnumerable<object> GetColumnsData(this DataRow row) => row.ItemArray.Select((t, i) => row[i]);

        public static IEnumerable<DataTable> GetTables(this DataSet ds) =>
            ds.Tables.Cast<DataTable>();

        /// <summary>
        ///     Determines whether the specified column in given row is null or empty.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="row"> The row. </param>
        /// <param name="columnTitle"> The column title. </param>
        /// <returns>
        ///     <c>true</c> if the column is null or empty; otherwise, <c>false</c> .
        /// </returns>
        public static bool IsNullOrEmpty<T>(this DataRow row, string columnTitle) => row.IsNullOrEmpty(columnTitle) || row[columnTitle].Equals(default(T));

        /// <summary>
        ///     Determines whether the specified column in given row is null or empty.
        /// </summary>
        /// <param name="row"> The row. </param>
        /// <param name="columnTitle"> The column title. </param>
        /// <returns>
        ///     <c>true</c> if the column is null or empty; otherwise, <c>false</c> .
        /// </returns>
        public static bool IsNullOrEmpty(this DataRow row, string columnTitle) => row[columnTitle] == null || row[columnTitle].ToString() == string.Empty ||
                                                                                  row[columnTitle] == DBNull.Value;

        public static bool IsValidConnectionString(string connectionString) => CheckConnectionString(connectionString) == null;

        /// <summary>
        ///     Selects the specified table.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="table"> The table. </param>
        /// <param name="columnTitle"> The column title. </param>
        /// <param name="defaultValue"> The default value. </param>
        /// <returns> </returns>
        public static IEnumerable<T> Select<T>(this DataTable table, string columnTitle, params T[] defaultValue)
        {
            var result = table.Select().Select(row => row[columnTitle]).ToList();
            return result.Any() ? result.Cast<T>() : defaultValue;
        }

        /// <summary>
        ///     Selects the specified table.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="table"> The table. </param>
        /// <param name="columnTitle"> The column title. </param>
        /// <param name="convertor"> The converter. </param>
        /// <returns> </returns>
        public static IEnumerable<T> Select<T>(this DataTable table, string columnTitle, Converter<object, T> convertor)
        {
            return table.Select().Select(row => row[columnTitle]).Cast(convertor);
        }

        /// <summary>
        ///     Selects the specified table according to de given value .
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="table"> The table. </param>
        /// <param name="columnTitle"> The column title. </param>
        /// <param name="convertor"> The converter. </param>
        /// <param name="defaultValue"> The default value. </param>
        /// <returns> </returns>
        public static IEnumerable<T> Select<T>(this DataTable table, string columnTitle, Converter<object, T> convertor, params T[] defaultValue)
        {
            var buffer = table.Select().Select(row => row[columnTitle]).Cast(convertor);
            var result = buffer as T[] ?? buffer.ToArray();
            return result.Any() ? result : defaultValue;
        }

        /// <summary>
        ///     Selects the specified reader.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="reader"> The data reader. </param>
        /// <param name="converter"> The converter. </param>
        /// <returns> </returns>
        public static IEnumerable<T> Select<T>(this IDataReader reader, Func<IDataReader, T> converter)
            where T : new()
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (converter == null)
                converter = rdr => InnerReaderSelect(rdr, () => new T());
            return While(reader.Read, () => converter(reader));
        }

        public static IEnumerable<T> Select<T>(this IDataReader reader)
            where T : new() => Select(reader, () => new T());

        public static IEnumerable<T> Select<T>(this IDataReader reader, Func<T> creator)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (creator == null)
                throw new ArgumentNullException(nameof(creator));
            return While(reader.Read, () => InnerReaderSelect(reader, creator));
        }

        public static IEnumerable<T> Select<T>(this SqlConnection             connection, string sql, Func<SqlDataReader, T> rowFiller,
                                               Action<SqlParameterCollection> fillParams = null)
        {
            if (rowFiller == null)
                throw new ArgumentNullException(nameof(rowFiller));

            var reader = ExecuteReader(connection, sql, fillParams, CommandBehavior.CloseConnection);
            return While(reader.Read, () => rowFiller(reader), connection.Close);
        }

        public static DataTable SelectDataTable(this SqlConnection connection, string selectCommandText, Action<SqlParameterCollection> fillParam = null,
                                                string             tableName = null)
        {
            return Execute(connection,
                           cmd =>
                           {
                               var result = new DataTable(tableName);
                               using (var adapter = new SqlDataAdapter(cmd)) adapter.Fill(result);

                               return result;
                           },
                           selectCommandText,
                           fillParam);
        }

        public static Exception TryConnect(this SqlConnection conn) => Catch(() => conn.EnsureClosed(c => c.Open()));

        private static T InnerReaderSelect<T>(IDataReader reader, Func<T> creator)
        {
            var properties = typeof(T).GetProperties();
            var t          = creator();
            foreach (var property in properties)
            {
                var value = reader[property.Name];
                if (DBNull.Value.Equals(value))
                    value = null;
                property.SetValue(t, value, new object[] { });
            }

            return t;
        }
    }
}