using Library.Data.Markers;
using Library.Interfaces;
using Library.Logging;
using Library.Results;
using Library.Threading;
using Library.Types;
using Library.Validations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Helpers;

public static class ServiceHelper
{
    public static async Task<Result> DeleteAsync<TViewModel, TDbEntity>([DisallowNull] this IService service, [DisallowNull] DbContext dbContext, TViewModel model, bool persist, bool detach, ILogger? logger = null)
        where TDbEntity : class, IIdenticalEntity<long>, new()
        where TViewModel : IHasKey<long?>
    {
        Check.IfArgumentNotNull(model?.Id);
        Check.IfArgumentNotNull(dbContext);

        var id = (long)(model?.Id.Value)!;
        logger?.Debug($"Deleting {nameof(TViewModel)}, id={id}");
        _ = dbContext.RemoveById<TDbEntity>(id);
        if (persist)
        {
            _ = await dbContext.SaveChangesAsync();
        }
        if (detach)
        {
            var entity = dbContext.Set<TDbEntity>().Where(e => id == e.Id).First();
            _ = DbContextHelper.Detach(dbContext, entity);
        }
        logger?.Info($"Deleted {nameof(TViewModel)}, id={id}");
        return Result.Success;
    }

    public static async Task<IReadOnlyList<TViewModel>> GetAllAsync<TViewModel, TDbEntity>([DisallowNull] this IService service, [DisallowNull] DbContext dbContext, Func<IEnumerable<TDbEntity?>, IEnumerable<TViewModel?>> toViewModel, AsyncLock asyncLock)
        where TDbEntity : class
        where TViewModel : class
    {
        var query = from entity in dbContext.Set<TDbEntity>()
                    select entity;
        var dbResult = await query.ToListLockAsync(asyncLock);
        var result = toViewModel(dbResult).Compact().ToReadOnlyList();
        return result;
    }

    public static async Task<TViewModel?> GetByIdAsync<TViewModel, TDbEntity>([DisallowNull] this IService service, long id, [DisallowNull] DbContext dbContext, Func<TDbEntity?, TViewModel?> toViewModel, AsyncLock asyncLock)
        where TDbEntity : class, IIdenticalEntity<long>
        where TViewModel : class
    {
        var query = from entity in dbContext.Set<TDbEntity>()
                    where entity.Id == id
                    select entity;
        var dbResult = await query.FirstOrDefaultLockAsync(asyncLock);
        var result = toViewModel(dbResult);
        return result;
    }

    public static async Task<TViewModel?> GetByIdAsync<TViewModel, TDbEntity>([DisallowNull] this IService service, long id, IQueryable<TDbEntity> entities, Func<TDbEntity?, TViewModel?> toViewModel)
        where TDbEntity : IHasKey<long>
    {
        var query = from entity in entities
                    where entity.Id == id
                    select entity;
        var dbResult = await query.FirstOrDefaultAsync();
        return toViewModel(dbResult);
    }

    public static Task<Result<TDbEntity?>> InsertAsync<TViewModel, TDbEntity>([DisallowNull] this IService service, [DisallowNull] DbContext dbContext, [DisallowNull] TViewModel model, [DisallowNull] Func<TViewModel, TDbEntity?> convert, Func<TViewModel, Task<Result<TViewModel>>>? validatorAsync = null, Func<TDbEntity, TDbEntity>? onCommitting = null, bool persist = true, Func<Task<Result<int>>>? saveChanges = null, ILogger? logger = null)
        where TDbEntity : class, IIdenticalEntity<long>
        => InnerManipulate<TViewModel, TDbEntity, long>(dbContext, model, dbContext.NotNull().Add, convert, validatorAsync, onCommitting, persist, (true, null), saveChanges, logger: logger);

    public static Task<Result<TDbEntity?>> InsertAsync<TViewModel, TDbEntity, TId>([DisallowNull] this IService service, [DisallowNull] DbContext dbContext, [DisallowNull] TViewModel model, [DisallowNull] Func<TViewModel, TDbEntity?> convert, Func<TViewModel, Task<Result<TViewModel>>>? validatorAsync = null, Func<TDbEntity, TDbEntity>? onCommitting = null, bool persist = true, Func<Task<Result<int>>>? saveChanges = null, ILogger? logger = null)
        where TDbEntity : class, IIdenticalEntity<TId>
        where TId : notnull
        => InnerManipulate<TViewModel, TDbEntity, TId>(dbContext, model, dbContext.Add, convert, validatorAsync, onCommitting, persist, (true, null), saveChanges, logger: logger);

