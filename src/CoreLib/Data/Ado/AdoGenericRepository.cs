using Library.Data.SqlServer;
using Library.Interfaces;
using Library.Results;
using Library.Types;
using Library.Validations;

namespace Library.Data.Ado;

public sealed class AdoGenericRepository<TEntity, TId> : AdoRepositoryBase, IAsyncCrud<TEntity, TId>
    where TEntity : new()
{
    public AdoGenericRepository(in Sql sql) : base(sql)
    {
    }

    public AdoGenericRepository(in string connectionString) : base(connectionString)
    {
    }

    public Task<Result> DeleteAsync(TEntity model, bool persist = true, CancellationToken token = default)
    {
        var idColumn = Sql.FindIdColumn<TEntity>();
        Check.MustBeArgumentNotNull(idColumn);
        this.Sql.ExecuteNonQuery("")
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken token = default)
    {
        var result = await this.OnGetAll<TEntity>(token).ToListAsync(token);
        return result.AsReadOnly();
    }

    public Task<TEntity?> GetByIdAsync(TId id, CancellationToken token = default)
    {
        Check.MustBeArgumentNotNull(id);
        return this.OnGetByIdAsync<TEntity>(id, token);
    }

    public Task<Result<TEntity>> InsertAsync(TEntity model, bool persist = true, CancellationToken token = default) => throw new NotImplementedException();

    public Task<Result<TEntity>> UpdateAsync(TId id, TEntity model, bool persist = true, CancellationToken token = default) => throw new NotImplementedException();
}

public sealed class AdoGenericRepository<TEntity> : AdoRepositoryBase, IAsyncCrud<TEntity>
    where TEntity : new()
{
    public AdoGenericRepository(in Sql sql) : base(sql)
    {
    }

    public AdoGenericRepository(in string connectionString) : base(connectionString)
    {
    }

    public Task<Result> DeleteAsync(TEntity model, bool persist = true, CancellationToken token = default) => throw new NotImplementedException();

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken token = default)
    {
        var result = await this.OnGetAll<TEntity>(token).ToListAsync(token);
        return result.AsReadOnly();
    }

    public Task<TEntity?> GetByIdAsync(long id, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        return this.OnGetByIdAsync<TEntity>(id, token);
    }

    public Task<Result<TEntity>> InsertAsync(TEntity model, bool persist = true, CancellationToken token = default) => throw new NotImplementedException();

    public Task<Result<TEntity>> UpdateAsync(long id, TEntity model, bool persist = true, CancellationToken token = default) => throw new NotImplementedException();
}