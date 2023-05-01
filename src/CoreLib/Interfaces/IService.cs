using Library.Results;

using Microsoft.EntityFrameworkCore.Storage;

namespace Library.Interfaces;

/// <summary>
/// A standardizer for services to CRUD data asynchronously.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
/// <typeparam name="TId">The type of the identifier.</typeparam>
/// <seealso cref="IAsyncReadService&lt;TViewModel, TId&gt;"/>
/// <seealso cref="IAsyncWriteService&lt;TViewModel, TId&gt;"/>
public interface IAsyncCrudService<TViewModel, TId> : IAsyncReadService<TViewModel, TId>, IAsyncWriteService<TViewModel, TId>
{
}

/// <summary>
/// A standardizer for services to CRUD data asynchronously.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
/// <seealso cref="IAsyncReadService&lt;TViewModel, TId&gt;"/>
/// <seealso cref="IAsyncWriteService&lt;TViewModel, TId&gt;"/>
public interface IAsyncCrudService<TViewModel> : IAsyncReadService<TViewModel>, IAsyncWriteService<TViewModel>
{ }

/// <summary>
/// A standardizer for services to read data asynchronously which supports pagination.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
/// <typeparam name="TId">The type of the identifier.</typeparam>
public interface IAsyncReadPagingService<TViewModel, in TId>
{
    /// <summary>
    /// Gets all db entities asynchronously.
    /// </summary>
    /// <returns></returns>
    Task<PagingResult<TViewModel>> GetAllAsync(PagingParams paging);

    /// <summary>
    /// Gets an entity by identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    Task<TViewModel?> GetByIdAsync(TId id);
}

/// <summary>
/// A standardizer for services to read data asynchronously which supports pagination.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
public interface IAsyncReadPagingService<TViewModel> : IAsyncReadPagingService<TViewModel, long>
{ }

/// <summary>
/// A standardizer for services to read data asynchronously.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
/// <typeparam name="TId">The type of the identifier (long, <see cref="Guid"/>, Id, ...).</typeparam>
public interface IAsyncReadService<TViewModel, in TId>
{
    /// <summary>
    /// Gets all <typeparamref name="TViewModel"/> s asynchronously.
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyList<TViewModel>> GetAllAsync();

    /// <summary>
    /// Gets an <typeparamref name="TViewModel"/> by identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    Task<TViewModel?> GetByIdAsync(TId id);
}

/// <summary>
/// A standardizer for services to read data asynchronously.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
public interface IAsyncReadService<TViewModel> : IAsyncReadService<TViewModel, long>
{ }

/// <summary>
/// <br/>
/// </summary>
public interface IAsyncSaveService
{
    /// <summary>
    /// Saves the data asynchronously.
    /// </summary>
    /// <returns></returns>
    Task<Result<int>> SaveChangesAsync();
}

public interface IAsyncTransactionalService
{
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task<Result> CommitTransactionAsync(CancellationToken cancellationToken = default);

    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}

public interface IAsyncTransactionSaveService : IAsyncTransactionalService, IAsyncSaveService, IResetChanges
{
}

///// <summary>
///// </summary>
///// <typeparam name="TViewModel">The type of the view model.</typeparam>
//public interface IAsyncViewModelFiller<TViewModel>
//{
//    /// <summary>
//    /// Fills the view model.
//    /// </summary>
//    /// <param name="model">The model.</param>
//    /// <returns></returns>
//    Task<TViewModel?> FillViewModelAsync(TViewModel? model);
//}

/// <summary>
/// A standardizer for services to write data asynchronously.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
/// <typeparam name="TId">The type of the identifier.</typeparam>
public interface IAsyncWriteService<TViewModel, TId>
{
    /// <summary>
    /// Deletes an entity asynchronously.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    Task<Result> DeleteAsync(TViewModel model, bool persist = true);

    /// <summary>
    /// Inserts an entity asynchronously.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    Task<Result<TViewModel>> InsertAsync(TViewModel model, bool persist = true);

