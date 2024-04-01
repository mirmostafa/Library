using System.Data;

using Library.CodeGeneration;
using Library.Dynamic;
using Library.Interfaces;
using Library.Results;
using Library.Validations;

using Microsoft.Data.SqlClient;

namespace Library.Data.SqlServer;

public sealed class Sql(string connectionString) : INew<Sql, string>
{
    public static object DefaultLogSender { get; } = nameof(Sql);
    public string ConnectionString { get; } = connectionString.ArgumentNotNull();

    public static async Task<bool> CanConnectAsync(string? connectionString, CancellationToken cancellationToken = default)
    {
        await using var conn = new SqlConnection(connectionString);
        return await conn.CanConnectAsync(cancellationToken: cancellationToken);
    }

    public static (TypePath Type, string Name)? FindIdColumn<TEntity>()
    {
        var idColumn = Array.Find(typeof(TEntity).GetProperties(), x => x.Name.EqualsTo("Id"));
        return idColumn == null
            ? default
            : (idColumn.PropertyType, idColumn.Name);
    }

    public static Sql New(string arg)
        => new(arg);

    public static async Task<TryMethodResult> TryConnectAsync(string? connectionString, CancellationToken cancellationToken = default)
    {
        await using var conn = new SqlConnection(connectionString);
        return await conn.TryConnectAsync(cancellationToken: cancellationToken);
    }

    public void ExecuteCommand(string cmdText, Action<SqlCommand>? executor = null, Action<SqlParameterCollection>? fillParams = null)
    {
        using var connection = new SqlConnection(this.ConnectionString);
        connection.Open();
        using var command = new SqlCommand(cmdText.NotNull(), connection) { CommandTimeout = connection.ConnectionTimeout };
        fillParams?.Invoke(command.Parameters);
        if (executor != null)
        {
            executor(command);
        }
        else
        {
            _ = command.ExecuteNonQuery();
        }
    }

    public int ExecuteNonQuery(string sql, Action<SqlParameterCollection>? fillParams = null)
    {
        var result = 0;
        this.ExecuteTransactionalCommand(sql, cmd => result = cmd.ExecuteNonQuery(), fillParams);
        return result;
    }

    public SqlDataReader ExecuteReader(string query) =>
        new SqlConnection(this.ConnectionString).ExecuteReader(query, behavior: CommandBehavior.CloseConnection);

    public Task<SqlDataReader> ExecuteReaderAsync(string query, CancellationToken cancellationToken = default) =>
        new SqlConnection(this.ConnectionString).ExecuteReaderAsync(query, behavior: CommandBehavior.CloseConnection, cancellationToken: cancellationToken);

    public object? ExecuteScalarCommand(string sql) =>
        this.ExecuteScalarCommand(sql, null);

    public object? ExecuteScalarCommand(string sql, Action<SqlParameterCollection>? fillParams)
    {
        object? result = null;
        this.ExecuteTransactionalCommand(sql, cmd => result = cmd.ExecuteScalar(), fillParams);
        return result;
    }

    public object? ExecuteScalarQuery(string sql) =>
        this.ExecuteScalarQuery(sql, null);

    public object? ExecuteScalarQuery(string sql, Action<SqlParameterCollection>? fillParams)
    {
        object? result = null;
        this.ExecuteCommand(sql, cmd => result = cmd.ExecuteScalar(), fillParams);
        return result;
    }

    public object? ExecuteStoredProcedure(string spName, Action<SqlParameterCollection>? fillParams = null) =>
        Execute(this.ConnectionString, conn => conn.ExecuteStoredProcedure(spName, fillParams));

