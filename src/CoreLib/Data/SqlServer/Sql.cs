using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

using Library.CodeGeneration;
using Library.Dynamic;
using Library.Interfaces;
using Library.Results;
using Library.Validations;

using Microsoft.Data.SqlClient;

namespace Library.Data.SqlServer;

public sealed class Sql(string connectionString, Action<string>? logTo = null) : INew<Sql, string>
{
    private readonly Action<string>? _logTo = logTo;

    public static object DefaultLogSender { get; } = nameof(Sql);
    public string ConnectionString { get; } = connectionString.ArgumentNotNull();

    public static async Task<bool> CanConnectAsync(string? connectionString, CancellationToken cancellationToken = default)
    {
        await using var conn = new SqlConnection(connectionString);
        return await conn.CanConnectAsync(cancellationToken: cancellationToken);
    }

    [DebuggerStepThrough, StackTraceHidden]
    public static (TypePath Type, string Name)? FindIdColumn<TEntity>()
        => FindIdColumn(typeof(TEntity));

    public static (TypePath Type, string Name)? FindIdColumn(in Type entityType)
    {
        Check.MustBeArgumentNotNull(entityType);
        var idColumn = Array.Find(entityType.GetProperties(), [DebuggerStepThrough, StackTraceHidden] (x) => x.Name.EqualsTo("Id"));
        return idColumn == null
            ? default
            : (idColumn.PropertyType, idColumn.Name);
    }

    public static (Func<string?> Schema, Func<string> Name, Func<IEnumerable<(string Name, TypePath Type)>> Columns, Func<(TypePath Type, string Name)?> IdColumn) GetTable<TType>()
        => GetTable(typeof(TType));

    public static (Func<string?> Schema, Func<string> Name, Func<IEnumerable<(string Name, TypePath Type)>> Columns, Func<(TypePath Type, string Name)?> IdColumn) GetTable(Type tableType)
    {
        Check.MustBeArgumentNotNull(tableType);

        string? schema()
        {
            var tableAttribute = tableType.GetCustomAttribute<TableAttribute>();
            return tableAttribute?.Schema;
        }
        string name()
        {
            var tableAttribute = tableType.GetCustomAttribute<TableAttribute>();
            return tableAttribute?.Name ?? tableType.Name;
        }
        IEnumerable<(string Name, TypePath Type)> columns()
            => tableType.GetProperties()
                .Where(x => x.GetCustomAttribute<NotMappedAttribute>() == null)
                .Select(x =>
                {
                    string name;
                    TypePath type;
                    int order;
                    var columnAttribute = x.GetCustomAttribute<ColumnAttribute>();
                    if (columnAttribute is { } attrib)
                    {
                        name = attrib.Name ?? x.Name;
                        type = attrib.TypeName ?? x!.DeclaringType!.FullName!;
                        order = attrib.Order;
                    }
                    else
                    {
                        name = x.Name;
                        type = x.PropertyType;
                        order = 0;
                    }
                    return (name, type, order);
                }).OrderBy(x => x.order).Select(x => (x.name, x.type));
        return (Schema: schema, Name: name, Columns: columns, IdColumn: () => FindIdColumn(tableType));
    }

    public static Sql New(string arg)
        => new(arg);

    /// <summary>
    /// Executes the provided converter function for each row in the IDataReader and returns an
    /// IEnumerable of the results. Throws an ArgumentNullException if the IDataReader is null.
    /// </summary>
    public static IEnumerable<T> Select<T>(IDataReader reader, Func<IDataReader, T> converter)
            where T : new()
            => reader is not null
                ? While(reader.Read, () => converter(reader))
                : throw new ArgumentNullException(nameof(reader));

    /// <summary>
    /// Extension method to select data from an IDataReader into an IEnumerable of type T.
    /// </summary>
    public static IEnumerable<T> Select<T>([DisallowNull] IDataReader reader)
        where T : new() => Select(reader, () => new T());

