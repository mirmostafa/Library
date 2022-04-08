using Library.Data.Markers;
using Library.Interfaces;
using Library.Types;
using Library.Validations;
using Microsoft.EntityFrameworkCore.Storage;

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

    public static async Task<TViewModel> SumbitChangesAsync<TViewModel>(this IAsyncWriteService<TViewModel, Guid> service, TViewModel model, bool persist = true)
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

        return !model.Id.IsNullOrEmpty()
            ? await service.UpdateAsync(model.Id, model, persist)
            : await service.InsertAsync(model, persist);
    }

    public static async Task<int> CommitChangesAsync<TService>(this TService service, bool persist = true, IDbContextTransaction? transaction = null)
        where TService : IAsyncSaveService, IResetChanges
    {
        Check.IfArgumentNotNull(service);

        int result;
        if (persist)
        {
            if (transaction is not null)
            {
                await transaction.CommitAsync();
            }

            result = await service.SaveChangesAsync();
            service.ResetChanges();
        }
        else
        {
            result = -1;
        }

        return result;
    }

}
