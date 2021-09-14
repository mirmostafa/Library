using Library.Data.Markers;
using Library.Interfaces;

namespace Library.Helpers;
public static class ServiceHelper
{
    public static async Task<long> SaveAsync<TViewModel>(this IWriteAsyncService<TViewModel> service, TViewModel? model, bool persist = true)
        where TViewModel : IHasId<long?>
    {
        Check.IfArgumentNotNull(service, nameof(service));
        Check.IfArgumentNotNull(model, nameof(model));
        return model.Id is { } id
            ? await service.UpdateAsync(id, model, persist)
            : await service.InsertAsync(model, persist);
    }
}
