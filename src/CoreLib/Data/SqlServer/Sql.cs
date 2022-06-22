using System.Data;
using System.Data.SqlClient;
using Library.Dynamic;
using Library.Validations;

namespace Library.Data.SqlServer;
public sealed class Sql
{

    public Sql(string connectionString) =>
        this.ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

    public string ConnectionString { get; }

    public object DefaultLogSender { get; } = "Sql";

    public int ExecuteNonQuery(string sql, Action<SqlParameterCollection>? fillParams = null)
    {
        var result = 0;
        this.TransactionalCommand(sql, cmd => result = cmd.ExecuteNonQuery(), fillParams);
        return result;
    }

    public SqlDataReader ExecuteReader(string query) =>
        new SqlConnection(this.ConnectionString).ExecuteReader(query, behavior: CommandBehavior.CloseConnection);

    public object? ExecuteScalar(string sql) =>
        this.ExecuteScalar(sql, null);

    public object? ExecuteScalar(string sql, Action<SqlParameterCollection>? fillParams)
    {
        object? result = null;
        this.TransactionalCommand(sql, cmd => result = cmd.ExecuteScalar(), fillParams);
        return result;
    }

    public DataSet FillByTableNames(params string[] tableNames)
    {
        var result = this.FillDataSet(tableNames.Select(t => SqlStatementBuilder.CreateSelect(t)).Merge(Environment.NewLine));
        for (var i = 0; i < tableNames.Length; i++)
        {
            result.Tables[i].TableName = tableNames[i];
        }

        return result;
    }

    private static TResult Execute<TResult>(string connectionString, Func<SqlConnection, TResult> func)
    {
        using var conn = new SqlConnection(connectionString);
        return func(conn);
    }

    public object? ExecuteStoredProcedure(string spName, Action<SqlParameterCollection>? fillParams = null) =>
        Execute(this.ConnectionString, conn => conn.ExecuteStoredProcedure(spName, fillParams));

    public DataSet FillDataSet(string query) =>
        Execute(this.ConnectionString, conn => conn.FillDataSet(query));

    public DataTable FillDataTable(string query) =>
        Execute(this.ConnectionString, conn => conn.FillDataTable(query));

    public DataTable FillDataTable(string query, Action<SqlParameterCollection>? fillParams = null)
    {
        var result = new DataTable();
        using var command = this.GetCommand(query);
        fillParams?.Invoke(command.Parameters);
        command.Connection.Open();
        result.Load(command.ExecuteReader());
        command.Connection.Close();

        return result;
    }

    public async Task<DataTable> FillDataTableAsync(string query, Action<SqlParameterCollection>? fillParams = null)
    {
        var result = new DataTable();
        using var command = this.GetCommand(query);
        fillParams?.Invoke(command.Parameters);
        await command.Connection.OpenAsync();
        result.Load(await command.ExecuteReaderAsync());
        return result;
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
        return conn.ExecuteReader(query, behavior: CommandBehavior.CloseConnection).Select<T>();
    }

    public IEnumerable<dynamic> Select(string query)
    {

        using var conn = new SqlConnection(this.ConnectionString);
        var reader = conn.ExecuteReader(query, behavior: CommandBehavior.CloseConnection);
        var columns = new List<string>();
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

    public IEnumerable<dynamic> Select(string query, Func<SqlDataReader, dynamic> rowFiller)
    {

        using var conn = new SqlConnection(this.ConnectionString);
        return conn.Select(query, rowFiller).ToList();
    }

    public void TransactionalCommand(string cmdText, Action<SqlCommand>? executor = null, Action<SqlParameterCollection>? fillParams = null)
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
}