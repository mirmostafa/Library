﻿using System.Linq.Expressions;
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
        where TEntity : class, IIdenticalEntity<long> =>
        dbContext.SetStateOf(entity, EntityState.Detached);
    public static TDbContext DetachGuid<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] in TEntity entity)
        where TDbContext : notnull, DbContext
        where TEntity : class, IIdenticalEntity<Guid> =>
        dbContext.SetStateGuidOf(entity, EntityState.Detached);
    public static TDbContext Detach<TDbContext, TEntity, TId>([DisallowNull] TDbContext dbContext, [DisallowNull] in TEntity entity)
        where TDbContext : notnull, DbContext
        where TEntity : class, IIdenticalEntity<TId>
        where TId : notnull =>
        SetStateOf<TDbContext, TEntity, TId>(dbContext, entity, EntityState.Detached);
    public static TEntity Detach<TDbContext, TEntity>([DisallowNull] this TEntity entity, [DisallowNull] in TDbContext dbContext)
        where TDbContext : notnull, DbContext
        where TEntity : class, IIdenticalEntity<long>
    {
        _ = Detach(dbContext, entity);
        return entity;
    }

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

    public static EntityState? GetStateOf<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] in TEntity entity)
        where TDbContext : notnull, DbContext
        where TEntity : class, IIdenticalEntity<long>
    {
        Check.IfArgumentNotNull(dbContext);
        Check.IfArgumentNotNull(entity);

        return GetLocalEntry(dbContext, entity)?.State;
    }

    public static EntityEntry<TEntity>? GetLocalEntry<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] in TEntity entity)
        where TDbContext : notnull, DbContext
        where TEntity : class, IIdenticalEntity<long>
    {
        var id = entity.ArgumentNotNull().Id;
        var ntt = dbContext.ArgumentNotNull().Set<TEntity>()?.Local?.FirstOrDefault(x => x.Id.Equals(id));
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
        var id = entity.ArgumentNotNull().Id;
        var ntt = dbContext.ArgumentNotNull().Set<TEntity>()?.Local?.FirstOrDefault(x => x.Id.Equals(id));
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
        var id = entity.ArgumentNotNull().Id;
        var ntt = dbContext.ArgumentNotNull().Set<TEntity>()?.Local?.FirstOrDefault(x => x.Id.Equals(id));
        if (ntt is null)
        {
            return null;
        }

        var entry = dbContext.Entry(ntt);
        return entry;
    }
    public static EntityEntry<TEntity>? GetEntry<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] in TEntity entity)
        where TDbContext : notnull, DbContext
        where TEntity : class, IIdenticalEntity<long>
    {
        var id = entity.ArgumentNotNull().Id;
        var ntt = dbContext.ArgumentNotNull().Set<TEntity>()?.FirstOrDefault(x => x.Id.Equals(id));
        if (ntt is null)
        {
            return null;
        }

        var entry = dbContext.Entry(ntt);
        return entry;
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
    public static DbContext RemoveById<TEntity, TId>([DisallowNull] DbContext dbContext, IEnumerable<TId> ids, bool detach = false)
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
    public static DbContext RemoveById<TEntity>([DisallowNull] this DbContext dbContext, long id, bool detach = false)
        where TEntity : class, IIdenticalEntity<long>, new() =>
        RemoveById<TEntity>(dbContext, new[] { id }, detach);
    public static DbContext RemoveById<TDbContext, TEntity, TId>([DisallowNull] DbContext dbContext, TId id, bool detach = false)
        where TEntity : class, IIdenticalEntity<TId>, new()
        where TId : notnull =>
        RemoveById<TEntity, TId>(dbContext, new[] { id }, detach);
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

    public static async Task<int> SaveChangesAsync(this EntityEntry entityEntry, bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        => await entityEntry?.Context.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken)!;
    public static async Task<int> SaveChangesAsync(this EntityEntry entityEntry, CancellationToken cancellationToken = default)
        => await entityEntry?.Context.SaveChangesAsync(cancellationToken)!;

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

    public static async Task<(EntityEntry<TEntity>? Entry, TEntity? Entity, int WrittenCount)> InsertAsync<TModel, TEntity>(
        this DbContext dbContext,
        [NotNull] TModel model,
        Func<TModel, TEntity?> convert,
        Func<TModel, Task<Result<TModel>>>? validatorAsync = null,
        Func<TEntity, TEntity>? onCommitting = null,
        bool persist = true,
        Func<Task<int>>? saveChanges = null)
        where TEntity : class, IIdenticalEntity<long> =>
        await InnerManipulate<TModel, TEntity, long>(dbContext, model, dbContext.Add, convert, validatorAsync, onCommitting, persist, (true, null), saveChanges);
    public static async Task<(EntityEntry<TEntity>? Entry, TEntity? Entity, int WrittenCount)> InsertAsync<TModel, TEntity, TId>(
        this DbContext dbContext,
        [NotNull] TModel model,
        Func<TModel, TEntity?> convert,
        Func<TModel, Task<Result<TModel>>>? validatorAsync = null,
        Func<TEntity, TEntity>? onCommitting = null,
        bool persist = true,
        Func<Task<int>>? saveChanges = null)
        where TEntity : class, IIdenticalEntity<TId>
        where TId : notnull =>
        await InnerManipulate<TModel, TEntity, TId>(dbContext, model, dbContext.Add, convert, validatorAsync, onCommitting, persist, (true, null), saveChanges);

    public static async Task<(EntityEntry<TEntity>? Entry, TEntity? Entity, int WrittenCount)> UpdateAsync<TModel, TEntity>(
        this DbContext dbContext,
        [NotNull] TModel model,
        Func<TModel, TEntity?> convert,
        Func<TModel, Task<Result<TModel>>>? validatorAsync = null,
        Func<TEntity, TEntity>? onCommitting = null,
        bool persist = true,
        Func<Task<int>>? saveChanges = null)
        where TEntity : class, IIdenticalEntity<long> =>
        await InnerManipulate<TModel, TEntity, long>(dbContext, model, dbContext.Attach, convert, validatorAsync, onCommitting, persist, (true, null), saveChanges);
    public static async Task<(EntityEntry<TEntity>? Entry, TEntity? Entity, int WrittenCount)> UpdateAsync<TModel, TEntity, TId>(
        this DbContext dbContext,
        [NotNull] TModel model,
        Func<TModel, TEntity?> convert,
        Func<TModel, Task<Result<TModel>>>? validatorAsync = null,
        Func<TEntity, TEntity>? onCommitting = null,
        bool persist = true,
        Func<Task<int>>? saveChanges = null)
        where TEntity : class, IIdenticalEntity<TId>
        where TId : notnull =>
        await InnerManipulate<TModel, TEntity, TId>(dbContext, model!, dbContext.Update, convert, validatorAsync, onCommitting, persist, (true, null), saveChanges);

    private static async Task<InnerManipulateRet<TModel, TEntity, TId>> InnerManipulate<TModel, TEntity, TId>(DbContext dbContext,
        [NotNull] TModel model,
        Func<TEntity, EntityEntry<TEntity>> manipulate,
        Func<TModel, TEntity?> convertToEntity,
        Func<TModel, Task<Result<TModel>>>? validatorAsync,
        Func<TEntity, TEntity>? onCommitting,
        bool persist,
        (bool UseTransaction, IDbContextTransaction? Transaction)? transactionInfo = null,
        Func<Task<int>>? saveChanges = null)
        where TEntity : class, IIdenticalEntity<TId>
        where TId : notnull
    {
        Check.IfArgumentNotNull(model);
        Check.IfArgumentNotNull(manipulate);
        Check.IfArgumentNotNull(convertToEntity);

        IDbContextTransaction? transaction = null;
        if (persist && transactionInfo is { } t)
        {
            if (t.UseTransaction)
            {
                transaction = t.Transaction ?? (await dbContext.Database.BeginTransactionAsync());
            }
        }
        if (validatorAsync is not null)
        {
            _ = await validatorAsync(model).HandleAsync();
        }

        var entity = convertToEntity(model);
        if (entity is null)
        {
            return default;
        }

        if (onCommitting is not null)
        {
            entity = onCommitting(entity);
        }
        var entry = manipulate(entity);

        if (persist)
        {
            try
            {
                var writterCount = await (saveChanges is not null ? saveChanges() : dbContext.SaveChangesAsync());
                if (transaction is not null)
                {
                    await transaction.CommitAsync();
                }

                return new(entry, entity, writterCount);
            }
            finally
            {
                _ = Detach<DbContext, TEntity, TId>(dbContext, entity);
            }
        }

        return new(entry, entity, default);
    }
    public static (TDbContext DbContext, EntityEntry<TEntity> Entry) ReAttach<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] in TEntity entity)
    where TDbContext : notnull, DbContext
    where TEntity : class, IIdenticalEntity<long> =>
        (dbContext, dbContext.Detach(entity).Attach(entity));
}

internal record struct InnerManipulateRet<TModel, TEntity, TId>(in EntityEntry<TEntity>? Entry, in TEntity? Entity, in int WrittenCount)
    where TEntity : class, IIdenticalEntity<TId>
    where TId : notnull
{
    public static implicit operator (EntityEntry<TEntity>? Entry, TEntity? Entity, int WrittenCount)(InnerManipulateRet<TModel, TEntity, TId> @this) =>
        new(@this.Entry, @this.Entity, @this.WrittenCount);
}