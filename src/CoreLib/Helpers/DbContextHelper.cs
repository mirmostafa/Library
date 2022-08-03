using System.Linq.Expressions;

using Library.Data.Markers;
using Library.Results;
using Library.Validations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Library.Helpers;

public static class DbContextHelper
{
    public static TDbContext Detach<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] in TEntity entity)
        where TDbContext : notnull, DbContext
        where TEntity : class, IIdenticalEntity<long>
        => dbContext.SetStateOf(entity, EntityState.Detached);

    public static TDbContext Detach<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, in IEnumerable<TEntity> entities)
            where TDbContext : notnull, DbContext
            where TEntity : class, IIdenticalEntity<long>
    {
        foreach (var entity in entities)
        {
            _ = dbContext.Detach(entity);
        }
        return dbContext;
    }

    public static TDbContext Detach<TDbContext, TEntity, TId>([DisallowNull] TDbContext dbContext, [DisallowNull] in TEntity entity)
        where TDbContext : notnull, DbContext
        where TEntity : class, IIdenticalEntity<TId>
        where TId : notnull
        => SetStateOf<TDbContext, TEntity, TId>(dbContext, entity, EntityState.Detached);

    public static TEntity Detach<TDbContext, TEntity>([DisallowNull] this TEntity entity, [DisallowNull] in TDbContext dbContext)
        where TDbContext : notnull, DbContext
        where TEntity : class, IIdenticalEntity<long>
    {
        _ = Detach(dbContext, entity);
        return entity;
    }

    public static TDbContext DetachGuid<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] in TEntity entity)
        where TDbContext : notnull, DbContext
        where TEntity : class, IIdenticalEntity<Guid>
        => dbContext.SetStateGuidOf(entity, EntityState.Detached);

    public static EntityEntry<TEntity> EnsureAttached<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] in TEntity entity)
        where TDbContext : notnull, DbContext
        where TEntity : class, IIdenticalEntity<long>
    {
        var result = dbContext.GetLocalEntry(entity);
        if (result is null or { State: EntityState.Detached })
        {
            result = dbContext.Attach(entity);
        }
        return result;
    }

    public static EntityEntry<TEntity>? GetEntry<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] in TEntity entity)
        where TDbContext : notnull, DbContext
        where TEntity : class, IIdenticalEntity<long>
    {
        Check.IfArgumentNotNull(dbContext);
        Check.IfArgumentNotNull(entity);

        var id = entity.Id;
        var ntt = dbContext.Set<TEntity>()?.FirstOrDefault(x => x.Id.Equals(id));
        if (ntt is null)
        {
            return null;
        }

        var entry = dbContext.Entry(ntt);
        return entry;
    }

    public static EntityEntry<TEntity>? GetLocalEntry<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] in TEntity entity)
        where TDbContext : notnull, DbContext
        where TEntity : class, IIdenticalEntity<long>
    {
        Check.IfArgumentNotNull(dbContext);
        Check.IfArgumentNotNull(entity);

        var id = entity.Id;
        var ntt = dbContext.Set<TEntity>()?.Local?.FirstOrDefault(x => x.Id.Equals(id));
        if (ntt is null)
        {
            return null;
        }

        var entry = dbContext.Entry(ntt);
        return entry;
    }

    public static EntityEntry<TEntity>? GetLocalEntry<TDbContext, TEntity, TId>([DisallowNull] TDbContext dbContext, [DisallowNull] in TEntity entity)
        where TDbContext : notnull, DbContext
        where TEntity : class, IIdenticalEntity<TId>
        where TId : notnull
    {
        Check.IfArgumentNotNull(dbContext);
        Check.IfArgumentNotNull(entity);

        var id = entity.Id;
        var ntt = dbContext.Set<TEntity>()?.Local?.FirstOrDefault(x => x.Id.Equals(id));
        if (ntt is null)
        {
            return null;
        }

        var entry = dbContext.Entry(ntt);
        return entry;
    }

    public static EntityEntry<TEntity>? GetLocalGuidEntry<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] in TEntity entity)
        where TDbContext : notnull, DbContext
        where TEntity : class, IIdenticalEntity<Guid>
    {
        Check.IfArgumentNotNull(dbContext);
        Check.IfArgumentNotNull(entity);

        var id = entity.Id;
        var ntt = dbContext.Set<TEntity>()?.Local?.FirstOrDefault(x => x.Id.Equals(id));
        if (ntt is null)
        {
            return null;
        }

        var entry = dbContext.Entry(ntt);
        return entry;
    }

    public static EntityState? GetStateOf<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] in TEntity entity)
        where TDbContext : notnull, DbContext
        where TEntity : class, IIdenticalEntity<long>
        => GetLocalEntry(dbContext, entity)?.State;

    public static async Task<Result<TEntity?>> InsertAsync<TModel, TEntity>(
        [DisallowNull] this DbContext dbContext,
        [DisallowNull] TModel model,
        [DisallowNull] Func<TModel, TEntity?> convert,
        Func<TModel, Task<Result<TModel>>>? validatorAsync = null,
        Func<TEntity, TEntity>? onCommitting = null,
        bool persist = true,
        Func<Task<int>>? saveChanges = null)
        where TEntity : class, IIdenticalEntity<long>
        => await InnerManipulate<TModel, TEntity, long>(dbContext, model, dbContext.NotNull().Add, convert, validatorAsync, onCommitting, persist, (true, null), saveChanges);

    public static async Task<Result<TEntity?>> InsertAsync<TModel, TEntity, TId>(
        this DbContext dbContext,
        [DisallowNull] TModel model,
        [DisallowNull] Func<TModel, TEntity?> convert,
        Func<TModel, Task<Result<TModel>>>? validatorAsync = null,
        Func<TEntity, TEntity>? onCommitting = null,
        bool persist = true,
        Func<Task<int>>? saveChanges = null)
        where TEntity : class, IIdenticalEntity<TId>
        where TId : notnull
        => await InnerManipulate<TModel, TEntity, TId>(dbContext, model, dbContext.Add, convert, validatorAsync, onCommitting, persist, (true, null), saveChanges);

    public static (TDbContext DbContext, EntityEntry<TEntity> Entry) ReAttach<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] in TEntity entity)
        where TDbContext : notnull, DbContext
        where TEntity : class, IIdenticalEntity<long>
        => (dbContext, dbContext.Detach(entity).Attach(entity));

    public static async Task<TDbContext> RemoveByEntityAsync<TDbContext, TEntity>([DisallowNull] TDbContext dbContext, [DisallowNull] Action<TEntity> setEntityId, bool persist = true, bool detach = false)
        where TDbContext : DbContext
        where TEntity : class, new()
    {
        var entity = new TEntity();
        setEntityId(entity);
        _ = dbContext.Remove(entity);
        if (persist)
        {
            _ = await dbContext.SaveChangesAsync();
        }
        if (detach)
        {
            //dbContext.Detach<TDbContext, TEntity>(entity);
        }

        return dbContext;
    }

    public static DbContext RemoveById<TEntity>([DisallowNull] this DbContext dbContext, in IEnumerable<long> ids, bool detach = false)
        where TEntity : class, IIdenticalEntity<long>, new()
    {
        Check.IfArgumentNotNull(dbContext);
        Check.IfArgumentNotNull(ids);

        Action<TEntity> det = detach ? entity => Catch(() => dbContext.Detach(entity)) : _ => { };

        foreach (var id in ids)
        {
            var entity = new TEntity { Id = id };
            det(entity);
            _ = dbContext.Remove(entity);
        }
        return dbContext;
    }

    public static DbContext RemoveById<TEntity, TId>([DisallowNull] DbContext dbContext, [DisallowNull] IEnumerable<TId> ids, bool detach = false)
        where TEntity : class, IIdenticalEntity<TId>, new()
        where TId : notnull
    {
        Check.IfArgumentNotNull(dbContext);
        Check.IfArgumentNotNull(ids);

        var entities = dbContext.Set<TEntity>().Where(e => ids.Contains(e.Id));
        if (detach)
        {
            _ = entities.ForEach(e => dbContext.Entry(e).State = EntityState.Detached);
        }

        dbContext.Set<TEntity>().RemoveRange(entities);
        return dbContext;
    }

    public static DbContext RemoveById<TEntity>([DisallowNull] this DbContext dbContext, [DisallowNull] long id, bool detach = false)
        where TEntity : class, IIdenticalEntity<long>, new()
        => RemoveById<TEntity>(dbContext, new[] { id }, detach);

    public static DbContext RemoveById<TDbContext, TEntity, TId>([DisallowNull] DbContext dbContext, [DisallowNull] TId id, bool detach = false)
        where TEntity : class, IIdenticalEntity<TId>, new()
        where TId : notnull
        => RemoveById<TEntity, TId>(dbContext, new[] { id }, detach);

    public static TDbContext ResetChanges<TDbContext>(this TDbContext dbContext)
        where TDbContext : DbContext
    {
        dbContext.ChangeTracker.Clear();
        return dbContext;
    }

    public static async Task<int> SaveChangesAsync(this EntityEntry entityEntry, [DisallowNull] bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        => await entityEntry?.Context.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken)!;

    public static async Task<int> SaveChangesAsync(this EntityEntry entityEntry, CancellationToken cancellationToken = default)
        => await entityEntry?.Context.SaveChangesAsync(cancellationToken)!;

    public static async Task<Result<int>> SaveChangesResultAsync<TDbContext>(this TDbContext dbContext)
        where TDbContext : DbContext
    {
        var result = Result<int>.CreateSuccess(await dbContext.SaveChangesAsync());
        return result;
    }

    public static EntityEntry<TEntity> SetModified<TEntity, TProperty>(
            this EntityEntry<TEntity> entityEntry,
        in Expression<Func<TEntity, TProperty>> propertyExpression,
        bool isModified = true)
        where TEntity : class
    {
        if (entityEntry is not null)
        {
            entityEntry.Property(propertyExpression).IsModified = isModified;
        }

        return entityEntry!;
    }

    public static TDbContext SetStateGuidOf<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] in TEntity entity, [DisallowNull] in EntityState state)
        where TDbContext : notnull, DbContext
        where TEntity : class, IIdenticalEntity<Guid>
    {
        Check.IfArgumentNotNull(dbContext);
        Check.IfArgumentNotNull(entity);

        if (GetLocalGuidEntry(dbContext, entity) is { } entry && entry.State != state)
        {
            entry.State = state;
        }
        return dbContext;
    }

    public static TDbContext SetStateOf<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] in TEntity entity, [DisallowNull] in EntityState state)
        where TDbContext : notnull, DbContext
        where TEntity : class, IIdenticalEntity<long>
    {
        Check.IfArgumentNotNull(dbContext);
        Check.IfArgumentNotNull(entity);

        if (GetLocalEntry(dbContext, entity) is { } entry && entry.State != state)
        {
            entry.State = state;
        }
        return dbContext;
    }

    public static TDbContext SetStateOf<TDbContext, TEntity, TId>([DisallowNull] TDbContext dbContext, [DisallowNull] in TEntity entity, [DisallowNull] in EntityState state)
        where TDbContext : notnull, DbContext
        where TEntity : class, IIdenticalEntity<TId>
        where TId : notnull
    {
        Check.IfArgumentNotNull(dbContext);
        Check.IfArgumentNotNull(entity);

        if (GetLocalEntry<TDbContext, TEntity, TId>(dbContext, entity) is { } entry && entry.State != state)
        {
            entry.State = state;
        }
        return dbContext;
    }

    public static Task<Result<TEntity?>> UpdateAsync<TModel, TEntity>(
        this DbContext dbContext,
        [DisallowNull] TModel model,
        [DisallowNull] Func<TModel, TEntity?> convert,
        Func<TModel, Task<Result<TModel>>>? validatorAsync = null,
        Func<TEntity, TEntity>? onCommitting = null,
        bool persist = true,
        Func<Task<int>>? saveChanges = null)
        where TEntity : class, IIdenticalEntity<long>
        => InnerManipulate<TModel, TEntity, long>(dbContext, model, dbContext.Attach, convert, validatorAsync, onCommitting, persist, (true, null), saveChanges);

    public static Task<Result<TEntity?>> UpdateAsync<TModel, TEntity, TId>(
        this DbContext dbContext,
        [DisallowNull] TModel model,
        [DisallowNull] Func<TModel, TEntity?> convert,
        Func<TModel, Task<Result<TModel>>>? validatorAsync = null,
        Func<TEntity, TEntity>? onCommitting = null,
        bool persist = true,
        Func<Task<int>>? saveChanges = null)
        where TEntity : class, IIdenticalEntity<TId>
        where TId : notnull
        => InnerManipulate<TModel, TEntity, TId>(dbContext, model!, dbContext.Update, convert, validatorAsync, onCommitting, persist, (true, null), saveChanges);

    private static async Task<Result<TEntity?>> InnerManipulate<TModel, TEntity, TId>(
        [DisallowNull] DbContext dbContext,
        [DisallowNull] TModel model,
        [DisallowNull] Func<TEntity, EntityEntry<TEntity>> manipulate,
        [DisallowNull] Func<TModel, TEntity?> convertToEntity,
        Func<TModel, Task<Result<TModel>>>? validatorAsync,
        Func<TEntity, TEntity>? onCommitting,
        [DisallowNull] bool persist,
        (bool UseTransaction, IDbContextTransaction? Transaction)? transactionInfo = null,
        Func<Task<int>>? saveChanges = null
        )
        where TEntity : class, IIdenticalEntity<TId>
        where TId : notnull
    {
        validateArguments(model, manipulate, convertToEntity);
        var transaction = await initTransaction(dbContext, persist, transactionInfo);
        if ((await validateModel(model, validatorAsync)).IsFailure)
        {
            return Result<TEntity>.Fail;
        }

        var entity = onBeforeManipulation(model, convertToEntity, onCommitting);
        return await manipulateAndSave(dbContext, manipulate, persist, transactionInfo, saveChanges, transaction, entity);

        static void validateArguments(TModel model, Func<TEntity, EntityEntry<TEntity>> manipulate, Func<TModel, TEntity?> convertToEntity)
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
        static async Task<Result<TModel>> validateModel(TModel model, Func<TModel, Task<Result<TModel>>>? validatorAsync)
            => validatorAsync is not null ? await validatorAsync(model) : Result<TModel>.CreateSuccess(model);
        static TEntity onBeforeManipulation(TModel model, Func<TModel, TEntity?> convertToEntity, Func<TEntity, TEntity>? onCommitting)
        {
            var entity = convertToEntity(model).NotNull(() => "Entity cannot be null.");
            if (onCommitting is not null)
            {
                entity = onCommitting(entity);
            }

            return entity;
        }
        static async Task<Result<TEntity?>> manipulateAndSave(DbContext dbContext, Func<TEntity, EntityEntry<TEntity>> manipulate, bool persist, (bool UseTransaction, IDbContextTransaction? Transaction)? transactionInfo, Func<Task<int>>? saveChanges, IDbContextTransaction? transaction, TEntity entity)
        {
            var entry = manipulate(entity);
            if (persist)
            {
                try
                {
                    var writterCount = await (saveChanges is not null ? saveChanges() : dbContext.SaveChangesAsync());
                    if (transactionInfo?.UseTransaction is true && transaction is not null)
                    {
                        await transaction.CommitAsync();
                    }

                    return Result<TEntity?>.CreateSuccess(entity);
                }
                finally
                {
                    _ = Detach<DbContext, TEntity, TId>(dbContext, entity);
                }
            }
            else
            {
                return Result<TEntity?>.CreateSuccess(entity);
            }
        }
    }
}

internal record struct InnerManipulateResult<TModel, TEntity, TId>(in EntityEntry<TEntity>? Entry, in TEntity? Entity, in int WrittenCount)
    where TEntity : class, IIdenticalEntity<TId>
    where TId : notnull
{
    public static implicit operator (EntityEntry<TEntity>? Entry, TEntity? Entity, int WrittenCount)([DisallowNull] InnerManipulateResult<TModel, TEntity, TId> @this)
        => new(@this.Entry, @this.Entity, @this.WrittenCount);
}