﻿using System.Runtime.CompilerServices;

using Microsoft.Data.SqlClient;

using static Library.Data.SqlServer.SqlStatementBuilder;

namespace Library.Data.SqlServer;

public class AdoRepositoryHelper(string connectionString)
{
    private readonly Sql _sql = Sql.New(connectionString);

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
        await foreach (var entity in this.InnerGetAll(query, mapper, cancellationToken))
        {
            yield return entity;
        }
    }

    [return: NotNull]
    public async IAsyncEnumerable<TEntity> GetAll<TEntity>([DisallowNull] string query, [DisallowNull] Func<SqlDataReader, TEntity> mapper, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var entity in this.InnerGetAll(query, mapper, cancellationToken))
        {
            yield return entity;
        }
    }

    [return: NotNull]
    public async IAsyncEnumerable<TEntity> GetAll<TEntity>([DisallowNull] string query, [EnumeratorCancellation] CancellationToken cancellationToken = default) where TEntity : new()
    {
        await foreach (var entity in this.InnerGetAll(query, r => Mapper<TEntity>(r, typeof(TEntity).GetProperties()), cancellationToken))
        {
            yield return entity;
        }
    }

    public Task<TEntity?> GetFirstOrDefaultAsync<TEntity>([DisallowNull] Func<SqlDataReader, TEntity> mapper, CancellationToken cancellationToken = default)
    {
        var query = Select<TEntity>().Top(1).WithNoLock().Build();
        return this.InnerGetAll(query, mapper, cancellationToken).FirstOrDefaultAsync();
    }

    public Task<TEntity?> GetFirstOrDefaultAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : new()
    {
        var query = Select<TEntity>().Top(1).WithNoLock().Build();
        return this.InnerGetAll(query, r => Mapper<TEntity>(r, typeof(TEntity).GetProperties()), cancellationToken).FirstOrDefaultAsync();
    }

    public Task<TEntity?> GetFirstOrDefaultAsync<TEntity>([DisallowNull] string query, [DisallowNull] Func<SqlDataReader, TEntity> mapper, CancellationToken cancellationToken = default)
        => this.InnerGetAll(query, mapper, cancellationToken).FirstOrDefaultAsync();

    public Task<TEntity?> GetFirstOrDefaultAsync<TEntity>([DisallowNull] string query, CancellationToken cancellationToken = default) where TEntity : new()
        => this.InnerGetAll(query, r => Mapper<TEntity>(r, typeof(TEntity).GetProperties()), cancellationToken).FirstOrDefaultAsync();

    private static TEntity Mapper<TEntity>(SqlDataReader reader, System.Reflection.PropertyInfo[] properties) where TEntity : new()
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
        Checker.MustBeArgumentNotNull(query);
        Checker.MustBeArgumentNotNull(mapper);

        using var reader = await this._sql.ExecuteReaderAsync(query, cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return mapper(reader);
        }
    }
}

public sealed class AdoRepositoryHelperOptions{
}