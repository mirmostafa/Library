using System.Runtime.CompilerServices;
using Library.Interfaces;
using Library.Threading;
using Library.Validations;
using Microsoft.EntityFrameworkCore;

namespace Library.Helpers;
public static class QueryableHelper
{
    public static async Task<TResult?> FirstOrDefaultLockAsync<TResult>(this IQueryable<TResult> query, IAsyncLock asyncLock)
        => await asyncLock.ArgumentNotNull(nameof(asyncLock)).LockAsync(async () => await query.ArgumentNotNull(nameof(query)).FirstOrDefaultAsync());

    public static async Task<PagingResult<T>> ApplyPagingAsync<T>(this IQueryable<T> query, PagingParams? paging)
    {
        var total = await query.CountAsync();
        if (paging?.PageSize is null or 0)
        {
            var res1 = await query.ToListAsync();
            return new(res1, paging, total);
        }
        var index = paging.PageIndex;
        var size = paging.PageSize.Value;
        var result = await query.Skip(index * size).Take(size).ToListAsync();
        return new(result, paging, total);
    }

    public static async Task<List<TResult>> ToListLockAsync<TResult>(this IQueryable<TResult> query, IAsyncLock asyncLock)
        => await asyncLock.ArgumentNotNull(nameof(asyncLock)).LockAsync(async () => await query.ArgumentNotNull(nameof(query)).ToListAsync());

    public static async IAsyncEnumerable<TSource> AsEnumerableAsync<TSource>(this IQueryable<TSource> query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Check.IfArgumentNotNull(query, nameof(query));
        await foreach (var item in query.AsAsyncEnumerable().WithCancellation(cancellationToken))
        {
            yield return item;
        }
    }
    public static IEnumerable<T> ToEnumerable<T>(this IQueryable<T> query)
    {
        if (query is null)
        {
            yield break;
        }

        foreach (var item in query)
        {
            yield return item;
        }
    }

}
