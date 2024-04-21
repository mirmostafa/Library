using Library.Data.SqlServer;
using Library.Results;

namespace Library.Data.Ado;

public sealed class AdoGenericRepository : AdoRepositoryBase
{
    public AdoGenericRepository(Sql sql) : base(sql)
    {
    }

    public AdoGenericRepository(string connectionString) : base(connectionString)
    {
    }

    public Task<Result<int>> DeleteAsync<TEntity>(TEntity model, bool persist = true, CancellationToken token = default)
        => this.OnDeleteAsync(model, persist, token);

    public async Task<IReadOnlyList<TEntity>> GetAllAsync<TEntity>(CancellationToken token = default)
        where TEntity : new()
    {
        var result = await this.OnGetAll<TEntity>(token).ToListAsync(token);
        return result.AsReadOnly();
    }

    public Task<TEntity?> GetByIdAsync<TEntity>(long id, CancellationToken token = default)
        where TEntity : new()
    {
        ArgumentNullException.ThrowIfNull(id);
        return this.OnGetByIdAsync<TEntity>(id, token);
    }

    public Task<Result<TEntity>> InsertAsync<TEntity>(TEntity model, bool persist = true, CancellationToken token = default)
    {
        return OnInsertAsync(model, persist, token);
    }

    public Task<Result<TEntity>> UpdateAsync<TEntity>(long id, TEntity model, bool persist = true, CancellationToken token = default)
    {
        return OnUpdateAsync(id,model, persist, token);
    }
}