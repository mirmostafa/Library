using Library.Data.Markers;
using Library.Interfaces;
using Library.Types;
using Library.Validations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Library.Helpers;

public static class ServiceHelper
{
    public static async Task<TViewModel> SaveViewModelAsync<TViewModel>(this IAsyncWriteService<TViewModel> service!!, TViewModel model, bool persist = true)
        where TViewModel : ICanSetKey<long?>
        => model.ArgumentNotNull().Id is { } id and > 0
            ? await service.UpdateAsync(id, model, persist)
            : await service.InsertAsync(model, persist);

    public static async Task<TViewModel> SaveViewModelAsync<TViewModel>(this IAsyncWriteService<TViewModel, Guid> service!!, TViewModel model, bool persist = true)
        where TViewModel : ICanSetKey<Guid>
        => !model.ArgumentNotNull().Id.IsNullOrEmpty()
            ? await service.UpdateAsync(model.Id, model, persist)
            : await service.InsertAsync(model, persist);

    public static async Task<TViewModel> SaveViewModelAsync<TViewModel>(this IAsyncWriteService<TViewModel, Id> service!!, TViewModel model, bool persist = true)
        where TViewModel : ICanSetKey<Id>
        => !model.ArgumentNotNull().Id.IsNullOrEmpty()
            ? await service.UpdateAsync(model.Id, model, persist)
            : await service.InsertAsync(model, persist);

    public static async Task<int> SubmitChangesAsync<TService>(this TService service, bool persist = true, IDbContextTransaction? transaction = null)
        where TService : IAsyncSaveService, IResetChanges
    {
        Check.IfArgumentNotNull(service);

        if (!persist)
        {
            return -1;
        }
        if (transaction is not null)
        {
            await transaction.CommitAsync();
        }

        var result = await service.SaveChangesAsync();
        service.ResetChanges();
        return result;
    }

    public static async Task<IReadOnlySet<TEntity>> GetAllEntities<TEntity>(IQueryable<TEntity> entities)
    {
        var query = from entity in entities
                    select entity;
        var dbResult = await query.ToListAsync();
        return dbResult.ToReadOnlySet();
    }

    public static async Task<IReadOnlyList<TEntity>> GetAllEntities<TEntity>(Func<IQueryable<TEntity>> getEntities)
    {
        var query = from entity in getEntities()
                    select entity;
        var dbResult = await query.ToListAsync();
        return dbResult.ToReadOnlyList();
    }

    public static async Task<IReadOnlyList<TViewModel>> GetAllViewModel<TEntity, TViewModel>(IQueryable<TEntity> entities, Func<TEntity, TViewModel> convert)
    {
        var dbEntities = await GetAllEntities(entities);
        var result = dbEntities.Select(convert);
        return result.ToReadOnlyList();
    }

    public static async Task<IReadOnlyList<TViewModel>> GetAllViewModel<TEntity, TViewModel>(Func<IQueryable<TEntity>> getEntities, Func<TEntity, TViewModel> convert)
        => await GetAllViewModel(getEntities(), convert);

    public static async Task<TEntity?> GetEntityById<TId, TEntity>(TId id, IQueryable<TEntity> entities)
        where TEntity : IHasKey<TId>
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var query = from entity in entities
                    where entity.Id.Equals(id)
                    select entity;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        var result = await query.FirstOrDefaultAsync();
        return result;
    }

    public static async Task<TViewModel?> GetViewModelById<TId, TEntity, TViewModel>(TId id, IQueryable<TEntity> entities, Func<TEntity?, TViewModel?> convert)
        where TEntity : IHasKey<TId>
    {
        var entity = await GetEntityById(id, entities);
        var result = convert(entity);
        return result;
    }
}