    public static Task<Result<TDbEntity?>> InsertAsync<TService, TViewModel, TDbEntity>([DisallowNull] this TService service, [DisallowNull] DbContext dbContext, [DisallowNull] TViewModel model, [DisallowNull] Func<TViewModel, TDbEntity?> convert, Func<TDbEntity, TDbEntity>? onCommitting = null, bool persist = true, Func<Task<Result<int>>>? saveChanges = null, ILogger? logger = null)
        where TDbEntity : class, IIdenticalEntity<long>
        where TService : IAsyncValidator<TViewModel>
        => InnerManipulate<TViewModel, TDbEntity, long>(dbContext, model, dbContext.Add, convert, service.ValidateAsync, onCommitting, persist, (true, null), saveChanges, logger);

    public static Task<Result<TDbEntity?>> InsertAsync<TService, TViewModel, TDbEntity>([DisallowNull] this TService service, [DisallowNull] DbContext dbContext, [DisallowNull] TViewModel model, [DisallowNull] Func<TViewModel, TDbEntity?> convert, Func<TDbEntity, TDbEntity>? onCommitting = null, bool persist = true, ILogger? logger = null)
        where TDbEntity : class, IIdenticalEntity<long>
        where TService : IAsyncValidator<TViewModel>, IAsyncSaveService
        => InnerManipulate<TViewModel, TDbEntity, long>(dbContext, model, dbContext.Add, convert, service.ValidateAsync, onCommitting, persist, (true, null), service.SaveChangesAsync, logger);

    public static Task<Result<TDbEntity?>> InsertAsync<TService, TViewModel, TDbEntity>([DisallowNull] this TService service, [DisallowNull] DbContext dbContext, [DisallowNull] TViewModel model, [DisallowNull] Func<TViewModel, TDbEntity?> convert, Func<TDbEntity, TDbEntity>? onCommitting = null, bool persist = true)
        where TDbEntity : class, IIdenticalEntity<long>
        where TService : IAsyncValidator<TViewModel>, IAsyncSaveService, ILoggerContainer
        => InnerManipulate<TViewModel, TDbEntity, long>(dbContext, model, dbContext.Add, convert, service.ValidateAsync, onCommitting, persist, (true, null), service.SaveChangesAsync, service.Logger);

    public static ServiceCollection RegisterServices<TType>(this ServiceCollection services)
    {
        // typeof(TType).Assembly.GetTypes()[80].GetInterfaces()[1].GetInterfaces().Contains(typeof(IService))
        var srvs = typeof(TType).Assembly.GetTypes().Where(t => t.IsClass && t.GetInterfaces().Contains(typeof(IService)));
        foreach (var srv in srvs)
        {
            var infcs = srv.GetInterfaces().Where(t => t.GetInterfaces().Contains(typeof(IService)));
            foreach (var infc in infcs)
            {
                _ = services.AddScoped(infc, srv);
            }
        }
        return services;
    }

    public static Task<Result<TViewModel>> SaveViewModelAsync<TViewModel>(this IAsyncWriteService<TViewModel> service, TViewModel model, bool persist = true)
            where TViewModel : ICanSetKey<long?>
        => model.ArgumentNotNull().Id is { } id and > 0
            ? service.UpdateAsync(id, model, persist)
            : service.InsertAsync(model, persist);

    public static Task<Result<TViewModel>> SaveViewModelAsync<TViewModel>(this IAsyncWriteService<TViewModel, Id> service, TViewModel model, bool persist = true)
        where TViewModel : ICanSetKey<Id>
        => !model.ArgumentNotNull().Id.IsNullOrEmpty()
            ? service.UpdateAsync(model.Id, model, persist)
            : service.InsertAsync(model, persist);

    public static async Task<Result<int>> SubmitChangesAsync<TService>(this TService service, bool persist = true, IDbContextTransaction? transaction = null)
        where TService : IAsyncSaveService, IResetChanges
    {
        if (!persist)
        {
            return Result<int>.CreateSuccess(-1);
        }
        if (transaction is not null)
        {
            await transaction.CommitAsync();
        }

        var result = await service.SaveChangesAsync();
        service.ResetChanges();
        return result;
    }

    public static Task<Result<TDbEntity?>> UpdateAsync<TViewModel, TDbEntity>([DisallowNull] this IService service, [DisallowNull] DbContext dbContext, [DisallowNull] TViewModel model, [DisallowNull] Func<TViewModel, TDbEntity?> convert, Func<TViewModel, Task<Result<TViewModel>>>? validatorAsync = null, Func<TDbEntity, TDbEntity>? onCommitting = null, bool persist = true, Func<Task<Result<int>>>? saveChanges = null, ILogger? logger = null)
        where TDbEntity : class, IIdenticalEntity<long>
        => InnerManipulate<TViewModel, TDbEntity, long>(dbContext, model, dbContext.Attach, convert, validatorAsync, onCommitting, persist, (true, null), saveChanges, logger: logger);

