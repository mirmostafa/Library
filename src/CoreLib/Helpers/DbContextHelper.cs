using System.Diagnostics;
using System.Linq.Expressions;

using Library.Data.Markers;
using Library.Results;
using Library.Validations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Library.Helpers;

[DebuggerStepThrough]
[StackTraceHidden]
public static class DbContextHelper
{
    public static IDbContextTransaction BeginTransaction(this DbContext dbContext)
        => dbContext.ArgumentNotNull().Database.BeginTransaction();

    /// <summary>
    /// Asynchronously starts a new transaction.
    /// <para>Don't forget to call <c>Commit</c> or <c>Rollback</c> at the end of transaction</para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Entity Framework Core does not support multiple parallel operations being run on the same
    /// DbContext instance. This includes both parallel execution of async queries and any explicit
    /// concurrent use from multiple threads. Therefore, always await async calls immediately, or
    /// use separate DbContext instances for operations that execute in parallel. See <see
    /// href="https://aka.ms/efcore-docs-threading">Avoiding DbContext threading issues</see> for
    /// more information.
    /// </para>
    /// <para>
    /// See <see href="https://aka.ms/efcore-docs-transactions">Transactions in EF Core</see> for
    /// more information.
    /// </para>
    /// </remarks>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous transaction initialization. The task result contains
    /// a <see cref="IDbContextTransaction"/> that represents the started transaction.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// If the <see cref="CancellationToken"/> is canceled.
    /// </exception>
    public static Task<IDbContextTransaction> BeginTransactionAsync(this DbContext dbContext, CancellationToken cancellationToken = default)
        => dbContext.ArgumentNotNull().Database.BeginTransactionAsync(cancellationToken);

