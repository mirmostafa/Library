using System.Collections;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Library.Collections;
using Library.Interfaces;
using Library.Results;
using Library.Validations;

namespace Library.Helpers;

[DebuggerStepThrough]
[StackTraceHidden]
public static class EnumerableHelper
{
    /// <summary>
    /// Adds an item to the System.Collections.Generic.ICollection`1.
    /// </summary>
    /// <typeparam name="TDic">The type of the dic.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dic">The dic.</param>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static TDic Add<TDic, TKey, TValue>([DisallowNull] this TDic dic, in TKey key, in TValue value)
        where TDic : IList<KeyValuePair<TKey, TValue>>
    {
        Check.IfArgumentNotNull(dic);
        dic.Add(new(key, value));
        return dic;
    }

    /// <summary>
    /// Adds the specified item immutably and returns a new instance of source.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">The source.</param>
    /// <param name="item">The item.</param>
    /// <returns></returns>
    public static IEnumerable<T> AddImmuted<T>(this IEnumerable<T>? source, T item)
    {
        if (source != null)
        {
            foreach (var i in source)
            {
                yield return i;
            }
        }

        yield return item;
    }

    /// <summary>
    /// Adds the specified item immutably and returns a new instance of source.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">The source.</param>
    /// <param name="item">The item.</param>
    /// <returns></returns>
    public static TList AddImmuted<TList, T>(this TList? source, T item)
        where TList : IList<T>, new()
    {
        var result = new TList();
        if (source?.Any() is true)
        {
            _ = result.AddRange(source);
        }
        result.Add(item);

        return result;
    }

    /// <summary>
    /// Adds a range of items to an IList.
    /// </summary>
    /// <typeparam name="T">The type of the items in the list.</typeparam>
    /// <param name="list">The list to add the items to.</param>
    /// <param name="items">The items to add to the list.</param>
    /// <returns>The list with the added items.</returns>
    //public static IList<T> AddRange<T>([DisallowNull] this IList<T> list, in IEnumerable<T> items)
    //{
    //    if (items?.Any() is true)
    //    {
    //        foreach (var item in items)
    //        {
    //            list.Add(item);
    //        }
    //    }
    //    return list;
    //}
    public static TList AddRange<TList, TItem>([DisallowNull] this TList list, in IEnumerable<TItem> items)
        where TList : ICollection<TItem>
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

    /// <summary>
    /// Adds a range of items to an ObservableCollection.
    /// </summary>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    /// <param name="list">The ObservableCollection to add the items to.</param>
    /// <param name="items">The items to add to the collection.</param>
    /// <returns>The ObservableCollection with the added items.</returns>
    [return: NotNullIfNotNull(nameof(list))]
    public static ObservableCollection<T>? AddRange<T>(this ObservableCollection<T>? list, in IEnumerable<T>? items)
    {
        if (list != null && items?.Any() is true)
        {
            foreach (var item in items)
            {
                list.Add(item);
            }
        }
        return list;
    }

    /// <summary>
    /// Adds a range of items to an ICollection.
    /// </summary>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    /// <param name="list">The ICollection to add the items to.</param>
    /// <param name="items">The items to add to the ICollection.</param>
    /// <returns>The ICollection with the added items.</returns>
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

    /// <summary>
    /// Adds a range of items to the <see cref="IFluentListTFluentList, TItem"/>.
    /// </summary>
    /// <typeparam name="TFluentList">The type of the fluent list.</typeparam>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    /// <param name="list">The list.</param>
    /// <param name="items">The items to add.</param>
    /// <returns>The list.</returns>
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

    /// <summary>
    /// Adds a range of items to an ICollection asynchronously.
    /// </summary>
    /// <typeparam name="TItem">The type of the items.</typeparam>
    /// <param name="list">The ICollection to add the items to.</param>
    /// <param name="asyncItems">The IAsyncEnumerable of items to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The ICollection with the added items.</returns>
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

    /// <summary>
    /// Adds a range of items to an existing IEnumerable in an immutable fashion.
    /// </summary>
    /// <typeparam name="T">The type of the items in the IEnumerable.</typeparam>
    /// <param name="source">The source IEnumerable.</param>
    /// <param name="items">The items to add.</param>
    /// <returns>A new IEnumerable containing the items from the source and the items to add.</returns>
    public static IEnumerable<T> AddRangeImmuted<T>(this IEnumerable<T>? source, IEnumerable<T>? items)
    {
        //Check if both source and items are null
        return (source, items) switch
        {
            (null, null) => Enumerable.Empty<T>(),
            //If source is not null, return source
            (_, null) => source,
            //If items is not null, return items
            (null, _) => items,
            //If both source and items are not null, call addRangeImmutedIterator
            (_, _) => addRangeImmutedIterator(source, items)
        };
        //Function to add items from both source and items
        static IEnumerable<T> addRangeImmutedIterator(IEnumerable<T> source, IEnumerable<T> items)
        {
            //Loop through source and yield each item
            foreach (var item in source)
            {
                yield return item;
            }
            //Loop through items and yield each item
            foreach (var item in items)
            {
                yield return item;
            }
        }
    }

    public static T Aggregate<T>(this T[] items, Func<T, T?, T> aggregator, T defaultValue = default!)
        => InnerAggregate(items, aggregator, defaultValue);

    public static T Aggregate<T>(this IEnumerable<T> items, Func<T, T?, T> aggregator, T defaultValue = default!)
        => InnerAggregate(items.ToArray(), aggregator, defaultValue);

