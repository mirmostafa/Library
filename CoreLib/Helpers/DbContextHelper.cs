using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Library.Data.Markers;
using Library.Validations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Library.Helpers;
public static class DbContextHelper
{
    public static TDbContext Detach<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] in TEntity entity)
        where TDbContext : notnull, DbContext
        where TEntity : class, IIdenticalEntity<long> =>
        dbContext.SetStateOf(entity, EntityState.Detached);

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

    public static TEntity Detach<TDbContext, TEntity>([DisallowNull] this TEntity entity, [DisallowNull] in TDbContext dbContext)
        where TDbContext : notnull, DbContext
        where TEntity : class, IIdenticalEntity<long>
    {
        Detach(dbContext, entity);
        return entity;
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

    public static DbContext RemoveById<TEntity>([DisallowNull] this DbContext dbContext, long id, bool detach = false)
        where TEntity : class, IIdenticalEntity<long>, new()
        => RemoveById<TEntity>(dbContext, new[] { id }, detach);

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

    public static async Task<(EntityEntry<TEntity>? entry, TEntity? entity, int writtenCount)> InsertAsync<TModel, TEntity>(
        this DbContext dbContext,
        TModel model,
        Func<TModel, TEntity?> convert,
        Func<TModel, Task>? validatorAsync = null,
        Func<EntityEntry<TEntity>, TEntity>? finalizeEntity = null,
        bool persist = true)
        where TEntity : class, IIdenticalEntity<long>
        => await InnerManipulate(dbContext, model, dbContext.Add, convert, validatorAsync, finalizeEntity, persist);

    public static async Task<(EntityEntry<TEntity>? entry, TEntity? entity, int writtenCount)> UpdateAsync<TModel, TEntity>(
        this DbContext dbContext,
        TModel model,
        Func<TModel, TEntity?> convert,
        Func<TModel, Task>? validatorAsync = null,
        Func<EntityEntry<TEntity>, TEntity>? finalizeEntity = null,
        bool persist = true)
        where TEntity : class, IIdenticalEntity<long>
        => await InnerManipulate(dbContext, model, dbContext.Attach, convert, validatorAsync, finalizeEntity, persist);

    private static async Task<(EntityEntry<TEntity> entry, TEntity entity, int writtenCount)> InnerManipulate<TModel, TEntity>(
        DbContext dbContext,
        TModel model,
        Func<TEntity, EntityEntry<TEntity>> manipulate,
        Func<TModel, TEntity?> convertToEntity,
        Func<TModel, Task>? validatorAsync,
        Func<EntityEntry<TEntity>, TEntity>? finalizeEntity,
        bool persist)
        where TEntity : class, IIdenticalEntity<long>
    {
        if (validatorAsync is not null)
        {
            await validatorAsync(model);
        }

        var entity = convertToEntity(model);
        if (entity is null)
        {
            return default;
        }

        var entry = manipulate(entity);
        if (finalizeEntity is not null)
        {
            entity = finalizeEntity(entry);
        }

        int writterCount = default;
        try
        {
            if (persist)
            {
                writterCount = await dbContext.SaveChangesAsync();
            }
        }
        finally
        {
            _ = dbContext.Detach(entity);
        }
        return (entry, entity, writterCount);
    }
}