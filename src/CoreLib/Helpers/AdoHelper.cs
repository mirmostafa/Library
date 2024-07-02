using System.Data;
using System.Globalization;

using Library.Data.SqlServer;
using Library.Results;
using Library.Validations;

using Microsoft.Data.SqlClient;

namespace Library.Helpers;

/// <summary>
/// A utility to do some common tasks about ADO arguments
/// </summary>
public static class AdoHelper
{
    /// <summary>
    /// Checks if a SqlConnection can connect.
    /// </summary>
    /// <param name="conn">The SqlConnection to check.</param>
    /// <returns>True if the SqlConnection can connect, false otherwise.</returns>
    public static async Task<bool> CanConnectAsync([DisallowNull] this SqlConnection conn, CancellationToken cancellationToken = default)
    {
        var result = await conn.TryConnectAsync(cancellationToken: cancellationToken);
        return result.IsSucceed;
    }

    public static SqlCommand CreateCommand([DisallowNull] this SqlConnection connection, [DisallowNull] string commandText, Action<SqlParameterCollection>? fillParams = null)
    {
        Check.MustBeArgumentNotNull(connection);
        Check.MustBeArgumentNotNull(commandText);

        var result = connection.CreateCommand();
        result.CommandText = commandText;
        result.CommandTimeout = connection.ConnectionTimeout;
        fillParams?.Invoke(result.Parameters);
        return result;
    }

    public static SqlCommand CreateCommand([DisallowNull] this SqlConnection conn, [DisallowNull] string sql)
    {
        Check.MustBeArgumentNotNull(conn);
        Check.MustBeArgumentNotNull(sql);

        var result = conn.CreateCommand();
        result.CommandText = sql;
        return result;
    }

    public static void EnsureClosed([DisallowNull] this SqlConnection connection, [DisallowNull] Action<SqlConnection> action, bool openConnection = false)
    {
        Check.MustBeArgumentNotNull(action);

        _ = connection.EnsureClosed(c =>
                {
                    action(c);
                    return true;
                },
                openConnection);
    }

    public static void EnsureClosed([DisallowNull] this SqlConnection connection, [DisallowNull] Action action, bool openConnection = false)
    {
        Check.MustBeArgumentNotNull(action);

        _ = connection.EnsureClosed(_ =>
                {
                    action();
                    return true;
                },
                openConnection);
    }

    public static TResult EnsureClosed<TResult>([DisallowNull] this SqlConnection connection, [DisallowNull] Func<TResult> action, bool openConnection = false)
    {
        Check.MustBeArgumentNotNull(action);

        return connection.EnsureClosed(_ => action(), openConnection);
    }

    public static TResult EnsureClosed<TResult>([DisallowNull] this SqlConnection connection, [DisallowNull] Func<SqlConnection, TResult> action, bool openConnection = false)
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

