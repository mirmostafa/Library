


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using Mohammad.Collections.Generic;
using Mohammad.Collections.ObjectModel;

namespace Mohammad.Helpers
{
    /// <summary>
    ///     A utility to do some common tasks about iterations. No conflict with LINQ.
    /// </summary>
    [Guid("BE1ED9E9-160B-498D-984E-79876168BE35")]
    public static partial class EnumerableHelper
    {
        /// <summary>
        ///     Adds the first.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="list"> The list. </param>
        /// <param name="item"> The item. </param>
        /// <returns> </returns>
        public static T[] AddFirst<T>(this IList<T> list, T item)
        {
            var array = new T[list.Count + 1];
            array[0] = item;
            list.CopyTo(array, 1);
            return array;
        }

        /// <summary>
        ///     Adds the many.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="collection"> The collection. </param>
        /// <param name="newItems"> The new items. </param>
        /// <exception cref="ArgumentNullException">collection</exception>
        public static void AddMany<T>(this ICollection<T> collection, IEnumerable<T> newItems)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (newItems == null)
            {
                throw new ArgumentNullException(nameof(newItems));
            }

            foreach (var item in newItems)
            {
                collection.Add(item);
            }
        }

        public static void AddMany<T>(this ICollection<T> collection, params T[] newItems)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (newItems == null)
            {
                throw new ArgumentNullException(nameof(newItems));
            }

