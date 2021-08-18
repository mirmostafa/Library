using System.Collections;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace Library.Helpers;
public static class EnumerableHelper
{
    public static IList<KeyValuePair<TKey, TValue>>? Add<TKey, TValue>(this IList<KeyValuePair<TKey, TValue>> list, in TKey key, in TValue value)
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

    public static void AddRange<T>(this ICollection<T> collection, params T[] newItems)
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

    public static IList<T> AddRange<T>(this IList<T> list, in IEnumerable<T> items)
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

    public static TList AddRange<TList, TItem>(this TList list, in IEnumerable<TItem> items)
        where TList : IList<TItem>
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
            foreach (var i in source.ArgumentNotNull())
            {
                yield return i;
            }
        }

        if (items is not null)
        {
            foreach (var item in items.ArgumentNotNull())
            {
                yield return item;
            }
        }
    }

    public static IEnumerable<T> Aggregate<T>(this IEnumerable<IEnumerable<T>> sources)
    {
        foreach (var items in sources.ArgumentNotNull())
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

    public static IReadOnlyList<T> Apply<T>(this IEnumerable<T> items)
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

    public static IReadOnlyList<T> AsReadOnlyList<T>(this IEnumerable<T> items)
    {
        Check.IfArgumentNotNull(items, nameof(items));

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
    public static void BuildTree<TSource, TItem>(this IEnumerable<TSource> rootElements,
        Func<TSource, TItem> getNewItem,
        Func<TSource, IEnumerable<TSource>> getChildren,
        in Action<TItem> addToRoots,
        Action<TItem, TItem> addChild)
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
            AddChildren(siteMap, root);
        }

        void AddChildren(in TSource siteMap, in TItem parent)
        {
            foreach (var sm in getChildren(siteMap))
            {
                var newChile = getNewItem(sm);
                addChild(parent, newChile);
                AddChildren(sm, newChile);
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
    public static IEnumerable<TOutput> Cast<TInput, TOutput>(this IEnumerable<TInput> input, Converter<TInput, TOutput> converter)
    {
        Check.IfArgumentNotNull(converter, nameof(converter));

        if (input is null)
        {
            yield break;
        }

        foreach (var item in input)
        {
            yield return converter(item);
        }
    }

    public static T ClearAndAdd<T>(this T collection, in object? item)
        where T : IList
    {
        collection.Clear();
        _ = collection.Add(item);
        return collection;
    }

    public static T ClearAndAddRange<T>(this T collection, in IEnumerable items)
        where T : IList
    {
        Check.IfArgumentNotNull(items, nameof(items));

        collection.Clear();
        return AddRange(collection, items);
    }

    public static TList ClearAndAddRange<TList, TItem>(this TList collection, in IEnumerable<TItem> items)
        where TList : IList<TItem>
    {
        Check.IfArgumentNotNull(items, nameof(items));

        collection.Clear();
        return AddRange(collection, items);
    }

    public static T AddRange<T>(T collection, IEnumerable items)
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
        if (items?.Any() is not true)
        {
            items = Enumerable.Empty<TSource>();
        }

        foreach (var item in items)
        {
            if (item is not null)
            {
                yield return item;
            }
        }
    }

    public static bool ContainsKey<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source, TKey key)
        where TKey : notnull => source.Any(x => x.Key?.Equals(key) ?? key is null);

    public static IEnumerable<T> FindDuplicates<T>(in IEnumerable<T> items)
    {
        var query = items.GroupBy(x => x).Where(g => g.Count() > 1).Select(y => y.Key);
        return query;
    }

    public static IEnumerable<TResult> ForEach<T, TResult>(this IEnumerable<T> items, Func<T, TResult> action)
    {
        Check.IfArgumentNotNull(items, nameof(items));
        Check.IfArgumentNotNull(action, nameof(action));
        foreach (var item in items)
        {
            yield return action(item);
        }
    }

    public static void ForEach<T>(this IEnumerable<T> items, in Action<T> action)
    {
        Check.IfArgumentNotNull(items, nameof(items));
        foreach (var item in items)
        {
            action?.Invoke(item);
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
        getChildren?.Invoke(root)
            .ForEach(c =>
            {
                childAction?.Invoke(c, root);
                ForEachTreeNode(c, getChildren, rootAction, childAction);
            });
    }

    public static IEnumerable<T> GetAll<T>(Func<IEnumerable<T>> getRootElements, Func<T, IEnumerable<T>?> getChildren)
    {
        getRootElements.IfArgumentNotNull(nameof(getRootElements));
        getChildren.IfArgumentNotNull(nameof(getChildren));

        var result = new List<T>();

        foreach (var item in getRootElements())
        {
            result.Add(item);
            FindChildren(item);
        }

        return result.AsEnumerable();

        void FindChildren(in T item)
        {
            var children = getChildren(item);
            if (children?.Any() is true)
            {
                foreach (var child in children)
                {
                    result.Add(child);
                    FindChildren(child);
                }
            }
        }
    }

    public static TValue GetByKey<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source, in TKey key)
    {
        foreach (var item in source.ArgumentNotNull())
        {
            if (item.Key?.Equals(key) is true)
            {
                return item.Value;
            }
        }
        throw new KeyNotFoundException(nameof(key));
    }

    public static KeyValuePair<TKey, TValue> GetByKey<TKey, TValue>(this IList<KeyValuePair<TKey, TValue>> list, TKey key)
        => list.FirstOrDefault(item => item.Key?.Equals(key) is true);

    public static IEnumerable<(T Item, int Count)> GroupCounts<T>(in IEnumerable<T> items)
    {
        var query = items.GroupBy(x => x).Select(x => (x.Key, x.Count()));
        return query;
    }

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

    public static IList<KeyValuePair<TKey, TValue>> RemoveByKey<TKey, TValue>(this IList<KeyValuePair<TKey, TValue>> list, in TKey key)
    {
        Check.IfArgumentNotNull(list, nameof(list));
        _ = list.Remove(GetByKey(list, key));
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

    public static IList<KeyValuePair<TKey, TValue>> SetByKey<TKey, TValue>(this IList<KeyValuePair<TKey, TValue>> list, in TKey key, in TValue value)
    {
        Check.IfArgumentNotNull(list, nameof(list));
        _ = list.RemoveByKey(key);
        list.Add(new(key, value));
        return list;
    }

    public static Dictionary<TKey, TValue>? ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>>? pairs)
         where TKey : notnull => pairs?.ToDictionary(pair => pair.Key, pair => pair.Value);

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

    public static async IAsyncEnumerable<TSource> ToEnumerableAsync<TSource>(this IQueryable<TSource> source, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var element in source.AsAsyncEnumerable().WithCancellation(cancellationToken))
        {
            yield return element;
        }
    }
}