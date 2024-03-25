using System.Diagnostics;
using System.Runtime.CompilerServices;

using Library.Interfaces;
using Library.Threading;
using Library.Validations;

using Microsoft.EntityFrameworkCore;

namespace Library.Helpers;

[DebuggerStepThrough]
[StackTraceHidden]
public static class QueryableHelper
{
    /// <summary>
    /// Retrieves the first element of a sequence, or a default value if the sequence contains no elements.
    /// </summary>
    /// <param name="query">The sequence to return the first element of.</param>
    /// <param name="asyncLock">The async lock.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the first
    /// element in the sequence, or default(TResult) if the sequence contains no elements.
    /// </returns>
    public static Task<TResult?> FirstOrDefaultLockAsync<TResult>(this IQueryable<TResult> query, IAsyncLock asyncLock, CancellationToken cancellationToken = default)
    {
        Check.MustBeArgumentNotNull(query);
        Check.MustBeArgumentNotNull(asyncLock);
        return asyncLock.LockAsync(async () => await query.FirstOrDefaultAsync(cancellationToken));
    }

    /// <summary>
    /// Converts an IQueryable to an IEnumerable with the option to cancel the operation.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the IQueryable.</typeparam>
    /// <param name="query">The IQueryable to convert.</param>
    /// <param name="cancellationToken">A CancellationToken to cancel the operation.</param>
    /// <returns>An IEnumerable containing the elements of the IQueryable.</returns>
    public static IEnumerable<T> ToEnumerable<T>(this IQueryable<T> query, CancellationToken cancellationToken = default)
    {
        if (query is null)
        {
            yield break;
        }
        var enumerator = query.GetEnumerator();
        while (!cancellationToken.IsCancellationRequested && enumerator.MoveNext())
        {
            yield return enumerator.Current;
        }
    }

    /// <summary>
    /// Converts an IQueryable to an IAsyncEnumerable with the given CancellationToken.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the IQueryable.</typeparam>
    /// <param name="query">The IQueryable to convert.</param>
    /// <param name="cancellationToken">The CancellationToken to use.</param>
    /// <returns>An IAsyncEnumerable containing the elements of the IQueryable.</returns>
    public static async IAsyncEnumerable<TSource> ToEnumerableAsync<TSource>(this IQueryable<TSource> query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Check.MustBeArgumentNotNull(query);
        await foreach (var item in query.AsAsyncEnumerable().WithCancellation(cancellationToken))
        {
            yield return item;
        }
    }

    /// <summary>
    /// Asynchronously locks a queryable and returns a list of results.
    /// </summary>
    public static Task<List<TResult>> ToListLockAsync<TResult>(this IQueryable<TResult> query, IAsyncLock asyncLock)
        => asyncLock.ArgumentNotNull().LockAsync(async () => await query.ToListAsync());

    /// <summary>
    /// Gets a paged list of items from a queryable source.
    /// </summary>
    /// <param name="query">The queryable source.</param>
    /// <param name="paging">The paging parameters.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paged list of items.</returns>
    public static async Task<PagingResult<T>> ToListPagingAsync<T>(this IQueryable<T> query, PagingParams paging, CancellationToken cancellationToken = default)
    {
        if (paging?.PageSize is null or 0)
        {
            var dbNoPagingResult = await query.ToListAsync(cancellationToken: cancellationToken);
            var t = await query.CountAsync(cancellationToken: cancellationToken);
            return new(dbNoPagingResult, t);
        }

        Check.MustBe(paging.PageIndex >= 0, () => new ArgumentOutOfRangeException(nameof(paging)));

        var total = await query.CountAsync(cancellationToken: cancellationToken);
        var skip = paging.PageIndex * paging.PageSize.Value;
        var take = paging.PageSize.Value;
        var dbResult = await query.Take(take..skip).ToListAsync(cancellationToken: cancellationToken);
        var pagingResult = new PagingResult<T>(dbResult, total);

        return pagingResult;
    }
}