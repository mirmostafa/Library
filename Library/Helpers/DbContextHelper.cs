using System.Linq.Expressions;
using Library.Coding;
using Library.Data.Markers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Library.Helpers;
public static class DbContextHelper
{
    public static TDbContext Detach<TDbContext, TEntity>(this TDbContext dbContext, TEntity entity)
        where TDbContext : DbContext
        where TEntity : class, IIdenticalEntity<long>
    {
        Check.IfArgumentNotNull(dbContext, nameof(dbContext));
        Check.IfArgumentNotNull(entity, nameof(entity));
        var local = dbContext.Set<TEntity>().Local.FirstOrDefault(entry => entry.Id.Equals(entity.Id));
        if (local is not null)
        {
            dbContext.Entry(local).State = EntityState.Detached;
        }
        return dbContext;
    }

    public static TEntity Detach<TDbContext, TEntity>(this TEntity entity, TDbContext dbContext)
        where TDbContext : DbContext
        where TEntity : class, IIdenticalEntity<long>
    {
        dbContext.IfArgumentNotNull(nameof(dbContext));
        entity.IfArgumentNotNull(nameof(entity));

        var local = dbContext.Set<TEntity>().Local.FirstOrDefault(entry => entry.Id.Equals(entity.Id));
        if (local is not null)
        {
            dbContext.Entry(local).State = EntityState.Detached;
        }
        return entity;
    }

    public static void RemoveById<TEntity>(this DbContext dbContext, IEnumerable<long> ids, bool detach = false)
        where TEntity : class, IIdenticalEntity<long>, new()
    {
        Check.IfArgumentNotNull(dbContext, nameof(dbContext));
        Check.IfArgumentNotNull(ids, nameof(ids));
        foreach (var id in ids)
        {
            var entity = new TEntity { Id = id };
            if (detach)
            {
                _ = Catch(() => dbContext.Detach(entity));
            }

            _ = dbContext.Remove(entity);
        }
    }

    public static EntityEntry<TEntity> RemoveById<TEntity>(this DbContext dbContext, long id, bool detach = false)
        where TEntity : class, IIdenticalEntity<long>, new()
    {
        Check.IfArgumentNotNull(dbContext, nameof(dbContext));
        var entity = new TEntity { Id = id };
        if (detach)
        {
            _ = CodeHelper.Catch(() => dbContext.Detach(entity));
        }

        return dbContext.Remove(entity);
    }

    public static async Task<int> SaveChangesAsync(this EntityEntry entityEntry, bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
          => await entityEntry?.Context.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken)!;

    public static async Task<int> SaveChangesAsync(this EntityEntry entityEntry, CancellationToken cancellationToken = default)
          => await entityEntry?.Context.SaveChangesAsync(cancellationToken)!;

    public static EntityEntry<TEntity> SetModified<TEntity, TProperty>(
        this EntityEntry<TEntity> entityEntry,
        Expression<Func<TEntity, TProperty>> propertyExpression,
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
        => await InnerManipulate(dbContext, model, convert, validatorAsync, finalizeEntity, persist, dbContext.Add);
    public static async Task<(EntityEntry<TEntity>? entry, TEntity? entity, int writtenCount)> UpdateAsync<TModel, TEntity>(
        this DbContext dbContext,
        TModel model,
        Func<TModel, TEntity?> convert,
        Func<TModel, Task>? validatorAsync = null,
        Func<EntityEntry<TEntity>, TEntity>? finalizeEntity = null,
        bool persist = true)
        where TEntity : class, IIdenticalEntity<long>
        => await InnerManipulate(dbContext, model, convert, validatorAsync, finalizeEntity, persist, dbContext.Update);

    private static async Task<(EntityEntry<TEntity> entry, TEntity entity, int writtenCount)> InnerManipulate<TModel, TEntity>(
        DbContext dbContext,
        TModel model,
        Func<TModel, TEntity?> convert,
        Func<TModel, Task>? validatorAsync,
        Func<EntityEntry<TEntity>, TEntity>? finalizeEntity,
        bool persist,
        Func<TEntity, EntityEntry<TEntity>> manipulate)
        where TEntity : class, IIdenticalEntity<long>
    {
        if (validatorAsync is not null)
        {
            await validatorAsync(model);
        }

        var entity = convert(model);
        if (entity is null)
        {
            return default;
        }

        var entry = manipulate(entity);
        if (finalizeEntity is not null)
        {
            entity = finalizeEntity(entry);
        }
        if (!persist)
        {
            return (entry, entity, default);
        }

        try
        {
            var writterCount = await dbContext.SaveChangesAsync();
            return (entry, entity, writterCount);
        }
        finally
        {
            _ = dbContext.Detach(entity);
        }
    }
}