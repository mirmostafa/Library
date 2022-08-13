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
    public static IDbContextTransaction BeginTransaction(this DbContext dbContext)
        => dbContext.Database.BeginTransaction();

    public static Task<IDbContextTransaction> BeginTransactionAsync(this DbContext dbContext, CancellationToken cancellationToken = default)
        => dbContext.Database.BeginTransactionAsync(cancellationToken);

    public static async Task<Result> CommitTransactionAsync(this DbContext dbContext, CancellationToken cancellationToken = default)
    {
        try
        {
            await dbContext.Database.CommitTransactionAsync(cancellationToken);
            return Result.Success;
        }
        catch (Exception ex)
        {
            return Result.CreateFail(ex.GetBaseException().Message, ex);
        }
    }

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
        => RemoveById<TEntity, long>(dbContext, ids, detach);

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

    public static TDbContext ResetChanges<TDbContext>(this TDbContext dbContext, bool disposeTransaction = true)
        where TDbContext : DbContext
    {
        dbContext.ChangeTracker.Clear();
        return disposeTransaction ? DisposeCurrentTransaction(dbContext) : dbContext;
    }

    public static Task<int> SaveChangesAsync(this EntityEntry entityEntry, [DisallowNull] bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        => entityEntry?.Context.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken)!;

    public static Task<int> SaveChangesAsync(this EntityEntry entityEntry, CancellationToken cancellationToken = default)
        => entityEntry?.Context.SaveChangesAsync(cancellationToken)!;

    public static async Task<Result<int>> SaveChangesResultAsync<TDbContext>(this TDbContext dbContext)
        where TDbContext : DbContext
    {
        try
        {
            var result = Result<int>.CreateSuccess(await dbContext.SaveChangesAsync());
            return result;
        }
        catch (Exception ex)
        {
            var result = Result<int>.CreateFail(ex.GetBaseException().Message, -1, ex);
            return result;
        }
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

    private static TDbContext DisposeCurrentTransaction<TDbContext>([DisallowNull] this TDbContext dbContext)
        where TDbContext : DbContext
    {
        dbContext.ArgumentNotNull().Database.CurrentTransaction?.Dispose();
        return dbContext;
    }
}