    /// <summary>
    /// Updates an entity asynchronously.
    /// </summary>
    /// <param name="id">   The identifier.</param>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    Task<Result<TViewModel>> UpdateAsync(TId id, TViewModel model, bool persist = true);
}

/// <summary>
/// A standardizer for services to write data asynchronously.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
public interface IAsyncWriteService<TViewModel> : IAsyncWriteService<TViewModel, long>
{ }

public interface IBusinessService : IService
{
}

/// <summary>
/// Database entity to view model converter
/// </summary>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
/// <typeparam name="TDbEntity">The type of the database entity.</typeparam>
public interface IDbEntityToViewModelConverter<out TViewModel, in TDbEntity>
{
    /// <summary>
    /// Creates a set of models from the database entity.
    /// </summary>
    /// <param name="entities">The entity.</param>
    /// <returns></returns>
    IEnumerable<TViewModel?> ToViewModel(IEnumerable<TDbEntity?> entities);

    /// <summary>
    /// Create a new model from the database entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    TViewModel? ToViewModel(TDbEntity? entity);
}

public interface IHierarchicalDbEntityService<TDbEntity>
{
    /// <summary>
    /// Gets the all child entities of specific entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    IAsyncEnumerable<TDbEntity> GetChildEntitiesAsync(TDbEntity entity);

    /// <summary>
    /// Gets the child entities by a specific identifier asynchronously.
    /// </summary>
    /// <param name="parentId">The parent identifier.</param>
    /// <returns></returns>
    IAsyncEnumerable<TDbEntity> GetChildEntitiesByIdAsync(long parentId);

    /// <summary>
    /// Gets the parent entity asynchronously.
    /// </summary>
    /// <param name="childId">The child identifier.</param>
    /// <returns></returns>
    Task<TDbEntity?> GetParentEntityAsync(long childId);

    /// <summary>
    /// Gets the root entities asynchronously.
    /// </summary>
    /// <returns></returns>
    IAsyncEnumerable<TDbEntity> GetRootEntitiesAsync();
}

public interface IHierarchicalViewModelService<TViewModel>
{
    /// <summary>
    /// Gets the child models.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    IEnumerable<TViewModel> GetChildModels(in TViewModel model);

    /// <summary>
    /// Gets the parent model.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    TViewModel GetParentModel(in TViewModel model);

    /// <summary>
    /// Gets the root models.
    /// </summary>
    /// <returns></returns>
    IEnumerable<TViewModel> GetRootModels();
}

/// <summary>
/// Supporting to clean tracked entities.
/// </summary>
public interface IResetChanges
{
    /// <summary>
    /// Resets the changes.
    /// </summary>
    void ResetChanges();
}

/// <summary>
/// A base interface for all services declared in the application
/// </summary>
public interface IService
{
}

/// <summary>
/// View model to database entity converter.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
/// <typeparam name="TDbEntity">The type of the database entity.</typeparam>
public interface IViewModelToDbEntityConverter<in TViewModel, out TDbEntity>
{
    /// <summary>
    /// Converts the models to database entities.
    /// </summary>
    /// <param name="models">The model.</param>
    /// <returns></returns>
    IEnumerable<TDbEntity?> ToDbEntity(IEnumerable<TViewModel?> models);

    /// <summary>
    /// Converts the model to database entity.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    TDbEntity? ToDbEntity(TViewModel? model);
}

/// <summary>
/// </summary>
/// <seealso cref="IEquatable&lt;PagingParams&gt;"/>
public record PagingParams(in int PageIndex = 0, in int? PageSize = null);

/// <summary>
///
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="System.IEquatable&lt;Library.Interfaces.PagingResult&lt;T&gt;&gt;" />
public record PagingResult<T>(IReadOnlyList<T> Result, in long TotalCount);

public interface IAsyncCreator<TViewModel>
{
    Task<TViewModel> CreateAsync();
}