    public static T Aggregate<T>(this T[] items, T defaultValue = default!) where T : IAdditionOperators<T, T, T>
        => InnerAggregate(items, (x, y) => x + y!, defaultValue);

    public static T Aggregate<T>(this IEnumerable<T> items, T defaultValue = default!) where T : IAdditionOperators<T, T, T>
        => InnerAggregate(items.ToArray(), (x, y) => x + y!, defaultValue);

    public static bool Any([NotNullWhen(true)] this IEnumerable? source)
        => source switch
        {
            null => false,
            ICollection collection => collection.Count > 0,
            _ => source.GetEnumerator().MoveNext()
        };

    public static bool Any<T>(this IList<T> list)
            => list.Count != 0;

    /// <summary>
    /// Get a <see cref="Span{T}"/> view over a <see cref="List{T}"/>'s data. Items should not be
    /// added or removed from the <see cref="List{T}"/> while the <see cref="Span{T}"/> is in use.
    /// </summary>
    /// <param name="list">The list to get the data view over.</param>
    public static Span<TItem> AsSpan<TItem>(this List<TItem> list)
        => CollectionsMarshal.AsSpan(list);

    public static Span<TItem> AsSpan<TItem>(this IEnumerable<TItem> items)
        => items is null ? default : MemoryExtensions.AsSpan(items.ToArray());

#if NET8_0_OR_GREATER
    [Obsolete("Use `AsImmutableArray` instead.", true)]
#endif
    public static ImmutableArray<TItem> AsImmutableArrayUnsafe<TItem>(this TItem[] itemArray)
        => Unsafe.As<TItem[], ImmutableArray<TItem>>(ref itemArray);
#if NET8_0_OR_GREATER
    public static ImmutableArray<TItem> AsImmutableArray<TItem>(this TItem[] itemArray)
        => ImmutableCollectionsMarshal.AsImmutableArray(itemArray);
#endif

    /// <summary>
    /// Builds a read-only list from an enumerable.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
    /// <param name="items">The enumerable to build the read-only list from.</param>
    /// <returns>A read-only list containing the elements from the enumerable.</returns>
    public static IReadOnlyList<T> Build<T>([DisallowNull] this IEnumerable<T> items)
    {
        Check.IfArgumentNotNull(items);

        return Array.AsReadOnly(items.ToArray());
    }

    /// <summary>
    /// Builds a tree from a given root element and its children.
    /// </summary>
    /// <typeparam name="TSource">The type of the root element.</typeparam>
    /// <typeparam name="TItem">The type of the item to be created.</typeparam>
    /// <param name="rootElements">The root elements.</param>
    /// <param name="getNewItem">A function that takes a TSource and returns a TItem.</param>
    /// <param name="getChildren">
    /// A function that takes a TSource and returns an IEnumerable of TSource.
    /// </param>
    /// <param name="addToRoots">An action that takes a TItem.</param>
    /// <param name="addChild">An action that takes two TItems.</param>
    public static void BuildTree<TSource, TItem>(
                [DisallowNull] this IEnumerable<TSource> rootElements, // rootElements is an IEnumerable of type TSource
                [DisallowNull] in Func<TSource, TItem> getNewItem, // getNewItem is a function that takes a TSource and returns a TItem
                [DisallowNull] in Func<TSource, IEnumerable<TSource>> getChildren, // getChildren is a function that takes a TSource and returns an IEnumerable of TSource
                [DisallowNull] in Action<TItem> addToRoots, // addToRoots is an action that takes a TItem
                [DisallowNull] in Action<TItem, TItem> addChild) // addChild is an action that takes two TItems
    {
        Check.IfArgumentNotNull(rootElements); // Check that rootElements is not null
        Check.IfArgumentNotNull(getNewItem); // Check that getNewItem is not null
        Check.IfArgumentNotNull(getChildren); // Check that getChildren is not null
        Check.IfArgumentNotNull(addToRoots); // Check that addToRoots is not null
        Check.IfArgumentNotNull(addChild); // Check that addChild is not null

        foreach (var siteMap in rootElements) // Iterate through each element in rootElements
        {
            var root = getNewItem(siteMap); // Get a new item from the current element in rootElements
            addToRoots(root); // Add the new item to the roots
            addChildren(siteMap, root, getChildren, addChild, getNewItem); // Call the addChildren function to add the children of the current element
        }

        static void addChildren(in TSource siteMap, in TItem parent, Func<TSource, IEnumerable<TSource>> getChildren, Action<TItem, TItem> addChild, Func<TSource, TItem> getNewItem)
        {
            foreach (var sm in getChildren(siteMap)) // Iterate through each element in the children of the current element
            {
                var newChile = getNewItem(sm); // Get a new item from the current element in the children
                addChild(parent, newChile); // Add the new item to the parent
                addChildren(sm, newChile, getChildren, addChild, getNewItem); // Call the addChildren function to add the children of the current element
            }
        }
    }

    /// <summary>
    /// Casts the elements of an IEnumerable to the specified type using the specified converter.
    /// </summary>
    /// <typeparam name="TInput">The type of the elements of input.</typeparam>
    /// <typeparam name="TOutput">The type to cast the elements of input to.</typeparam>
    /// <param name="input">The IEnumerable to cast.</param>
    /// <param name="converter">
    /// A Converter delegate that converts each element from one type to another.
    /// </param>
    /// <returns>
    /// An IEnumerable that contains each element of the source sequence converted to the specified type.
    /// </returns>
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
    public static IEnumerable<IEnumerable<T>> ChunkBy<T>([DisallowNull] this IEnumerable<T> source, int chunkSize)
        => source.Select((x, i) => new { Index = i, Value = x })
                 .GroupBy(x => x.Index / chunkSize)
                 .Select(x => x.Select(v => v.Value));

