using System.Reflection;

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
    #region CRUD

    public static async Task<Result> DeleteAsync<TViewModel, TDbEntity>([DisallowNull] this IService service, [DisallowNull] DbContext dbContext, TViewModel model, bool persist, bool? detach = null, ILogger? logger = null)
        where TDbEntity : class, IIdenticalEntity<long>, new()
        where TViewModel : IHasKey<long?>
    {
        Check.IfArgumentNotNull(model?.Id);
        Check.IfArgumentNotNull(dbContext);

        var id = (long)(model?.Id.Value)!;
        logger?.Debug($"Deleting {nameof(TDbEntity)}, id={id}");
        _ = dbContext.RemoveById<TDbEntity>(id);
        if (persist)
        {
            _ = await dbContext.SaveChangesAsync();
        }
        if (detach ?? persist)
        {
            var entity = dbContext.Set<TDbEntity>().Where(e => id == e.Id).First();
            _ = DbContextHelper.Detach(dbContext, entity);
        }
        logger?.Info($"Deleted {nameof(TDbEntity)}, id={id}");
        return Result.Success;
    }

    public static Task<IReadOnlyList<TViewModel>> GetAllAsync<TViewModel, TDbEntity>([DisallowNull] this IService service, [DisallowNull] DbContext dbContext, Func<IEnumerable<TDbEntity?>, IEnumerable<TViewModel?>> toViewModel, AsyncLock asyncLock)
        where TDbEntity : class
        where TViewModel : class
        => service.GetAllAsync(dbContext.Set<TDbEntity>(), toViewModel, asyncLock);

    public static async Task<IReadOnlyList<TViewModel>> GetAllAsync<TViewModel, TDbEntity>([DisallowNull] this IService service, [DisallowNull] IQueryable<TDbEntity> dbEntities, Func<IEnumerable<TDbEntity?>, IEnumerable<TViewModel?>> toViewModel, AsyncLock asyncLock)
        where TDbEntity : class
        where TViewModel : class
    {
        var query = from entity in dbEntities
                    select entity;
        var dbResult = await query.ToListLockAsync(asyncLock);
        var result = toViewModel(dbResult).Compact().ToReadOnlyList();
        return result;
    }

    public static Task<TViewModel?> GetByIdAsync<TViewModel, TDbEntity>([DisallowNull] this IService service, long id, [DisallowNull] DbContext dbContext, Func<TDbEntity?, TViewModel?> toViewModel, AsyncLock asyncLock)
        where TDbEntity : class, IIdenticalEntity<long>
        where TViewModel : class
        => GetByIdAsync(service, id, dbContext.Set<TDbEntity>(), toViewModel, asyncLock);

    public static async Task<TViewModel?> GetByIdAsync<TViewModel, TDbEntity>([DisallowNull] this IService service, long id, [DisallowNull] IQueryable<TDbEntity> entities, Func<TDbEntity?, TViewModel?> toViewModel, AsyncLock asyncLock)
        where TDbEntity : IHasKey<long>
    {
        var query = from entity in entities
                    where entity.Id == id
                    select entity;
        var dbResult = await query.FirstOrDefaultLockAsync(asyncLock);
        return toViewModel(dbResult);
    }

    public static Task<Result<ManipulationResult<TViewModel, TDbEntity?>>> InsertAsync<TViewModel, TDbEntity>(
        [DisallowNull] this IService service,
        [DisallowNull] DbContext dbContext,
        [DisallowNull] TViewModel model,
        [DisallowNull] Func<TViewModel, TDbEntity?> convert,
        Func<TViewModel, Task<Result<TViewModel>>>? validatorAsync = null,
        bool persist = true,
        Func<Task<Result<int>>>? saveChanges = null,
        ILogger? logger = null,
        Func<TDbEntity, TDbEntity>? onCommitting = null,
        Action<TViewModel, TDbEntity>? onCommitted = null)
        where TDbEntity : class, IIdenticalEntity<long>
        => InnerManipulate<TViewModel, TDbEntity, long>(dbContext, model, dbContext.NotNull().Add, convert, validatorAsync, null, persist, (true, null), saveChanges, logger, onCommitted);

    public static Task<Result<ManipulationResult<TViewModel, TDbEntity?>>> InsertAsync<TViewModel, TDbEntity, TId>(
        [DisallowNull] this IService service,
        [DisallowNull] DbContext dbContext,
        [DisallowNull] TViewModel model,
        [DisallowNull] Func<TViewModel, TDbEntity?> convert,
        Func<TViewModel, Task<Result<TViewModel>>>? validatorAsync = null,
        bool persist = true,
        Func<Task<Result<int>>>? saveChanges = null,
        ILogger? logger = null,
        Func<TDbEntity, TDbEntity>? onCommitting = null,
        Action<TViewModel, TDbEntity>? onCommitted = null)
        where TDbEntity : class, IIdenticalEntity<TId>
        where TId : notnull
        => InnerManipulate<TViewModel, TDbEntity, TId>(dbContext, model, dbContext.NotNull().Add, convert, validatorAsync, null, persist, (true, null), saveChanges, logger, onCommitted);

    public static Task<Result<ManipulationResult<TViewModel, TDbEntity?>>> InsertAsync<TService, TViewModel, TDbEntity>(
        [DisallowNull] this TService service,
        [DisallowNull] DbContext dbContext,
        [DisallowNull] TViewModel model,
        [DisallowNull] Func<TViewModel, TDbEntity?> convert,
        bool persist = true,
        ILogger? logger = null,
        Func<TDbEntity, TDbEntity>? onCommitting = null,
        Action<TViewModel, TDbEntity>? onCommitted = null)
        where TDbEntity : class, IIdenticalEntity<long>
        where TService : IAsyncValidator<TViewModel>, IAsyncSaveService
        => InnerManipulate<TViewModel, TDbEntity, long>(dbContext, model, dbContext.Add, convert, service.ValidateAsync, onCommitting, persist, (true, null), service.SaveChangesAsync, logger, onCommitted);

    public static Task<Result<ManipulationResult<TViewModel, TDbEntity?>>> InsertAsync<TService, TViewModel, TDbEntity>(
        [DisallowNull] this TService service,
        [DisallowNull] DbContext dbContext,
        [DisallowNull] TViewModel model,
        [DisallowNull] Func<TViewModel, TDbEntity?> convert,
        bool persist = true)
        where TViewModel : ICanSetKey<long?>
        where TDbEntity : class, IIdenticalEntity<long>
        where TService : IAsyncValidator<TViewModel>, IAsyncSaveService, ILoggerContainer
        => InnerManipulate<TViewModel, TDbEntity, long>(
            dbContext,
            model,
            dbContext.Add,
            convert,
            service.ValidateAsync,
            null,
            persist,
            (true, null),
            service.SaveChangesAsync,
            service.Logger,
            onCommitted: (m, e) => m.Id = e.Id);

    public static Task<Result<ManipulationResult<TViewModel, TDbEntity?>>> UpdateAsync<TService, TViewModel, TDbEntity>([DisallowNull] this TService service, [DisallowNull] DbContext dbContext, [DisallowNull] TViewModel model, [DisallowNull] Func<TViewModel, TDbEntity?> convert, bool persist = true, Func<TDbEntity, TDbEntity>? onCommitting = null, ILogger? logger = null,
        Action<TViewModel, TDbEntity>? onCommitted = null)
        where TDbEntity : class, IIdenticalEntity<long>
        where TService : IAsyncValidator<TViewModel>, IAsyncSaveService
        => InnerManipulate<TViewModel, TDbEntity, long>(dbContext, model, dbContext.Attach, convert, service.ValidateAsync, onCommitting, persist, (true, null), service.SaveChangesAsync, logger, onCommitted);

    public static Task<Result<ManipulationResult<TViewModel, TDbEntity?>>> UpdateAsync<TViewModel, TDbEntity, TId>([DisallowNull] this IService service, [DisallowNull] DbContext dbContext, [DisallowNull] TViewModel model, [DisallowNull] Func<TViewModel, TDbEntity?> convert, Func<TViewModel, Task<Result<TViewModel>>>? validatorAsync = null, Func<TDbEntity, TDbEntity>? onCommitting = null, bool persist = true, Func<Task<Result<int>>>? saveChanges = null, ILogger? logger = null,
        Action<TViewModel, TDbEntity>? onCommitted = null)
        where TDbEntity : class, IIdenticalEntity<TId>
        where TId : notnull
        => InnerManipulate<TViewModel, TDbEntity, TId>(dbContext, model!, dbContext.Update, convert, validatorAsync, onCommitting, persist, (true, null), saveChanges, logger, onCommitted);

    private static async Task<Result<ManipulationResult<TViewModel, TDbEntity?>>> InnerManipulate<TViewModel, TDbEntity, TId>(
        [DisallowNull] DbContext dbContext,
        [DisallowNull] TViewModel model,
        [DisallowNull] Func<TDbEntity, EntityEntry<TDbEntity>> manipulate,
        [DisallowNull] Func<TViewModel, TDbEntity?> convertToEntity,
        Func<TViewModel, Task<Result<TViewModel>>>? validatorAsync,
        Func<TDbEntity, TDbEntity>? onCommitting,
        bool persist,
        (bool UseTransaction, IDbContextTransaction? Transaction)? transactionInfo,
        Func<Task<Result<int>>>? saveChanges,
        ILogger? logger,
        Action<TViewModel, TDbEntity>? onCommitted)
        where TDbEntity : class, IIdenticalEntity<TId>
        where TId : notnull
    {
        //! Argument checks
        Check.IfArgumentNotNull(model);
        Check.IfArgumentNotNull(manipulate);
        Check.IfArgumentNotNull(convertToEntity);

        if (persist)
        {
            logger?.Debug($"Manipulation started. Entity: {nameof(TDbEntity)}, Action:{nameof(manipulate)}");
        }
        //! Setup transcation
        var transaction = persist && transactionInfo is { } t and { UseTransaction: true }
            ? await dbContext.Database.BeginTransactionAsync()
            : null;

        //! Validation checks
        if (validatorAsync is not null)
        {
            var validationResult = await validatorAsync(model);
            if (validationResult.IsFailure)
            {
                return Result<ManipulationResult<TViewModel, TDbEntity?>>.From(validationResult, (model, null));
            }
        }
        //! Before committing
        var entity = convertToEntity(model) // Convert model to entity
            .NotNull(() => "Entity cannot be null.").Fluent() // Cannot be null
            .IfTrue(onCommitting is not null, x => onCommitting!(x)).GetValue(); // On Before commit

        //! Manipulation
        var entry = manipulate(entity);

        //! Persist
        Result<int> saveResult;
        if (persist)
        {
            try
            {
                if (transactionInfo?.UseTransaction is true && transaction is not null)
                {
                    await transaction.CommitAsync();
                }
                saveResult = await (saveChanges is not null ? saveChanges() : CodeHelper.CatchResult(() => dbContext.SaveChangesAsync()));
                if (saveResult.IsSucceed && onCommitted is not null)
                {
                    onCommitted(model, entity);
                }
            }
            finally
            {
                // Should be detached bcuz this entity is atached in current scope.
                _ = DbContextHelper.Detach<DbContext, TDbEntity, TId>(dbContext, entity);
            }
        }
        else
        {
            saveResult = Result<int>.CreateSuccess(-1);
        }

        var result = Result<ManipulationResult<TViewModel, TDbEntity?>>.From(saveResult, (model, entity));
        if (persist && result)
        {
            if (result)
            {
                logger?.Debug($"Manipulation ended. Entity: {nameof(TDbEntity)}, Action:{nameof(manipulate)}");
            }
            else
            {
                logger?.Debug($"Manipulation ended with error. Entity: {nameof(TDbEntity)}, Action:{nameof(manipulate)}");
            }
        }

        return result;
    }

    #endregion CRUD

    #region RegisterServices

    public static IServiceCollection RegisterServices<TStartup, TService>(this IServiceCollection services)
        => RegisterServices<TService>(services, typeof(TStartup).Assembly);

    public static IServiceCollection RegisterServices<TService>(this IServiceCollection services, Assembly assembly)
    {
        var srvs = assembly.GetTypes().Where(t => t.IsClass && t.GetInterfaces().Contains(typeof(TService)));
        foreach (var srv in srvs)
        {
            var infcs = srv.GetInterfaces().Where(t => t.GetInterfaces().Contains(typeof(TService)));
            foreach (var infc in infcs)
            {
                _ = services.AddScoped(infc, srv);
            }
        }
        return services;
    }

    public static IServiceCollection RegisterServicesWithIService<TStartup>(this IServiceCollection services)
        => RegisterServices<IService>(services, typeof(TStartup).Assembly);

    #endregion RegisterServices

    #region Save & Submit Changes

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
        if (result.IsSucceed)
        {
            service.ResetChanges();
        }

        return result;
    }

    #endregion Save & Submit Changes

    public static Task<Result<TViewModel>> ModelResult<TViewModel, TDbEntity>(this Task<Result<ManipulationResult<TViewModel, TDbEntity>>> manipulationResultTask)
        => manipulationResultTask.ToResultAsync(x => x.Model);
}

public record struct ManipulationResult<TViewModel, TDbEntity>(in TViewModel Model, in TDbEntity Entity)
{
    public static implicit operator (TViewModel Model, TDbEntity? Entity)(ManipulationResult<TViewModel, TDbEntity> value) => (value.Model, value.Entity);
    public static implicit operator ManipulationResult<TViewModel, TDbEntity>((TViewModel Model, TDbEntity Entity) value) => new(value.Model, value.Entity);
    public static implicit operator TViewModel(ManipulationResult<TViewModel, TDbEntity> value) => value.Model;
}