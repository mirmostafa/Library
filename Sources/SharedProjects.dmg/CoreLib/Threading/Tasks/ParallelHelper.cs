using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mohammad.Threading.Tasks
{
    public static class ParallelHelper
    {
        public static void ForProcessor(int fromInclusive, int toExclusive, Action<int> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            Parallel.For(fromInclusive, toExclusive, new ParallelOptions {MaxDegreeOfParallelism = Environment.ProcessorCount}, action);
        }

        /// <summary>
        ///     Divides the source to the count of processor and dispatches items between them.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="action">The action.</param>
        /// <exception cref="System.ArgumentNullException">action</exception>
        public static void ForEachProcessor<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            Parallel.ForEach(source, new ParallelOptions {MaxDegreeOfParallelism = Environment.ProcessorCount}, action);
        }

        /// <summary>
        ///     Creates tasks in the count if source items, runs them and wait for all, to be done.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="function">The function.</param>
        /// <returns></returns>
        public static List<TResult> ForEachTask<T, TResult>(this IEnumerable<T> source, Func<T, TResult> function)
        {
            return ForEachAsync(source, function).Result;
        }

        public static void ForEachTask<T>(this IEnumerable<T> source, Action<T> function) { ForEachAsync(source, function).Wait(); }

        /// <summary>
        ///     Creates tasks in the count if source items, runs them. But doesn't wait for all, to be done.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="function">The function.</param>
        /// <returns></returns>
        public static async Task<List<TResult>> ForEachAsync<T, TResult>(this IEnumerable<T> source, Func<T, TResult> function)
        {
            var tasks = source.Select(item1 => Task.Run(() => function(item1))).ToList();
            var result = await Task.WhenAll(tasks);
            return result.ToList();
        }

        /// <summary>
        ///     Creates tasks in the count if source items, runs them and collects all of tasks in a single task.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public static Task ForEachAsync<T>(this IEnumerable<T> source, Action<T> action)
        {
            var tasks = source.Select(item1 => Task.Run(() => action(item1))).ToList();
            return Task.WhenAll(tasks);
        }

        public static void FastForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            var items = source as T[] ?? source.ToArray();
            Parallel.ForEach(Partitioner.Create(0, items.Length),
                range =>
                {
                    for (var i = range.Item1; i < range.Item2; i++)
                        action(items[i]);
                });
        }

        public static void FastWhere<T>(this IEnumerable<T> source, Func<T, bool> predictor, Action<T> action)
        {
            //var items = source as T[] ?? source.ToArray();
            //Parallel.ForEach(Partitioner.Create(0, items.Length),
            //    range =>
            //    {
            //        for (var i = range.Item1; i < range.Item2; i++)
            //            if (predictor(items[i]))
            //                action(items[i]);
            //    });

            Parallel.ForEach(Partitioner.Create(source),
                item =>
                {
                    if (predictor(item))
                        action(item);
                });
        }

        public static void FastSelect<TSource, TDestination>(this IEnumerable<TSource> source, Func<TSource, TDestination> selector, Action<TDestination> action)
        {
            Parallel.ForEach(Partitioner.Create(source), item => action(selector(item)));
        }

        public static void FastForEachBreak<T>(this IEnumerable<T> source, Func<T, bool> action)
        {
            var items = source as IList<T> ?? source.ToList();
            Parallel.ForEach(Partitioner.Create(0, items.Count),
                range =>
                {
                    for (var i = range.Item1; i < range.Item2; i++)
                        if (!action(items[i]))
                            break;
                });
        }

        public static void FastForEach<T>(this IEnumerable<T> source, Action<T, ParallelLoopState> action)
        {
            var items = source as IList<T> ?? source.ToList();
            Parallel.ForEach(Partitioner.Create(0, items.Count),
                (range, loopState) =>
                {
                    for (var i = range.Item1; i < range.Item2; i++)
                        action(items[i], loopState);
                });
        }

        public static TResult[] FastForEachFunc<T, TResult>(this IEnumerable<T> source, Func<T, TResult> action)
        {
            var items = source as IList<T> ?? source.ToList();
            var result = new TResult[items.Count];
            Parallel.ForEach(Partitioner.Create(0, items.Count),
                range =>
                {
                    for (var i = range.Item1; i < range.Item2; i++)
                        result[i] = action(items[i]);
                });
            return result;
        }

        public static void FastForEachFunc<TItem, TResult>(this IEnumerable<TItem> source, Func<TItem, TResult> action, Action<TResult> onGotResult)
        {
            var items = source as IList<TItem> ?? source.ToList();
            Parallel.ForEach(Partitioner.Create(0, items.Count),
                range =>
                {
                    for (var i = range.Item1; i < range.Item2; i++)
                        onGotResult(action(items[i]));
                });
        }

        public static TResult[] FastForEachFunc<T, TResult>(this IEnumerable<T> source, Func<T, ParallelLoopState, TResult> action)
        {
            var items = source as T[] ?? source.ToArray();
            var result = new TResult[items.Length];
            Parallel.ForEach(Partitioner.Create(0, items.Length),
                (range, loopState) =>
                {
                    for (var i = range.Item1; i < range.Item2; i++)
                        result[i] = action(items[i], loopState);
                });
            return result;
        }

        public static IEnumerable<TResult> FastForEachLazy<T, TResult>(this IEnumerable<T> source, Func<T, TResult> action) => from item in source.AsParallel()
                                                                                                                               select action(item);
    }
}