    public void ExecuteTransactionalCommand(string cmdText, Action<SqlCommand>? executor = null, Action<SqlParameterCollection>? fillParams = null)
    {
        using var connection = new SqlConnection(this.ConnectionString);
        connection.Open();
        var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
        using var command = new SqlCommand(cmdText.NotNull(), connection, transaction) { CommandTimeout = connection.ConnectionTimeout };
        fillParams?.Invoke(command.Parameters);
        try
        {
            if (executor != null)
            {
                executor(command);
            }
            else
            {
                _ = command.ExecuteNonQuery();
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public DataSet FillDataSet(string query) =>
        Execute(this.ConnectionString, conn => conn.FillDataSet(query));

    public DataSet FillDataSetByTableNames(params string[] tableNames)
    {
        Check.MustBeArgumentNotNull(tableNames);

        var result = this.FillDataSet(tableNames.Select(t => SqlStatementBuilder.CreateSelect(t)).Merge(Environment.NewLine));
        for (var i = 0; i < tableNames.Length; i++)
        {
            result.Tables[i].TableName = tableNames[i];
        }

        return result;
    }

    public DataTable FillDataTable(string query) =>
        Execute(this.ConnectionString, conn => conn.FillDataTable(query));

    public IEnumerable<DataTable> FillDataTables(params string[] queries)
    {
        Check.MustBeArgumentNotNull(queries);

        using var connection = new SqlConnection(this.ConnectionString);
        using var cmd = connection.CreateCommand();
        using var da = new SqlDataAdapter(cmd);

        connection.Open();
        foreach (var query in queries)
        {
            cmd.CommandText = query;
            var dataTable = new DataTable();
            _ = da.Fill(dataTable);
            yield return dataTable;
        }
    }

    public T? FirstOrDefault<T>(string query)
        where T : new()
    {
        using var conn = new SqlConnection(this.ConnectionString);
        return conn.ExecuteReader(query, behavior: CommandBehavior.CloseConnection).Select<T>().FirstOrDefault();
    }

    public SqlCommand GetCommand(string query)
    {
        var connection = new SqlConnection(this.ConnectionString);
        var result = connection.CreateCommand(query);
        result.Disposed += delegate
        {
            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }

            connection.Dispose();
        };
        return result;
    }

    public async Task<DataTable> LoadDataTableAsync(string query, Action<SqlParameterCollection>? fillParams = null, CancellationToken cancellationToken = default)
    {
        var result = new DataTable();
        using var command = this.GetCommand(query);
        fillParams?.Invoke(command.Parameters);
        await command.Connection.OpenAsync(cancellationToken);
        result.Load(await command.ExecuteReaderAsync(cancellationToken));
        return result;
    }

    public DataTable LoadFillDataTable(string query, Action<SqlParameterCollection>? fillParams = null)
    {
        var result = new DataTable();
        using var command = this.GetCommand(query);
        fillParams?.Invoke(command.Parameters);
        command.Connection.Open();
        result.Load(command.ExecuteReader());
        command.Connection.Close();

        return result;
    }

    public IEnumerable<T> Select<T>(string query, Func<SqlDataReader, T> rowFiller)
    {
        using var conn = new SqlConnection(this.ConnectionString);
        return conn.Select(query, rowFiller).ToList();
    }

    public IEnumerable<T> Select<T>(string query, Func<IDataReader, T> convertor)
        where T : new()
    {
        using var conn = new SqlConnection(this.ConnectionString);
        return conn.ExecuteReader(query, behavior: CommandBehavior.CloseConnection).Select(convertor);
    }

    public IEnumerable<T> Select<T>(string query, Func<T> creator)
    {
        using var conn = new SqlConnection(this.ConnectionString);
        return conn.ExecuteReader(query, behavior: CommandBehavior.CloseConnection).Select(creator);
    }

    public IEnumerable<T> Select<T>(string query)
        where T : new()
    {
        using var conn = new SqlConnection(this.ConnectionString);
        return conn.ExecuteReader(query, behavior: CommandBehavior.CloseConnection).Select<T>().ToList();
    }

    public IEnumerable<dynamic> Select(string query)
    {
        var columns = new List<string>();
        using var conn = new SqlConnection(this.ConnectionString);
        var reader = conn.ExecuteReader(query, behavior: CommandBehavior.CloseConnection);
        for (var i = 0; i < reader.FieldCount; i++)
        {
            columns.Add(reader.GetName(i));
        }

        while (reader.Read())
        {
            var result = new Expando();
            foreach (var column in columns)
            {
                result[column] = reader[column];
            }

            yield return result;
        }
    }

    public IEnumerable<dynamic> Select(string query, Func<SqlDataReader, dynamic> rowFiller) =>
        Execute<IEnumerable<dynamic>>(this.ConnectionString, conn => conn.Select(query, rowFiller).ToList());

    private static TResult Execute<TResult>(string connectionString, Func<SqlConnection, TResult> func)
    {
        using var conn = new SqlConnection(connectionString);
        return func(conn);
    }
}