    /// <summary>
    /// Clears the specified collection and adds the specified item to it.
    /// </summary>
    /// <typeparam name="T">The type of the collection.</typeparam>
    /// <param name="collection">The collection to clear and add to.</param>
    /// <param name="item">The item to add to the collection.</param>
    /// <returns>The collection with the item added.</returns>
    public static T ClearAndAdd<T>([DisallowNull] this T collection, in object? item)
            where T : notnull, IList
    {
        collection.ArgumentNotNull().Clear();
        _ = collection.Add(item);
        return collection;
    }

    /// <summary>
    /// Clears the list and adds a range of items to it.
    /// </summary>
    /// <typeparam name="TItem">The type of the items.</typeparam>
    /// <param name="list">The list to clear and add items to.</param>
    /// <param name="items">The items to add.</param>
    /// <returns>The list with the added items.</returns>
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

    /// <summary>
    /// Returns an empty IEnumerable of the specified type, regardless of the input.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">The IEnumerable to clear.</param>
    /// <returns>An empty IEnumerable of the specified type.</returns>
    [return: NotNull]
    public static IEnumerable<T> ClearImmuted<T>(this IEnumerable<T>? source)
            => Enumerable.Empty<T>();

    /// <summary> Recursively collects all items in a collection of items that implement
    /// IParent<TItem>. </summary> <param name="items">The collection of items to collect.</param>
    /// <returns>A collection of all items in the collection, including all children.</returns>
    public static IEnumerable<TItem> Collect<TItem>(IEnumerable<TItem> items)
            where TItem : IParent<TItem>
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

    /// <summary>
    /// Returns an IEnumerable of non-null elements from the given IEnumerable of nullable elements.
    /// </summary>
    public static IEnumerable<TSource> Compact<TSource>(this IEnumerable<TSource?>? items) where TSource : class =>
        items?
             .Where([DebuggerStepThrough] (x) => x is not null)
             .Select([DebuggerStepThrough] (x) => x!) ?? Enumerable.Empty<TSource>();

    /// <summary>
    /// Checks if the given IEnumerable contains a key-value pair with the specified key.
    /// </summary>
    public static bool ContainsKey<TKey, TValue>([DisallowNull] this IEnumerable<(TKey Key, TValue Value)> source, TKey key) =>
        source.ArgumentNotNull().Where(kv => kv.Key?.Equals(key) ?? key is null).Any();

    public static T[] Copy<T>(this T[] array) =>
        array.ToEnumerable().ToArray();

    /// <summary> Counts the number of elements in a sequence that are not enumerated. </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam> <param name="source">The
    /// IEnumerable<T> to count.</param> <returns>The number of elements in the sequence that are
    /// not enumerated.</returns>
    public static int CountNotEnumerated<T>(this IEnumerable<T> source)
    {
        (var succeed, var count) = TryCountNonEnumerated(source);
        return succeed ? count : throw new Exceptions.Validations.InvalidOperationValidationException();
    }

    /// <summary>
    /// Returns an empty IEnumerable if the given IEnumerable is null, otherwise returns the given IEnumerable.
    /// </summary>
    [return: NotNull]
    public static IEnumerable<T> DefaultIfEmpty<T>(IEnumerable<T>? items)
            => items is null ? Enumerable.Empty<T>() : items;

    public static Dictionary<TKey, TValue> DictionaryFromKeys<TKey, TValue>(IEnumerable<TKey> keys, TValue? defaultValue = default)
                where TKey : notnull
    {
        var result = new Dictionary<TKey, TValue>();
        foreach (var key in keys)
        {
            result.Add(key, default!);
        }
        return result;
    }

    /// <summary>
    /// Creates an empty array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T[] EmptyArray<T>()
        => Array.Empty<T>();

    /// <summary>
    /// Compares two IEnumerable objects for equality.
    /// </summary>
    /// <typeparam name="T">The type of the objects in the IEnumerable.</typeparam>
    /// <param name="enum1">The first IEnumerable to compare.</param>
    /// <param name="enum2">The second IEnumerable to compare.</param>
    /// <param name="ignoreIndexes">Whether to ignore the order of the elements when comparing.</param>
    /// <returns>True if the two IEnumerables are equal, false otherwise.</returns>
    public static bool Equal<T>(IEnumerable<T> enum1, IEnumerable<T> enum2, bool ignoreIndexes)
            => ignoreIndexes
                ? !enum1.ArgumentNotNull().Except(enum2).Any() && !enum2.ArgumentNotNull().Except(enum1).Any()
                : enum1.SequenceEqual(enum2);

    /// <summary>
    /// Returns a collection of elements from the input sequence that do not satisfy the specified predicate.
    /// </summary>
    public static IEnumerable<T> Except<T>(this IEnumerable<T> items, Func<T, bool> exceptor)
            => items.Where(x => !exceptor(x));

    /// <summary>
    /// Returns an IEnumerable of TItem from the source IEnumerable, excluding any items that match
    /// the given exclude Func.
    /// </summary>
    public static IEnumerable<TItem> Exclude<TItem>(this IEnumerable<TItem> source, Func<TItem, bool> exclude)
            => source.Where(x => !exclude(x));

