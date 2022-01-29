namespace Library.Interfaces;
public interface IService
{
}

public interface IReadAsyncService<TViewModel, in TId> : IService
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

public interface IReadAsyncService<TViewModel> : IReadAsyncService<TViewModel, long> { }

public interface IReadAsyncPagingService<TViewModel, in TId> : IService
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
public interface IReadAsyncPagingService<TViewModel> : IReadAsyncPagingService<TViewModel, long> { }

public interface IWriteAsyncService<in TViewModel, TId> : IService
{
    /// <summary>
    /// Deletes an entity asynchronously.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    Task<bool> DeleteAsync(TId id, bool persist = true);

    /// <summary>
    /// Inserts an entity asynchronously.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    Task<TId> InsertAsync(TViewModel model, bool persist = true);

    /// <summary>
    /// Updates an entity asynchronously.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    Task<TId> UpdateAsync(TId id, TViewModel model, bool persist = true);
}

public interface IWriteAsyncService<in TViewModel> : IWriteAsyncService<TViewModel, long> { }

public interface IAsyncViewModelCrudService<TViewModel, TId>
    : IService, IReadAsyncService<TViewModel, TId>, IWriteAsyncService<TViewModel, TId>
{
}

public interface IAsyncViewModelCrudService<TViewModel> : IAsyncViewModelCrudService<TViewModel, long>
    , IService, IReadAsyncService<TViewModel>, IWriteAsyncService<TViewModel>
{ }

public interface IResetChanges
{
    void ResetChanges();
}

public interface ILazySaveService : IService, IResetChanges
{
    /// <summary>
    /// Saves the data asynchronously.
    /// </summary>
    /// <returns></returns>
    Task SaveChangesAsync();
}

public interface ILazySaveService<in TParam> : IService, IResetChanges
{
    /// <summary>
    /// Saves  the enity asynchronously.
    /// </summary>
    /// <param name="entity">The model.</param>
    /// <returns></returns>
    Task SaveAsync(TParam entity);
}

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

public interface IViewModelFill<TViewModel>
{
    /// <summary>
    /// Fills the view model.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    Task<TViewModel?> FillViewModelAsync(TViewModel? model);
}

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
    Task<IEnumerable<TDbEntity>> GetChildEnitiesAsync(TDbEntity entity);

    Task<IEnumerable<TDbEntity>> GetChildEnitiesByIdAsync(long parentId);

    Task<TDbEntity?> GetParentEntityAsync(long childId);

    Task<IEnumerable<TDbEntity>> GetRootEnitiesAsync();
}

public interface IHierarchicalViewModelService<TViewModel> : IService
{
    IEnumerable<TViewModel> GetChildModels(in TViewModel model);

    TViewModel GetParentModel(in TViewModel model);

    IEnumerable<TViewModel> GetRootModels();
}

public record PagingParams(in int PageIndex = 0, in int? PageSize = null);
public record PagingResult<T>(IReadOnlyList<T> Result, in PagingParams? Paging, in long TotalCount);