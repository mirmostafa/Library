using System.Data;
using System.Globalization;

using Library.Results;
using Library.Validations;

using Microsoft.Data.SqlClient;

namespace Library.Helpers;

/// <summary>
/// A utility to do some common tasks about ADO arguments
/// </summary>
public static partial class AdoHelper
{
    /// <summary>
    /// Checks if a SqlConnection can connect.
    /// </summary>
    /// <param name="conn">The SqlConnection to check.</param>
    /// <returns>True if the SqlConnection can connect, false otherwise.</returns>
    public static async Task<bool> CanConnectAsync(this SqlConnection conn, CancellationToken cancellationToken = default)
    {
        var result = await conn.TryConnectAsync(cancellationToken: cancellationToken);
        return result.IsSucceed;
    }

    /// <summary>
    /// Checks if a value retrieved from a SqlDataReader is DBNull and provides a default value if
    /// it is.
    /// </summary>
    /// <typeparam name="T">The type of the value to check.</typeparam>
    /// <param name="reader">The SqlDataReader to retrieve the value from.</param>
    /// <param name="columnName">The name of the column in the SqlDataReader.</param>
    /// <param name="defaultValue">The default value to return if the retrieved value is DBNull.</param>
    /// <param name="converter">A function to convert the retrieved object to the desired type.</param>
    /// <returns>
    /// The converted value if it's not DBNull, or the provided defaultValue if it is DBNull.
    /// </returns>
    public static T CheckDbNull<T>(this SqlDataReader reader, string columnName, T defaultValue, Func<object, T> converter)
    {
        Check.MustBeArgumentNotNull(reader);
        Check.MustBeArgumentNotNull(columnName);

        // Retrieve the value from the SqlDataReader using the columnName.
        var value = reader[columnName];

        // Use the ObjectHelper.CheckDbNull method to check for DBNull and provide a default value
        // if needed.
        return ObjectHelper.CheckDbNull(value, defaultValue, converter);
    }

