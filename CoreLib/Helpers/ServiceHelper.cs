using Library.Data.Markers;
using Library.Interfaces;
using Library.Validations;

namespace Library.Helpers;
public static class ServiceHelper
{
    public static async Task<long?> SaveAsync<TViewModel>(this IWriteAsyncService<TViewModel> service, TViewModel? model, bool persist = true)
        where TViewModel : ICanSetKey<long?>
    {
        Check.IfArgumentNotNull(service);
        Check.IfArgumentNotNull(model);

        if (model.Id is { } id)
        {
            return await service.UpdateAsync(id, model, persist);
        }
        else
        {
            model.Id = await service.InsertAsync(model, persist);
            return model.Id;
        }
    }

    public static async Task<TId> SaveAsync<TViewModel, TId>(this IWriteAsyncService<TViewModel, TId> service, TViewModel? model, bool persist = true)
        where TViewModel : ICanSetKey<TId>
    {
        Check.IfArgumentNotNull(service);
        Check.IfArgumentNotNull(model);
        
        if (model.Id is { } id)
        {
            return await service.UpdateAsync(id, model, persist);
        }
        else
        {
            model.Id = await service.InsertAsync(model, persist);
            return model.Id;
        }
    }
}
