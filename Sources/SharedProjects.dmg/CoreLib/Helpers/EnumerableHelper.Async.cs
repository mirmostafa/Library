using System;
using System.Collections.Generic;
using Mohammad.Threading;

namespace Mohammad.Helpers
{
    partial class EnumerableHelper
    {
        [Obsolete]
        public static void ForEachAsync<TSource>(this IEnumerable<TSource> source, Action<TSource> actor)
        {
            ForEachAsync(source, actor, null);
        }

        [Obsolete]
        public static void ForEachAsync<TSource>(this IEnumerable<TSource> source, Action<TSource> actor, Action ended)
        {
            ForEachAsync(source, actor, true, ended);
        }

        [Obsolete]
        public static void ForEachAsync<TSource>(this IEnumerable<TSource> source, Action<TSource> actor, bool join, Action ended)
        {
            ForEachAsync(source, actor, 2, @join, ended);
        }

        [Obsolete]
        public static void ForEachAsync<TSource>(this IEnumerable<TSource> source, Action<TSource> actor, int threadCount)
        {
            ForEachAsync(source, actor, threadCount, true, null);
        }

        [Obsolete]
        public static void ForEachAsync<TSource>(this IEnumerable<TSource> source, Action<TSource> actor, int threadCount, bool join, Action ended)
        {
            Async.ForEach(source, actor, threadCount, @join, ended);
        }
    }
}