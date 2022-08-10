using System.Data;
using System.Data.SqlClient;
using System.Globalization;

using Library.Validations;

namespace Library.Helpers;

/// <summary>
/// A utility to do some common tasks about ADO arguments
/// </summary>
public static partial class AdoHelper
{
    public static bool CanConnect(this SqlConnection conn)
        => conn.TryConnectAsync() == null;

    public static async Task<Exception?> CheckConnectionStringAsync(string connectionString)
        => await Using(() => new SqlConnection(connectionString), x => x.TryConnectAsync());

    public static T CheckDbNull<T>(this SqlDataReader reader, string columnName, T defaultValue, Func<object, T> converter)
        => reader is null || reader.IsClosed
                ? throw new ArgumentNullException(nameof(reader))
                : ObjectHelper.CheckDbNull(reader[columnName], defaultValue, converter);

    public static SqlCommand CreateCommand(this SqlConnection connection, string commandText, Action<SqlParameterCollection>? fillParams = null)
    {
        Check.IfArgumentNotNull(connection);
        Check.IfArgumentNotNull(commandText);

        var result = connection.CreateCommand();
        result.CommandText = commandText;
        result.CommandTimeout = connection.ConnectionTimeout;
        fillParams?.Invoke(result.Parameters);
        return result;
    }

    public static void EnsureClosed(this SqlConnection connection, Action<SqlConnection> action, bool openConnection = false)
        => connection.EnsureClosed(c =>
            {
                action(c);
                return true;
            },
            openConnection);

    public static void EnsureClosed(this SqlConnection connection, Action action, bool openConnection = false)
        => connection.EnsureClosed(c =>
            {
                action();
                return true;
            },
            openConnection);

    public static TResult EnsureClosed<TResult>(this SqlConnection connection, Func<TResult> action, bool openConnection = false)
        => connection.EnsureClosed(c => action(), openConnection);

    public static TResult EnsureClosed<TResult>(this SqlConnection connection, Func<SqlConnection, TResult> action, bool openConnection = false)
    {
        Check.IfArgumentNotNull(connection);
        Check.IfArgumentNotNull(action);

        try
        {
            if (openConnection)
            {
                connection.Open();
            }

            return action(connection);
        }
        finally
        {
            connection.Close();
        }
    }

    public static async Task<TResult> EnsureClosedAsync<TResult>(this SqlConnection connection,
            Func<SqlConnection, Task<TResult>> actionAsync,
            bool openConnection = false)
    {
        Check.IfArgumentNotNull(connection);
        Check.IfArgumentNotNull(actionAsync);

        try
        {
            if (openConnection)
            {
                await connection.OpenAsync();
            }

            return await actionAsync(connection);
        }
        finally
        {
            connection.Close();
        }
    }

    public static async Task EnsureClosedAsync(this SqlConnection connection,
            Func<SqlConnection, Task> actionAsync,
            bool openConnection = false)
    {
        Check.IfArgumentNotNull(connection);
        Check.IfArgumentNotNull(actionAsync);

        try
        {
            if (openConnection)
            {
                await connection.OpenAsync();
            }

            await actionAsync(connection);
        }
        finally
        {
            connection.Close();
        }
    }

    public static TResult Execute<TResult>(this SqlConnection connection,
            Func<SqlCommand, TResult> execute,
            string sql,
            Action<SqlParameterCollection>? fillParams = null)
    {
        Check.IfArgumentNotNull(connection);
        Check.IfArgumentNotNull(execute);
        Check.IfArgumentNotNull(sql);

        using var command = connection.CreateCommand(sql, fillParams);
        return connection.EnsureClosed(conn => execute(command), true);
    }

