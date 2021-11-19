﻿using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Library.Collections;
using Library.Results;
using Library.Validations;

namespace Library.Helpers;
public static class EnumerableHelper
{
    public static IList<KeyValuePair<TKey, TValue>>? Add<TKey, TValue>([DisallowNull] this IList<KeyValuePair<TKey, TValue>> list, in TKey key, in TValue value)
    {
        Check.IfArgumentNotNull(list, nameof(list));
        list.Add(new(key, value));
        return list;
    }

    /// <summary>
    /// Adds the specified item immutaly and returns a new instance of source.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">The source.</param>
    /// <param name="item">The item.</param>
    /// <returns></returns>
    public static IEnumerable<T> AddImmuted<T>(this IEnumerable<T>? source, T item)
    {
        if (source is not null)
        {
            foreach (var i in source)
            {
                yield return i;
            }
        }

        yield return item;
    }

    public static void AddRange<T>([DisallowNull] this ICollection<T> collection, params T[] newItems)
    {
        Check.IfArgumentNotNull(collection, nameof(collection));
        if (newItems?.Any() is true)
        {
            foreach (var item in newItems)
            {
                collection.Add(item);
            }
        }
    }

    public static IList<T> AddRange<T>([DisallowNull] this IList<T> list, in IEnumerable<T> items)
    {
        Check.IfArgumentNotNull(list, nameof(list));
        if (items?.Any() is true)
        {
            foreach (var item in items)
            {
                list.Add(item);
            }
        }

        return list!;
    }

    public static TList AddRange<TList, TItem>([DisallowNull] this TList list, in IEnumerable<TItem> items)
        where TList : notnull, ICollection<TItem>
    {
        Check.IfArgumentNotNull(list, nameof(list));
        if (items?.Any() is true)
        {
            foreach (var item in items)
            {
                list.Add(item);
            }
        }

        return list!;
    }

    public static TList AddRange<TList, TItem>([DisallowNull] this TList list, params TItem[] items)
        where TList : notnull, ICollection<TItem>
    {
        Check.IfArgumentNotNull(list, nameof(list));
        if (items?.Any() is true)
        {
            foreach (var item in items)
            {
                list.Add(item);
            }
        }

        return list!;
    }

    public static IEnumerable<T> AddRangeImmuted<T>(this IEnumerable<T>? source, IEnumerable<T>? items)
    {
        if (source is not null)
        {
            foreach (var i in source.ArgumentNotNull(nameof(source)))
            {
                yield return i;
            }
        }

        if (items is not null)
        {
            foreach (var item in items.ArgumentNotNull(nameof(items)))
            {
                yield return item;
            }
        }
    }

    public static IEnumerable<T> Aggregate<T>(this IEnumerable<IEnumerable<T>> sources)
    {
        foreach (var items in sources.ArgumentNotNull(nameof(sources)))
        {
            foreach (var item in items)
            {
                yield return item;
            }
        }
    }

    public static bool Any(this IEnumerable source)
    {
        if (source is null)
        {
            return false;
        }

        foreach (var _ in source)
        {
            return true;
        }
        return false;
    }

    public static IReadOnlyList<T> Build<T>([DisallowNull] this IEnumerable<T> items)
    {
        Check.IfArgumentNotNull(items, nameof(items));

        var result = new List<T>();
        foreach (var item in items)
        {
            result.Add(item);
        }
        return result.AsReadOnly();
    }

    public static IEnumerable<T> AsEnumerableItem<T>(T item)
    {
        yield return item;
    }

