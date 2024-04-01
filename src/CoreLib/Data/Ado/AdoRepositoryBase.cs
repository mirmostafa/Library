using System.Runtime.CompilerServices;

using Library.Data.SqlServer;
using Library.Validations;

using Microsoft.Data.SqlClient;

using static Library.Data.SqlServer.SqlStatementBuilder;

namespace Library.Data.Ado;

public abstract class AdoRepositoryBase
{
    protected AdoRepositoryBase(in Sql sql)
        => this.Sql = sql;

    protected AdoRepositoryBase(in string connectionString)
        : this(Sql.New(connectionString)) { }

    protected Sql Sql { get; }

    [return: NotNull]
    protected virtual async IAsyncEnumerable<TEntity> OnGetAll<TEntity>([DisallowNull] Func<SqlDataReader, TEntity> mapper, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var query = Select<TEntity>().WithNoLock().Build();
        await foreach (var entity in this.InnerGetAll(query, mapper.ArgumentNotNull(), cancellationToken))
        {
            yield return entity;
        }
    }

    [return: NotNull]
    protected virtual async IAsyncEnumerable<TEntity> OnGetAll<TEntity>([DisallowNull] string query, [DisallowNull] Func<SqlDataReader, TEntity> mapper, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var entity in this.InnerGetAll(query.ArgumentNotNull(), mapper.ArgumentNotNull(), cancellationToken))
        {
            yield return entity;
        }
    }

    [return: NotNull]
    protected virtual async IAsyncEnumerable<TEntity> OnGetAll<TEntity>([DisallowNull] string query, [EnumeratorCancellation] CancellationToken cancellationToken = default) where TEntity : new()
    {
        await foreach (var entity in this.InnerGetAll(query.ArgumentNotNull(), r => Mapper<TEntity>(r, typeof(TEntity).GetProperties()), cancellationToken))
        {
            yield return entity;
        }
    }

    [return: NotNull]
    protected virtual async IAsyncEnumerable<TEntity> OnGetAll<TEntity>([EnumeratorCancellation] CancellationToken cancellationToken = default)
        where TEntity : new()
    {
        var query = Select<TEntity>().WithNoLock().Build();
        await foreach (var entity in this.InnerGetAll(query, r => Mapper<TEntity>(r, typeof(TEntity).GetProperties()), cancellationToken))
        {
            yield return entity;
        }
    }

    protected virtual Task<TEntity?> OnGetByIdAsync<TEntity>(object idValue, [DisallowNull] Func<SqlDataReader, TEntity> mapper, CancellationToken cancellationToken = default)
    {
        var query = Select<TEntity>().Top(1).Where($"{Sql.FindIdColumn<TEntity>()} = {idValue}").WithNoLock().Build();
        return this.InnerGetAll(query, mapper.ArgumentNotNull(), cancellationToken).FirstOrDefaultAsync();
    }

    protected virtual Task<TEntity?> OnGetByIdAsync<TEntity>(object idValue, CancellationToken cancellationToken = default)
        where TEntity : new()
    {
        var query = Select<TEntity>().Top(1).Where($"{Sql.FindIdColumn<TEntity>()} = {idValue}").WithNoLock().Build();
        return this.InnerGetAll(query, r => Mapper<TEntity>(r, typeof(TEntity).GetProperties()), cancellationToken).FirstOrDefaultAsync();
    }

    private static TEntity Mapper<TEntity>(in SqlDataReader reader, in System.Reflection.PropertyInfo[] properties)
        where TEntity : new()
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
        await using var reader = await this.Sql.ExecuteReaderAsync(query, cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return mapper(reader);
        }
    }
}