    public static async Task<TResult> ExecuteAsync<TResult>(this SqlConnection connection,
            Func<SqlCommand, Task<TResult>> executeAsync,
            string sql,
            Action<SqlParameterCollection>? fillParams = null)
    {
        Check.IfArgumentNotNull(connection);
        Check.IfArgumentNotNull(executeAsync);
        Check.IfArgumentNotNull(sql);

        using var command = connection.CreateCommand(sql, fillParams);
        return await connection.EnsureClosedAsync(conn => executeAsync(command), true);
    }

    public static int ExecuteNonQuery(this SqlConnection connection, string sql, Action<SqlParameterCollection>? fillParams = null)
            => connection.Execute(cmd => cmd.ExecuteNonQuery(), sql, fillParams);

    /// <summary>
    /// Executes the reader.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connection">The connection.</param>
    /// <param name="query">     The query.</param>
    /// <param name="fillParams">The fill parameters.</param>
    /// <returns></returns>
    [Obsolete("Please use Sql, instead.", true)]
    public static IEnumerable<T> ExecuteReader<T>(SqlConnection connection, string query, Action<SqlParameterCollection>? fillParams = null)
        where T : new()
    {
        using var command = connection.CreateCommand(query, fillParams);
        connection.Open();
        return command.ExecuteReader(CommandBehavior.CloseConnection).Select<T>();
    }

    public static SqlDataReader ExecuteReader(this SqlConnection connection,
            string sql,
            Action<SqlParameterCollection>? fillParams = null,
            CommandBehavior behavior = CommandBehavior.Default)
    {
        using var command = connection.CreateCommand(sql, fillParams);
        connection.StateChange += (_, e) =>
        {
            if (e.CurrentState == ConnectionState.Closed)
            {
                command?.Dispose();
            }
        };
        connection.Open();
        return command.ExecuteReader(behavior);
    }

    public static async Task<SqlDataReader> ExecuteReaderAsync(this SqlConnection connection,
            string sql,
            Action<SqlParameterCollection>? fillParams = null,
            CommandBehavior behavior = CommandBehavior.Default)
    {
        using var command = connection.CreateCommand(sql, fillParams);
        connection.StateChange += (_, e) =>
        {
            if (e.CurrentState == ConnectionState.Closed)
            {
                command?.Dispose();
            }
        };
        connection.Open();
        return await command.ExecuteReaderAsync(behavior);
    }

    public static object ExecuteScalar(this SqlConnection connection, string sql, Action<SqlParameterCollection>? fillParams = null)
            => connection.Execute(cmd => cmd.ExecuteScalar(), sql, fillParams);

    public static async Task<object?> ExecuteScalarAsync(this SqlConnection connection, string sql, Action<SqlParameterCollection>? fillParams = null)
            => await connection.ExecuteAsync(cmd => cmd.ExecuteScalarAsync(), sql, fillParams);

    public static object? ExecuteStoredProcedure(this SqlConnection connection,
            string spName,
            Action<SqlParameterCollection>? fillParams = null,
            Action<string>? logger = null)
    {
        Check.IfArgumentNotNull(connection);

        object? result = null;
        using (var cmd = connection.CreateCommand())
        {
            var cmdText = new StringBuilder($"Exec [{spName}]");
            if (fillParams is not null)
            {
                fillParams(cmd.Parameters);
                for (var index = 0; index < cmd.Parameters.Count; index++)
                {
                    var parameter = cmd.Parameters[index];
                    _ = cmdText.Append(CultureInfo.InvariantCulture, $"\t{Environment.NewLine}{parameter.ParameterName} = '{parameter.Value}'");
                    if (index != cmd.Parameters.Count - 1)
                    {
                        _ = cmdText.Append(", ");
                    }
                }
            }

            logger?.Invoke(cmdText.ToString());
            ExecuteTransactional(connection,
                trans =>
                {
                    cmd.Transaction = trans;
                    cmd.CommandText = cmdText.ToString();
                    result = cmd.ExecuteScalar();
                });
        }

        return result;
    }

