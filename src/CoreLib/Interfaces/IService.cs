#pragma warning disable CA1040 // Avoid empty interfaces

using Library.Results;

using Microsoft.EntityFrameworkCore.Storage;

namespace Library.Interfaces;

/// <summary>
/// Interface for an asynchronous CRUD service that provides read and write operations for a view
/// model type with an ID type.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
/// <typeparam name="TId">The type of the identifier.</typeparam>
/// <seealso cref="IAsyncRead&lt;TViewModel, TId&gt;"/>
/// <seealso cref="IAsyncWrite&lt;TViewModel, TId&gt;"/>
public interface IAsyncCrud<TViewModel, TId> : IAsyncRead<TViewModel, TId>, IAsyncWrite<TViewModel, TId>;

/// <summary>
/// Represents an interface for an asynchronous CRUD service that provides read and write operations
/// for a view model.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
/// <seealso cref="IAsyncRead&lt;TViewModel, TId&gt;"/>
/// <seealso cref="IAsyncWrite&lt;TViewModel, TId&gt;"/>
public interface IAsyncCrud<TViewModel> : IAsyncRead<TViewModel>, IAsyncWrite<TViewModel>;

/// <summary>
/// Interface for an asynchronous read paging service.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
/// <typeparam name="TId">The type of the identifier.</typeparam>
public interface IAsyncPagingRead<TViewModel, in TId>
{
    /// <summary>
    /// Retrieves a paged result of view models asynchronously.
    /// </summary>
    /// <param name="paging">The paging parameters.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    AsyncPagingResult<TViewModel> GetAllAsync(PagingParams paging, CancellationToken token = default);

    /// <summary>
    /// Asynchronously retrieves a view model by its ID.
    /// </summary>
    /// <param name="id">The ID of the view model to retrieve.</param>
    /// <param name="token">A cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<TViewModel?> GetByIdAsync(TId id, CancellationToken token = default);
}

/// <summary>
/// Represents an asynchronous read paging service for a specific type of view model.
/// </summary>
public interface IAsyncPagingRead<TViewModel> : IAsyncPagingRead<TViewModel, long>;

public interface IAsyncRead<TViewModel, in TId>
{
    /// <summary>
    /// Asynchronously retrieves a list of view models.
    /// </summary>
    /// <param name="token">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<IReadOnlyList<TViewModel>> GetAllAsync(CancellationToken token = default);

    //IAsyncEnumerable<TViewModel> GetAllAsync(CancellationToken token = default);

    /// <summary>
    /// Asynchronously retrieves a view model by its ID.
    /// </summary>
    /// <param name="id">The ID of the view model to retrieve.</param>
    /// <param name="token">A cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<TViewModel?> GetByIdAsync(TId id, CancellationToken token = default);
}

/// <summary>
/// A standardizer for services to read data asynchronously.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
public interface IAsyncRead<TViewModel> : IAsyncRead<TViewModel, long>;

public interface IAsyncSaveChanges
{
    /// <summary>
    /// Saves the data asynchronously.
    /// </summary>
    Task<Result<int>> SaveChangesAsync(CancellationToken token = default);
}

public interface IAsyncTransactional
{
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken token = default);

    Task<Result> CommitTransactionAsync(CancellationToken token = default);

    Task RollbackTransactionAsync(CancellationToken token = default);
}

public interface IAsyncTransactionSave : IAsyncTransactional, IAsyncSaveChanges, IResetChanges;

/// <summary>
/// A standardizer for services to write data asynchronously.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
/// <typeparam name="TId">The type of the identifier.</typeparam>
public interface IAsyncWrite<TViewModel, TId>
{
    /// <summary>
    /// Deletes an entity asynchronously.
    /// </summary>
    Task<Result<int>> DeleteAsync(TViewModel model, bool persist = true, CancellationToken token = default);

    /// <summary>
    /// Inserts an entity asynchronously.
    /// </summary>
    /// <param name="model">The model.</param>
    Task<Result<TViewModel>> InsertAsync(TViewModel model, bool persist = true, CancellationToken token = default);

    /// <summary>
    /// Updates an entity asynchronously.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="model">The model.</param>
    Task<Result<TViewModel>> UpdateAsync(TId id, TViewModel model, bool persist = true, CancellationToken token = default);
}

/// <summary>
/// A standardizer for services to write data asynchronously.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
public interface IAsyncWrite<TViewModel> : IAsyncWrite<TViewModel, long>;

public interface IBusinessService : IService;

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
    //IEnumerable<TViewModel?> ToViewModel(IEnumerable<TDbEntity?> entities);
    IEnumerable<TViewModel?> ToViewModel(IEnumerable<TDbEntity?> entities) =>
        entities.Select(this.ToViewModel);

    /// <summary>
    /// Create a new model from the database entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    [return: NotNullIfNotNull(nameof(entity))]
    TViewModel? ToViewModel(TDbEntity? entity);
}

public interface IHierarchicalDbEntityActor<TDbEntity>
{
    /// <summary>
    /// Gets the all child entities of specific entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity.</param>
    IAsyncEnumerable<TDbEntity> GetChildEntitiesAsync(TDbEntity entity, CancellationToken token = default);

    /// <summary>
    /// Gets the child entities by a specific identifier asynchronously.
    /// </summary>
    /// <param name="parentId">The parent identifier.</param>
    IAsyncEnumerable<TDbEntity> GetChildEntitiesByIdAsync(long parentId, CancellationToken token = default);

    /// <summary>
    /// Gets the parent entity asynchronously.
    /// </summary>
    /// <param name="childId">The child identifier.</param>
    Task<TDbEntity?> GetParentEntityAsync(long childId, CancellationToken token = default);

    /// <summary>
    /// Gets the root entities asynchronously.
    /// </summary>
    IAsyncEnumerable<TDbEntity> GetRootEntitiesAsync(CancellationToken token = default);
}

public interface IHierarchicalViewModelActor<TViewModel>
{
    /// <summary>
    /// Gets the child models.
    /// </summary>
    /// <param name="model">The model.</param>
    IEnumerable<TViewModel> GetChildModels(in TViewModel model);

    /// <summary>
    /// Gets the parent model.
    /// </summary>
    /// <param name="model">The model.</param>
    TViewModel GetParentModel(in TViewModel model);

    /// <summary>
    /// Gets the root models.
    /// </summary>
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
public interface IService;

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
    IEnumerable<TDbEntity?> ToDbEntity(IEnumerable<TViewModel?> models) =>
        models.Select(this.ToDbEntity);

    /// <summary>
    /// Converts the model to database entity.
    /// </summary>
    /// <param name="model">The model.</param>
    [return: NotNullIfNotNull(nameof(model))]
    TDbEntity? ToDbEntity(TViewModel? model);
}

public record PagingParams(in int PageIndex = 0, in int? PageSize = null);

public record PagingResult<T>(IReadOnlyList<T> Result, in long TotalCount);
public record AsyncPagingResult<T>(IAsyncEnumerable<T> Result, in long TotalCount);

public interface IAsyncCreator<TViewModel>
{
    Task<TViewModel> CreateAsync(CancellationToken token = default);
}

#pragma warning restore CA1040 // Avoid empty interfaces