    public static SqlCommand CreateCommand(this SqlConnection connection, string commandText, Action<SqlParameterCollection>? fillParams = null)
    {
        Check.MustBeArgumentNotNull(connection);
        Check.MustBeArgumentNotNull(commandText);

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
        Check.MustBeArgumentNotNull(connection);
        Check.MustBeArgumentNotNull(action);

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
            bool openConnection = false, CancellationToken cancellationToken = default)
    {
        Check.MustBeArgumentNotNull(connection);
        Check.MustBeArgumentNotNull(actionAsync);

        try
        {
            if (openConnection)
            {
                await connection.OpenAsync(cancellationToken);
            }

            return await actionAsync(connection);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public static async Task EnsureClosedAsync(this SqlConnection connection,
            Func<SqlConnection, CancellationToken, Task> actionAsync,
            bool openConnection = false, CancellationToken cancellationToken = default)
    {
        Check.MustBeArgumentNotNull(connection);
        Check.MustBeArgumentNotNull(actionAsync);

        try
        {
            if (openConnection)
            {
                await connection.OpenAsync(cancellationToken);
            }

            await actionAsync(connection, cancellationToken);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public static TResult Execute<TResult>(this SqlConnection connection,
            Func<SqlCommand, TResult> execute,
            string sql,
            Action<SqlParameterCollection>? fillParams = null)
    {
        Check.MustBeArgumentNotNull(connection);
        Check.MustBeArgumentNotNull(execute);
        Check.MustBeArgumentNotNull(sql);

        using var command = connection.CreateCommand(sql, fillParams);
        return connection.EnsureClosed(conn => execute(command), true);
    }

    public static async Task<TResult> ExecuteAsync<TResult>(this SqlConnection connection,
            Func<SqlCommand, Task<TResult>> executeAsync,
            string sql,
            Action<SqlParameterCollection>? fillParams = null, CancellationToken cancellationToken = default)
    {
        Check.MustBeArgumentNotNull(connection);
        Check.MustBeArgumentNotNull(executeAsync);
        Check.MustBeArgumentNotNull(sql);

        using var command = connection.CreateCommand(sql, fillParams);
        return await connection.EnsureClosedAsync(conn => executeAsync(command), true, cancellationToken: cancellationToken);
    }

    public static int ExecuteNonQuery(this SqlConnection connection, string sql, Action<SqlParameterCollection>? fillParams = null)
            => connection.Execute(cmd => cmd.ExecuteNonQuery(), sql, fillParams);

    /// <summary>
    /// Executes the reader.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connection">The connection.</param>
    /// <param name="query">The query.</param>
    /// <param name="fillParams">The fill parameters.</param>
    /// <returns></returns>
    [Obsolete("Please use Sql, instead.", true)]
    public static IEnumerable<T> ExecuteReader<T>(this SqlConnection connection, string query, Action<SqlParameterCollection>? fillParams = null)
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
            CommandBehavior behavior = CommandBehavior.Default, CancellationToken cancellationToken = default)
    {
        using var command = connection.CreateCommand(sql, fillParams);
        connection.StateChange += (_, e) =>
        {
            if (e.CurrentState == ConnectionState.Closed)
            {
                command?.Dispose();
            }
        };
        await connection.CloseAsync();
        return await command.ExecuteReaderAsync(behavior, cancellationToken);
    }

    public static object ExecuteScalar(this SqlConnection connection, string sql, Action<SqlParameterCollection>? fillParams = null)
            => connection.Execute(cmd => cmd.ExecuteScalar(), sql, fillParams);

    public static async Task<object?> ExecuteScalarAsync(this SqlConnection connection, string sql, Action<SqlParameterCollection>? fillParams = null, CancellationToken cancellationToken = default)
            => await connection.ExecuteAsync(cmd => cmd.ExecuteScalarAsync(), sql, fillParams, cancellationToken: cancellationToken);

    public static object? ExecuteStoredProcedure(this SqlConnection connection,
            string spName,
            Action<SqlParameterCollection>? fillParams = null,
            Action<string>? logger = null)
    {
        Check.MustBeArgumentNotNull(connection);

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
        Check.MustBeArgumentNotNull(connection);
        Check.MustBeArgumentNotNull(executionBlock);

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
        Check.MustBeArgumentNotNull(columnName);
        Check.MustBeArgumentNotNull(converter);
        Check.MustBeArgumentNotNull(reader);

        return converter(reader[columnName]);
    }

    public static T? Field<T>(this DataRow row, string columnName, Converter<object?, T?>? converter)
    {
        Check.MustBeArgumentNotNull(row);
        Check.MustBeArgumentNotNull(columnName);
        return converter is not null ? converter(row.Field<object>(columnName)) : row.Field<T>(columnName);
    }

    public static DataSet FillDataSet(this SqlConnection connection, string sql)
    {
        Check.MustBeArgumentNotNull(sql);

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
    /// <param name="table">The table.</param>
    /// <param name="columnTitle">The column title.</param>
    /// <returns></returns>
    public static T? FirstCol<T>(this DataTable table, string columnTitle)
        => FirstCol(table, columnTitle, default(T));

    /// <summary>
    /// Returns the first row data in specific column of the specified table in string format.
    /// </summary>
    /// <param name="table">The table.</param>
    /// <param name="columnTitle">The column title.</param>
    /// <returns></returns>
    public static string? FirstCol(this DataTable table, string columnTitle)
        => FirstCol(table, columnTitle, obj => obj.ToString(), string.Empty);

    /// <summary>
    /// Returns the first row data in specific column of the specified table.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="table">The table.</param>
    /// <param name="columnTitle">The column title.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static T FirstCol<T>(this DataTable table, string columnTitle, T defaultValue)
        => table.Select(columnTitle, defaultValue).First();

    /// <summary>
    /// Returns the first row data in specific column of the specified table.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="table">The table.</param>
    /// <param name="columnTitle">The column title.</param>
    /// <param name="convertor">The converter.</param>
    /// <returns></returns>
    public static T FirstCol<T>(this DataTable table, string columnTitle, Converter<object, T> convertor)
        => table.Select(columnTitle, convertor).First();

    /// <summary>
    /// first the specified table according to de given value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="table">The table.</param>
    /// <param name="columnTitle">The column title.</param>
    /// <param name="convertor">The converter.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static T FirstCol<T>(this DataTable table, string columnTitle, Converter<object, T> convertor, T defaultValue)
        => table.Select(columnTitle, convertor, defaultValue).First();

    /// <summary>
    /// Gets the column data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rows">The rows.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <returns></returns>
    public static IEnumerable<T?> GetColumnData<T>(this DataRowCollection rows, string columnName)
        where T : class
        => rows.Cast<DataRow>().Where(row => row is not null).Select(row => row[columnName] as T);

    /// <summary>
    /// Gets the column data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rows">The rows.</param>
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
    /// <param name="row">The row.</param>
    /// <param name="columnTitle">The column title.</param>
    /// <returns><c>true</c> if the column is null or empty; otherwise, <c>false</c> .</returns>
    public static bool IsNullOrEmpty<T>(this DataRow row, string columnTitle)
        => row.ArgumentNotNull().IsNullOrEmpty(columnTitle) || row[columnTitle].Equals(default(T));

    /// <summary>
    /// Determines whether the specified column in given row is null or empty.
    /// </summary>
    /// <param name="row">The row.</param>
    /// <param name="columnTitle">The column title.</param>
    /// <returns><c>true</c> if the column is null or empty; otherwise, <c>false</c> .</returns>
    public static bool IsNullOrEmpty(this DataRow row, string columnTitle)
        => row is null || row[columnTitle] is null || StringHelper.IsNullOrEmpty(row[columnTitle].ToString()) || row[columnTitle] == DBNull.Value;

    /// <summary>
    /// Selects the specified table.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="table">The table.</param>
    /// <param name="columnTitle">The column title.</param>
    /// <param name="convertor">The converter.</param>
    /// <param name="predicate">The predicate.</param>
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
        Check.MustBeArgumentNotNull(table);

        var type = typeof(T);
        var properties = type.GetProperties();
        var columnNames = table.Columns.Cast<DataColumn>().Compact().Select(col => col.ColumnName?.ToUpperInvariant());
        foreach (var row in table.Select())
        {
            var t = new T();
            var row1 = row;
            foreach (var property in properties.Where(property => columnNames.Contains(property.Name?.ToUpperInvariant()))
                .Where(property => row1[property.Name] is not null && row1[property.Name] != DBNull.Value))
            {
                property.SetValue(t, row[property.Name], []);
            }

            yield return t;
        }
    }

    /// <summary>
    /// Selects the specified column title from the given DataTable and returns the result as an
    /// IEnumerable of type T.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the returned IEnumerable.</typeparam>
    /// <param name="table">The DataTable from which to select the column.</param>
    /// <param name="columnTitle">The title of the column to select.</param>
    /// <param name="defaultValue">The default value to return if the column is empty.</param>
    /// <returns>An IEnumerable of type T containing the selected column values.</returns>
    public static IEnumerable<T> Select<T>(this DataTable table, string columnTitle, params T[] defaultValue)
    {
        Check.MustBeArgumentNotNull(table);

        var result = table.Select().Select(row => row[columnTitle]).ToList();
        return result.Count != 0 ? result.Cast<T>() : defaultValue;
    }

    /// <summary>
    /// Selects the specified column from the given DataTable and converts it to the specified type
    /// using the given converter.
    /// </summary>
    public static IEnumerable<T> Select<T>(this DataTable table, string columnTitle, Converter<object, T> convertor)
            => table is null
                ? throw new ArgumentNullException(nameof(table))
                : table.Select().Select(row => row[columnTitle]).Cast(convertor);

    /// <summary>
    /// Selects the specified column title from the DataTable and converts it to the specified type
    /// using the provided convertor.
    /// </summary>
    /// <typeparam name="T">The type to convert the column to.</typeparam>
    /// <param name="table">The DataTable to select from.</param>
    /// <param name="columnTitle">The title of the column to select.</param>
    /// <param name="convertor">The convertor to use to convert the column.</param>
    /// <param name="defaultValue">The default value to return if the column is empty.</param>
    /// <returns>An IEnumerable of the specified type containing the converted column values.</returns>
    public static IEnumerable<T> Select<T>(this DataTable table, string columnTitle, Converter<object, T> convertor, params T[] defaultValue)
    {
        Check.MustBeArgumentNotNull(table);

        var buffer = table.Select().Select(row => row[columnTitle]).Cast(convertor);
        var result = buffer as T[] ?? buffer.ToArray();
        return result.Length != 0 ? result : defaultValue;
    }

    /// <summary>
    /// Executes the provided converter function for each row in the IDataReader and returns an
    /// IEnumerable of the results. Throws an ArgumentNullException if the IDataReader is null.
    /// </summary>
    public static IEnumerable<T> Select<T>(this IDataReader reader, Func<IDataReader, T> converter)
            where T : new()
            => reader is not null
                ? While(reader.Read, () => converter(reader))
                : throw new ArgumentNullException(nameof(reader));

    /// <summary>
    /// Extension method to select data from an IDataReader into an IEnumerable of type T.
    /// </summary>
    public static IEnumerable<T> Select<T>(this IDataReader reader)
        where T : new() => Select(reader, () => new T());

    /// <summary>
    /// Executes the specified reader and creates a collection of objects using the specified
    /// creator function.
    /// </summary>
    /// <typeparam name="T">The type of the objects to create.</typeparam>
    /// <param name="reader">The reader to execute.</param>
    /// <param name="creator">The function used to create objects.</param>
    /// <returns>A collection of objects created using the specified creator function.</returns>
    public static IEnumerable<T> Select<T>(this IDataReader reader, Func<T> creator)
    {
        Check.MustBeArgumentNotNull(reader);
        Check.MustBeArgumentNotNull(creator);

        var properties = typeof(T).GetProperties();
        var columnNames = For(reader.FieldCount, reader.GetName).Except(x => !properties.Select(x => x.Name).Contains(x));
        while (reader.Read())
        {
            var t = creator();
            foreach (var columnName in columnNames)
            {
                var value = reader[columnName];
                if (value == DBNull.Value)
                {
                    value = null;
                }

                var property = properties.FirstOrDefault(x => x.Name == columnName);
                _ = Catch(() => property!.SetValue(t, value, []));
            }
            yield return t;
        }
    }

    /// <summary>
    /// Executes a SQL query and returns the result as an IEnumerable of type T.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="connection">The connection to the database.</param>
    /// <param name="sql">The SQL query to execute.</param>
    /// <param name="rowFiller">A function to fill the result from the SqlDataReader.</param>
    /// <param name="fillParams">An optional action to fill the SqlParameterCollection.</param>
    /// <returns>An IEnumerable of type T.</returns>
    public static IEnumerable<T> Select<T>(this SqlConnection connection,
                string sql,
                Func<SqlDataReader, T> rowFiller,
                Action<SqlParameterCollection>? fillParams = null)
    {
        Check.MustBeArgumentNotNull(connection);

        var reader = ExecuteReader(connection, sql, fillParams, CommandBehavior.CloseConnection);
        return While(reader.Read, () => rowFiller(reader), connection.Close);
    }

    /// <summary>
    /// Executes a select command and returns the result as a DataTable.
    /// </summary>
    /// <param name="connection">The connection to the database.</param>
    /// <param name="selectCommandText">The select command text.</param>
    /// <param name="fillParam">An optional action to fill the command parameters.</param>
    /// <param name="tableName">An optional table name for the DataTable.</param>
    /// <returns>A DataTable containing the result of the select command.</returns>
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
    /// <param name="tableName">Name of the table.</param>
    /// <returns></returns>
    [Obsolete("Please use Sql, instead.", true)]
    public static IEnumerable<T> SelectTable<T>(this SqlConnection connection, string tableName)
        where T : new()
        => connection.Execute(cmd => cmd.ExecuteReader(CommandBehavior.CloseConnection).Select<T>(), $"SELECT * FROM [{tableName}]");

    /// <summary>
    /// Tries to connect to a SqlConnection asynchronously.
    /// </summary>
    /// <param name="conn">The SqlConnection to connect to.</param>
    /// <returns>An exception if the connection fails, otherwise null.</returns>
    public static async Task<TryMethodResult> TryConnectAsync(this SqlConnection conn, CancellationToken cancellationToken = default)
    {
        try
        {
            await conn.EnsureClosedAsync((c, t) => c.OpenAsync(t), cancellationToken: cancellationToken);
            return TryMethodResult.Success();
        }
        catch (Exception ex)
        {
            return TryMethodResult.Fail(ex);
        }
    }
}