    public static IReadOnlyList<T> ToReadOnlyList<T>([DisallowNull] this IEnumerable<T> items) => new List<T>(items).AsReadOnly();

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
    public static void BuildTree<TSource, TItem>(
        [DisallowNull] this IEnumerable<TSource> rootElements,
        [DisallowNull] Func<TSource, TItem> getNewItem,
        [DisallowNull] Func<TSource, IEnumerable<TSource>> getChildren,
        [DisallowNull] in Action<TItem> addToRoots,
        [DisallowNull] Action<TItem, TItem> addChild)
    {
        getChildren.IfArgumentNotNull(nameof(getChildren));
        getNewItem.IfArgumentNotNull(nameof(getNewItem));
        addToRoots.IfArgumentNotNull(nameof(addToRoots));
        addChild.IfArgumentNotNull(nameof(addChild));
        rootElements.IfArgumentNotNull(nameof(rootElements));

        foreach (var siteMap in rootElements)
        {
            var root = getNewItem(siteMap);
            addToRoots(root);
            addChildren(siteMap, root);
        }

        void addChildren(in TSource siteMap, in TItem parent)
        {
            foreach (var sm in getChildren(siteMap))
            {
                var newChile = getNewItem(sm);
                addChild(parent, newChile);
                addChildren(sm, newChile);
            }
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
    public static IEnumerable<TOutput> Cast<TInput, TOutput>(this IEnumerable<TInput> input, [DisallowNull] Converter<TInput, TOutput> converter)
    {
        if (input is null)
        {
            yield break;
        }
        Check.IfArgumentNotNull(converter, nameof(converter));

        foreach (var item in input)
        {
            yield return converter(item);
        }
    }

    public static T ClearAndAdd<T>([DisallowNull] this T collection, in object? item)
        where T : notnull, IList
    {
        collection.ArgumentNotNull(nameof(collection)).Clear();
        _ = collection.Add(item);
        return collection;
    }

    public static T ClearAndAddRange<T>(this T collection, [DisallowNull] in IEnumerable items)
        where T : notnull, IList
    {
        Check.IfArgumentNotNull(items, nameof(items));

        collection.Clear();
        return AddRange(collection, items);
    }

    public static TList ClearAndAddRange<TList, TItem>(this TList list, [DisallowNull] in IEnumerable<TItem> items)
        where TList : notnull, ICollection<TItem>
    {
        Check.IfArgumentNotNull(items, nameof(items));

        list.Clear();
        return list.AddRange(items);
    }

    public static T AddRange<T>(T collection, [DisallowNull] IEnumerable items)
        where T : IList
    {
        Check.IfArgumentNotNull(items, nameof(items));
        foreach (var item in items)
        {
            _ = collection.Add(item);
        }
        return collection;
    }

    public static IEnumerable<TSource> Compact<TSource>(this IEnumerable<TSource?>? items)
        where TSource : class
    {
        var buffer = items;
        if (buffer?.Any() is not true)
        {
            buffer = Enumerable.Empty<TSource>();
        }

        foreach (var item in buffer)
        {
            if (item is not null)
            {
                yield return item;
            }
        }
    }

    public static IEnumerable<T> FindDuplicates<T>(in IEnumerable<T> items)
        => items.ArgumentNotNull(nameof(items)).GroupBy(x => x).Where(g => g.Count() > 1).Select(y => y.Key);

    public static IEnumerable<TResult> ForEachItem<T, TResult>([DisallowNull] this IEnumerable<T> items, [DisallowNull] Func<T, TResult> action)
    {
        Check.IfArgumentNotNull(items);
        Check.IfArgumentNotNull(action);
        foreach (var item in items)
        {
            yield return action(item);
        }
    }

    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> items, [DisallowNull] Action<T> action, bool build = false)
    {
        Check.IfArgumentNotNull(items);
        var buffer = build ? items.Build() : items;

        foreach (var item in buffer)
        {
            action?.Invoke(item);
            yield return item;
        }
    }

    public static IReadOnlyList<T> ForEachEager<T>(this IEnumerable<T> items, [DisallowNull] Action<T> action)
    {
        Check.IfArgumentNotNull(items);
        foreach (var item in items)
        {
            action?.Invoke(item);
        }
        return items.Build();
    }

    public static void ForEachTreeNode<T>(T root, Func<T, IEnumerable<T>>? getChildren, Action<T>? rootAction, Action<T, T>? childAction)
        where T : class
    {
        if (root is null)
        {
            return;
        }

        rootAction?.Invoke(root);
        getChildren?.Invoke(root)
            .ForEach(c =>
            {
                childAction?.Invoke(c, root);
                ForEachTreeNode(c, getChildren, rootAction, childAction);
            });
    }

    public static IEnumerable<T> GetAll<T>([DisallowNull] Func<IEnumerable<T>> getRootElements, [DisallowNull] Func<T, IEnumerable<T>?> getChildren)
    {
        getRootElements.IfArgumentNotNull(nameof(getRootElements));
        getChildren.IfArgumentNotNull(nameof(getChildren));

        var result = new List<T>();

        foreach (var item in getRootElements())
        {
            result.Add(item);
            findChildren(item);
        }

        return result.AsEnumerable();

        void findChildren(in T item)
        {
            var children = getChildren(item);
            if (children?.Any() is true)
            {
                foreach (var child in children)
                {
                    result.Add(child);
                    findChildren(child);
                }
            }
        }
    }
    public static KeyValuePair<TKey, TValue> GetItemByKey<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source, in TKey key)
    {
        foreach (var item in source.ArgumentNotNull(nameof(source)))
        {
            if (item.Key?.Equals(key) is true)
            {
                return item;
            }
        }
        throw new KeyNotFoundException(nameof(key));
    }
    public static TValue GetByKey<TKey, TValue>(this IEnumerable<(TKey Key, TValue Value)> source, TKey key)
        => source.ArgumentNotNull(nameof(source)).Where(kv => kv.Key?.Equals(key) ?? key is null).First().Value;