    public static Task<Result<TDbEntity?>> UpdateAsync<TViewModel, TDbEntity, TId>([DisallowNull] this IService service, [DisallowNull] DbContext dbContext, [DisallowNull] TViewModel model, [DisallowNull] Func<TViewModel, TDbEntity?> convert, Func<TViewModel, Task<Result<TViewModel>>>? validatorAsync = null, Func<TDbEntity, TDbEntity>? onCommitting = null, bool persist = true, Func<Task<Result<int>>>? saveChanges = null, ILogger? logger = null)
        where TDbEntity : class, IIdenticalEntity<TId>
        where TId : notnull
        => InnerManipulate<TViewModel, TDbEntity, TId>(dbContext, model!, dbContext.Update, convert, validatorAsync, onCommitting, persist, (true, null), saveChanges, logger: logger);

    private static async Task<Result<TDbEntity?>> InnerManipulate<TViewModel, TDbEntity, TId>([DisallowNull] DbContext dbContext, [DisallowNull] TViewModel model, [DisallowNull] Func<TDbEntity, EntityEntry<TDbEntity>> manipulate, [DisallowNull] Func<TViewModel, TDbEntity?> convertToEntity, Func<TViewModel, Task<Result<TViewModel>>>? validatorAsync, Func<TDbEntity, TDbEntity>? onCommitting, [DisallowNull] bool persist, (bool UseTransaction, IDbContextTransaction? Transaction)? transactionInfo = null, Func<Task<Result<int>>>? saveChanges = null, ILogger? logger = null)
        where TDbEntity : class, IIdenticalEntity<TId>
        where TId : notnull
    {
        validateArguments(model, manipulate, convertToEntity);
        var transaction = await initTransaction(dbContext, persist, transactionInfo);
        if ((await validateModel(model, validatorAsync)).IsFailure)
        {
            return Result<TDbEntity>.Fail;
        }

        var entity = onBeforeManipulation(model, convertToEntity, onCommitting);
        return await manipulateAndSave(dbContext, manipulate, persist, transactionInfo, saveChanges, transaction, entity);

        static void validateArguments(TViewModel model, Func<TDbEntity, EntityEntry<TDbEntity>> manipulate, Func<TViewModel, TDbEntity?> convertToEntity)
        {
            Check.IfArgumentNotNull(model);
            Check.IfArgumentNotNull(manipulate);
            Check.IfArgumentNotNull(convertToEntity);
        }
        static async Task<IDbContextTransaction?> initTransaction(DbContext dbContext, bool persist, (bool UseTransaction, IDbContextTransaction? Transaction)? transactionInfo)
        {
            IDbContextTransaction? transaction = null;
            if (persist && transactionInfo is { } t)
            {
                if (t.UseTransaction)
                {
                    transaction = t.Transaction ?? (await dbContext.Database.BeginTransactionAsync());
                }
            }

            return transaction;
        }
        static async Task<Result<TViewModel>> validateModel(TViewModel model, Func<TViewModel, Task<Result<TViewModel>>>? validatorAsync)
            => validatorAsync is not null ? await validatorAsync(model) : Result<TViewModel>.CreateSuccess(model);
        static TDbEntity onBeforeManipulation(TViewModel model, Func<TViewModel, TDbEntity?> convertToEntity, Func<TDbEntity, TDbEntity>? onCommitting)
        {
            var entity = convertToEntity(model).NotNull(() => "Entity cannot be null.");
            if (onCommitting is not null)
            {
                entity = onCommitting(entity);
            }

            return entity;
        }
        static async Task<Result<TDbEntity?>> manipulateAndSave(DbContext dbContext, Func<TDbEntity, EntityEntry<TDbEntity>> manipulate, bool persist, (bool UseTransaction, IDbContextTransaction? Transaction)? transactionInfo, Func<Task<Result<int>>>? saveChanges, IDbContextTransaction? transaction, TDbEntity entity)
        {
            var entry = manipulate(entity);
            if (persist)
            {
                try
                {
                    if (transactionInfo?.UseTransaction is true && transaction is not null)
                    {
                        await transaction.CommitAsync();
                    }
                    var writterCount = await (saveChanges is not null ? saveChanges() : CodeHelper.CatchResult(() => dbContext.SaveChangesAsync()));

                    return Result<TDbEntity?>.ConvertFrom(writterCount);
                }
                finally
                {
                    _ = DbContextHelper.Detach<DbContext, TDbEntity, TId>(dbContext, entity);
                }
            }
            else
            {
                return Result<TDbEntity?>.CreateSuccess(entity);
            }
        }
    }
}