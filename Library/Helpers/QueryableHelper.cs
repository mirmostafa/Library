using Library.Services;
using Library.Threading;
using Microsoft.EntityFrameworkCore;

namespace Library.Helpers
{
    public static class QueryableHelper
    {
        public static async Task<TResult?> FirstOrDefaultLockAsync<TResult>(this IQueryable<TResult> query, IAsyncLock asyncLock)
            => await asyncLock.ArgumentNotNull(nameof(asyncLock)).LockAsync(async () => await query.ArgumentNotNull(nameof(query)).FirstOrDefaultAsync());

        public static async IAsyncEnumerable<TResult> ToEnumerableAsync<TResult>(this IQueryable<TResult> query)
        {
            Check.IfArgumentNotNull(query, nameof(query));
            await foreach (var item in query.AsAsyncEnumerable())
            {
                yield return item;
            }
        }

        public static async Task<PagingResult<T>> ApplyPagingAsync<T>(this IQueryable<T> query, Paging? paging)
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

        public static async Task<IReadOnlyList<TResult>> ToListAsync<TResult>(this IAsyncEnumerable<TResult> items, CancellationToken cancellationToken = default)
        {
            var results = new List<TResult>();
            await foreach (var item in items.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                results.Add(item);
            }

            return results;
        }

        public static async Task<List<TResult>> ToListLockAsync<TResult>(this IQueryable<TResult> query, IAsyncLock asyncLock)
            => await asyncLock.ArgumentNotNull(nameof(asyncLock)).LockAsync(async () => await query.ArgumentNotNull(nameof(query)).ToListAsync());
    }
}