    public static async Task<TResult> EnsureClosedAsync<TResult>([DisallowNull] this SqlConnection connection,
            [DisallowNull] Func<SqlConnection, Task<TResult>> actionAsync,
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

    public static async Task EnsureClosedAsync([DisallowNull] this SqlConnection connection,
            [DisallowNull] Func<SqlConnection, CancellationToken, Task> actionAsync,
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

    public static int ExecuteNonQuery([DisallowNull] this SqlConnection connection, [DisallowNull] string sql, Action<SqlParameterCollection>? fillParams = null)
        => connection.ArgumentNotNull().Execute(cmd => cmd.ExecuteNonQuery(), sql.ArgumentNotNull(), fillParams);

    public static SqlDataReader ExecuteReader([DisallowNull] this SqlConnection connection,
            [DisallowNull] string sql,
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
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        return command.ExecuteReader(behavior);
    }

    public static async Task<SqlDataReader> ExecuteReaderAsync([DisallowNull] this SqlConnection connection,
            [DisallowNull] string sql,
            Action<SqlParameterCollection>? fillParams = null,
            CommandBehavior behavior = CommandBehavior.Default, CancellationToken cancellationToken = default)
    {
        await using var command = connection.CreateCommand(sql, fillParams);
        connection.StateChange += (_, e) =>
        {
            if (e.CurrentState == ConnectionState.Closed)
            {
                command?.Dispose();
            }
        };
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }

        return await command.ExecuteReaderAsync(behavior, cancellationToken);
    }

    public static object ExecuteScalar([DisallowNull] this SqlConnection connection, [DisallowNull] string sql, Action<SqlParameterCollection>? fillParams = null)
        => connection.Execute(cmd => cmd.ExecuteScalar(), sql, fillParams);

    public static async Task<object?> ExecuteScalarAsync([DisallowNull] this SqlConnection connection, [DisallowNull] string sql, Action<SqlParameterCollection>? fillParams = null, CancellationToken cancellationToken = default)
        => await connection.ExecuteAsync(cmd => cmd.ExecuteScalarAsync(), sql, fillParams, cancellationToken: cancellationToken);

    public static object? ExecuteStoredProcedure([DisallowNull] this SqlConnection connection,
            [DisallowNull] string spName,
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

    public static void ExecuteTransactional([DisallowNull] this SqlConnection connection, [DisallowNull] Action<SqlTransaction>? executionBlock)
    {
        Check.MustBeArgumentNotNull(connection);
        Check.MustBeArgumentNotNull(executionBlock);

        var leaveOpen = connection.State == ConnectionState.Open;
        if (!leaveOpen)
        {
            connection.Open();
        }

        using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
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

    public static T Field<T>([DisallowNull] this IDataReader reader, [DisallowNull] string columnName, [DisallowNull] Converter<object, T> converter)
    {
        Check.MustBeArgumentNotNull(columnName);
        Check.MustBeArgumentNotNull(converter);
        Check.MustBeArgumentNotNull(reader);

        return converter(reader[columnName]);
    }

    public static T? Field<T>([DisallowNull] this DataRow row, [DisallowNull] string columnName, [DisallowNull] Converter<object?, T?>? converter)
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

    public static DataSet FillDataSetByTableNames([DisallowNull] this SqlConnection connection, params string[] tableNames)
    {
        Check.MustBeArgumentNotNull(connection);
        Check.MustBeArgumentNotNull(tableNames);

        var result = connection.FillDataSet(tableNames.Select(t => SqlStatementBuilder.CreateSelect(t)).Merge(Environment.NewLine));
        for (var i = 0; i < tableNames.Length; i++)
        {
            result.Tables[i].TableName = tableNames[i];
        }

        return result;
    }

    public static DataTable FillDataTable([DisallowNull] this SqlConnection connection, [DisallowNull] string sql, Action<SqlParameterCollection>? fillParams = null)
    {
        var result = new DataTable();
        using (var command = connection.CreateCommand(sql, fillParams))
        {
            EnsureClosed(connection, () => result.Load(command.ExecuteReader()), true);
        }

        return result;
    }

    public static IEnumerable<DataTable> FillDataTables([DisallowNull] this SqlConnection conn, params string[] queries)
    {
        Check.MustBeArgumentNotNull(queries);

        using var cmd = conn.CreateCommand();
        using var da = new SqlDataAdapter(cmd);

        conn.Open();
        foreach (var query in queries)
        {
            cmd.CommandText = query;
            var dataTable = new DataTable();
            _ = da.Fill(dataTable);
            yield return dataTable;
        }
    }

    public static IEnumerable<DataTable> GetTables(this DataSet ds)
            => ds is null
            ? throw new ArgumentNullException(nameof(ds))
            : ds.Tables.Cast<DataTable>();

    public static DataTable LoadDataTable([DisallowNull] this SqlConnection connection, [DisallowNull] string query, Action<SqlParameterCollection>? fillParams = null)
    {
        var result = new DataTable();
        using var command = connection.CreateCommand(query);
        fillParams?.Invoke(command.Parameters);
        command.Connection.Open();
        result.Load(command.ExecuteReader());
        command.Connection.Close();

        return result;
    }

    public static async Task<DataTable> LoadDataTableAsync([DisallowNull] this SqlConnection conn, [DisallowNull] string query, Action<SqlParameterCollection>? fillParams = null, CancellationToken cancellationToken = default)
    {
        var result = new DataTable();
        await using var command = conn.CreateCommand(query);
        fillParams?.Invoke(command.Parameters);
        await command.Connection.OpenAsync(cancellationToken);
        result.Load(await command.ExecuteReaderAsync(cancellationToken));
        return result;
    }

    /// <summary>
    /// Tries to connect to a SqlConnection asynchronously.
    /// </summary>
    /// <param name="conn">The SqlConnection to connect to.</param>
    /// <returns>An exception if the connection fails, otherwise null.</returns>
    public static async Task<TryMethodResult> TryConnectAsync([DisallowNull] this SqlConnection conn, CancellationToken cancellationToken = default)
    {
        Check.MustBeArgumentNotNull(conn);

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

    private static TResult Execute<TResult>([DisallowNull] this SqlConnection connection,
        [DisallowNull] Func<SqlCommand, TResult> execute,
        [DisallowNull] string sql,
        Action<SqlParameterCollection>? fillParams = null)
    {
        Check.MustBeArgumentNotNull(connection);
        Check.MustBeArgumentNotNull(execute);
        Check.MustBeArgumentNotNull(sql);

        using var command = connection.CreateCommand(sql, fillParams);
        return connection.EnsureClosed(_ => execute(command), true);
    }

    private static async Task<TResult> ExecuteAsync<TResult>([DisallowNull] this SqlConnection connection,
            [DisallowNull] Func<SqlCommand, Task<TResult>> executeAsync,
            [DisallowNull] string sql,
            Action<SqlParameterCollection>? fillParams = null, CancellationToken cancellationToken = default)
    {
        Check.MustBeArgumentNotNull(connection);
        Check.MustBeArgumentNotNull(executeAsync);
        Check.MustBeArgumentNotNull(sql);

        await using var command = connection.CreateCommand(sql, fillParams);
        return await connection.EnsureClosedAsync(_ => executeAsync(command), true, cancellationToken: cancellationToken);
    }
}