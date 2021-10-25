using Library.Data.Markers;
using Library.Validations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Library.Helpers;
public static class DbContextHelper
{
    public static TDbContext Detach<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] TEntity entity)
        where TDbContext : notnull, DbContext
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

    public static TEntity Detach<TDbContext, TEntity>([DisallowNull] this TEntity entity, [DisallowNull] TDbContext dbContext)
        where TDbContext : notnull, DbContext
        where TEntity : class, IIdenticalEntity<long>
    {
        Detach(dbContext, entity);
        return entity;
    }

    public static DbContext RemoveById<TEntity>([DisallowNull] this DbContext dbContext, IEnumerable<long> ids, bool detach = false)
        where TEntity : class, IIdenticalEntity<long>, new()
    {
        Check.IfArgumentNotNull(dbContext, nameof(dbContext));
        Check.IfArgumentNotNull(ids, nameof(ids));

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