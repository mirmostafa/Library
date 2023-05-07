using System.ComponentModel;
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

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static async Task<Result> DeleteAsync<TViewModel, TDbEntity>([DisallowNull] IAsyncWriteService<TViewModel> service, [DisallowNull] DbContext dbContext, TViewModel model, bool persist, bool? detach = null, ILogger? logger = null)
        where TDbEntity : class, IIdenticalEntity<long>, new()
        where TViewModel : IHasKey<long?>
    {
        if (!Check.TryMustBeArgumentNotNull(model?.Id, out var res1))
        {
            return res1;
        }
        if (!Check.TryMustBeArgumentNotNull(dbContext, out var res2))
        {
            return res2;
        }

        var id = model.Id.Value;
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

    public static Task<IReadOnlyList<TViewModel>> GetAllAsync<TViewModel, TDbEntity>([DisallowNull] IAsyncReadService<TViewModel> service, [DisallowNull] DbContext dbContext, Func<IEnumerable<TDbEntity?>, IEnumerable<TViewModel?>> toViewModel, AsyncLock asyncLock)
        where TDbEntity : class
        where TViewModel : class
        => ServiceHelper.GetAllAsync(service, dbContext.Set<TDbEntity>(), toViewModel, asyncLock);

    public static async Task<IReadOnlyList<TViewModel>> GetAllAsync<TViewModel, TDbEntity>([DisallowNull] IAsyncReadService<TViewModel> _, [DisallowNull] IQueryable<TDbEntity> dbEntities, Func<IEnumerable<TDbEntity?>, IEnumerable<TViewModel?>> toViewModel, AsyncLock asyncLock)
        where TDbEntity : class
        where TViewModel : class
    {
        var query = from entity in dbEntities
                    select entity;
        var dbResult = await query.ToListLockAsync(asyncLock);
        //var result = dbResult?.Any() is true ? toViewModel(dbResult).ToReadOnlyList() : Enumerable.Empty<TViewModel>().ToReadOnlyList();
        var result = toViewModel(dbResult).Compact().ToReadOnlyList();
        return result;
    }

    public static Task<TViewModel?> GetByIdAsync<TViewModel, TDbEntity>([DisallowNull] IAsyncReadService<TViewModel> service, long id, [DisallowNull] DbContext dbContext, Func<TDbEntity?, TViewModel?> toViewModel, AsyncLock asyncLock)
        where TDbEntity : class, IIdenticalEntity<long>
        where TViewModel : class
        => GetByIdAsync(service, id, dbContext.Set<TDbEntity>(), toViewModel, asyncLock);

    public static async Task<TViewModel?> GetByIdAsync<TViewModel, TDbEntity>([DisallowNull] IAsyncReadService<TViewModel> service, long id, [DisallowNull] IQueryable<TDbEntity> entities, Func<TDbEntity?, TViewModel?> toViewModel, AsyncLock asyncLock)
        where TDbEntity : IHasKey<long>
    {
        var query = from entity in entities
                    where entity.Id == id
                    select entity;
        var dbResult = await query.FirstOrDefaultLockAsync(asyncLock);
        return toViewModel(dbResult);
    }

    public static Task<Result<ManipulationResult<TViewModel, TDbEntity?>>> InsertAsync<TViewModel, TDbEntity>(
        [DisallowNull] IAsyncWriteService<TViewModel> service,
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
        => InnerManipulate<TViewModel, TDbEntity, long>(dbContext, model, convert, validatorAsync, dbContext.NotNull().Add, null, persist, (true, null), saveChanges, onCommitted, logger);

    public static Task<Result<ManipulationResult<TViewModel, TDbEntity?>>> InsertAsync<TViewModel, TDbEntity, TId>(
        [DisallowNull] IAsyncReadService<TViewModel> service,
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
        => InnerManipulate<TViewModel, TDbEntity, TId>(dbContext, model, convert, validatorAsync, dbContext.NotNull().Add, null, persist, (true, null), saveChanges, onCommitted, logger);

    public static Task<Result<ManipulationResult<TViewModel, TDbEntity?>>> InsertAsync<TService, TViewModel, TDbEntity>(
        [DisallowNull] TService service,
        [DisallowNull] DbContext dbContext,
        [DisallowNull] TViewModel model,
        [DisallowNull] Func<TViewModel, TDbEntity?> convert,
        bool persist = true,
        ILogger? logger = null,
        Func<TDbEntity, TDbEntity>? onCommitting = null,
        Action<TViewModel, TDbEntity>? onCommitted = null)
        where TDbEntity : class, IIdenticalEntity<long>
        where TService : IAsyncWriteService<TViewModel>, IAsyncValidator<TViewModel>, IAsyncSaveService
        => InnerManipulate<TViewModel, TDbEntity, long>(dbContext, model, convert, service.ValidateAsync, dbContext.Add, onCommitting, persist, (true, null), service.SaveChangesAsync, onCommitted, logger);

    public static Task<Result<ManipulationResult<TViewModel, TDbEntity?>>> InsertAsync<TService, TViewModel, TDbEntity>(
        [DisallowNull] TService service,
        [DisallowNull] DbContext dbContext,
        [DisallowNull] TViewModel model,
        [DisallowNull] Func<TViewModel, TDbEntity?> convert,
        bool persist = true)
        where TViewModel : ICanSetKey<long?>
        where TDbEntity : class, IIdenticalEntity<long>
        where TService : IAsyncWriteService<TViewModel>, IAsyncValidator<TViewModel>, IAsyncSaveService, ILoggerContainer
        => InnerManipulate<TViewModel, TDbEntity, long>(
            dbContext,
            model,
            convert,
            service.ValidateAsync,
            dbContext.Add,
            null,
            persist,
            (true, null),
            service.SaveChangesAsync,
            onCommitted: (m, e) => m.Id = e.Id,
            logger: service.Logger);

    public static Task<Result<ManipulationResult<TViewModel, TDbEntity?>>> UpdateAsync<TService, TViewModel, TDbEntity>([DisallowNull] TService service, [DisallowNull] DbContext dbContext, [DisallowNull] TViewModel model, [DisallowNull] Func<TViewModel, TDbEntity?> convert, bool persist = true, Func<TDbEntity, TDbEntity>? onCommitting = null, ILogger? logger = null,
        Action<TViewModel, TDbEntity>? onCommitted = null)
        where TDbEntity : class, IIdenticalEntity<long>
        where TService : IAsyncWriteService<TViewModel>, IAsyncValidator<TViewModel>, IAsyncSaveService
        => InnerManipulate<TViewModel, TDbEntity, long>(dbContext, model, convert, service.ValidateAsync, dbContext.Attach, onCommitting, persist, (true, null), service.SaveChangesAsync, onCommitted, logger);

    public static Task<Result<ManipulationResult<TViewModel, TDbEntity?>>> UpdateAsync<TViewModel, TDbEntity, TId>([DisallowNull] IAsyncWriteService<TViewModel> service, [DisallowNull] DbContext dbContext, [DisallowNull] TViewModel model, [DisallowNull] Func<TViewModel, TDbEntity?> convert, Func<TViewModel, Task<Result<TViewModel>>>? validatorAsync = null, Func<TDbEntity, TDbEntity>? onCommitting = null, bool persist = true, Func<Task<Result<int>>>? saveChanges = null, ILogger? logger = null,
        Action<TViewModel, TDbEntity>? onCommitted = null)
        where TDbEntity : class, IIdenticalEntity<TId>
        where TId : notnull
        => InnerManipulate<TViewModel, TDbEntity, TId>(dbContext, model!, convert, validatorAsync, dbContext.Update, onCommitting, persist, (true, null), saveChanges, onCommitted, logger);

    private static async Task<Result<ManipulationResult<TViewModel, TDbEntity?>>> InnerManipulate<TViewModel, TDbEntity, TId>(
        DbContext dbContext,
        TViewModel model,
        Func<TViewModel, TDbEntity?> convertToEntity,
        Func<TViewModel, Task<Result<TViewModel>>>? validatorAsync,
        Func<TDbEntity, EntityEntry<TDbEntity>> manipulate,
        Func<TDbEntity, TDbEntity>? onCommitting,
        bool persist,
        (bool UseTransaction, IDbContextTransaction? Transaction)? transactionInfo,
        Func<Task<Result<int>>>? saveChanges,
        Action<TViewModel, TDbEntity>? onCommitted,
        ILogger? logger)
        where TDbEntity : class, IIdenticalEntity<TId>
        where TId : notnull
    {
        //! Argument checks
        if (Check.TryMustBeArgumentNotNull(model, out var res1))
        {
            return getResult(res1);
        }
        if (Check.TryMustBeArgumentNotNull(manipulate, out var res2))
        {
            return getResult(res2);
        }
        if (Check.TryMustBeArgumentNotNull(convertToEntity, out var res3))
        {
            return getResult(res3);
        }

        if (persist)
        {
            logger?.Debug($"Manipulation started. Entity: {nameof(TDbEntity)}, Action:{nameof(manipulate)}");
        }

        //! Setup transaction
        var transaction = persist && transactionInfo is { } t and { UseTransaction: true }
            ? await dbContext.Database.BeginTransactionAsync()
            : null;

        //! Manipulation Validation checks
        if (validatorAsync is not null)
        {
            var validationResult = await validatorAsync(model);
            if (validationResult.IsFailure)
            {
                return getResult(validationResult, (model, null));
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
                saveResult = await (saveChanges is not null ? saveChanges() : CodeHelper.CatchResultAsync(() => dbContext.SaveChangesAsync()));
                if (saveResult.IsSucceed && onCommitted is not null)
                {
                    onCommitted(model, entity);
                }
            }
            finally
            {
                //! Should be detached bcuz this entity is attached in current scope.
                _ = DbContextHelper.Detach<DbContext, TDbEntity, TId>(dbContext, entity);
            }
        }
        else
        {
            saveResult = Result<int>.CreateSuccess(-1);
        }

        var result = getResult(saveResult, (model, entity));
        if (persist && result)
        {
            if (result)
            {
                logger?.Debug($"Manipulation ended successfully. Entity: {nameof(TDbEntity)}, Action:{nameof(manipulate)}");
            }
            else
            {
                logger?.Debug($"Manipulation ended with error. Entity: {nameof(TDbEntity)}, Action:{nameof(manipulate)}");
            }
        }
        return result;

        static Result<ManipulationResult<TViewModel, TDbEntity?>> getResult(Result result, ManipulationResult<TViewModel, TDbEntity?> entity = default)
            => Result<ManipulationResult<TViewModel, TDbEntity?>>.From(result, entity);
    }

    #endregion CRUD

    #region RegisterServices

    public static IServiceCollection RegisterServices<TService>(this IServiceCollection services, Assembly assembly)
        => RegisterServices<TService>(services, assembly, assembly);

    public static IServiceCollection RegisterServices<TService>(this IServiceCollection services, Type interfaceModule, Type serviceModule)
        => RegisterServices<TService>(services, interfaceModule.Assembly, serviceModule.Assembly);

    public static IServiceCollection RegisterServices<TServiceInterface>(
        this IServiceCollection serviceCollection,
        in Assembly interfaceAsm,
        in Assembly serviceAsm,
        in Action<(IServiceCollection ServiceCollection, Type ServiceInterface, Type ServiceType)>? add = default)
    {
        var addToServices = add ?? (((IServiceCollection ServiceCollection, Type ServiceInterface, Type ServiceType) x) => _ = x.ServiceCollection.AddScoped(x.ServiceInterface, x.ServiceType));
        var interfaces = interfaceAsm.GetTypes().Where(t => t.IsInterface && t.GetInterfaces().Contains(typeof(TServiceInterface))).ToList();
        var services = serviceAsm.GetTypes().Where(t => t.IsClass && t.GetInterfaces().Contains(typeof(TServiceInterface))).ToList();
        foreach (var iface in interfaces)
        {
            foreach (var svc in services)
            {
                if (svc.GetInterface(iface.Name) != null)
                {
                    addToServices((serviceCollection, iface, svc));
                    break;
                }
            }
        }

        return serviceCollection;
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