    public static IEnumerable<TSource> Filter<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
            => source.Where(predicate);

    /// <summary>
    /// Finds the duplicates in a given IEnumerable of type T.
    /// </summary>
    /// <returns>An IEnumerable of type T containing the duplicates.</returns>
    public static IEnumerable<T> FindDuplicates<T>(this IEnumerable<T> items)
    {
        //Create a new HashSet to store the items
        var buffer = new HashSet<T>();
        //Return the items from the IEnumerable collection that are not added to the HashSet
        return items.Where(x => !buffer.Add(x));
    }

    /// <summary>
    /// Applies a folder function to each item in the IEnumerable and returns the result.
    /// </summary>
    /// <param name="items">The IEnumerable to fold.</param>
    /// <param name="folder">The folder function to apply.</param>
    /// <param name="initialValue">The initial value to use.</param>
    /// <returns>The result of the fold.</returns>
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

    /// <summary>
    /// Executes an action for each item in the IEnumerable.
    /// </summary>
    /// <typeparam name="T">The type of the items in the IEnumerable.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="items">The IEnumerable to iterate over.</param>
    /// <param name="action">The action to execute for each item.</param>
    /// <returns>An IEnumerable containing the results of the action.</returns>
    public static IEnumerable<TResult> ForEach<T, TResult>([DisallowNull] this IEnumerable<T> items, [DisallowNull] Func<T, TResult> action)
    {
        Check.IfArgumentNotNull(items);
        Check.IfArgumentNotNull(action);

        foreach (var item in items)
        {
            yield return action(item);
        }
    }
    //public static (TItems Items, IEnumerable<TResult> Results) ForEach<TItems, TItem, TResult>([DisallowNull] this TItems items, [DisallowNull] Func<TItem, TResult> action)
    //    where TItems: IEnumerable<TItem>
    //{
    //    return (items, items.ForEach(action));
    //}

    /// <summary>
    /// Asynchronously iterates over an <see cref="IAsyncEnumerableTItem"/> and applies an action to
    /// each item.
    /// </summary>
    /// <typeparam name="TItem">The type of the items in the <see cref="IAsyncEnumerableTItem"/>.</typeparam>
    /// <param name="asyncItems">The <see cref="IAsyncEnumerableTItem"/> to iterate over.</param>
    /// <param name="action">The action to apply to each item.</param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// An <see cref="IAsyncEnumerableTItem"/> containing the results of applying the action to each item.
    /// </returns>
    public static async IAsyncEnumerable<TItem> ForEachAsync<TItem>([DisallowNull] this IAsyncEnumerable<TItem> asyncItems, Func<TItem, TItem> action, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Check.IfArgumentNotNull(asyncItems);
        await foreach (var item in asyncItems.WithCancellation(cancellationToken))
        {
            yield return action(item);
        }
    }

    /// <summary>
    /// Asynchronously iterates over an <see cref="IAsyncEnumerableTItem"/> and performs an action
    /// on each item.
    /// </summary>
    /// <typeparam name="TItem">The type of the items in the <see cref="IAsyncEnumerableTItem"/>.</typeparam>
    /// <param name="asyncItems">The <see cref="IAsyncEnumerableTItem"/> to iterate over.</param>
    /// <param name="action">The action to perform on each item.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe.</param>
    /// <returns>The <see cref="IAsyncEnumerableTItem"/>.</returns>
    public static async IAsyncEnumerable<TItem> ForEachAsync<TItem>([DisallowNull] this IAsyncEnumerable<TItem> asyncItems, Action<TItem> action, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Check.IfArgumentNotNull(asyncItems);
        await foreach (var item in asyncItems.WithCancellation(cancellationToken))
        {
            action(item);
            yield return item;
        }
    }

    /// <summary>
    /// Executes an action for each item in the given enumerable and returns a read-only list of the items.
    /// </summary>
    /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
    /// <param name="items">The enumerable of items.</param>
    /// <param name="action">The action to execute for each item.</param>
    /// <returns>A read-only list of the items.</returns>
    public static IReadOnlyList<T> ForEachEager<T>(this IEnumerable<T> items, [DisallowNull] Action<T> action)
    {
        Check.IfArgumentNotNull(items);
        foreach (var item in items)
        {
            action?.Invoke(item);
        }
        return items.Build();
    }

    /// <summary>
    /// Executes the specified action for each item in the specified collection in parallel.
    /// </summary>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    /// <param name="items">The collection of items.</param>
    /// <param name="action">The action to execute for each item.</param>
    public static void ForEachParallel<TItem>(IEnumerable<TItem> items, Action<TItem> action)
            => Parallel.ForEach(items, action);

    /// <summary>
    /// Recursively iterates through a tree of objects, performing an action on each node.
    /// </summary>
    /// <typeparam name="T">The type of the tree nodes.</typeparam>
    /// <param name="root">The root node of the tree.</param>
    /// <param name="getChildren">A function to get the children of a node.</param>
    /// <param name="rootAction">An action to perform on the root node.</param>
    /// <param name="childAction">An action to perform on each child node.</param>
    /// <remarks>
    /// This code is a recursive method that performs an action on each node of a tree.
    /// It takes in a root node, a function to get the children of a node, an action to perform on the root node, and an action to perform on each child node.
    ///</remarks>
    public static void ForEachTreeNode<T>(T root, Func<T, IEnumerable<T>>? getChildren, Action<T>? rootAction, Action<T, T>? childAction)
                        where T : class
    {
        //If the root node is null, return
        if (root is null)
        {
            return;
        }

        //Perform the root action on the root node
        rootAction?.Invoke(root);

        //For each child node, perform the child action and recursively call the method on the child node
        _ = (getChildren?.Invoke(root)
            .ForEach(c =>
            {
                childAction?.Invoke(c, root);
                ForEachTreeNode(c, getChildren, rootAction, childAction);
            }));
    }

