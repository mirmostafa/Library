﻿using System.Collections;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

using Library.Collections;
using Library.Interfaces;
using Library.Results;
using Library.Validations;

namespace Library.Helpers;

public static class EnumerableHelper
{
    public static IList<KeyValuePair<TKey, TValue>> Add<TKey, TValue>([DisallowNull] this IList<KeyValuePair<TKey, TValue>> list, in TKey key, in TValue value)
    {
        Check.IfArgumentNotNull(list);
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

    public static IList<T> AddRange<T>([DisallowNull] this IList<T> list, in IEnumerable<T> items)
    {
        if (items?.Any() is true)
        {
            foreach (var item in items)
            {
                list.Add(item);
            }
        }
        return list;
    }

    public static ICollection<T> AddRange<T>([DisallowNull] this ICollection<T> list, in IEnumerable<T> items)
    {
        if (items?.Any() is true)
        {
            foreach (var item in items)
            {
                list.Add(item);
            }
        }
        return list;
    }

    public static IFluentList<TFluentList, TItem> AddRange<TFluentList, TItem>([DisallowNull] this IFluentList<TFluentList, TItem> list, IEnumerable<TItem>? items)
        where TFluentList : IFluentList<TFluentList, TItem>
    {
        if (items?.Any() is true)
        {
            foreach (var item in items)
            {
                _ = list.Add(item);
            }
        }
        return list;
    }

    public static async Task<ICollection<TItem>> AddRangeAsync<TItem>([DisallowNull] this ICollection<TItem> list, [DisallowNull] IAsyncEnumerable<TItem> asyncItems, CancellationToken cancellationToken = default)
    {
        Check.IfArgumentNotNull(list);
        Check.IfArgumentNotNull(asyncItems);

        await foreach (var item in asyncItems.WithCancellation(cancellationToken))
        {
            list.Add(item);
        }
        return list;
    }

    public static IEnumerable<T> AddRangeImmuted<T>(this IEnumerable<T>? source, IEnumerable<T>? items)
    {
        if (source is not null)
        {
            foreach (var i in source)
            {
                yield return i;
            }
        }

        if (items is not null)
        {
            foreach (var item in items)
            {
                yield return item;
            }
        }
    }

    public static IEnumerable<T> Aggregate<T>(this IEnumerable<IEnumerable<T>> sources)
    {
        if (sources is not null)
        {
            foreach (var item in sources.Where(items => items is not null).SelectMany(items => items))
            {
                yield return item;
            }
        }
    }

    public static bool Any([NotNullWhen(true)] this IEnumerable source)
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

    public static IEnumerable<T> AsEnumerableItem<T>(T item)
    {
        yield return item;
    }

    public static IReadOnlyList<T> Build<T>([DisallowNull] this IEnumerable<T> items)
    {
        Check.IfArgumentNotNull(items);

        var result = new List<T>();
        foreach (var item in items)
        {
            result.Add(item);
        }
        return result.AsReadOnly();
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
    public static void BuildTree<TSource, TItem>(
        [DisallowNull] this IEnumerable<TSource> rootElements,
        [DisallowNull] in Func<TSource, TItem> getNewItem,
        [DisallowNull] in Func<TSource, IEnumerable<TSource>> getChildren,
        [DisallowNull] in Action<TItem> addToRoots,
        [DisallowNull] in Action<TItem, TItem> addChild)
    {
        Check.IfArgumentNotNull(rootElements);
        Check.IfArgumentNotNull(getNewItem);
        Check.IfArgumentNotNull(getChildren);
        Check.IfArgumentNotNull(addToRoots);
        Check.IfArgumentNotNull(addChild);

        foreach (var siteMap in rootElements)
        {
            var root = getNewItem(siteMap);
            addToRoots(root);
            addChildren(siteMap, root, getChildren, addChild, getNewItem);
        }

        static void addChildren(in TSource siteMap, in TItem parent, Func<TSource, IEnumerable<TSource>> getChildren, Action<TItem, TItem> addChild, Func<TSource, TItem> getNewItem)
        {
            foreach (var sm in getChildren(siteMap))
            {
                var newChile = getNewItem(sm);
                addChild(parent, newChile);
                addChildren(sm, newChile, getChildren, addChild, getNewItem);
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
        Check.IfArgumentNotNull(converter);

        foreach (var item in input)
        {
            yield return converter(item);
        }
    }

    [Obsolete("Use .Net 6.0 Chunk, instead.")]
    public static IEnumerable<IEnumerable<T>> ChunkBy<T>([DisallowNull] this IEnumerable<T> source, int chunkSize) => source.Select((x, i) => new { Index = i, Value = x })
                        .GroupBy(x => x.Index / chunkSize)
                        .Select(x => x.Select(v => v.Value));

    public static T ClearAndAdd<T>([DisallowNull] this T collection, in object? item)
        where T : notnull, IList
    {
        collection.ArgumentNotNull().Clear();
        _ = collection.Add(item);
        return collection;
    }

    public static ICollection<TItem> ClearAndAddRange<TItem>(this ICollection<TItem> list, [DisallowNull] in IEnumerable<TItem> items)
    {
        Check.IfArgumentNotNull(items);

        list.Clear();
        foreach (var item in items)
        {
            list.Add(item);
        }
        return list;
    }

    public static IEnumerable<TItem> Collect<TItem>(IEnumerable<TItem> items)
        where TItem : IHasChild<TItem>
    {
        foreach (var item in items)
        {
            yield return item;

            foreach (var child in Collect(item.Children))
            {
                yield return child;
            }
        }
    }

    public static IEnumerable<TItem> Collect<TItem>(this TItem root)
        where TItem : IHasChild<TItem>
        => Collect(AsEnumerableItem(root));

    public static IEnumerable<TSource> Compact<TSource>(this IEnumerable<TSource?>? items)
                where TSource : class => items is null
            ? Enumerable.Empty<TSource>()
            : items.Where(x => x is not null).Select(x => x!);

    public static bool ContainsKey<TKey, TValue>([DisallowNull] this IEnumerable<(TKey Key, TValue Value)> source, TKey key) => source.ArgumentNotNull().Where(kv => kv.Key?.Equals(key) ?? key is null).Any();

    public static int CountNotEnumerated<T>(this IEnumerable<T> source)
    {
        (var succeed, var count) = TryCountNotEnumerated(source);
        return succeed ? count : throw new Exceptions.Validations.InvalidOperationValidationException();
    }

    [return: NotNull]
    public static IEnumerable<T> DefaultIfEmpty<T>(IEnumerable<T>? items) => items is null ? Enumerable.Empty<T>() : items;

    /// <summary>
    ///     Creates an empty array.
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    /// <returns> </returns>
    public static T[] EmptyArray<T>() => Array.Empty<T>();

    public static IEnumerable<TItem> Exclude<TItem>(this IEnumerable<TItem> source, Func<TItem, bool> exclude) => source.Where(x => !exclude(x));

    public static IEnumerable<T> FindDuplicates<T>(in IEnumerable<T> items) => items.ArgumentNotNull(nameof(items)).GroupBy(x => x).Where(g => g.Count() > 1).Select(y => y.Key);

    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> items, [DisallowNull] Action<T> action)
    {
        Check.IfArgumentNotNull(items);
        var buffer = items;

        foreach (var item in buffer)
        {
            action?.Invoke(item);
            yield return item;
        }
    }

    public static async IAsyncEnumerable<TItem> ForEachAsync<TItem>([DisallowNull] this IAsyncEnumerable<TItem> asyncItems, Func<TItem, TItem> action, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Check.IfArgumentNotNull(asyncItems);
        await foreach (var item in asyncItems.WithCancellation(cancellationToken))
        {
            yield return action(item);
        }
    }

    public static async IAsyncEnumerable<TItem> ForEachAsync<TItem>([DisallowNull] this IAsyncEnumerable<TItem> asyncItems, Action<TItem> action, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Check.IfArgumentNotNull(asyncItems);
        await foreach (var item in asyncItems.WithCancellation(cancellationToken))
        {
            action(item);
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

    public static IEnumerable<TResult> ForEachItem<T, TResult>([DisallowNull] this IEnumerable<T> items, [DisallowNull] Func<T, TResult> action)
    {
        Check.IfArgumentNotNull(items);
        Check.IfArgumentNotNull(action);

        foreach (var item in items)
        {
            yield return action(item);
        }
    }

    public static void ForEachTreeNode<T>(T root, Func<T, IEnumerable<T>>? getChildren, Action<T>? rootAction, Action<T, T>? childAction)
        where T : class
    {
        if (root is null)
        {
            return;
        }

        rootAction?.Invoke(root);
        _ = (getChildren?.Invoke(root)
            .ForEach(c =>
            {
                childAction?.Invoke(c, root);
                ForEachTreeNode(c, getChildren, rootAction, childAction);
            }));
    }

    public static IEnumerable<T> GetAll<T>([DisallowNull] Func<IEnumerable<T>> getRootElements, [DisallowNull] Func<T, IEnumerable<T>?> getChildren)
    {
        _ = getRootElements.ArgumentNotNull();
        _ = getChildren.ArgumentNotNull();

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

    public static TValue GetByKey<TKey, TValue>(this IEnumerable<(TKey Key, TValue Value)> source, TKey key) => source.ArgumentNotNull(nameof(source)).Where(kv => kv.Key?.Equals(key) ?? key is null).First().Value;

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

    public static TryMethodResult<TResult?> GetValue<TResult>([DisallowNull] this HashSet<TResult> resultSet, TResult equalValue)
        where TResult : new()
    {
        var tryResult = resultSet.TryGetValue(equalValue, out var actualValue);
        return new(tryResult, actualValue);
    }

    public static IEnumerable<(T Item, int Count)> GroupCounts<T>(in IEnumerable<T> items) => items.GroupBy(x => x).Select(x => (x.Key, x.Count()));

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

    public static string MergeToString<T>(this IEnumerable<T> source) => source.Aggregate(new StringBuilder(), (current, item) => current.Append(item)).ToString();

    public static async IAsyncEnumerable<int> RangeAsync(int start, int count)
    {
        await Task.Yield();
        foreach (var num in Enumerable.Range(start, count))
        {
            for (var i = 0; i < count; i++)
            {
                yield return start + i;
            }
        }
    }

    public static IList<KeyValuePair<TKey, TValue>> RemoveByKey<TKey, TValue>([DisallowNull] this IList<KeyValuePair<TKey, TValue>> list, in TKey key)
    {
        Check.IfArgumentNotNull(list);
        _ = list.Remove(list.GetItemByKey(key));
        return list;
    }

    public static IEnumerable<TSource> RemoveDefaults<TSource>(this IEnumerable<TSource> source, TSource? defaultValue = default) => defaultValue is null ? source.Where(item => item is not null) : source.Where(item => (item?.Equals(defaultValue)) ?? true);

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

    public static IEnumerable<T> SelectAll<T>(this IEnumerable<IEnumerable<T>> values)
    {
        foreach (var value in values)
        {
            foreach (var item in value)
            {
                yield return item;
            }
        }
    }

    public static async IAsyncEnumerable<TResult> SelectAsync<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Task<TResult>> selectorAsync)
    {
        foreach (var item in source)
        {
            yield return await selectorAsync(item);
        }
    }

    public static IList<KeyValuePair<TKey, TValue>> SetByKey<TKey, TValue>([DisallowNull] this IList<KeyValuePair<TKey, TValue>> list, in TKey key, in TValue value)
    {
        Check.IfArgumentNotNull(list);
        _ = list.RemoveByKey(key);
        list.Add(new(key, value));
        return list;
    }

    public static Dictionary<TKey, TValue> SetByKey<TKey, TValue>([DisallowNull] this Dictionary<TKey, TValue> dic, TKey key, TValue value)
        where TKey : notnull
    {
        //! Still in Alpha test. Not operational.
        //x dic.ContainsKey(key).If().Then(() => dic[key] = value).Else(() => dic.Add(key, value)).Build();
        if (dic.ContainsKey(key))
        {
            dic[key] = value;
        }
        else
        {
            dic.Add(key, value);
        }

        return dic;
    }

    public static Dictionary<TKey, TValue>? ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>>? pairs)
        where TKey : notnull => pairs?.ToDictionary(pair => pair.Key, pair => pair.Value);

    public static IEnumerable<T> ToEnumerable<T>(this IEnumerable<T> source)
    {
        if (source is not null)
        {
            foreach (var item in source)
            {
                yield return item;
            }
        }
    }

    public static IEnumerable<(TKey, TValue)> ToEnumerable<TKey, TValue>(this Dictionary<TKey, TValue> source)
        where TKey : notnull
    {
        if (source is not null)
        {
            foreach (var item in source)
            {
                yield return (item.Key, item.Value);
            }
        }
    }

    public static async Task<IEnumerable<TItem>> ToEnumerableAsync<TItem>([DisallowNull] this IAsyncEnumerable<TItem> asyncItems, CancellationToken cancellationToken = default)
    {
        Check.IfArgumentNotNull(asyncItems);
        var result = New<List<TItem>>();
        await foreach (var item in asyncItems.WithCancellation(cancellationToken))
        {
            result.Add(item);
        }
        return result;
    }

    public static async Task<List<TItem>> ToListAsync<TItem>([DisallowNull] this IAsyncEnumerable<TItem> asyncItems, CancellationToken cancellationToken = default)
    {
        Check.IfArgumentNotNull(asyncItems);
        var result = New<List<TItem>>();
        await foreach (var item in asyncItems.WithCancellation(cancellationToken))
        {
            result.Add(item);
        }
        return result;
    }

    [return: NotNull]
    public static async Task<List<TItem>> ToListCompactAsync<TItem>(this IAsyncEnumerable<TItem?>? asyncItems, CancellationToken cancellationToken = default) => asyncItems is null
            ? await ToListAsync(EmptyAsyncEnumerable<TItem>.Empty, cancellationToken: cancellationToken)
            : await WhereAsync(asyncItems, x => x is not null, cancellationToken).ToListAsync(cancellationToken: cancellationToken);

    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source) => new(source);

    public static IReadOnlyList<T> ToReadOnlyList<T>([DisallowNull] this IEnumerable<T> items)
        => new List<T>(items).AsReadOnly();

    public static IReadOnlySet<T> ToReadOnlySet<T>([DisallowNull] this IEnumerable<T> items)
        => ImmutableList.CreateRange(items).ToHashSet();

    public static TryMethodResult<int> TryCountNotEnumerated<T>([DisallowNull] this IEnumerable<T> source) => new(source.TryGetNonEnumeratedCount(out var count), count);

    public static async IAsyncEnumerable<TItem> WhereAsync<TItem>([DisallowNull] this IAsyncEnumerable<TItem> asyncItems, Func<TItem, bool>? func, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var item in asyncItems.WithCancellation(cancellationToken))
        {
            if (func is not null && func(item))
            {
                yield return item;
            }
        }
    }
}