    public static void ExecuteTransactional(this SqlConnection connection,
            string sql,
            Action<SqlCommand> executor,
            Action<SqlParameterCollection>? fillParams = null)
        => ExecuteTransactional(connection,
                transaction =>
                {
                    using var command = new SqlCommand(sql, connection, transaction);
                    fillParams?.Invoke(command.Parameters);
                    executor(command);
                });

    public static void ExecuteTransactional(this SqlConnection connection, Action executionBlock)
        => ExecuteTransactional(connection, tran => executionBlock());

    public static void ExecuteTransactional(this SqlConnection connection, Action<SqlTransaction>? executionBlock)
    {
        Check.IfArgumentNotNull(connection);
        Check.IfArgumentNotNull(executionBlock);

        var leaveOpen = connection.State == ConnectionState.Open;
        if (!leaveOpen)
        {
            connection.Open();
        }

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
            {
                connection.Close();
            }
        }
    }

    public static T Field<T>(this IDataReader reader, string columnName, Converter<object, T> converter)
    {
        Check.IfArgumentNotNull(columnName);
        Check.IfArgumentNotNull(converter);
        Check.IfArgumentNotNull(reader);

        return converter(reader[columnName]);
    }

    public static T? Field<T>(this DataRow row, string columnName, Converter<object?, T?>? converter)
    {
        Check.IfArgumentNotNull(row);
        Check.IfArgumentNotNull(columnName);
        return converter is not null ? converter(row.Field<object>(columnName)) : row.Field<T>(columnName);
    }

    public static DataSet FillDataSet(this SqlConnection connection, string sql)
    {
        Check.IfArgumentNotNull(sql);

        var result = new DataSet();
        using (var da = new SqlDataAdapter(sql, connection))
        {
            _ = da.Fill(result);
        }

        return result;
    }

    public static DataTable FillDataTable(this SqlConnection connection, string sql, Action<SqlParameterCollection>? fillParams = null)
    {
        var result = new DataTable();
        using (var command = connection.CreateCommand(sql, fillParams))
        {
            EnsureClosed(connection, () => result.Load(command.ExecuteReader()), true);
        }

        return result;
    }

    /// <summary>
    /// Returns the first row data in specific column of the specified table.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="table">      The table.</param>
    /// <param name="columnTitle">The column title.</param>
    /// <returns></returns>
    public static T? FirstCol<T>(this DataTable table, string columnTitle)
        => FirstCol(table, columnTitle, default(T));

    /// <summary>
    /// Returns the first row data in specific column of the specified table in string format.
    /// </summary>
    /// <param name="table">      The table.</param>
    /// <param name="columnTitle">The column title.</param>
    /// <returns></returns>
    public static string? FirstCol(this DataTable table, string columnTitle)
        => FirstCol(table, columnTitle, obj => obj.ToString(), string.Empty);

    /// <summary>
    /// Returns the first row data in specific column of the specified table.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="table">       The table.</param>
    /// <param name="columnTitle"> The column title.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static T FirstCol<T>(this DataTable table, string columnTitle, T defaultValue)
        => table.Select(columnTitle, defaultValue).First();

    /// <summary>
    /// Returns the first row data in specific column of the specified table.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="table">      The table.</param>
    /// <param name="columnTitle">The column title.</param>
    /// <param name="convertor">  The converter.</param>
    /// <returns></returns>
    public static T FirstCol<T>(this DataTable table, string columnTitle, Converter<object, T> convertor)
        => table.Select(columnTitle, convertor).First();

    /// <summary>
    /// first the specified table according to de given value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="table">       The table.</param>
    /// <param name="columnTitle"> The column title.</param>
    /// <param name="convertor">   The converter.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static T FirstCol<T>(this DataTable table, string columnTitle, Converter<object, T> convertor, T defaultValue)
        => table.Select(columnTitle, convertor, defaultValue).First();

    /// <summary>
    /// Gets the column data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rows">      The rows.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <returns></returns>
    public static IEnumerable<T?> GetColumnData<T>(this DataRowCollection rows, string columnName)
        where T : class
        => rows.Cast<DataRow>().Where(row => row is not null).Select(row => row[columnName] as T);

    /// <summary>
    /// Gets the column data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rows">       The rows.</param>
    /// <param name="columnIndex">Index of the column.</param>
    /// <returns></returns>
    public static IEnumerable<T?> GetColumnData<T>(this DataRowCollection rows, int columnIndex = 0)
        where T : class
        => rows.Cast<DataRow>().Where(row => row is not null).Select(row => row[columnIndex] as T);

    /// <summary>
    /// Gets the columns data.
    /// </summary>
    /// <param name="row">The row.</param>
    /// <returns></returns>
    public static IEnumerable<object> GetColumnsData(this DataRow row)
        => row is null
            ? throw new ArgumentNullException(nameof(row))
            : row.ItemArray.Select((t, i) => row[i]);

    public static IEnumerable<DataTable> GetTables(this DataSet ds)
        => ds is null
            ? throw new ArgumentNullException(nameof(ds))
            : ds.Tables.Cast<DataTable>();

    /// <summary>
    /// Determines whether the specified column in given row is null or empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="row">        The row.</param>
    /// <param name="columnTitle">The column title.</param>
    /// <returns><c>true</c> if the column is null or empty; otherwise, <c>false</c> .</returns>
    public static bool IsNullOrEmpty<T>(this DataRow row, string columnTitle)
        => row.ArgumentNotNull().IsNullOrEmpty(columnTitle) || row[columnTitle].Equals(default(T));

    /// <summary>
    /// Determines whether the specified column in given row is null or empty.
    /// </summary>
    /// <param name="row">        The row.</param>
    /// <param name="columnTitle">The column title.</param>
    /// <returns><c>true</c> if the column is null or empty; otherwise, <c>false</c> .</returns>
    public static bool IsNullOrEmpty(this DataRow row, string columnTitle)
        => row is null || row[columnTitle] is null || StringHelper.IsEmpty(row[columnTitle].ToString()) || row[columnTitle] == DBNull.Value;

    public static bool IsValidConnectionString(string connectionString)
        => CheckConnectionStringAsync(connectionString)?.Result == null;

    /// <summary>
    /// Selects the specified table.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="table">      The table.</param>
    /// <param name="columnTitle">The column title.</param>
    /// <param name="convertor">  The converter.</param>
    /// <param name="predicate">  The predicate.</param>
    /// <returns></returns>
    [Obsolete("Please use Sql, instead.", true)]
    public static IEnumerable<T> Select<T>(this DataTable table, string columnTitle, Func<object, T> convertor, Predicate<object> predicate)
        => table.ArgumentNotNull().Select().Where(row => predicate?.Invoke(row) is not false).Select(row => convertor(row[columnTitle]));

    /// <summary>
    /// Selects the specified table.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="table">The table.</param>
    /// <returns></returns>
    [Obsolete("Please use Sql, instead.", true)]
    public static IEnumerable<T> Select<T>(this DataTable table)
        where T : new()
    {
        Check.IfArgumentNotNull(table);

        var type = typeof(T);
        var properties = type.GetProperties();
        var columnNames = table.Columns.Cast<DataColumn>().Select(col => col.ColumnName.ToLowerInvariant());
        foreach (var row in table.Select())
        {
            var t = new T();
            var row1 = row;
            foreach (var property in properties.Where(property => columnNames.Contains(property.Name.ToLowerInvariant()))
                .Where(property => row1[property.Name] is not null && row1[property.Name] != DBNull.Value))
            {
                property.SetValue(t, row[property.Name], Array.Empty<object>());
            }

            yield return t;
        }
    }

    /// <summary>
    /// Selects the specified table.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="table">       The table.</param>
    /// <param name="columnTitle"> The column title.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static IEnumerable<T> Select<T>(this DataTable table, string columnTitle, params T[] defaultValue)
    {
        Check.IfArgumentNotNull(table);

        var result = table.Select().Select(row => row[columnTitle]).ToList();
        return result.Any() ? result.Cast<T>() : defaultValue;
    }

    /// <summary>
    /// Selects the specified table.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="table">      The table.</param>
    /// <param name="columnTitle">The column title.</param>
    /// <param name="convertor">  The converter.</param>
    /// <returns></returns>
    public static IEnumerable<T> Select<T>(this DataTable table, string columnTitle, Converter<object, T> convertor)
        => table is null
            ? throw new ArgumentNullException(nameof(table))
            : table.Select().Select(row => row[columnTitle]).Cast(convertor);

    /// <summary>
    /// Selects the specified table according to de given value .
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="table">       The table.</param>
    /// <param name="columnTitle"> The column title.</param>
    /// <param name="convertor">   The converter.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static IEnumerable<T> Select<T>(this DataTable table, string columnTitle, Converter<object, T> convertor, params T[] defaultValue)
    {
        Check.IfArgumentNotNull(table);

        var buffer = table.Select().Select(row => row[columnTitle]).Cast(convertor);
        var result = buffer as T[] ?? buffer.ToArray();
        return result.Any() ? result : defaultValue;
    }

    /// <summary>
    /// Selects the specified reader.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="reader">   The data reader.</param>
    /// <param name="converter">The converter.</param>
    /// <returns></returns>
    public static IEnumerable<T> Select<T>(this IDataReader reader, Func<IDataReader, T> converter)
        where T : new()
        => reader is not null
            ? While(reader.Read, () => converter(reader))
            : throw new ArgumentNullException(nameof(reader));

    public static IEnumerable<T> Select<T>(this IDataReader reader)
            where T : new() => Select(reader, () => new T());

    public static IEnumerable<T> Select<T>(this IDataReader reader, Func<T> creator)
        => reader is null
            ? throw new ArgumentNullException(nameof(reader))
            : While(reader.Read, () => InnerReaderSelect(reader, creator));

    public static IEnumerable<T> Select<T>(this SqlConnection connection,
            string sql,
            Func<SqlDataReader, T> rowFiller,
            Action<SqlParameterCollection>? fillParams = null)
    {
        Check.IfArgumentNotNull(connection);

        var reader = ExecuteReader(connection, sql, fillParams, CommandBehavior.CloseConnection);
        return While(reader.Read, () => rowFiller(reader), connection.Close);
    }

    public static DataTable SelectDataTable(this SqlConnection connection,
            string selectCommandText,
            Action<SqlParameterCollection>? fillParam = null,
            string? tableName = null)
        => Execute(connection,
                cmd =>
                {
                    var result = new DataTable(tableName);
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        _ = adapter.Fill(result);
                    }

                    return result;
                },
                selectCommandText,
                fillParam);

    /// <summary>
    /// Selects the table.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connection">The connection.</param>
    /// <param name="tableName"> Name of the table.</param>
    /// <returns></returns>
    [Obsolete("Please use Sql, instead.", true)]
    public static IEnumerable<T> SelectTable<T>(SqlConnection connection, string tableName)
        where T : new()
        => connection.Execute(cmd => cmd.ExecuteReader(CommandBehavior.CloseConnection).Select<T>(), $"SELECT * FROM [{tableName}]");

    public static async Task<Exception?> TryConnectAsync(this SqlConnection conn)
    {
        try
        {
            await conn.EnsureClosedAsync(c => c.OpenAsync());
            return null;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    private static T InnerReaderSelect<T>(IDataReader reader, Func<T> creator)
    {
        var properties = typeof(T).GetProperties();
        var t = creator();
        foreach (var property in properties)
        {
            var value = reader[property.Name];
            if (DBNull.Value.Equals(value))
            {
                value = null;
            }

            property.SetValue(t, value, Array.Empty<object>());
        }

        return t;
    }
}