    /// <summary>
    /// Executes a set of functions in parallel and combines the results using a join function.
    /// </summary>
    public static TOutput Fork<TInput, TOutput>(this IEnumerable<Func<TInput, TOutput>> prongs, Func<IEnumerable<TOutput>, TOutput> joinFunc, TInput input)
            => joinFunc(prongs.Select(x => x(input)));

    /// <summary>
    /// Gets all elements from a given root element and its children using a provided function.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="getRootElements">A function to get the root elements.</param>
    /// <param name="getChildren">A function to get the children of an element.</param>
    /// <returns>An enumerable of all elements.</returns>
    public static IEnumerable<T> GetAll<T>([DisallowNull] Func<IEnumerable<T>> getRootElements, [DisallowNull] Func<T, IEnumerable<T>?> getChildren)
    {
        // Check that the parameters are not null
        _ = getRootElements.ArgumentNotNull();
        _ = getChildren.ArgumentNotNull();

        // Create a list to store the result
        var result = new List<T>();

        // Iterate through the root elements
        foreach (var item in getRootElements())
        {
            // Add the root element to the result list
            result.Add(item);
            // Call the findChildren method to find the children of the root element
            findChildren(item);
        }

        // Return the result list as an enumerable
        return result.AsEnumerable();

        // Method to find the children of an element
        void findChildren(in T item)
        {
            // Get the children of the element
            var children = getChildren(item);
            // Check if the element has any children
            if (children?.Any() is true)
            {
                // Iterate through the children
                foreach (var child in children)
                {
                    // Add the child to the result list
                    result.Add(child);
                    // Call the findChildren method to find the children of the child
                    findChildren(child);
                }
            }
        }
    }

    /// <summary>
    /// Gets the value from the given source by the specified key.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="key">The key.</param>
    /// <returns>The value.</returns>
    public static TValue GetByKey<TKey, TValue>(this IEnumerable<(TKey Key, TValue Value)> source, TKey key)
            => source.ArgumentNotNull(nameof(source)).Where(kv => kv.Key?.Equals(key) ?? key is null).First().Value;