    /// <summary>
    /// Executes the specified reader and creates a collection of objects using the specified
    /// creator function.
    /// </summary>
    /// <typeparam name="T">The type of the objects to create.</typeparam>
    /// <param name="reader">The reader to execute.</param>
    /// <param name="creator">The function used to create objects.</param>
    /// <returns>A collection of objects created using the specified creator function.</returns>
    public static IEnumerable<T> Select<T>([DisallowNull] IDataReader reader, [DisallowNull] Func<T> creator)
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

                var property = Array.Find(properties, x => x.Name == columnName);
                _ = Catch(() => property!.SetValue(t, value, []));
            }
            yield return t;
        }
    }

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
        this._logTo?.Invoke(cmdText);
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

    public async Task<int> ExecuteNonQueryAsync(string sql, Action<SqlParameterCollection>? fillParams = null, CancellationToken cancellationToken = default)
        => await this.ExecuteTransactionalCommandAsync<int>(sql, cmd => cmd.ExecuteNonQueryAsync(), fillParams, cancellationToken);

    public IEnumerable<TResult> ExecuteReader<TResult>(string query, Func<SqlDataReader, TResult> mapper)
    {
        Check.MustBeArgumentNotNull(query);
        Check.MustBeArgumentNotNull(mapper);

        this._logTo?.Invoke(query);
        var sqlConnection = new SqlConnection(this.ConnectionString);
        using var reader = sqlConnection.ExecuteReader(query, behavior: CommandBehavior.CloseConnection);
        while (reader.Read())
        {
            yield return mapper(reader);
        }
    }

    public async IAsyncEnumerable<TResult> ExecuteReaderAsync<TResult>(string query, Func<SqlDataReader, TResult> mapper, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Check.MustBeArgumentNotNull(query);
        Check.MustBeArgumentNotNull(mapper);

        this._logTo?.Invoke(query);
        var sqlConnection = new SqlConnection(this.ConnectionString);
        await using var reader = await sqlConnection.ExecuteReaderAsync(query, behavior: CommandBehavior.CloseConnection, cancellationToken: cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            yield return mapper(reader);
        }
    }

    public object? ExecuteScalarCommand(string sql) =>
        this.ExecuteScalarCommand(sql, null);

    public object? ExecuteScalarCommand(string sql, Action<SqlParameterCollection>? fillParams)
    {
        object? result = null;
        this.ExecuteTransactionalCommand(sql, cmd => result = cmd.ExecuteScalar(), fillParams);
        return result;
    }

    public Task<object?> ExecuteScalarCommandAsync(string sql, CancellationToken cancellationToken = default)
        => this.ExecuteScalarCommandAsync(sql, null, cancellationToken);

    public async Task<object?> ExecuteScalarCommandAsync(string sql, Action<SqlParameterCollection>? fillParams, CancellationToken cancellationToken = default)
        => await this.ExecuteTransactionalCommandAsync(sql, cmd => cmd.ExecuteScalarAsync(), fillParams, cancellationToken);

    public object? ExecuteScalarQuery(string sql) =>
        this.ExecuteScalarQuery(sql, null);

    public object? ExecuteScalarQuery(string sql, Action<SqlParameterCollection>? fillParams)
    {
        object? result = null;
        this.ExecuteCommand(sql, cmd => result = cmd.ExecuteScalar(), fillParams);
        return result;
    }

    public object? ExecuteStoredProcedure(string spName, Action<SqlParameterCollection>? fillParams = null)
    {
        this._logTo?.Invoke(spName);
        return Execute(this.ConnectionString, conn => conn.ExecuteStoredProcedure(spName, fillParams));
    }

    public void ExecuteTransactionalCommand(string cmdText, Action<SqlCommand>? executor = null, Action<SqlParameterCollection>? fillParams = null)
    {
        using var connection = new SqlConnection(this.ConnectionString);
        connection.Open();
        var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
        using var command = new SqlCommand(cmdText.NotNull(), connection, transaction) { CommandTimeout = connection.ConnectionTimeout };
        fillParams?.Invoke(command.Parameters);
        this._logTo?.Invoke(cmdText);
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

    public async Task<TResult> ExecuteTransactionalCommandAsync<TResult>(string cmdText, Func<SqlCommand, Task<TResult>> executeAsync, Action<SqlParameterCollection>? fillParams = null, CancellationToken cancellationToken = default)
    {
        Check.MustBeArgumentNotNull(cmdText);
        Check.MustBeArgumentNotNull(executeAsync);

        await using var connection = new SqlConnection(this.ConnectionString);
        await connection.OpenAsync(cancellationToken);
        var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
        await using var command = new SqlCommand(cmdText.NotNull(), connection, (SqlTransaction)transaction) { CommandTimeout = connection.ConnectionTimeout };
        fillParams?.Invoke(command.Parameters);
        this._logTo?.Invoke(cmdText);
        try
        {
            var result = await executeAsync(command);
            await transaction.CommitAsync(cancellationToken);

            return result;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task ExecuteTransactionalCommandAsync(string cmdText, Func<SqlCommand, Task<int>>? executeAsync = null, Action<SqlParameterCollection>? fillParams = null, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(this.ConnectionString);
        await connection.OpenAsync(cancellationToken);
        var transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
        await using var command = new SqlCommand(cmdText.NotNull(), connection, (SqlTransaction)transaction) { CommandTimeout = connection.ConnectionTimeout };
        fillParams?.Invoke(command.Parameters);
        this._logTo?.Invoke(cmdText);
        try
        {
            int result;
            if (executeAsync != null)
            {
                result = await executeAsync(command);
            }
            else
            {
                result = await command.ExecuteNonQueryAsync(cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public T? FirstOrDefault<T>(string query)
        where T : new()
    {
        using var conn = new SqlConnection(this.ConnectionString);
        this._logTo?.Invoke(query);
        return Select<T>(conn.ExecuteReader(query, behavior: CommandBehavior.CloseConnection)).FirstOrDefault();
    }

    public IEnumerable<T> Select<T>(string query, Func<SqlDataReader, T> rowFiller)
    {
        using var conn = new SqlConnection(this.ConnectionString);
        this._logTo?.Invoke(query);
        return Select(conn, query, rowFiller).ToList();
    }

    public IEnumerable<T> Select<T>(string query, Func<IDataReader, T> convertor)
        where T : new()
    {
        using var conn = new SqlConnection(this.ConnectionString);
        this._logTo?.Invoke(query);
        return Select(conn.ExecuteReader(query, behavior: CommandBehavior.CloseConnection), convertor);
    }

    public IEnumerable<T> Select<T>(string query, Func<T> creator)
    {
        using var conn = new SqlConnection(this.ConnectionString);
        this._logTo?.Invoke(query);
        return Select(conn.ExecuteReader(query, behavior: CommandBehavior.CloseConnection), creator);
    }

    public IEnumerable<T> Select<T>(string query)
        where T : new()
    {
        using var conn = new SqlConnection(this.ConnectionString);
        this._logTo?.Invoke(query);
        return Select<T>(conn.ExecuteReader(query, behavior: CommandBehavior.CloseConnection)).ToList();
    }

    public IEnumerable<dynamic> Select(string query)
    {
        var columns = new List<string>();
        using var conn = new SqlConnection(this.ConnectionString);
        this._logTo?.Invoke(query);
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

    public IEnumerable<dynamic> Select(string query, Func<SqlDataReader, dynamic> rowFiller)
    {
        this._logTo?.Invoke(query);
        return Execute<IEnumerable<dynamic>>(this.ConnectionString, conn => Select(conn, query, rowFiller).ToList());
    }

    private static TResult Execute<TResult>(string connectionString, Func<SqlConnection, TResult> func)
    {
        using var conn = new SqlConnection(connectionString);
        return func(conn);
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
    private static IEnumerable<T> Select<T>([DisallowNull] SqlConnection connection,
        [DisallowNull] string sql,
        [DisallowNull] Func<SqlDataReader, T> rowFiller,
        Action<SqlParameterCollection>? fillParams = null)
    {
        Check.MustBeArgumentNotNull(connection);

        var reader = connection.ExecuteReader(sql, fillParams, CommandBehavior.CloseConnection);
        return While(reader.Read, () => rowFiller(reader), connection.Close);
    }
}