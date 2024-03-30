using System.Runtime.CompilerServices;

using Library.Validations;

using Microsoft.Data.SqlClient;

using static Library.Data.SqlServer.SqlStatementBuilder;

namespace Library.Data.SqlServer;

public class AdoRepositoryHelper(in Sql sql)
{
    private readonly Sql _sql = sql;

    public AdoRepositoryHelper(in string connectionString)
        : this(Sql.New(connectionString)) { }

    [return: NotNull]
    public async IAsyncEnumerable<TEntity> GetAll<TEntity>([EnumeratorCancellation] CancellationToken cancellationToken = default) where TEntity : new()
    {
        var query = Select<TEntity>().WithNoLock().Build();
        await foreach (var entity in this.InnerGetAll(query, r => Mapper<TEntity>(r, typeof(TEntity).GetProperties()), cancellationToken))
        {
            yield return entity;
        }
    }

    [return: NotNull]
    public async IAsyncEnumerable<TEntity> GetAll<TEntity>([DisallowNull] Func<SqlDataReader, TEntity> mapper, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var query = Select<TEntity>().WithNoLock().Build();
        await foreach (var entity in this.InnerGetAll(query, mapper.ArgumentNotNull(), cancellationToken))
        {
            yield return entity;
        }
    }

    [return: NotNull]
    public async IAsyncEnumerable<TEntity> GetAll<TEntity>([DisallowNull] string query, [DisallowNull] Func<SqlDataReader, TEntity> mapper, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var entity in this.InnerGetAll(query.ArgumentNotNull(), mapper.ArgumentNotNull(), cancellationToken))
        {
            yield return entity;
        }
    }

    [return: NotNull]
    public async IAsyncEnumerable<TEntity> GetAll<TEntity>([DisallowNull] string query, [EnumeratorCancellation] CancellationToken cancellationToken = default) where TEntity : new()
    {
        await foreach (var entity in this.InnerGetAll(query.ArgumentNotNull(), r => Mapper<TEntity>(r, typeof(TEntity).GetProperties()), cancellationToken))
        {
            yield return entity;
        }
    }

    public Task<TEntity?> GetFirstOrDefaultAsync<TEntity>([DisallowNull] Func<SqlDataReader, TEntity> mapper, CancellationToken cancellationToken = default)
    {
        var query = Select<TEntity>().Top(1).WithNoLock().Build();
        return this.InnerGetAll(query, mapper.ArgumentNotNull(), cancellationToken).FirstOrDefaultAsync();
    }

    public Task<TEntity?> GetFirstOrDefaultAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : new()
    {
        var query = Select<TEntity>().Top(1).WithNoLock().Build();
        return this.InnerGetAll(query, r => Mapper<TEntity>(r, typeof(TEntity).GetProperties()), cancellationToken).FirstOrDefaultAsync();
    }

    public Task<TEntity?> GetFirstOrDefaultAsync<TEntity>([DisallowNull] string query, [DisallowNull] Func<SqlDataReader, TEntity> mapper, CancellationToken cancellationToken = default)
        => this.InnerGetAll(query.ArgumentNotNull(), mapper.ArgumentNotNull(), cancellationToken).FirstOrDefaultAsync();

    public Task<TEntity?> GetFirstOrDefaultAsync<TEntity>([DisallowNull] string query, CancellationToken cancellationToken = default) where TEntity : new()
        => this.InnerGetAll(query.ArgumentNotNull(), r => Mapper<TEntity>(r, typeof(TEntity).GetProperties()), cancellationToken).FirstOrDefaultAsync();

    private static TEntity Mapper<TEntity>(in SqlDataReader reader, in System.Reflection.PropertyInfo[] properties) where TEntity : new()
    {
        var result = new TEntity();
        foreach (var property in properties)
        {
            property.SetValue(reader[property.Name], result);
        }
        return result;
    }

    [return: NotNull]
    private async IAsyncEnumerable<TEntity> InnerGetAll<TEntity>([DisallowNull] string query, [DisallowNull] Func<SqlDataReader, TEntity> mapper, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var reader = await this._sql.ExecuteReaderAsync(query, cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return mapper(reader);
        }
    }
}