using Library.Results;

namespace Library.Interfaces;
/// <summary>
/// A base infatce for all services declared in the application
/// </summary>
public interface IService
{
}

/// <summary>
/// A standardizer for sercvies to read data asynchronously.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
/// <typeparam name="TId">The type of the identifier (long, Guid, Id, ...).</typeparam>
/// <seealso cref="Library.Interfaces.IService" />
public interface IAsyncReadService<TViewModel, in TId> : IService
{
    /// <summary>
    /// Gets all db entities asynchronously.
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyList<TViewModel>> GetAllAsync();

    /// <summary>
    /// Gets an entity by identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    Task<TViewModel?> GetByIdAsync(TId id);
}

/// <summary>
/// A standardizer for sercvies to read data asynchronously.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
/// <seealso cref="Library.Interfaces.IService" />
public interface IAsyncReadService<TViewModel> : IAsyncReadService<TViewModel, long> { }

/// <summary>
/// A standardizer for sercvies to read data asynchronously which supports pagination.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
/// <typeparam name="TId">The type of the identifier.</typeparam>
/// <seealso cref="Library.Interfaces.IService" />
public interface IAsyncReadPagingService<TViewModel, in TId> : IService
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
/// A standardizer for sercvies to read data asynchronously which supports pagination.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
/// <seealso cref="Library.Interfaces.IService" />
public interface IAsyncReadPagingService<TViewModel> : IAsyncReadPagingService<TViewModel, long> { }

/// <summary>
/// A standardizer for sercvies to write data asynchronously.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
/// <typeparam name="TId">The type of the identifier.</typeparam>
/// <seealso cref="Library.Interfaces.IService" />
public interface IAsyncWriteService<TViewModel, TId> : IService
{
    /// <summary>
    /// Inserts an entity asynchronously.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    Task<Result<TViewModel>> InsertAsync(TViewModel model, bool persist = true);

    /// <summary>
    /// Updates an entity asynchronously.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    Task<Result<TViewModel>> UpdateAsync(TId id, TViewModel model, bool persist = true);

    /// <summary>
    /// Deletes an entity asynchronously.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    Task<Result> DeleteAsync(TViewModel model, bool persist = true);
}

/// <summary>
/// A standardizer for sercvies to write data asynchronously.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
/// <seealso cref="Library.Interfaces.IService" />
public interface IAsyncWriteService<TViewModel> : IAsyncWriteService<TViewModel, long> { }

/// <summary>
/// A standardizer for sercvies to CRUD data asynchronously.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
/// <typeparam name="TId">The type of the identifier.</typeparam>
/// <seealso cref="Library.Interfaces.IService" />
/// <seealso cref="Library.Interfaces.IAsyncReadService&lt;TViewModel, TId&gt;" />
/// <seealso cref="Library.Interfaces.IAsyncWriteService&lt;TViewModel, TId&gt;" />
public interface IAsyncCrudService<TViewModel, TId>
    : IService, IAsyncReadService<TViewModel, TId>, IAsyncWriteService<TViewModel, TId>
{
}

/// <summary>
/// A standardizer for sercvies to CRUD data asynchronously.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
/// <seealso cref="Library.Interfaces.IService" />
/// <seealso cref="Library.Interfaces.IAsyncReadService&lt;TViewModel, TId&gt;" />
/// <seealso cref="Library.Interfaces.IAsyncWriteService&lt;TViewModel, TId&gt;" />
public interface IAsyncCrudService<TViewModel> : IAsyncCrudService<TViewModel, long>
    , IService, IAsyncReadService<TViewModel>, IAsyncWriteService<TViewModel>
{ }

/// <summary>
/// Supporting to clean tracked entities.
/// </summary>
public interface IResetChanges
{
    void ResetChanges();
}

/// <summary>
/// A service which supports lazy-loadind and clean tracked entities.
/// </summary>
/// <seealso cref="Library.Interfaces.IService" />
/// <seealso cref="Library.Interfaces.IResetChanges" />
public interface IAsyncSaveService : IService//x , IResetChanges
{
    /// <summary>
    /// Saves the data asynchronously.
    /// </summary>
    /// <returns></returns>
    Task<int> SaveChangesAsync();
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

/// <summary>
/// 
/// </summary>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
public interface IViewModelFill<TViewModel>
{
    /// <summary>
    /// Fills the view model.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    Task<TViewModel?> FillViewModelAsync(TViewModel? model);
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

public interface IHierarchicalDbEntityService<TDbEntity> : IService
{
    /// <summary>
    /// Gets the all child enities of specific entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    Task<IEnumerable<TDbEntity>> GetChildEnitiesAsync(TDbEntity entity);

    /// <summary>
    /// Gets the child enities by a specific identifier asynchronously.
    /// </summary>
    /// <param name="parentId">The parent identifier.</param>
    /// <returns></returns>
    Task<IEnumerable<TDbEntity>> GetChildEnitiesByIdAsync(long parentId);

    /// <summary>
    /// Gets the parent entity asynchronously.
    /// </summary>
    /// <param name="childId">The child identifier.</param>
    /// <returns></returns>
    Task<TDbEntity?> GetParentEntityAsync(long childId);

    /// <summary>
    /// Gets the root enities asynchronously.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<TDbEntity>> GetRootEnitiesAsync();
}

public interface IHierarchicalViewModelService<TViewModel> : IService
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
/// 
/// </summary>
/// <seealso cref="System.IEquatable&lt;Library.Interfaces.PagingParams&gt;" />
/// <seealso cref="System.IEquatable&lt;Library.Interfaces.PagingParams&gt;" />
public record PagingParams(in int PageIndex = 0, in int? PageSize = null);
/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="System.IEquatable&lt;Library.Interfaces.PagingResult&lt;T&gt;&gt;" />
public record PagingResult<T>(IReadOnlyList<T> Result, in long TotalCount);