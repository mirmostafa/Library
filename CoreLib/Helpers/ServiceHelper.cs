using Library.Data.Markers;
using Library.Interfaces;
using Library.Types;
using Library.Validations;

namespace Library.Helpers;
public static class ServiceHelper
{
    public static async Task<TViewModel> SubmitChangesAsync<TViewModel>(this IAsyncWriteService<TViewModel> service, TViewModel? model, bool persist = true)
        where TViewModel : ICanSetKey<long?>
    {
        Check.IfArgumentNotNull(service);
        Check.IfArgumentNotNull(model);

        return model.Id is { } id
            ? await service.UpdateAsync(id, model, persist)
            : await service.InsertAsync(model, persist);
    }

    public static async Task<TViewModel> SubmitChangesAsync<TViewModel>(this IAsyncWriteService<TViewModel, Guid> service, TViewModel? model, bool persist = true)
        where TViewModel : ICanSetKey<Guid>
    {
        Check.IfArgumentNotNull(service);
        Check.IfArgumentNotNull(model);
        return !model.Id.IsNullOrEmpty()
            ? await service.UpdateAsync(model.Id, model, persist)
            : await service.InsertAsync(model, persist);
    }

    public static async Task<TViewModel> SubmitChangesAsync<TViewModel>(this IAsyncWriteService<TViewModel, Id> service, TViewModel? model, bool persist = true)
        where TViewModel : ICanSetKey<Id>
    {
        Check.IfArgumentNotNull(service);
        Check.IfArgumentNotNull(model);
        var result = !model.Id.IsNullOrEmpty()
            ? await service.UpdateAsync(model.Id, model, persist)
            : await service.InsertAsync(model, persist);
        return result;
    }

    public static async Task<int> SaveChangesAsync<TService>(this TService service, bool persist)
        where TService : IAsyncSaveService, IResetChanges
    {
        int result;
        if (persist)
        {
            result = await service.SaveChangesAsync();
            service.ResetChanges();
        }
        else
        {
            result = 0;
        }

        return result;
    }

}