            foreach (var item in newItems)
            {
                collection.Add(item);
            }
        }

        public static void AddMany<T>(this List<T> collection, params T[] newItems)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (newItems == null)
            {
                throw new ArgumentNullException(nameof(newItems));
            }

            foreach (var item in newItems)
            {
                collection.Add(item);
            }
        }

        public static void AddMany(this IList list, IEnumerable newItems)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (newItems == null)
            {
                throw new ArgumentNullException(nameof(newItems));
            }

            foreach (var item in newItems)
            {
                list.Add(item);
            }
        }

        /// <summary>
        ///     Adds the many.
        /// </summary>
        /// <typeparam name="TKey"> The type of the key. </typeparam>
        /// <typeparam name="TValue"> The type of the value. </typeparam>
        /// <param name="dict"> The dict. </param>
        /// <param name="pairs"> The pairs. </param>
        /// <exception cref="ArgumentNullException">dict</exception>
        public static void AddMany<TKey, TValue>(this IDictionary<TKey, TValue> dict, IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            if (dict == null)
            {
                throw new ArgumentNullException(nameof(dict));
            }

            if (pairs == null)
            {
                throw new ArgumentNullException(nameof(pairs));
            }

            foreach (var pair in pairs)
            {
                dict.Add(pair);
            }
        }

        /// <summary>
        ///     Adds the many.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <typeparam name="TKey"> The type of the key. </typeparam>
        /// <typeparam name="TValue"> The type of the value. </typeparam>
        /// <param name="dict"> The dict. </param>
        /// <param name="newItems"> The new items. </param>
        /// <param name="key"> The key. </param>
        /// <param name="value"> The value. </param>
        /// <exception cref="ArgumentNullException">dict</exception>
        public static void AddMany<T, TKey, TValue>(this IDictionary<TKey, TValue> dict, IEnumerable<T> newItems, Func<T, TKey> key, Func<T, TValue> value)
        {
            if (dict == null)
            {
                throw new ArgumentNullException(nameof(dict));
            }

            if (newItems == null)
            {
                throw new ArgumentNullException(nameof(newItems));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            foreach (var item in newItems)
            {
                dict.Add(key(item), value(item));
            }
        }

        public static bool Any(this IEnumerable source) => Enumerable.Any(source.Cast<object>());

        /// <summary>
        ///     Ases the enuemrable.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="ts"> The ts. </param>
        /// <returns> </returns>
        public static IEnumerable<T> AsEnuemrable<T>(params T[] ts) => ts.AsEnumerable();

        /// <summary>
        ///     Ases the enuemrable.
        /// </summary>
        /// <param name="ts"> The ts. </param>
        /// <returns> </returns>
        public static IEnumerable AsEnuemrable(params object[] ts) => ts.AsEnumerable();

        public static IEnumerable AsEnuemrable(object source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var items = source as IEnumerable;
            if (items == null)
            {
                throw new InvalidCastException();
            }

            foreach (var item in items)
            {
                yield return item;
            }
        }

        public static IEnumerable<T> AsEnuemrable<T>(object source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var items = source as IEnumerable<T>;
            if (items == null)
            {
                throw new InvalidCastException();
            }

            foreach (var item in items)
            {
                yield return item;
            }
        }

        /// <summary>
        ///     Builds a tree.
        /// </summary>
        /// <typeparam name="TSource"> The type of the site map. </typeparam>
        /// <typeparam name="TItem"> The type of the menu item. </typeparam>
        /// <param name="rootElements"> The get root elements. </param>
        /// <param name="getNewItem"> The get new T menu item. </param>
        /// <param name="getChildren"> The get children. </param>
        /// <param name="addToRoots"> The add to roots. </param>
        /// <param name="addChild"> The add to children. </param>
        /// <exception cref="ArgumentNullException">getChildren</exception>
        public static void BuildTree<TSource, TItem>(this IEnumerable<TSource> rootElements,
            Func<TSource, TItem> getNewItem,
            Func<TSource, IEnumerable<TSource>> getChildren,
            Action<TItem> addToRoots,
            Action<TItem, TItem> addChild)
        {
            if (getChildren == null)
            {
                throw new ArgumentNullException(nameof(getChildren));
            }

            if (getNewItem == null)
            {
                throw new ArgumentNullException(nameof(getNewItem));
            }

            if (addToRoots == null)
            {
                throw new ArgumentNullException(nameof(addToRoots));
            }

            if (addChild == null)
            {
                throw new ArgumentNullException(nameof(addChild));
            }

            if (rootElements == null)
            {
                throw new ArgumentNullException(nameof(rootElements));
            }

            void AddChildren(TSource siteMap, TItem parent)
            {
                foreach (var sm in getChildren(siteMap))
                {
                    var newChile = getNewItem(sm);
                    addChild(parent, newChile);
                    AddChildren(sm, newChile);
                }
            }

            foreach (var siteMap in rootElements)
            {
                var root = getNewItem(siteMap);
                addToRoots(root);
                AddChildren(siteMap, root);
            }
        }

        /// <summary>
        ///     Converts all the values in a collection
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="input">The input collection.</param>
        /// <param name="converter">The converter function.</param>
        /// <returns>An enumerable collection of the output type.</returns>
        public static IEnumerable<TOutput> Cast<TInput, TOutput>(this IEnumerable<TInput> input, Converter<TInput, TOutput> converter)
        {
            if (converter == null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            if (input == null)
            {
                yield break;
            }

            foreach (var item in input)
            {
                yield return converter(item);
            }
        }

        /// <summary>
        ///     Converts all the values in a collection
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="input">The input collection.</param>
        /// <param name="converter">The converter function.</param>
        /// <returns>An enumerable collection of the output type.</returns>
        public static IEnumerable<TOutput> Cast<TOutput>(this IEnumerable input, Converter<object, TOutput> converter)
        {
            if (converter == null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            if (input == null)
            {
                yield break;
            }

            foreach (var item in input)
            {
                yield return converter(item);
            }
        }

        public static IEnumerable<T> CastOrNull<T>(this IEnumerable source)
            where T : class => Cast(source, item => item.As<T>());

        public static void CleanNulls<TSource>(this IList<TSource> source)
            where TSource : class => source.RemoveRange(t => t == null);

        public static IEnumerable<T> Combine<T>(this IEnumerable<T> items, params IEnumerable<T>[] others)
        {
            foreach (var item in items)
            {
                yield return item;
            }

            foreach (var other in others)
            foreach (var item in other)
            {
                yield return item;
            }
        }

        /// <summary>
        ///     Determines whether the specified source contains the given item, according the comparison.
        /// </summary>
        /// <typeparam name="TSource"> The type of the source. </typeparam>
        /// <param name="source"> The source items. </param>
        /// <param name="item"> The item. </param>
        /// <param name="comparison"> The comparison to compare the items. </param>
        /// <returns>
        ///     <c>true</c> if the specified source contains the given item; otherwise, <c>false</c> .
        /// </returns>
        public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource item, Comparison<TSource> comparison) => source.Any(
            currItem => comparison(currItem, item) == 0);

        public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource item, Func<TSource, TSource, bool> comparer) => source.Any(
            currItem => comparer(currItem, item));

        public static IEnumerable<TSource> Copy<TSource>(this ICollection<TSource> source)
        {
            var result = new TSource[source?.Count ?? 0];
            source?.CopyTo(result, 0);
            return result;
        }

        public static IEnumerable<TSource> Copy<TSource>(this TSource[] source)
        {
            var result = new TSource[source?.Length ?? 0];
            source?.CopyTo(result, 0);
            return result;
        }

        /// <summary>
        ///     Counts the specified source.
        /// </summary>
        /// <param name="source"> The source. </param>
        /// <returns> </returns>
        public static int Count(this IEnumerable source)
        {
            var result = 0;
            source.ForEach(item => result++);
            return result;
        }

        /// <summary>
        ///     Counts the specified source.
        /// </summary>
        /// <param name="source"> The source. </param>
        /// <returns> </returns>
        public static int Count<T>(params T[] items) => items.Length;

        public static int CountOf<TSource>(this IEnumerable<TSource> source, TSource item)
            where TSource : class => source.Where(s => s == item).Count();

        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source, Func<T, T, bool> comparer, Func<T, int> getHashCode = null) => source.Distinct(
            new LibEqualityComparer<T>(comparer, getHashCode));

        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source, Func<T, object> getProp) => source.GroupBy(getProp).Select(x => x.First());

        /// <summary>
        ///     Returns the element at index.
        /// </summary>
        /// <param name="source"> The source. </param>
        /// <param name="index"> The index. </param>
        /// <returns> </returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static object ElementAt(this IEnumerable source, int index)
        {
            var counter = 0;
            foreach (var item in source)
            {
                if (counter == index)
                {
                    return item;
                }

                if (counter > index)
                {
                    break;
                }

                counter++;
            }

            throw new ArgumentOutOfRangeException();
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> source, Func<T, bool> deselector)
            where T : class => source.Where(item => !deselector(item));

        public static IEnumerable<TData> Except<TData>(this IEnumerable<TData> source, TData item)
            where TData : class => Except(source, curr => curr == item);

        public static IEnumerable<T> ExceptDefaults<T>(this IEnumerable<T> source)
            where T : struct => source.Where(item => !item.Equals(default(T)));

        public static IEnumerable<T> ExceptNulls<T>(this IEnumerable<T> source)
            where T : class => source.Where(item => item != null);

        public static IEnumerable<T> Flatten<T>(T first, Func<T, T> getNext)
            where T : class
        {
            yield return first;
            var current = first;
            while ((current = getNext(current)) != null)
            {
                yield return current;
            }
        }

        public static IEnumerable<T> FlattenTreeNode<T>(this IEnumerable<T> e, Func<T, IEnumerable<T>> getChildren) =>
            e.ToList().FlattenTreeNode(getChildren);

        public static IEnumerable<T> FlattenTreeNode<T>(this IList<T> e, Func<T, IEnumerable<T>> getChildren) => e
            .SelectMany(
                c => getChildren(c)
                    .FlattenTreeNode(getChildren))
            .Concat(e);

        public static void For<TSource>(this IEnumerable<TSource> source, Action<TSource, int> actor)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }

            var items = source.ToArray();
            for (var i = 0; i < items.LongLength; i++)
            {
                actor(items[i], i);
            }
        }

        public static void For(int min, int max, Action<int> action, int step = 1)
        {
            for (var i = min; i < max; i += step)
            {
                action(i);
            }
        }

        public static void For(int max, Action<int> action, int min = 0, int step = 1)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            for (var i = min; i < max; i += step)
            {
                action(i);
            }
        }

        public static void For(int max, Action action, int min = 0, int step = 1)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            for (var i = min; i < max; i += step)
            {
                action();
            }
        }

        public static IEnumerable<TResult> For<TResult>(int count, Func<int, TResult> selector)
        {
            for (var index = 0; index < count; index++)
            {
                yield return selector(index);
            }
        }

        public static IEnumerable<TResult> For<TObject, TResult>(TObject o, int count, Func<TObject, int, TResult> selector)
        {
            for (var index = 0; index < count; index++)
            {
                yield return selector(o, index);
            }
        }

        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> actor)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }

            foreach (var item in source)
            {
                actor(item);
            }
        }

        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource, int> actor)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }

            var index = 0;
            foreach (var item in source)
            {
                actor(item, index++);
            }
        }

        /// <summary>
        ///     Performs the actor to each items in specific source
        /// </summary>
        /// <param name="source"> The items to be looped through. </param>
        /// <param name="actor"> The actor applies to each item </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="source" />
        ///     is
        ///     <c>null</c>
        ///     .
        /// </exception>
        public static void ForEach(this IEnumerable source, Action<object> actor)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }

            foreach (var item in source)
            {
                actor(item);
            }
        }

        /// <summary>
        ///     Fors the each.
        /// </summary>
        /// <param name="source"> The source. </param>
        /// <param name="actor"> The actor. </param>
        public static void ForEach(this IDictionary source, Action<object, object> actor)
        {
            foreach (var key in source.Keys)
            {
                actor(key, source[key]);
            }
        }

        public static void ForEach<TKey, TValue>(this IDictionary<TKey, TValue> source, Action<TKey, TValue> actor)
        {
            foreach (var key in source.Keys)
            {
                actor(key, source[key]);
            }
        }

        public static IEnumerable<TDestination> ForEachFunc<TSource, TDestination>(this IEnumerable<TSource> source, Func<TSource, TDestination> actor)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }

            foreach (var item in source)
            {
                yield return actor(item);
            }
        }

        public static void ForEachTreeNode<T>(this IEnumerable<T> roots, Func<T, IEnumerable<T>> getChildren, Action<T> rootAction, Action<T, T> childAction)
            where T : class => roots.ForEach(r => ForEachTreeNode(r, getChildren, rootAction, childAction));

        public static void ForEachTreeNode<T>(T root, Func<T, IEnumerable<T>> getChildren, Action<T> rootAction, Action<T, T> childAction)
            where T : class
        {
            if (root == null)
            {
                return;
            }

            rootAction?.Invoke(root);
            getChildren(root)
                .ForEach(c =>
                {
                    childAction?.Invoke(c, root);
                    ForEachTreeNode(c, getChildren, rootAction, childAction);
                });
        }

        /// <summary>
        ///     Generates the specified generator.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="generator"> The generator. </param>
        /// <param name="count"> The count. </param>
        /// <returns> </returns>
        public static IEnumerable<T> Generate<T>(Func<T> generator, int count)
        {
            for (var i = 0; i < count; i++)
            {
                yield return generator();
            }
        }

        public static IEnumerable<T> Generate<T>(Func<int, T> generator, int count)
        {
            for (var i = 0; i < count; i++)
            {
                yield return generator(i);
            }
        }

        public static IEnumerable<T> GetAll<T>(Func<IEnumerable<T>> getRootElements, Func<T, IEnumerable<T>> getChildren)
        {
            if (getRootElements == null)
            {
                throw new ArgumentNullException(nameof(getRootElements));
            }

            if (getChildren == null)
            {
                throw new ArgumentNullException(nameof(getChildren));
            }

            var result = new List<T>();

            void FindChildren(T item)
            {
                foreach (var child in getChildren(item))
                {
                    result.Add(child);
                    FindChildren(child);
                }
            }

            foreach (var item in getRootElements())
            {
                result.Add(item);
                FindChildren(item);
            }

            return result;
        }

        public static T GetNext<T>(this IEnumerable<T> source, T item)
        {
            var list = source as IList<T> ?? source.ToList();
            return list.Count() - 1 == list.IndexOf(item) ? default : list.ElementAt<T>(list.IndexOf(item) + 1);
        }

        public static bool HasDuplicates<TEnum, TPropType>(this IEnumerable<TEnum> source, Func<TEnum, TPropType> fieldSelector) => source
            .GroupBy(fieldSelector)
            .Any(
                grp =>
                    grp
                        .ElementAtOrDefault(
                            1) !=
                    null);

        public static bool HasDuplicates<T>(this IEnumerable<T> source, Func<T, T, bool> comparer, Func<T, int> getHashCode = null)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            var items = source as T[] ?? source.ToArray();
            var equalityComparer = new LibEqualityComparer<T>(comparer, getHashCode);
            return items.Distinct(equalityComparer).Count() != items.Count();
        }

        public static bool HasItemsAtLeast(this IEnumerable source, uint count)
        {
            var enumerator = source.GetEnumerator();
            var index = 0;
            while (enumerator.MoveNext() && index < count)
            {
                index++;
            }

            return index == count;
        }

        public static IEnumerable IfZero(IEnumerable source, IEnumerable defaultSource) => !source.Cast<object>().Any() ? defaultSource : source;

        /// <summary>
        ///     Gets index of a specific element.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="source"> The source. </param>
        /// <param name="predicate"> The predicate. </param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns> </returns>
        public static int? IndexOf<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var list = source as IList<T> ?? source.ToList();
            if (!list.Any())
            {
                return -1;
            }

            int? result = 0;
            bool found;
            using (var enumerator = list.GetEnumerator())
            {
                found = false;
                while (enumerator.MoveNext() && !(found = predicate(enumerator.Current)))
                {
                    result++;
                }
            }

            return found ? result : null;
        }

        public static int? IndexOf<T>(this IEnumerable<T> source, T item)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var index = 0;
            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (Equals(enumerator.Current, item))
                    {
                        return index;
                    }

                    index++;
                }
            }

            return null;
        }

        public static TSource[] Initialize<TSource>(Func<TSource[]> creator, Func<TSource> action)
        {
            var result = creator();
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = action();
            }

            return result;
        }

        public static bool IsLast<T>(this IEnumerable<T> source, T item)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var list = source as IList<T> ?? source.ToList();
            return list.Count() - 1 == list.IndexOf(item);
        }

        /// <summary>
        ///     Iterates the specified source. (Iteration Design Pattern)
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static IEnumerable<TSource> Iterate<TSource>(this IEnumerable<TSource> source)
        {
            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }
        }

        /// <summary>
        ///     Iterates the specified action. (Iteration Design Pattern)
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="action">The action.</param>
        public static void Iterate<TSource>(this IEnumerable<TSource> source, Action<TSource> action) => source?.Iterate().ForEach(action);

        public static IEnumerable<TSource> Iterate<TSource>(this IEnumerable<TSource> source, int steps)
            where TSource : class
        {
            if (steps < 2)
            {
                throw new ArgumentOutOfRangeException($"{nameof(steps)} must be greater than 2");
            }

            var index = 0;
            foreach (var item in source.Iterate())
            {
                index++;
                if (index % steps == 0)
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        ///     Iterates the specified steps. (Iteration Design Pattern)
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="steps">The steps.</param>
        /// <param name="action">The action.</param>
        public static void Iterate<TSource>(this IEnumerable<TSource> source, int steps, Action<TSource> action)
            where TSource : class => source?.Iterate(steps).ForEach(action);

        public static IEnumerable<T> Lookup<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> getChildren, Func<T, bool> predicate = null)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (getChildren == null)
            {
                throw new ArgumentNullException(nameof(getChildren));
            }

            foreach (var item in source)
            {
                if (predicate?.Invoke(item) == true)
                {
                    yield return item;
                }

                foreach (var child in Lookup(getChildren(item), getChildren, predicate))
                {
                    yield return child;
                }
            }
        }

        public static IEnumerable<T> LookupParents<T>(T node, Func<T, T> getParent, Func<T, bool> predicate = null)
            where T : class
        {
            if (getParent == null)
            {
                throw new ArgumentNullException(nameof(getParent));
            }

            var parent = getParent(node);
            if (predicate == null)
            {
                predicate = p => true;
            }

            while (parent != null)
            {
                if (predicate(parent))
                {
                    yield return parent;
                }

                parent = getParent(parent);
            }
        }

        public static IEnumerable<T> Merge<T>(params IEnumerable<T>[] sources) => sources.SelectMany(source => source);

        public static IEnumerable<TResult> Merge<TResult, T>(params IEnumerable<T>[] source)
            where T : TResult => source.SelectMany(items => items.Cast<TResult>());

        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> query, (int PageCount, int? PageIndex) paging) => query.Paginate(paging.PageCount,
            paging.PageIndex);

        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> query, int pageCount, int? pageIndex)
        {
            if (!pageIndex.HasValue)
            {
                return query;
            }

            return query.Skip(pageCount * pageIndex.Value).Take(pageCount);
        }

        public static IEnumerable<IEnumerable<TSource>> Partition<TSource>(this IEnumerable<TSource> items, int partitionCount)
        {
            var result = Repeat(() => new List<TSource>(), partitionCount).ToArray();
            var counter = 0;
            foreach (var item in items)
            {
                result[counter++ % partitionCount].Add(item);
            }

            return result;
        }

        public static IEnumerable<int> Range(int start, int count, int step)
        {
            for (var number = start; number < start + count; number += step)
            {
                yield return number;
            }
        }

        public static IEnumerable<TSource> RemoveDefaults<TSource>(this IEnumerable<TSource> source, TSource defaultValue = default) =>
            defaultValue == null ? source.Where(item => item != null) : source.Where(item => item.Equals(defaultValue));

        public static IEnumerable<TSource> RemoveNulls<TSource>(this IEnumerable<TSource> source)
            where TSource : class => RemoveDefaults(source);

        public static void RemoveRange<TEntity>(this ICollection<TEntity> source, IEnumerable<TEntity> entities) => entities.ForEach(e => source.Remove(e));

        public static void RemoveRange<TEntity>(this ICollection<TEntity> source, Predicate<TEntity> match) => RemoveRange(source,
            source.Where(
                entity => match(entity)));

        public static void RemoveRange<TEntity>(this IList<TEntity> source, IEnumerable<TEntity> entities) => entities.ForEach(e => source.Remove(e));

        public static void RemoveRange<TEntity>(this IList<TEntity> source, Predicate<TEntity> match) => RemoveRange(source,
            source.Where(entity => match(entity)));

        /// <summary>
        ///     Repeats the specified items.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="items"> The items. </param>
        /// <param name="actor"> The actor. </param>
        public static void Repeat<T>(this IEnumerable<T> items, Action<T> actor)
        {
            foreach (var item in items)
            {
                actor(item);
            }
        }

        public static IEnumerable<TSource> Repeat<TSource>(Func<TSource> action, int count)
        {
            for (var i = 0; i < count; i++)
            {
                yield return action();
            }
        }

        public static void RunAll(this IEnumerable<Action> actions) => actions?.ForEach(a => a());

        public static IEnumerable<TResult> RunAll<TResult>(this IEnumerable<Func<TResult>> actions) => actions?.ForEachFunc(a => a());

        public static void RunAllWhile(this IEnumerable<Action> actions, Func<bool> predicate)
        {
            foreach (var action in actions)
            {
                if (!predicate())
                {
                    break;
                }

                action();
            }
        }

        /// <summary>
        ///     Selects the many recursive.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="items"> The items. </param>
        /// <param name="selector"> The selector. </param>
        /// <returns> </returns>
        /// <exception cref="ArgumentNullException">items</exception>
        public static IEnumerable<T> SelectManyRecursive<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> selector)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            var children = items.SelectMany(selector);
            return children.Any() ? items.Concat(SelectManyRecursive(children, selector)) : items.Concat(children);
        }

        /// <summary>
        ///     Selects the tree.
        /// </summary>
        /// <typeparam name="TElement"> The type of the element. </typeparam>
        /// <param name="root"> The root element. </param>
        /// <param name="getNext"> The get next. </param>
        /// <returns> </returns>
        public static IEnumerable<TElement> SelectTree<TElement>(TElement root, Func<TElement, IEnumerable<TElement>> getNext)
        {
            yield return root;
            foreach (var child in InnerSelectTreeDynamic(root, getNext))
            {
                yield return child;
            }
        }

        /// <summary>
        ///     Shrinks the specified source.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="source"> The source. </param>
        public static void Shrink<T>(this ICollection<T> source)
        {
            var buffer = new List<T>();
            buffer.AddMany(source);
            foreach (var item in buffer.Where(item => item == null))
            {
                source.Remove(item);
            }
        }

        /// <summary>
        ///     Takes the groups.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="items"> The items. </param>
        /// <param name="groupSize"> Size of the group. </param>
        /// <param name="takeEvery"> The take every. </param>
        /// <returns> </returns>
        /// <exception cref="ArgumentNullException">items</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <c>takeEvery</c>
        ///     is out of range.
        /// </exception>
        public static IEnumerable<IEnumerable<T>> TakeGroups<T>(this IEnumerable<T> items, int groupSize, int takeEvery)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (takeEvery > groupSize)
            {
                throw new ArgumentOutOfRangeException(nameof(takeEvery), "Argument takeEvery must be less than or equal to groupSize.");
            }

            var group = new Queue<T>(groupSize);

            foreach (var t in items)
            {
                if (group.Count < groupSize)
                {
                    group.Enqueue(t);
                }
                else
                {
                    // return a copy
                    yield return group.ToList();

                    if (groupSize == takeEvery)
                    {
                        group.Clear();
                    }
                    else
                    {
                        for (var x = 0; x < takeEvery; x++)
                        {
                            group.Dequeue();
                        }
                    }

                    group.Enqueue(t);
                }
            }

            yield return group;
        }

        /// <summary>
        ///     Takes the groups.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="items"> The items. </param>
        /// <param name="groupSize"> Size of the group. </param>
        /// <returns> </returns>
        public static IEnumerable<IEnumerable<T>> TakeGroups<T>(this IEnumerable<T> items, int groupSize) => TakeGroups(items, groupSize, groupSize);

        /// <summary>
        ///     Converts the source to the array.
        /// </summary>
        /// <param name="source"> The source. </param>
        /// <returns> </returns>
        public static object[] ToArray(IEnumerable source)
        {
            var arrayList = new ArrayList();
            foreach (var item in source)
            {
                arrayList.Add(item);
            }

            return arrayList.ToArray();
        }

        /// <summary>
        ///     Converts the source to the array.
        /// </summary>
        /// <param name="source"> The source. </param>
        /// <returns> </returns>
        public static object[] ToArray(params object[] source) => source;

        /// <summary>
        ///     Ases the array.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="item"> The item. </param>
        /// <returns> </returns>
        public static T[] ToArray<T>(T item) => ToEnumerable(item).ToArray();

        /// <summary>
        ///     /// Converts the given numbers to the array.
        /// </summary>
        /// <param name="ts"> The numbers. </param>
        /// <returns> </returns>
        public static T[] ToArray<T>(params T[] ts) => ts;

        public static Collection<TSource> ToCollection<TSource>(this IEnumerable<TSource> source)
        {
            var result = new Collection<TSource>();
            source.ForEach(result.Add);
            return result;
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> pairs) => pairs.ToDictionary(
            pair => pair.Key,
            pair => pair.Value);

        public static IDictionary<T, T> ToDictionary<T>(this IEnumerable<T> items)
        {
            IDictionary<T, T> dict = new Dictionary<T, T>();
            items.ToPairs().ForEach(dict.Add);
            return dict;
        }

        public static IEnumerable<T> ToEnumerable<T>(this IEnumerable<T> source) => new Enumerable<T>(source);

        public static IEnumerable<T> ToEnumerable<T>(T obj)
        {
            yield return obj;
        }

        public static IIndexerEnumerable<T> ToIndexerEnumerable<T>(this IEnumerable<T> items) => new IndexerEnumerable<T>(items);

        public static EventualCollection<T> ToLibraryCollection<T>(this IEnumerable<T> source)
        {
            var result = new EventualCollection<T>();
            source.ForEach(result.Add);
            return result;
        }

        public static IEnumerable<KeyValuePair<T, T>> ToPairs<T>(this IEnumerable<T> items) => items.Select(item => new KeyValuePair<T, T>(item, item));

        /// <summary>
        ///     Zips the specified source1.
        /// </summary>
        /// <typeparam name="TResult"> The type of the result. </typeparam>
        /// <typeparam name="T1"> The type of the 1. </typeparam>
        /// <typeparam name="T2"> The type of the 2. </typeparam>
        /// <param name="source1"> The source1. </param>
        /// <param name="source2"> The source2. </param>
        /// <param name="zipper"> The zipper. </param>
        /// <returns> </returns>
        public static IEnumerable<TResult> Zip<TResult, T1, T2>(IEnumerable<T1> source1, IEnumerable<T2> source2, Func<T1, T2, TResult> zipper) =>
            source1.SelectMany(item1 => source2, zipper);

        public static void Enumerate<T>(this IEnumerable<T> source, Func<T, int, bool> moveNext)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (moveNext == null)
            {
                throw new ArgumentNullException(nameof(moveNext));
            }

            var index = 0;
            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (!moveNext(enumerator.Current, index++))
                    {
                        break;
                    }
                }
            }
        }

        public static void Enumerate<T>(this IEnumerable<T> source, Action<T, int> moveNext)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (moveNext == null)
            {
                throw new ArgumentNullException(nameof(moveNext));
            }

            var index = 0;
            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    moveNext(enumerator.Current, index++);
                }
            }
        }
    }
}