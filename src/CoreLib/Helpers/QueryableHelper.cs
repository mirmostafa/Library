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

    public static async Task<PagingResult<T>> ToListPagingAsync<T>(this IQueryable<T> query, PagingParams? paging, CancellationToken cancellationToken = default)
    {
        var total = await query.CountAsync(cancellationToken: cancellationToken);

        if (paging is null or { PageSize: null or 0 })
        {
            var dbNoPagingResult = await query.ToListAsync(cancellationToken: cancellationToken);
            return new(dbNoPagingResult, total);
        }

        Check.IfArgumentBiggerThan(paging.PageIndex, 0);

        var skip = paging.PageIndex * paging.PageSize.Value;
        var take = paging.PageSize.Value;
        var dbResult = await query.Take(take..skip).ToListAsync(cancellationToken: cancellationToken);
        var pagingResult = new PagingResult<T>(dbResult, total);

        return pagingResult;
    }

    public static async Task<List<TResult>> ToListLockAsync<TResult>(this IQueryable<TResult> query, IAsyncLock asyncLock)
        => await asyncLock.ArgumentNotNull(nameof(asyncLock)).LockAsync(async () => await query.ToListAsync());

    public static async IAsyncEnumerable<TSource> ToEnumerableAsync<TSource>(this IQueryable<TSource> query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
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