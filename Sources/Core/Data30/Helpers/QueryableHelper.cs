using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Mohammad.Helpers
{
    public static class QueryableHelper
    {
        public static async IAsyncEnumerable<TSource> ToEnumerableAsync<TSource>(this IQueryable<TSource> source,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var element in source.AsAsyncEnumerable().WithCancellation(cancellationToken))
            {
                yield return element;
            }
        }
    }
}