    /// <summary>
    /// Gets the item from the source collection by the specified key.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="source">The source collection.</param>
    /// <param name="key">The key to search for.</param>
    /// <returns>The item with the specified key.</returns>
    public static KeyValuePair<TKey, TValue> GetItemByKey<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source, in TKey key)
    {
        foreach (var item in source.ArgumentNotNull())
        {
            if (item.Key?.Equals(key) ?? false)
            {
                return item;
            }
        }
        throw new KeyNotFoundException(nameof(key));
    }

    /// <summary>
    /// Tries to get a value from a HashSet and returns a TryMethodResult with the result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="resultSet">The HashSet to search.</param>
    /// <param name="equalValue">The value to search for.</param>
    /// <returns>A TryMethodResult with the result.</returns>
    public static TryMethodResult<TResult?> GetValue<TResult>([DisallowNull] this HashSet<TResult> resultSet, TResult equalValue)
            where TResult : new()
    {
        var tryResult = resultSet.TryGetValue(equalValue, out var actualValue);
        return TryMethodResult<TResult?>.TryParseResult(tryResult, actualValue);
    }

    /// <summary>
    /// Groups the items in the given IEnumerable and returns a collection of tuples containing the
    /// item and its count.
    /// </summary>
    public static IEnumerable<(T Item, int Count)> GroupCounts<T>(in IEnumerable<T> items)
            => items.GroupBy(x => x).Select(x => (x.Key, x.Count()));

    public static TEnumerable IfEach<TEnumerable, TItem>(this TEnumerable source, Func<TItem, bool> condition, Action<TItem> trueness, Action<TItem> falseness)
            where TEnumerable : IEnumerable<TItem>
    {
        Check.IfArgumentNotNull(source);
        Check.IfArgumentNotNull(condition);

        foreach (var item in source)
        {
            if (condition(item))
            {
                trueness?.Invoke(item);
            }
            else
            {
                falseness?.Invoke(item);
            }
        }

        return source;
    }

    /// <summary>
    /// Returns the given items if it is not empty, otherwise returns the default values.
    /// </summary>
    public static IEnumerable<T?> IfEmpty<T>(this IEnumerable<T?>? items, [DisallowNull] IEnumerable<T?> defaultValues)
            => items?.Any() is true ? items : defaultValues;

    public static IEnumerable<int> IndexesOf<T>(this IEnumerable<T> source, T item)
    {
        var index = 0;
        foreach (var sourceItem in source)
        {
            if (EqualityComparer<T>.Default.Equals(sourceItem, item))
            {
                yield return index;
            }
            index++;
        }
    }

    public static T[] InitializeItems<T>(this T[] items, T defaultItem)
    {
        for (var index = 0; index < items.Length; index++)
        {
            items[index] = defaultItem;
        }
        return items;
    }

    /// <summary>
    /// Inserts an item into an IEnumerable at a specified index.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">The IEnumerable to insert into.</param>
    /// <param name="index">The index at which to insert the item.</param>
    /// <param name="item">The item to insert.</param>
    /// <returns>An IEnumerable containing the inserted item.</returns>
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

    public static bool IsSame<T>(this IEnumerable<T>? items1, IEnumerable<T>? items2)
        => (items1 == null && items2 == null) || (items1 != null && items2 != null && (items1.Equals(items2) || items1.SequenceEqual(items2)));

    public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> mapper)
        => source.Select(mapper);

    public static IEnumerable<T> Merge<T>(params IEnumerable<T>[] enumerables)
    {
        foreach (var enumerable in enumerables)
        {
            foreach (var item in enumerable)
            {
                yield return item;
            }
        }
    }

    /// <summary>
    /// Merges the elements of an IEnumerable into a single string.
    /// </summary>
    public static string MergeToString<T>(this IEnumerable<T> source)
        => source.Aggregate(new StringBuilder(), (current, item) => current.Append(item)).ToString();

    public static T Pop<T>(this IList<T> list, int index = -1)
    {
        var i = index >= 0 ? index : list.Count + index;
        var result = list[i];
        list.RemoveAt(i);
        return result;
    }

    public static Result<TValue?> Pop<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
        where TKey : notnull
    {
        Check.IfArgumentNotNull(dic);

        var result = dic.TryGetValue(key);
        if (result)
        {
            _ = dic.Remove(key);
        }

        return result;
    }

    public static Result<KeyValuePair<TKey, TValue>> Pop<TKey, TValue>(this Dictionary<TKey, TValue> dic)
        where TKey : notnull
    {
        Check.IfArgumentNotNull(dic);

        var result = dic.LastOrDefault();
        if (!result.IsDefault())
        {
            _ = dic.Remove(result.Key);
            return Result<KeyValuePair<TKey, TValue>>.CreateSuccess(result);
        }
        return Result<KeyValuePair<TKey, TValue>>.CreateFailure();
    }

    /// <summary>
    /// Reduces a sequence of nullable values using the specified reducer function.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="items">The sequence of nullable values to reduce.</param>
    /// <param name="reducer">A function to reduce the sequence of nullable values.</param>
    /// <returns>The reduced value.</returns>
    public static T? Reduce<T>(this IEnumerable<T?> items, Func<(T? Result, T? Item), T?> reducer)
    {
        T? result = default;
        foreach (var item in items)
        {
            result = reducer((result, item));
        }
        return result;
    }

    /// <summary>
    /// Removes an item from the list by its key.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="list">The list.</param>
    /// <param name="key">The key.</param>
    /// <returns>The list with the item removed.</returns>
    public static IList<KeyValuePair<TKey, TValue>> RemoveByKey<TKey, TValue>([DisallowNull] this IList<KeyValuePair<TKey, TValue>> list, in TKey key)
    {
        Check.IfArgumentNotNull(list);
        _ = list.Remove(list.GetItemByKey(key));
        return list;
    }

    /// <summary>
    /// Returns an IEnumerable of TSource that contains all elements of the source sequence that are
    /// not equal to the default value.
    /// </summary>
    public static IEnumerable<TSource> RemoveDefaults<TSource>(this IEnumerable<TSource> source, TSource? defaultValue = default)
            => defaultValue is null ? source.Where(item => item is not null) : source.Where(item => (!item?.Equals(defaultValue)) ?? false);

    /// <summary>
    /// Removes the specified item from the source IEnumerable.
    /// </summary>
    /// <param name="source">The source IEnumerable.</param>
    /// <param name="item">The item to remove.</param>
    /// <returns>An IEnumerable without the specified item.</returns>
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

    /// <summary>
    /// Removes all null values from the given IEnumerable of type TSource.
    /// </summary>
    public static IEnumerable<TSource> RemoveNulls<TSource>(this IEnumerable<TSource> source)
            where TSource : class => RemoveDefaults(source);

    /// <summary>
    /// Runs all actions in the given enumerable while the predicate returns true.
    /// </summary>
    /// <param name="actions">The actions to run.</param>
    /// <param name="predicate">The predicate to check.</param>
    /// <returns>The result of the actions.</returns>
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
    /// Selects all elements from a sequence of sequences.
    /// </summary>
    /// <param name="values">The sequence of sequences.</param>
    /// <returns>A sequence containing all elements of the input sequences.</returns>
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

    /// <summary> Asynchronously projects each element of a sequence into a new form. </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam> <typeparam
    /// name="TResult">The type of the value returned by selectorAsync.</typeparam> <param
    /// name="source">An IEnumerable<T> to project each element of.</param> <param
    /// name="selectorAsync">A transform function to apply to each element.</param> <returns>An
    /// IAsyncEnumerable<T> whose elements are the result of invoking the transform function on each
    /// element of source.</returns>
    public static async IAsyncEnumerable<TResult> SelectAsync<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Task<TResult>> selectorAsync)
    {
        foreach (var item in source)
        {
            yield return await selectorAsync(item);
        }
    }

    /// <summary>
    /// Selects all items from a collection of collections and returns them as a single collection.
    /// </summary>
    /// <param name="sources">The collection of collections to select from.</param>
    /// <returns>A single collection containing all items from the source collections.</returns>
    public static IEnumerable<T> SelectManyAndCompact<T>(this IEnumerable<IEnumerable<T>> sources)
    {
        if (sources is not null)
        {
            foreach (var item in sources.Where(items => items is not null).SelectMany(items => items))
            {
                yield return item;
            }
        }
    }

    /// <summary>
    /// Sets the value of the specified key in the list.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="list">The list.</param>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <returns>The list with the specified key and value.</returns>
    public static IList<KeyValuePair<TKey, TValue>> SetByKey<TKey, TValue>([DisallowNull] this IList<KeyValuePair<TKey, TValue>> list, in TKey key, in TValue value)
    {
        Check.IfArgumentNotNull(list);
        _ = list.RemoveByKey(key);
        list.Add(new(key, value));
        return list;
    }

    /// <summary>
    /// Adds a key-value pair to a dictionary, or updates the value of an existing key-value pair.
    /// </summary>
    /// <param name="dic">The dictionary to add or update the key-value pair.</param>
    /// <param name="key">The key of the key-value pair.</param>
    /// <param name="value">The value of the key-value pair.</param>
    /// <returns>The dictionary with the added or updated key-value pair.</returns>
    public static Dictionary<TKey, TValue> SetByKey<TKey, TValue>([DisallowNull] this Dictionary<TKey, TValue> dic, TKey key, TValue value)
                where TKey : notnull
        //This method adds a key-value pair to a dictionary, or updates the value of an existing key-value pair.
        //It takes a dictionary, a key, and a value as parameters.
        //It checks if the dictionary is not null, and if the key already exists in the dictionary, it updates the value, otherwise it adds the key-value pair.
        //It returns the dictionary.
    {
        Check.IfArgumentNotNull(dic);

        //Check if the key already exists in the dictionary
        if (dic.ContainsKey(key))
        {
            //If it does, update the value
            dic[key] = value;
        }
        else
        {
            //Otherwise, add the key-value pair
            dic.Add(key, value);
        }

        //Return the dictionary
        return dic;
    }

    public static IEnumerable<T> Slice<T>(this IEnumerable<T> items, int start = 0, int end = 0, int steps = 0)
    {
        var index = 0;
        void empty()
        {
        }
        bool getTrue() => true;
        var incIndex = steps switch
        {
            0 or 1 => (Action)empty,
            < 1 => () => index--,
            _ => () => index++,
        };
        var resIndex = steps is 0 or 1 ? (Action)empty : () => { index = 0; };
        var shouldReturn = steps is 0 or 1 ? (Func<bool>)getTrue : () => index == 0 || steps == index;
        var buffer = items.AsEnumerable();
        if (steps < 0)
        {
            buffer = buffer.Reverse();
        }

        if (start != 0)
        {
            buffer = buffer.Skip(start);
        }

        if (end != 0)
        {
            buffer = buffer.Take(end);
        }

        foreach (var item in buffer)
        {
            if (shouldReturn())
            {
                yield return item;
                resIndex();
            }
            incIndex();
        }
    }

    /// <summary>
    /// Creates an array from a single item.
    /// </summary>
    /// <typeparam name="T">The type of the item.</typeparam>
    /// <param name="item">The item to create an array from.</param>
    /// <returns>An array containing the item.</returns>
    public static T[] ToArray<T>(T item)
        => ToEnumerable(item).ToArray();

    public static Dictionary<TKey, TValue>? ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>>? pairs) where TKey : notnull
            => pairs?.ToDictionary(pair => pair.Key, pair => pair.Value);

    /// <summary>
    /// Converts a single item into an IEnumerable of that item.
    /// </summary>
    /// <param name="item">The item to convert.</param>
    /// <returns>An IEnumerable containing the item.</returns>
    [return: NotNull]
    public static IEnumerable<T> ToEnumerable<T>(T item)
    //This code creates an IEnumerable of type T and returns the item passed in as an argument.
    {
        //The yield keyword is used to return the item passed in as an argument.
        yield return item;
    }

    /// <summary>
    /// Creates an IEnumerable from a given IEnumerable.
    /// </summary>
    /// <param name="source">The source IEnumerable.</param>
    /// <returns>An IEnumerable.</returns>
    public static IEnumerable<T> ToEnumerable<T>(this IEnumerable<T> source)
    {
        //Check if the source is null
        if (source is null)
        {
            //If it is, return an empty IEnumerable
            yield break;
        }
        //Loop through each item in the source
        foreach (var item in source)
        {
            //Return each item in the source
            yield return item;
        }
    }

    /// <summary>
    /// Converts a Dictionary to an IEnumerable of (TKey, TValue) tuples.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the Dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the Dictionary.</typeparam>
    /// <param name="source">The Dictionary to convert.</param>
    /// <returns>An IEnumerable of (TKey, TValue) tuples.</returns>
    public static IEnumerable<(TKey, TValue)> ToEnumerable<TKey, TValue>(this Dictionary<TKey, TValue> source)
            where TKey : notnull
    {
        if (source is null)
        {
            yield break;
        }
        foreach (var item in source)
        {
            yield return (item.Key, item.Value);
        }
    }

    /// <summary>
    /// Converts an IAsyncEnumerable to an IEnumerable.
    /// </summary>
    /// <typeparam name="TItem">The type of the items in the IAsyncEnumerable.</typeparam>
    /// <param name="asyncItems">The IAsyncEnumerable to convert.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An IEnumerable containing the items from the IAsyncEnumerable.</returns>
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

    public static ImmutableArray<T> ToImmutableArray<T>(IEnumerable<T> items)
    {
        var builder = ImmutableArray.CreateBuilder<T>();
        builder.AddRange(items);
        return builder.ToImmutable();
    }

    /// <summary>
    /// Converts an IAsyncEnumerable to a List.
    /// </summary>
    /// <typeparam name="TItem">The type of the items in the IAsyncEnumerable.</typeparam>
    /// <param name="asyncItems">The IAsyncEnumerable to convert.</param>
    /// <param name="cancellationToken">The CancellationToken to use for cancellation.</param>
    /// <returns>A List containing the items from the IAsyncEnumerable.</returns>
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

    /// <summary>
    /// Returns a list of non-null items from the given async enumerable.
    /// </summary>
    /// <typeparam name="TItem">The type of the items in the enumerable.</typeparam>
    /// <param name="asyncItems">The async enumerable.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of non-null items from the given async enumerable.</returns>
    [return: NotNull]
    public static async Task<List<TItem>> ToListCompactAsync<TItem>(this IAsyncEnumerable<TItem?>? asyncItems, CancellationToken cancellationToken = default)
            => asyncItems is null
                ? await ToListAsync(EmptyAsyncEnumerable<TItem>.Empty, cancellationToken: cancellationToken)
                : await WhereAsync(asyncItems, x => x is not null, cancellationToken).ToListAsync(cancellationToken: cancellationToken);

    /// <summary>
    /// Converts an IEnumerable of type T to an ObservableCollection of type T.
    /// </summary>
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
            => new(source);

    /// <summary>
    /// Converts an IEnumerable to an IReadOnlyList.
    /// </summary>
    public static IReadOnlyList<T> ToReadOnlyList<T>([DisallowNull] this IEnumerable<T> items)
            => items is List<T> l ? l.AsReadOnly() : new List<T>(items).AsReadOnly();

    /// <summary>
    /// Converts an IEnumerable of type T to an IReadOnlySet of type T.
    /// </summary>
    public static IReadOnlySet<T> ToReadOnlySet<T>([DisallowNull] this IEnumerable<T> items)
            => ImmutableList.CreateRange(items).ToHashSet();

    /// <summary>
    /// Attempts to determine the number of elements in a sequence without forcing an enumeration.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence that contains elements to be counted.</param>
    /// <returns>
    /// <para>true if the count of source can be determined without enumeration; otherwise, false.</para>
    /// <para>
    /// When this method returns, contains the count of source if successful, or zero if the method
    /// failed to determine the count.
    /// </para>
    /// </returns>
    public static TryMethodResult<int> TryCountNonEnumerated<T>([DisallowNull] this IEnumerable<T> source)
        => TryMethodResult<int>.TryParseResult(source.TryGetNonEnumeratedCount(out var count), count);

    public static TryMethodResult<TValue> TryGetValue<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key) where TKey : notnull
        => TryMethodResult<TValue>.TryParseResult(dic.TryGetValue(key, out var value), value);

    public static Dictionary<TKey, TValue> Update<TKey, TValue>(this Dictionary<TKey, TValue> src, Dictionary<TKey, TValue> dst)
        where TKey : notnull
    {
        var a = src.Keys;
        Check.IfArgumentNotNull(dst);
        _ = src.IfEach<Dictionary<TKey, TValue>, KeyValuePair<TKey, TValue>>(
            x => dst.ContainsKey(x.Key)
            , x => dst[x.Key] = x.Value
            , x => dst.Add(x.Key, x.Value));
        return dst;
    }

    /// <summary>
    /// Asynchronously filters a sequence of values based on a predicate.
    /// </summary>
    /// <typeparam name="TItem">The type of the elements of the input sequence.</typeparam>
    /// <param name="asyncItems">The sequence to filter.</param>
    /// <param name="func">A function to test each element for a condition.</param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// An <see cref="IAsyncEnumerableTItem"/> that contains elements from the input sequence that
    /// satisfy the condition.
    /// </returns>
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

    /// <summary>
    /// Enumerates the elements of an IEnumerable with the option to cancel the operation.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the IEnumerable.</typeparam>
    /// <param name="query">The IEnumerable to enumerate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An IEnumerable containing the elements of the source sequence.</returns>
    public static IEnumerable<T> WithCancellation<T>(this IEnumerable<T> query, CancellationToken cancellationToken = default)
    {
        //Check if the query is null
        if (query is null)
        {
            //If it is, return an empty sequence
            yield break;
        }
        //Get the enumerator for the query
        using var enumerator = query.GetEnumerator();
        //Loop through the query until the cancellation token is requested or the enumerator has no more elements
        while (!cancellationToken.IsCancellationRequested && enumerator.MoveNext())
        {
            //Return the current element
            yield return enumerator.Current;
        }
    }

    public static IEnumerable<IEnumerable<T>> Zip<T>(params IEnumerable<T>[] value)
    {
        var buffer = new List<T>();
        foreach (var topLayer in value)
        {
            foreach (var innerLayer in topLayer)
            {
                buffer.Add(innerLayer);
            }
            yield return buffer;
            buffer.Clear();
        }
    }

    private static T InnerAggregate<T>(this T[] items, Func<T, T?, T> aggregator, T defaultValue = default!)
        => items switch
        {
            [] => defaultValue,
            [var item] => item,
            { Length: 2 } => aggregator(items[0], items[1]),
            [var item, .. var others] => aggregator(item, InnerAggregate(others, aggregator, defaultValue))
        };
}