    public static bool ContainsKey<TKey, TValue>([DisallowNull] this IEnumerable<(TKey Key, TValue Value)> source, TKey key)
        => source.ArgumentNotNull(nameof(source)).Where(kv => kv.Key?.Equals(key) ?? key is null).Any();

    public static IEnumerable<(T Item, int Count)> GroupCounts<T>(in IEnumerable<T> items)
        => items.GroupBy(x => x).Select(x => (x.Key, x.Count()));

    public static IEnumerable<T> InsertImmuted<T>(this IEnumerable<T> source, int index, T item)
    {
        var items = source as T[] ?? source.ToArray();
        for (var i = 0; i < items.Length; i++)
        {
            if (i == index)
            {
                yield return item;
            }

            yield return items[i];
        }
    }

    public static string MergeToString<T>(this IEnumerable<T> source)
        => source.Aggregate(new StringBuilder(), (current, item) => current.Append(item)).ToString();

    public static IList<KeyValuePair<TKey, TValue>> RemoveByKey<TKey, TValue>([DisallowNull] this IList<KeyValuePair<TKey, TValue>> list, in TKey key)
    {
        Check.IfArgumentNotNull(list, nameof(list));
        _ = list.Remove(list.GetItemByKey(key));
        return list;
    }

    public static IEnumerable<TSource> RemoveDefaults<TSource>(this IEnumerable<TSource> source, TSource? defaultValue = default)
        => defaultValue is null ? source.Where(item => item is not null) : source.Where(item => (item?.Equals(defaultValue)) ?? true);

    public static IEnumerable<T> RemoveImmuted<T>(this IEnumerable<T>? source, T item)
    {
        if (source is not null)
        {
            foreach (var i in source)
            {
                if (i?.Equals(item) is not true)
                {
                    yield return i;
                }
            }
        }
    }

    public static IEnumerable<TSource> RemoveNulls<TSource>(this IEnumerable<TSource> source)
        where TSource : class => RemoveDefaults(source);

    public static IList<KeyValuePair<TKey, TValue>> SetByKey<TKey, TValue>([DisallowNull] this IList<KeyValuePair<TKey, TValue>> list, in TKey key, in TValue value)
    {
        Check.IfArgumentNotNull(list, nameof(list));
        _ = list.RemoveByKey(key);
        list.Add(new(key, value));
        return list;
    }
    public static Dictionary<TKey, TValue> SetByKey<TKey, TValue>([DisallowNull] this Dictionary<TKey, TValue> dic, TKey key, TValue value)
        where TKey : notnull
        => dic
            .ArgumentNotNull(nameof(dic))
            .If(dic.ContainsKey(key), () => dic[key] = value, () => dic.Add(key, value));