    /// <summary>
    /// Commits the transaction asynchronously.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the operation.</returns>
    public static async Task<Result> CommitTransactionAsync(this DbContext dbContext, CancellationToken cancellationToken = default)
    {
        Check.MustBeArgumentNotNull(dbContext);
        try
        {
            await dbContext.ArgumentNotNull().Database.CommitTransactionAsync(cancellationToken);
            return Result.Succeed;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.GetBaseException().Message, ex);
        }
    }

    /// <summary>
    /// Compiles an asynchronous query expression for a given DbContext.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the DbContext.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <typeparam name="TParam">The type of the parameter.</typeparam>
    /// <param name="db">The DbContext.</param>
    /// <param name="queryExpression">The query expression.</param>
    /// <returns>A Func that takes a parameter of type TParam and returns a Task of type TResult.</returns>
    public static Func<TParam, Task<TResult>> CompileAsyncQuery<TDbContext, TResult, TParam>(
            this TDbContext db,
            Expression<Func<TDbContext, TParam, TResult>> queryExpression)
        where TDbContext : DbContext
    {
        var rawResult = EF.CompileAsyncQuery(queryExpression);
        Task<TResult> result(TParam param) => rawResult(db, param);
        return result;
    }

    /// <summary>
    /// Compiles an asynchronous query expression for a given DbContext.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the DbContext.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="db">The DbContext.</param>
    /// <param name="queryExpression">The query expression.</param>
    /// <returns>A Func that can be used to execute the query.</returns>
    public static Func<Task<TResult?>> CompileAsyncQuery<TDbContext, TResult>(
            this TDbContext db,
            Expression<Func<TDbContext, TResult?>> queryExpression)
            where TDbContext : DbContext =>
        () => EF.CompileAsyncQuery(queryExpression)(db);

    /// <summary>
    /// Compiles an asynchronous query expression into a delegate.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TParam1">The type of the parameter.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="db">The database context.</param>
    /// <param name="queryExpression">The query expression.</param>
    /// <returns>A delegate that can be used to execute the query.</returns>
    public static Func<TParam1, IAsyncEnumerable<TResult>> CompileAsyncQuery<TContext, TParam1, TResult>(
            this TContext db,
            Expression<Func<TContext, TParam1, IQueryable<TResult>>> queryExpression)
                where TContext : DbContext
    {
        var rawResult = EF.CompileAsyncQuery(queryExpression);
        IAsyncEnumerable<TResult> result(TParam1 param) => rawResult(db, param);
        return result;
    }

    /// <summary>
    /// Compiles an asynchronous query expression into a Func that can be used to execute the query.
    /// </summary>
    /// <typeparam name="TContext">The type of the DbContext.</typeparam>
    /// <typeparam name="TResult">The type of the query result.</typeparam>
    /// <param name="db">The DbContext instance.</param>
    /// <param name="queryExpression">The query expression.</param>
    /// <returns>A Func that can be used to execute the query.</returns>
    public static Func<IAsyncEnumerable<TResult>> CompileAsyncQuery<TContext, TResult>(
            this TContext db,
            Expression<Func<TContext, IQueryable<TResult>>> queryExpression)
                where TContext : DbContext
    {
        var rawResult = EF.CompileAsyncQuery(queryExpression);
        IAsyncEnumerable<TResult> result() => rawResult(db);
        return result;
    }

    /// <summary>
    /// Detaches the specified entity from the specified DbContext.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the database context.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="dbContext">The database context.</param>
    /// <param name="entity">The entity.</param>
    /// <returns>The detached entity.</returns>
    public static TDbContext Detach<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] in TEntity entity)
            where TDbContext : notnull, DbContext
            where TEntity : class, IIdenticalEntity<long>
            => dbContext.SetStateOf(entity, EntityState.Detached);

    /// <summary>
    /// Detaches a collection of entities from the DbContext.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the database context.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="dbContext">The database context.</param>
    /// <param name="entities">The entities to detach.</param>
    /// <returns>The database context.</returns>
    public static TDbContext Detach<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, in IEnumerable<TEntity> entities)
            where TDbContext : notnull, DbContext
            where TEntity : class, IIdenticalEntity<long>
    {
        Check.MustBeArgumentNotNull(entities);
        foreach (var entity in entities)
        {
            _ = dbContext.Detach(entity);
        }
        return dbContext;
    }

    /// <summary>
    /// Sets the state of the specified entity to detached in the specified DbContext.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the DbContext.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TId">The type of the entity's identifier.</typeparam>
    /// <param name="dbContext">The DbContext.</param>
    /// <param name="entity">The entity.</param>
    /// <returns>The DbContext.</returns>
    public static TDbContext Detach<TDbContext, TEntity, TId>([DisallowNull] TDbContext dbContext, [DisallowNull] in TEntity entity)
            where TDbContext : notnull, DbContext
            where TEntity : class, IIdenticalEntity<TId>
            where TId : notnull
            => SetStateOf<TDbContext, TEntity, TId>(dbContext, entity, EntityState.Detached);

    /// <summary>
    /// Detaches the specified entity from the specified DbContext.
    /// </summary>
    /// <param name="entity">The entity to detach.</param>
    /// <param name="dbContext">The DbContext from which to detach the entity.</param>
    /// <returns>The detached entity.</returns>
    public static TEntity Detach<TDbContext, TEntity>([DisallowNull] this TEntity entity, [DisallowNull] in TDbContext dbContext)
            where TDbContext : notnull, DbContext
            where TEntity : class, IIdenticalEntity<long>
    {
        _ = Detach(dbContext, entity);
        return entity;
    }

    /// <summary>
    /// Detaches the given entity from the given DbContext, setting its state to EntityState.Detached.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the DbContext.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="dbContext">The DbContext.</param>
    /// <param name="entity">The entity.</param>
    /// <returns>The DbContext.</returns>
    public static TDbContext DetachGuid<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] in TEntity entity)
            where TDbContext : notnull, DbContext
            where TEntity : class, IIdenticalEntity<Guid>
            => dbContext.SetStateGuidOf(entity, EntityState.Detached);

    /// <summary>
    /// Ensures that the given entity is attached to the given DbContext.
    /// </summary>
    /// <param name="dbContext">The DbContext to attach the entity to.</param>
    /// <param name="entity">The entity to attach.</param>
    /// <returns>The EntityEntry of the attached entity.</returns>
    public static EntityEntry<TEntity> EnsureAttached<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] in TEntity entity)
            where TDbContext : notnull, DbContext
            where TEntity : class, IIdenticalEntity<long>
    {
        Check.MustBeArgumentNotNull(dbContext);
        var result = dbContext.GetLocalEntry(entity);
        if (result is null or { State: EntityState.Detached })
        {
            result = dbContext.Attach(entity);
        }
        return result;
    }

    /// <summary>
    /// Gets the EntityEntry of the specified entity from the specified DbContext.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the DbContext.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="dbContext">The DbContext.</param>
    /// <param name="entity">The entity.</param>
    /// <returns>The EntityEntry of the specified entity from the specified DbContext.</returns>
    public static EntityEntry<TEntity>? GetEntry<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] in TEntity entity)
            where TDbContext : notnull, DbContext
            where TEntity : class, IIdenticalEntity<long>
    {
        Check.MustBeArgumentNotNull(dbContext);
        Check.MustBeArgumentNotNull(entity);

        var id = entity.Id;
        var ntt = dbContext.Set<TEntity>()?.FirstOrDefault(x => x.Id.Equals(id));
        if (ntt is null)
        {
            return null;
        }

        var entry = dbContext.Entry(ntt);
        return entry;
    }

    /// <summary>
    /// Gets the local entry of the specified entity from the given DbContext.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the DbContext.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="dbContext">The DbContext.</param>
    /// <param name="entity">The entity.</param>
    /// <returns>The EntityEntry of the entity.</returns>
    public static EntityEntry<TEntity>? GetLocalEntry<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] in TEntity entity)
            where TDbContext : notnull, DbContext
            where TEntity : class, IIdenticalEntity<long>
    {
        Check.MustBeArgumentNotNull(dbContext);
        Check.MustBeArgumentNotNull(entity);

        var id = entity.Id;
        var ntt = dbContext.Set<TEntity>()?.Local?.FirstOrDefault(x => x.Id.Equals(id));
        if (ntt is null)
        {
            return null;
        }

        var entry = dbContext.Entry(ntt);
        return entry;
    }

    /// <summary>
    /// Gets the local entry of an entity from a given DbContext.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the database context.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TId">The type of the entity's identifier.</typeparam>
    /// <param name="dbContext">The database context.</param>
    /// <param name="entity">The entity.</param>
    /// <returns>The local entry of the entity.</returns>
    public static EntityEntry<TEntity>? GetLocalEntry<TDbContext, TEntity, TId>([DisallowNull] TDbContext dbContext, [DisallowNull] in TEntity entity)
            where TDbContext : notnull, DbContext
            where TEntity : class, IIdenticalEntity<TId>
            where TId : notnull
    {
        Check.MustBeArgumentNotNull(dbContext);
        Check.MustBeArgumentNotNull(entity);

        var id = entity.Id;
        var ntt = dbContext.Set<TEntity>()?.Local?.FirstOrDefault(x => x.Id.Equals(id));
        if (ntt is null)
        {
            return null;
        }

        var entry = dbContext.Entry(ntt);
        return entry;
    }

    /// <summary>
    /// Gets the local Guid entry from the specified DbContext and entity.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the database context.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="dbContext">The database context.</param>
    /// <param name="entity">The entity.</param>
    /// <returns>The EntityEntry of the entity.</returns>
    public static EntityEntry<TEntity>? GetLocalGuidEntry<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] in TEntity entity)
            where TDbContext : notnull, DbContext
            where TEntity : class, IIdenticalEntity<Guid>
    {
        Check.MustBeArgumentNotNull(dbContext);
        Check.MustBeArgumentNotNull(entity);

        var id = entity.Id;
        var ntt = dbContext.Set<TEntity>()?.Local?.FirstOrDefault(x => x.Id.Equals(id));
        if (ntt is null)
        {
            return null;
        }

        var entry = dbContext.Entry(ntt);
        return entry;
    }

    /// <summary>
    /// Gets the state of the specified entity in the given DbContext.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the DbContext.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="dbContext">The DbContext.</param>
    /// <param name="entity">The entity.</param>
    /// <returns>The state of the entity.</returns>
    public static EntityState? GetStateOf<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] in TEntity entity)
        where TDbContext : notnull, DbContext
        where TEntity : class, IIdenticalEntity<long>
        => GetLocalEntry(dbContext, entity)?.State;

    /// <summary>
    /// ReAttach method for a generic TDbContext and TEntity.
    /// </summary>
    public static (TDbContext DbContext, EntityEntry<TEntity> Entry) ReAttach<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] in TEntity entity)
        where TDbContext : notnull, DbContext
        where TEntity : class, IIdenticalEntity<long>
    {
        var entry = dbContext.Detach(entity).Attach(entity);
        return (dbContext, entry);
    }

    /// <summary>
    /// Removes an entity from the database asynchronously.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the database context.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="dbContext">The database context.</param>
    /// <param name="setEntityId">The action to set the entity's ID.</param>
    /// <param name="persist">Whether to persist the changes.</param>
    /// <param name="detach">Whether to detach the entity.</param>
    /// <returns>The database context.</returns>
    public static async Task<TDbContext> RemoveByEntityAsync<TDbContext, TEntity>([DisallowNull] TDbContext dbContext, [DisallowNull] Action<TEntity> setEntityId, bool persist = true, bool detach = false)
        where TDbContext : DbContext
        where TEntity : class, new()
    {
        Check.MustBeArgumentNotNull(dbContext);
        Check.MustBeArgumentNotNull(setEntityId);

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

    /// <summary>
    /// Removes entities of type TEntity from the DbContext by their Ids.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="dbContext">The DbContext.</param>
    /// <param name="ids">The Ids of the entities to remove.</param>
    /// <param name="detach">Whether to detach the entities from the DbContext.</param>
    /// <returns>The DbContext.</returns>
    public static DbContext RemoveById<TEntity>([DisallowNull] this DbContext dbContext, in IEnumerable<long> ids, bool detach = false)
            where TEntity : class, IIdenticalEntity<long>, new()
            => RemoveById<TEntity, long>(dbContext, ids, detach);

    /// <summary>
    /// Removes entities from the database by their Ids.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TId">The type of the Id.</typeparam>
    /// <param name="dbContext">The database context.</param>
    /// <param name="ids">The Ids of the entities to remove.</param>
    /// <param name="detach">Whether to detach the entities from the context.</param>
    /// <returns>The database context.</returns>
    public static DbContext RemoveById<TEntity, TId>([DisallowNull] DbContext dbContext, [DisallowNull] IEnumerable<TId> ids, bool detach = false)
            where TEntity : class, IIdenticalEntity<TId>, new()
            where TId : notnull
    {
        Check.MustBeArgumentNotNull(dbContext);
        Check.MustBeArgumentNotNull(ids);

        var entities = dbContext.Set<TEntity>().Where(e => ids.Contains(e.Id));
        if (detach)
        {
            entities.ForEach(e => dbContext.Entry(e).State = EntityState.Detached);
        }

        dbContext.Set<TEntity>().RemoveRange(entities);
        return dbContext;
    }

    /// <summary>
    /// Removes an entity from the database by its Id.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="dbContext">The database context.</param>
    /// <param name="id">The Id of the entity.</param>
    /// <param name="detach">Whether to detach the entity from the context.</param>
    /// <returns>The database context.</returns>
    public static DbContext RemoveById<TEntity>([DisallowNull] this DbContext dbContext, [DisallowNull] long id, bool detach = false)
            where TEntity : class, IIdenticalEntity<long>, new()
            => RemoveById<TEntity>(dbContext, [id], detach);

    /// <summary>
    /// Removes an entity from the database by its Id.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the database context.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TId">The type of the entity's Id.</typeparam>
    /// <param name="dbContext">The database context.</param>
    /// <param name="id">The Id of the entity to remove.</param>
    /// <param name="detach">Whether to detach the entity from the context.</param>
    /// <returns>The database context.</returns>
    public static DbContext RemoveById<TDbContext, TEntity, TId>([DisallowNull] DbContext dbContext, [DisallowNull] TId id, bool detach = false)
        where TEntity : class, IIdenticalEntity<TId>, new()
        where TId : notnull =>
        RemoveById<TEntity, TId>(dbContext, [id], detach);

    public static DbContext RemoveById<TDbContext, TEntity>([DisallowNull] DbContext dbContext, [DisallowNull] Guid id, bool detach = false)
        where TEntity : class, IIdenticalEntity<Guid>, new() =>
        RemoveById<TEntity, Guid>(dbContext, [id], detach);

    /// <summary>
    /// Resets the changes of the given DbContext and optionally disposes the current transaction.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the DbContext.</typeparam>
    /// <param name="dbContext">The DbContext.</param>
    /// <param name="disposeTransaction">True to dispose the current transaction, false otherwise.</param>
    /// <returns>The DbContext.</returns>
    public static TDbContext ResetChanges<TDbContext>(this TDbContext dbContext, bool disposeTransaction = true)
            where TDbContext : DbContext
    {
        Check.MustBeArgumentNotNull(dbContext);
        dbContext.ChangeTracker.Clear();
        return disposeTransaction ? DisposeCurrentTransaction(dbContext) : dbContext;
    }

    /// <summary>
    /// Saves changes to the database asynchronously for the given entity entry, optionally
    /// accepting all changes on success.
    /// </summary>
    public static Task<int> SaveChangesAsync(this EntityEntry entityEntry, [DisallowNull] bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
            => entityEntry?.Context.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken)!;

    /// <summary>
    /// Saves changes to the database asynchronously for the given entity entry.
    /// </summary>
    public static Task<Result<int>> SaveChangesResultAsync(this EntityEntry entityEntry, CancellationToken cancellationToken = default)
        => entityEntry?.Context.SaveChangesResultAsync(cancellationToken)!;

    /// <summary> Saves changes to the database and returns a Result<int> containing the number of
    /// changes saved or an exception message and the exception itself. </summary> <param
    /// name="dbContext">The DbContext to save changes to.</param> <param
    /// name="cancellationToken">The CancellationToken to use.</param> <returns>A Result<int>
    /// containing the number of changes saved or an exception message and the exception itself.</returns>
    public static async Task<Result<int>> SaveChangesResultAsync<TDbContext>(this TDbContext dbContext, CancellationToken cancellationToken = default)
                where TDbContext : DbContext
    {
        Check.MustBeArgumentNotNull(dbContext);

        try
        {
            var saveResult = await dbContext.SaveChangesAsync(cancellationToken);
            var result = Result.Success(saveResult);
            return result;
        }
        catch (Exception ex)
        {
            var result = Result.Fail<int>(ex);
            return result;
        }
    }

    public static EntityEntry<TEntity> SetEntryModified<TEntity>(
            this EntityEntry<TEntity> entityEntry,
            bool isModified = true)
            where TEntity : class
    {
        Check.MustBeArgumentNotNull(entityEntry);
        entityEntry.State = isModified ? EntityState.Modified : EntityState.Unchanged;

        return entityEntry;
    }

    /// <summary>
    /// Sets the IsModified flag of a property on an EntityEntry.
    /// </summary>
    /// <param name="entityEntry">The EntityEntry to set the IsModified flag on.</param>
    /// <param name="propertyExpression">The property to set the IsModified flag on.</param>
    /// <param name="isModified">The value to set the IsModified flag to.</param>
    /// <returns>The EntityEntry with the IsModified flag set.</returns>
    public static EntityEntry<TEntity> SetModified<TEntity, TProperty>(
            this EntityEntry<TEntity> entityEntry,
            in Expression<Func<TEntity, TProperty>> propertyExpression,
            bool isModified = true)
            where TEntity : class
    {
        Check.MustBeArgumentNotNull(entityEntry);

        entityEntry.Property(propertyExpression).IsModified = isModified;
        return entityEntry;
    }

    /// <summary>
    /// Sets the state of the entity with a Guid identifier in the given DbContext.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the database context.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="dbContext">The database context.</param>
    /// <param name="entity">The entity.</param>
    /// <param name="state">The state.</param>
    /// <returns>The given DbContext.</returns>
    public static TDbContext SetStateGuidOf<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] in TEntity entity, [DisallowNull] in EntityState state)
            where TDbContext : notnull, DbContext
            where TEntity : class, IIdenticalEntity<Guid>
    {
        Check.MustBeArgumentNotNull(dbContext);
        Check.MustBeArgumentNotNull(entity);

        if (GetLocalGuidEntry(dbContext, entity) is { } entry && entry.State != state)
        {
            entry.State = state;
        }
        return dbContext;
    }

    /// <summary>
    /// Sets the state of the specified entity in the specified DbContext.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the DbContext.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="dbContext">The DbContext.</param>
    /// <param name="entity">The entity.</param>
    /// <param name="state">The state.</param>
    /// <returns>The DbContext.</returns>
    public static TDbContext SetStateOf<TDbContext, TEntity>([DisallowNull] this TDbContext dbContext, [DisallowNull] in TEntity entity, [DisallowNull] in EntityState state)
            where TDbContext : notnull, DbContext
            where TEntity : class, IIdenticalEntity<long>
    {
        Check.MustBeArgumentNotNull(dbContext);
        Check.MustBeArgumentNotNull(entity);

        if (GetLocalEntry(dbContext, entity) is { } entry && entry.State != state)
        {
            entry.State = state;
        }
        return dbContext;
    }

    /// <summary>
    /// Sets the state of the entity in the given DbContext.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the database context.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TId">The type of the entity's identifier.</typeparam>
    /// <param name="dbContext">The database context.</param>
    /// <param name="entity">The entity.</param>
    /// <param name="state">The state.</param>
    /// <returns>The given DbContext.</returns>
    public static TDbContext SetStateOf<TDbContext, TEntity, TId>([DisallowNull] TDbContext dbContext, [DisallowNull] in TEntity entity, [DisallowNull] in EntityState state)
            where TDbContext : notnull, DbContext
            where TEntity : class, IIdenticalEntity<TId>
            where TId : notnull
    {
        Check.MustBeArgumentNotNull(dbContext);
        Check.MustBeArgumentNotNull(entity);

        if (GetLocalEntry<TDbContext, TEntity, TId>(dbContext, entity) is { } entry && entry.State != state)
        {
            entry.State = state;
        }
        return dbContext;
    }

    /// <summary>
    /// Disposes the current transaction of the given DbContext.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the DbContext.</typeparam>
    /// <param name="dbContext">The DbContext.</param>
    /// <returns>The given DbContext.</returns>
    private static TDbContext DisposeCurrentTransaction<TDbContext>([DisallowNull] this TDbContext dbContext)
            where TDbContext : DbContext
    {
        Check.MustBeArgumentNotNull(dbContext);
        dbContext.Database?.CurrentTransaction?.Dispose();
        return dbContext;
    }
}