    public static Dictionary<TKey, TValue>? ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>>? pairs)
        where TKey : notnull
        => pairs?.ToDictionary(pair => pair.Key, pair => pair.Value);

    public static TList AddFluent<TList, TItem>(this TList list, TItem item)
        where TList : ICollection<TItem>
        => list.ArgumentNotNull(nameof(list)).Fluent(() => list.Add(item));

    public static async IAsyncEnumerable<TItem> ForEachAsync<TItem>([DisallowNull] this IAsyncEnumerable<TItem> asyncItems, Func<TItem, TItem> action, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Check.IfArgumentNotNull(asyncItems, nameof(asyncItems));
        await foreach (var item in asyncItems.WithCancellation(cancellationToken))
        {
            yield return action(item);
        }
    }
    public static async IAsyncEnumerable<TItem> ForEachAsync<TItem>([DisallowNull] this IAsyncEnumerable<TItem> asyncItems, Action<TItem> action, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Check.IfArgumentNotNull(asyncItems, nameof(asyncItems));
        await foreach (var item in asyncItems.WithCancellation(cancellationToken))
        {
            action(item);
            yield return item;
        }
    }

    public static async Task<List<TItem>> ToListAsync<TItem>(this IAsyncEnumerable<TItem> items, CancellationToken cancellationToken = default)
        => await New<List<TItem>>().AddRangeAsync(items, cancellationToken);

    public static async Task<TList> AddRangeAsync<TList, TItem>([DisallowNull] this TList list, ConfiguredCancelableAsyncEnumerable<TItem> asyncItems)
        where TList : notnull, ICollection<TItem>
    {
        Check.IfArgumentNotNull(list, nameof(list));
        await foreach (var item in asyncItems)
        {
            list.Add(item);
        }
        return list;
    }

    public static async Task<TList> AddRangeAsync<TList, TItem>([DisallowNull] this TList list, IAsyncEnumerable<TItem> asyncItems, CancellationToken cancellationToken = default)
        where TList : notnull, ICollection<TItem>
    {
        Check.IfArgumentNotNull(list, nameof(list));
        await foreach (var item in asyncItems.WithCancellation(cancellationToken))
        {
            list.Add(item);
        }
        return list;
    }

    /// <summary>
    ///     Creates an empty array.
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    /// <returns> </returns>
    public static T[] EmptyArray<T>()
        => Array.Empty<T>();

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

    public static IEnumerable<TItem> Exclude<TItem>(this IEnumerable<TItem> source, Func<TItem, bool> exclude)
        => source.ArgumentNotNull(nameof(source)).Where(x => !exclude(x));

    public static TFluentList AddRange<TFluentList, TItem>([DisallowNull] this TFluentList list, IEnumerable<TItem>? items)
        where TFluentList : IFluentList<TFluentList, TItem>
    {
        Check.IfArgumentNotNull(list, nameof(list));
        if (items?.Any() is true)
        {
            foreach (var item in items)
            {
                list.Add(item);
            }
        }
        return list;
    }

    public static IEnumerable<T> ToEnumerable<T>(this IEnumerable<T> source)
        => source?.Select(x => x) ?? Enumerable.Empty<T>();

    [Obsolete("Use .Net 6.0 Check, instead.")]
    public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, int chunkSize)
        => source.Select((x, i) => new { Index = i, Value = x })
                 .GroupBy(x => x.Index / chunkSize)
                 .Select(x => x.Select(v => v.Value));

    public static TryMethodResult<int> TryCountNotEnumerated<T>(this IEnumerable<T> source)
        => new(source.TryGetNonEnumeratedCount(out var count), count);

    public static int CountNotEnumerated<T>(this IEnumerable<T> source)
    {
        var (succeed, count) = TryCountNotEnumerated(source);
        return succeed ? count : -1;
    }
}