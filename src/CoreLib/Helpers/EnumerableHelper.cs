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
    /// Adds a range of items to the specified collection.
    /// </summary>
    /// <typeparam name="TList">The type of the collection to which items will be added.</typeparam>
    /// <typeparam name="TItem">The type of the items to be added.</typeparam>
    /// <param name="list">The collection to which the items will be added.</param>
    /// <param name="items">The items to be added to the collection.</param>
    /// <returns>The updated collection with added items.</returns>
    /// <remarks>
    /// This extension method allows adding a range of items to a collection that implements
    /// ICollection. The method checks if the 'items' enumerable is not null and contains items
    /// before performing the addition.
    /// </remarks>
    public static TList AddRange<TList, TItem>([DisallowNull] this TList list, in IEnumerable<TItem> items)
        where TList : ICollection<TItem>
    {
        if (items?.Any() is true)
        {
            // Iterate through each item in the 'items' enumerable and add it to the collection.
            foreach (var item in items)
            {
                list.Add(item);
            }
        }
        return list; // Return the updated collection with added items.
    }

    public static void AddRange<TList, TItem>([DisallowNull] this TList list, params TItem[] items)
        where TList : ICollection<TItem>
    {
        if (items?.Any() is true)
        {
            // Iterate through each item in the 'items' enumerable and add it to the collection.
            foreach (var item in items)
            {
                list.Add(item);
            }
        }
    }

    /// <summary>
    /// Adds a range of items to an ObservableCollection.
    /// </summary>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    /// <param name="list">The ObservableCollection to add the items to.</param>
    /// <param name="items">The items to add to the collection.</param>
    /// <returns>The ObservableCollection with the added items.</returns>
    [return: NotNullIfNotNull(nameof(list))]
    public static ObservableCollection<T>? AddRange<T>(this ObservableCollection<T>? list, in IEnumerable<T> items)
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
        Check.MustBeArgumentNotNull(list);
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
        Check.MustBeArgumentNotNull(list);
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
        Check.MustBeArgumentNotNull(list);
        Check.MustBeArgumentNotNull(asyncItems);

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
    [return: NotNull]
    public static IEnumerable<T> AddRangeImmuted<T>(this IEnumerable<T>? source, IEnumerable<T>? items)
    {
        return (source, items) switch
        {
            (null, null) => Enumerable.Empty<T>(),
            (_, null) => source,
            (null, _) => items,
            (_, _) => addRangeImmutedIterator(source, items)
        };
        static IEnumerable<T> addRangeImmutedIterator(IEnumerable<T> source, IEnumerable<T> items)
        {
            if (source != null)
            {
                foreach (var item in source)
                {
                    yield return item;
                }
            }

            if (items != null)
            {
                foreach (var item in items)
                {
                    yield return item;
                }
            }
        }
    }

    /// <summary>
    /// Aggregates the elements of an IEnumerable using a custom aggregator function and an optional
    /// default value.
    /// </summary>
    /// <typeparam name="T">The type of elements in the IEnumerable.</typeparam>
    /// <param name="items">The IEnumerable of elements to be aggregated.</param>
    /// <param name="aggregator">A function that specifies how to aggregate elements.</param>
    /// <param name="defaultValue">An optional default value to use when the IEnumerable is empty.</param>
    /// <returns>The aggregated result of the IEnumerable elements.</returns>
    public static T Aggregate<T>(this IEnumerable<T> items, Func<T, T, T> aggregator, T defaultValue)
    {
        Check.MustBeArgumentNotNull(aggregator);
        var itemArray = items.ArgumentNotNull().ToArray();
        return itemArray switch
        {
        [] => defaultValue, // Return the default value if the array is empty.
        [var item] => item, // Return the single item if there's only one element.
            { Length: 2 } => aggregator.ArgumentNotNull()(itemArray.First(), itemArray.Last()), // Aggregate two elements using the aggregator function.
            [var item, .. var others] => aggregator(item, Aggregate(others, aggregator, defaultValue)) // Recursively aggregate remaining elements.
        };
    }

    public static T Aggregate<T>(this IEnumerable<T> items, Func<(T current, T result), T> aggregator, T defaultValue) =>
        Aggregate(items, (T curr, T res) => aggregator((curr, res)), defaultValue);

    /// <summary>
    /// Aggregates the elements of an array using addition operators and an optional default value.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the array, which must implement addition operators.
    /// </typeparam>
    /// <param name="items">The array of elements to be aggregated.</param>
    /// <param name="defaultValue">An optional default value to use when the array is empty.</param>
    /// <returns>The aggregated result of the array elements using addition operators.</returns>
    public static T AggregateAdd<T>(this T[] items, T defaultValue = default!) where T : IAdditionOperators<T, T, T>
        => Aggregate(items, (x, y) => x + y!, defaultValue);

    /// <summary>
    /// Aggregates the elements of an IEnumerable using addition operators and an optional default value.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the IEnumerable, which must implement addition operators.
    /// </typeparam>
    /// <param name="items">The IEnumerable of elements to be aggregated.</param>
    /// <param name="defaultValue">An optional default value to use when the IEnumerable is empty.</param>
    /// <returns>The aggregated result of the IEnumerable elements using addition operators.</returns>
    public static T AggregateAdd<T>(this IEnumerable<T> items, T defaultValue = default!) where T : IAdditionOperators<T, T, T>
        => Aggregate(items, (x, y) => x + y!, defaultValue);

    /// <summary>
    /// Determines whether any element exists in the given enumerable.
    /// </summary>
    /// <param name="source">The enumerable to check for elements.</param>
    /// <returns>True if the enumerable contains any elements, otherwise false.</returns>
    public static bool Any([NotNullWhen(true)] this IEnumerable? source) =>
        source switch
        {
            null => false, // If the enumerable is null, no elements exist.
            ICollection collection => collection.Count > 0, // If the enumerable is an ICollection, check its Count property.
            _ => source.GetEnumerator().MoveNext() // Use enumerator to check if any elements exist.
        };

    /// <summary>
    /// Determines whether any element exists in the given IList.
    /// </summary>
    /// <typeparam name="T">The type of elements in the IList.</typeparam>
    /// <param name="source">The IList to check for elements.</param>
    /// <returns>True if the IList contains any elements, otherwise false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Any<T>([DisallowNull] this IList<T> source) =>
        source?.Count > 0; // Check if the count of elements in the IList is not zero or null.

    /// <summary>
    /// Determines whether any element exists in the given array.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="source">The array to check for elements.</param>
    /// <returns>True if the array contains any elements, otherwise false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Any<T>(this T[] source) =>
        source?.Length > 0; // Check if the length of the array is not zero or null.

    /// <summary>
    /// Determines whether any element exists in the given ICollection.
    /// </summary>
    /// <param name="source">The ICollection to check for elements.</param>
    /// <returns>True if the ICollection contains any elements, otherwise false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Any<T>(this ICollection source) =>
        source?.Count > 0; // Check if the count of elements in the ICollection is not zero.

    /// <summary>
    /// Get a <see cref="Span{T}"/> view over a <see cref="List{T}"/>'s data. Items should not be
    /// added or removed from the <see cref="List{T}"/> while the <see cref="Span{T}"/> is in use.
    /// </summary>
    /// <param name="list">The list to get the data view over.</param>
    public static Span<TItem> AsSpan<TItem>(this List<TItem> list) =>
        CollectionsMarshal.AsSpan(list);

    /// <summary>
    /// Converts an IEnumerable to a Span.
    /// </summary>
    /// <typeparam name="TItem">The type of items in the IEnumerable.</typeparam>
    /// <param name="items">The IEnumerable to convert to a Span.</param>
    /// <returns>A Span containing the items from the IEnumerable.</returns>
    public static Span<TItem> AsSpan<TItem>(this IEnumerable<TItem> items)
    {
        // Check if the input IEnumerable is null.
        if (items is null)
        {
            // Return the default Span if the input is null.
            return default;
        }

        // Convert the IEnumerable to an array and create a Span from it.
        return MemoryExtensions.AsSpan(items.ToArray());
    }

    /// <summary>
    /// Builds a read-only list from an enumerable.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
    /// <param name="items">The enumerable to build the read-only list from.</param>
    /// <returns>A read-only list containing the elements from the enumerable.</returns>
    public static IReadOnlyList<T> Build<T>([DisallowNull] this IEnumerable<T> items)
    {
        Check.MustBeArgumentNotNull(items);

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
        Check.MustBeArgumentNotNull(rootElements); // Check that rootElements is not null
        Check.MustBeArgumentNotNull(getNewItem); // Check that getNewItem is not null
        Check.MustBeArgumentNotNull(getChildren); // Check that getChildren is not null
        Check.MustBeArgumentNotNull(addToRoots); // Check that addToRoots is not null
        Check.MustBeArgumentNotNull(addChild); // Check that addChild is not null

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
        Check.MustBeArgumentNotNull(converter);

        foreach (var item in input)
        {
            yield return converter(item);
        }
    }

    /// <summary>
    /// Casts elements of the source sequence to the specified type if possible.
    /// </summary>
    /// <typeparam name="TParams">The type of elements in the source sequence.</typeparam>
    /// <typeparam name="TResult">The target type to cast elements to.</typeparam>
    /// <param name="source">The source sequence containing elements to be cast.</param>
    /// <returns>An IEnumerable containing elements cast to the specified type.</returns>
    public static IEnumerable<TResult> CastSafe<TParams, TResult>(this IEnumerable<TParams> source)
    {
        if (source != null)
        {
            foreach (var item in source)
            {
                if (item is TResult u)
                {
                    yield return u; // Yield the casted item.
                }
            }
        }
    }

    /// <summary>
    /// Splits the source sequence into chunks of specified size.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
    /// <param name="source">The source sequence to be chunked.</param>
    /// <param name="chunkSize">The size of each chunk.</param>
    /// <returns>An IEnumerable of IEnumerable representing chunks of the source sequence.</returns>
    [Obsolete("Use .NET 6.0 Chunk, instead.")]
    public static IEnumerable<IEnumerable<T>> ChunkBy<T>([DisallowNull] this IEnumerable<T> source, int chunkSize) =>
        // Select each element with its index, then group elements by the calculated chunk index.
        source.Select((x, i) => new { Index = i, Value = x })
              .GroupBy(x => x.Index / chunkSize)
              .Select(x => x.Select(v => v.Value)); // Select values within each chunk group.

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
        Check.MustBeArgumentNotNull(list);

        list.Clear();
        if (items != null)
        {
            foreach (var item in items)
            {
                list.Add(item);
            }
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
    public static IEnumerable<T> ClearImmuted<T>(this IEnumerable<T>? source) =>
        Enumerable.Empty<T>();

    /// <summary> Recursively collects all items in a collection of items that implement
    /// IParent<TItem>. </summary> <param name="items">The collection of items to collect.</param>
    /// <returns>A collection of all items in the collection, including all children.</returns>
    public static IEnumerable<TItem> Collect<TItem>(IEnumerable<TItem> items)
            where TItem : IParent<TItem>
    {
        if (items != null)
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
    }

    /// <summary>
    /// Returns an IEnumerable of non-null elements from the given IEnumerable of nullable elements.
    /// </summary>
    [return: NotNull]
    public static IEnumerable<TSource> Compact<TSource>(this IEnumerable<TSource?>? items) where TSource : class =>
        items?
             .Where([DebuggerStepThrough] (x) => x is not null)
             .Select([DebuggerStepThrough] (x) => x!)
        ?? Enumerable.Empty<TSource>();

    /// <summary>
    /// Checks if the given IEnumerable contains a key-value pair with the specified key.
    /// </summary>
    public static bool ContainsKey<TKey, TValue>([DisallowNull] this IEnumerable<(TKey Key, TValue Value)> source, TKey key) =>
        source.ArgumentNotNull().Where(kv => kv.Key?.Equals(key) ?? key is null).Any();

    /// <summary>
    /// Creates a new array by copying elements from an existing array.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The array from which elements will be copied.</param>
    /// <returns>A new array containing elements copied from the original array.</returns>
    public static T[] Copy<T>(this T[] array) =>
        // Convert the array to an IEnumerable and then create an array from it.
        array.Iterate().ToArray();

    /// <summary>
    /// Creates a new list by copying elements from an existing IList.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="array">The IList from which elements will be copied.</param>
    /// <returns>A new list containing elements copied from the original IList.</returns>
    public static IList<T> Copy<T>(this IList<T> array) =>
        // Convert the IList to an IEnumerable and then create a List from it.
        array.Iterate().ToList();

    /// <summary>
    /// Creates an immutable array from an existing array.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The array to copy elements from.</param>
    /// <returns>An immutable array containing elements from the original array.</returns>
    public static ImmutableArray<T> CopyImmutable<T>(this T[] array) =>
        // Convert the array to an IEnumerable and then create an immutable array from it.
        array.Iterate().ToImmutableArray();

    /// <summary>
    /// Creates an immutable list from an existing IList.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="array">The IList to copy elements from.</param>
    /// <returns>An immutable list containing elements from the original IList.</returns>
    public static ImmutableList<T> CopyImmutable<T>(this IList<T> array) =>
        // Convert the IList to an IEnumerable and then create an immutable list from it.
        array.Iterate().ToImmutableList();

    /// <summary> Counts the number of elements in a sequence that are not enumerated. </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam> <param name="source">The
    /// IEnumerable<T> to count.</param> <returns>The number of elements in the sequence that are
    /// not enumerated.</returns>
    public static int CountNotEnumerated<T>(this IEnumerable<T> source)
    {
        (var succeed, var count) = TryCountNonEnumerated(source);
        return succeed ? count : Exceptions.Validations.InvalidOperationValidationException.Throw<int>();
    }

    /// <summary>
    /// Applies a folder function to each item in the IEnumerable and returns the result.
    /// </summary>
    /// <param name="items">The IEnumerable to fold.</param>
    /// <param name="folder">The folder function to apply.</param>
    /// <param name="initialValue">The initial value to use.</param>
    /// <returns>The result of the fold.</returns>
    public static IEnumerable<T> CreateIterator<T>(this IEnumerable<T> items, [DisallowNull] Action<T> action)
    {
        Check.MustBeArgumentNotNull(items);
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
    public static IEnumerable<TResult> CreateIterator<T, TResult>([DisallowNull] this IEnumerable<T> items, [DisallowNull] Func<T, TResult> action)
    {
        Check.MustBeArgumentNotNull(items);
        Check.MustBeArgumentNotNull(action);
        return iterate(items, action);

        static IEnumerable<TResult> iterate(IEnumerable<T> items, Func<T, TResult> action)
        {
            foreach (var item in items)
            {
                yield return action(item);
            }
        }
    }

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
    public static async IAsyncEnumerable<TItem> CreateIteratorAsync<TItem>([DisallowNull] this IAsyncEnumerable<TItem> asyncItems, Func<TItem, TItem> action, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Check.MustBeArgumentNotNull(asyncItems);
        Check.MustBeArgumentNotNull(action);

        await foreach (var item in asyncItems.WithCancellation(cancellationToken))
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
    /// Asynchronously iterates over an <see cref="IAsyncEnumerableTItem"/> and performs an action
    /// on each item.
    /// </summary>
    /// <typeparam name="TItem">The type of the items in the <see cref="IAsyncEnumerableTItem"/>.</typeparam>
    /// <param name="asyncItems">The <see cref="IAsyncEnumerableTItem"/> to iterate over.</param>
    /// <param name="action">The action to perform on each item.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe.</param>
    /// <returns>The <see cref="IAsyncEnumerableTItem"/>.</returns>
    public static async IAsyncEnumerable<TItem> CreateIteratorAsync<TItem>([DisallowNull] this IAsyncEnumerable<TItem> asyncItems, Action<TItem> action, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Check.MustBeArgumentNotNull(asyncItems);
        Check.MustBeArgumentNotNull(action);

        await foreach (var item in asyncItems.WithCancellation(cancellationToken))
        {
            action(item);
            yield return item;
        }
    }

    /// <summary>
    /// Returns an empty IEnumerable if the given IEnumerable is null, otherwise returns the given IEnumerable.
    /// </summary>
    [return: NotNull]
    public static IEnumerable<T> DefaultIfNull<T>(this IEnumerable<T>? items)
        => items ?? Enumerable.Empty<T>();

    /// <summary>
    /// Creates a new <see cref="Dictionary{TKey, TValue}"/> from a sequence of keys, with an
    /// optional default value.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary (must be not null).</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="keys">The sequence of keys to populate the dictionary with.</param>
    /// <param name="defaultValue">
    /// An optional default value for the dictionary's values (default is null).
    /// </param>
    /// <returns>A new dictionary populated with the given keys and optional default values.</returns>
    public static Dictionary<TKey, TValue?> DictionaryFromKeys<TKey, TValue>(IEnumerable<TKey> keys, TValue? defaultValue = default)
        where TKey : notnull =>
        // Create a new dictionary by selecting key-value pairs from the keys sequence with default values.
        new(keys.CreateIterator(x => new KeyValuePair<TKey, TValue?>(x, defaultValue)));

    /// <summary>
    /// Creates an empty array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T[] EmptyArray<T>() =>
        Array.Empty<T>();

    /// <summary>
    /// Compares two IEnumerable objects for equality.
    /// </summary>
    /// <typeparam name="T">The type of the objects in the IEnumerable.</typeparam>
    /// <param name="enum1">The first IEnumerable to compare.</param>
    /// <param name="enum2">The second IEnumerable to compare.</param>
    /// <param name="ignoreIndexes">Whether to ignore the order of the elements when comparing.</param>
    /// <returns>True if the two IEnumerables are equal, false otherwise.</returns>
    public static bool Equal<T>(IEnumerable<T> enum1, IEnumerable<T> enum2, bool ignoreIndexes) =>
        ignoreIndexes
            ? !enum1.ArgumentNotNull().Except(enum2).Any() && !enum2.ArgumentNotNull().Except(enum1).Any()
            : enum1.SequenceEqual(enum2);

    /// <summary>
    /// Returns a collection of elements from the input sequence that do not satisfy the specified predicate.
    /// </summary>
    public static IEnumerable<T> Except<T>(this IEnumerable<T> items, Func<T, bool> exceptor)
    {
        Check.MustBeArgumentNotNull(exceptor);

        return items.Where(x => !exceptor(x));
    }

    /// <summary>
    /// Returns an IEnumerable of TItem from the source IEnumerable, excluding any items that match
    /// the given exclude Func.
    /// </summary>
    public static IEnumerable<TItem> Exclude<TItem>(this IEnumerable<TItem> source, Func<TItem, bool> exclude) =>
        source.Where(x => !exclude(x));

    /// <summary>
    /// Finds and returns duplicate elements from the source sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
    /// <param name="source">The source sequence to search for duplicates.</param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> containing the duplicate elements from the source sequence.
    /// </returns>
    /// <remarks>
    /// This method uses a <see cref="HashSet{T}"/> to keep track of unique elements while iterating
    /// through the source sequence. It returns elements that are encountered more than once in the
    /// source sequence, indicating duplicates.
    /// </remarks>
    public static IEnumerable<T> FindDuplicates<T>(this IEnumerable<T> source)
    {
        var buffer = new HashSet<T>(); // Initialize a HashSet to store unique elements.

        // Use the LINQ Where operator to filter elements that have already been added to the HashSet.
        return source.Where([DebuggerStepThrough] (x) => !buffer.Add(x));
    }

    public static IEnumerable<T> Flatten<T>([DisallowNull] IEnumerable<T> roots, [DisallowNull] Func<T, IEnumerable<T>?> getChildren) =>
        roots.SelectAllChildren(getChildren);

    [Obsolete("Please use `Fluent()`, instead.", true)]
    public static TCollection FluentAdd<TCollection, T>(this TCollection collection, T item)
            where TCollection : ICollection<T>
    {
        collection.Add(item);
        return collection;
    }

    /// <summary>
    /// Executes an action for each item in the given enumerable and returns a read-only list of the items.
    /// </summary>
    /// <typeparam name="T">The type of the items in the enumerable.</typeparam>
    /// <param name="items">The enumerable of items.</param>
    /// <param name="action">The action to execute for each item.</param>
    /// <returns>A read-only list of the items.</returns>
    [return: NotNull]
    public static void ForEach<T>(this IEnumerable<T> items, [DisallowNull] Action<T> action) =>
        _ = items.CreateIterator(action).Build();

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
    public static void ForEach<T>(T root, Func<T, IEnumerable<T>>? getChildren, Action<T>? rootAction, Action<T, T>? childAction)
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
            .CreateIterator(c =>
            {
                childAction?.Invoke(c, root);
                ForEach(c, getChildren, rootAction, childAction);
            }).Build());
    }

    /// <summary>
    /// Executes the specified action for each item in the specified collection in parallel.
    /// </summary>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    /// <param name="items">The collection of items.</param>
    /// <param name="action">The action to execute for each item.</param>
    public static void ForEachParallel<TItem>(IEnumerable<TItem> items, Action<TItem> action) =>
        Parallel.ForEach(items, action);

    /// <summary>
    /// Gets the item from the source collection by the specified key.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="source">The source collection.</param>
    /// <param name="key">The key to search for.</param>
    /// <returns>The item with the specified key.</returns>
    public static KeyValuePair<TKey, TValue> GetItemByKey<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source, TKey key) =>
        source.ArgumentNotNull().Where(x => x.Key?.Equals(key) ?? false).FirstOrDefault();

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
        var tryResult = resultSet.ArgumentNotNull().TryGetValue(equalValue, out var actualValue);
        return TryMethodResult<TResult?>.TryParseResult(tryResult, actualValue);
    }

    /// <summary>
    /// Gets the value from the given source by the specified key.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="key">The key.</param>
    /// <returns>The value.</returns>
    public static TValue GetValueByKey<TKey, TValue>(this IEnumerable<(TKey Key, TValue Value)> source, TKey key) =>
        source.ArgumentNotNull().Where(kv => kv.Key?.Equals(key) ?? key is null).First().Value;

    /// <summary>
    /// Groups the items in the given IEnumerable and returns a collection of tuples containing the
    /// item and its count.
    /// </summary>
    public static IEnumerable<(T Item, int Count)> GroupCounts<T>(in IEnumerable<T> items) =>
        items.GroupBy(x => x).CreateIterator(x => (x.Key, x.Count()));

    /// <summary>
    /// Finds the duplicates in a given IEnumerable of type T.
    /// </summary>
    /// <returns>An IEnumerable of type T containing the duplicates.</returns>
    //public static IEnumerable<T> FindDuplicates<T>(this IEnumerable<T> items)
    //{
    //    //Create a new HashSet to store the items
    //    var buffer = new HashSet<T>();
    //    //Return the items from the IEnumerable collection that are not added to the HashSet
    //    return items.Where(x => !buffer.Add(x));
    //}
    public static bool HasDuplicates<T>(this IEnumerable<T> source) =>
        FindDuplicates(source).Any();

    /// <summary>
    /// Performs a specified action on each item in the source sequence based on a condition.
    /// </summary>
    /// <typeparam name="TEnumerable">The type of the source sequence.</typeparam>
    /// <typeparam name="TItem">The type of items in the source sequence.</typeparam>
    /// <param name="source">The source sequence to iterate over.</param>
    /// <param name="condition">A function that determines whether an item meets the condition.</param>
    /// <param name="trueness">The action to perform on items that meet the condition.</param>
    /// <param name="falseness">The action to perform on items that do not meet the condition.</param>
    /// <returns>The original source sequence.</returns>
    /// <remarks>
    /// This method iterates through the source sequence and performs the specified actions on each
    /// item based on the condition. The condition is determined by the provided <paramref
    /// name="condition"/> function. If an item meets the condition, the <paramref name="trueness"/>
    /// action is invoked; otherwise, the <paramref name="falseness"/> action is invoked.
    /// </remarks>
    public static TEnumerable IfEach<TEnumerable, TItem>(this TEnumerable source, Func<TItem, bool> condition, Action<TItem> trueness, Action<TItem> falseness)
        where TEnumerable : IEnumerable<TItem>
    {
        Check.MustBeArgumentNotNull(source);
        Check.MustBeArgumentNotNull(condition);

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
    public static IEnumerable<T?> IfEmpty<T>(this IEnumerable<T?>? items, [DisallowNull] IEnumerable<T?> defaultValues) =>
        items?.Any() is true ? items : defaultValues;

    /// <summary>
    /// Returns an enumerable of indexes at which a specified item appears in the source sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
    /// <param name="source">The source sequence to search in.</param>
    /// <param name="item">The item to find the indexes of.</param>
    /// <returns>An enumerable of indexes at which the specified item appears in the source sequence.</returns>
    /// <remarks>
    /// This method iterates through the source sequence and returns an enumerable of indexes at
    /// which the specified item appears. If the item is not found in the sequence, an empty
    /// enumerable is returned.
    /// </remarks>
    public static IEnumerable<int> IndexesOf<T>(this IEnumerable<T> source, T item)
    {
        Check.MustBeArgumentNotNull(source);
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

    /// <summary>
    /// Initializes all elements in an array with a specified default value.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="items">The array to be initialized.</param>
    /// <param name="defaultItem">The default value to initialize the elements with.</param>
    /// <returns>The array with all elements set to the specified default value.</returns>
    /// <remarks>
    /// This method initializes all elements in the given array with the specified default value. It
    /// iterates through each element in the array and assigns the default value to it.
    /// </remarks>
    [return: NotNullIfNotNull(nameof(items))]
    public static T[]? InitializeItems<T>(this T[] items, T defaultItem)
    {
        if (items != null)
        {
            for (var index = 0; index < items.Length; index++)
            {
                items[index] = defaultItem;
            }
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

    /// <summary>
    /// Compares two IEnumerable sequences for equality.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequences.</typeparam>
    /// <param name="items1">The first IEnumerable sequence to compare.</param>
    /// <param name="items2">The second IEnumerable sequence to compare.</param>
    /// <returns>
    /// <c>true</c> if both sequences are equal or both are <c>null</c>; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method compares two IEnumerable sequences for equality. Sequences are considered equal
    /// if they have the same elements in the same order. It also handles cases where one or both of
    /// the input sequences are null.
    /// </remarks>
    public static bool IsSame<T>(this IEnumerable<T>? items1, IEnumerable<T>? items2) =>
        (items1 == null && items2 == null) || (items1 != null && items2 != null && (items1.Equals(items2) || items1.SequenceEqual(items2)));

    /// <summary>
    /// Converts a single item into an IEnumerable of that item.
    /// </summary>
    /// <param name="item">The item to convert.</param>
    /// <returns>An IEnumerable containing the item.</returns>
    /// <remarks>
    /// This code creates an IEnumerable of type T and returns the item passed in as an argument.
    /// </remarks>
    [return: NotNull]
    public static IEnumerable<T> Iterate<T>(T item)
    {
        //The yield keyword is used to return the item passed in as an argument.
        yield return item;
    }

    [return: NotNull]
    public static IEnumerable<T> Iterate<T>(params T[] items) =>
            items.Iterate();

    /// <summary>
    /// Creates an IEnumerable from a given IEnumerable.
    /// </summary>
    /// <param name="source">The source IEnumerable.</param>
    /// <returns>An IEnumerable.</returns>
    public static IEnumerable<T> Iterate<T>(this IEnumerable<T> source)
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
    /// Converts an IEnumerable to an IEnumerable of objects.
    /// </summary>
    /// <param name="items">The input IEnumerable to convert.</param>
    /// <returns>An IEnumerable of objects containing the items from the input IEnumerable.</returns>
    public static IEnumerable<object> Iterate(this IEnumerable items)
    {
        // Check if the input enumerable is null, if so, return an empty enumerable.
        if (items is null)
        {
            yield break; // The sequence is empty, so we yield break to exit the enumeration.
        }

        // Iterate through each item in the input enumerable.
        foreach (var item in items)
        {
            // Yield return each item, effectively converting it to an IEnumerable<object>.
            yield return item;
        }
    }

    /// <summary>
    /// Projects each element of an IEnumerable sequence into a new form using the specified mapping function.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
    /// <typeparam name="TResult">The type of elements in the resulting sequence after mapping.</typeparam>
    /// <param name="source">The source IEnumerable sequence.</param>
    /// <param name="mapper">
    /// A function that transforms each element of the source sequence into a new element of the
    /// result sequence.
    /// </param>
    /// <returns>An IEnumerable sequence containing the mapped elements.</returns>
    /// <remarks>
    /// This method applies the specified mapping function to each element in the source sequence,
    /// producing a new sequence of elements of type TResult.
    /// </remarks>
    public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> mapper) =>
        source.CreateIterator(mapper);

    /// <summary>
    /// Merges multiple IEnumerable sequences into a single IEnumerable sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the IEnumerable sequences.</typeparam>
    /// <param name="enumerables">The IEnumerable sequences to merge.</param>
    /// <returns>A single IEnumerable sequence containing elements from all input sequences.</returns>
    /// <remarks>
    /// This method merges multiple IEnumerable sequences into a single IEnumerable sequence. It
    /// enumerates each input sequence one by one and yields its elements in the merged sequence.
    /// </remarks>
    public static IEnumerable<T> Merge<T>(params IEnumerable<T>[] enumerables)
    {
        if (enumerables?.Any() ?? false)
        {
            foreach (var enumerable in enumerables)
            {
                if (enumerable?.Any() ?? false)
                {
                    foreach (var item in enumerable)
                    {
                        yield return item;
                    }
                }
            }
        }
    }

    public static IEnumerable<T> Merge<T>(IEnumerable<T> enumerable1, IEnumerable<T> enumerable2) =>
        [.. enumerable1, .. enumerable2];

    /// <summary>
    /// Removes and returns the element at the specified index from the list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to pop an element from.</param>
    /// <param name="index">
    /// The index of the element to remove and return. If negative, it counts from the end of the
    /// list (-1 for the last element).
    /// </param>
    /// <returns>The element removed from the list.</returns>
    /// <remarks>
    /// This method removes and returns the element at the specified index from the list. If the
    /// index is negative, it counts from the end of the list (e.g., -1 for the last element).
    /// </remarks>
    public static T Pop<T>(this IList<T> list, int index = -1)
    {
        // Calculate the actual index based on the input index, considering negative values.
        var i = index >= 0 ? index : list.Count + index;

        // Get the element at the specified index.
        var result = list[i];

        // Remove the element from the list.
        list.RemoveAt(i);

        // Return the removed element.
        return result;
    }

    /// <summary>
    /// Removes and returns the value associated with the specified key from the dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="dic">The dictionary to pop a value from.</param>
    /// <param name="key">The key of the value to pop.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing the value associated with the specified key if found,
    /// or a failure result if the key is not found in the dictionary.
    /// </returns>
    /// <remarks>
    /// This method removes and returns the value associated with the specified key from the
    /// dictionary. If the key is not found in the dictionary, it returns a failure result.
    /// </remarks>
    public static Result<TValue?> Pop<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
        where TKey : notnull
    {
        // Check if the input dictionary is null, and throw an exception if it is.
        Check.MustBeArgumentNotNull(dic);

        // Try to get the value associated with the specified key.
        var result = dic.TryGetValue(key);

        // If the key was found in the dictionary:
        if (result)
        {
            // Remove the key-value pair from the dictionary.
            _ = dic.Remove(key);
        }

        // Return the result, which may contain the value or indicate failure.
        return result;
    }

    /// <summary>
    /// Removes and returns the last key-value pair from the dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="dic">The dictionary to pop a key-value pair from.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing the last key-value pair from the dictionary if
    /// successful, or a failure result if the dictionary is empty.
    /// </returns>
    /// <remarks>
    /// This method removes and returns the last key-value pair from the dictionary. If the
    /// dictionary is empty, it returns a failure result.
    /// </remarks>
    public static Result<KeyValuePair<TKey, TValue>> Pop<TKey, TValue>(this Dictionary<TKey, TValue> dic)
        where TKey : notnull
    {
        // Check if the input dictionary is null, and throw an exception if it is.
        Check.MustBeArgumentNotNull(dic);

        // Attempt to get the last key-value pair from the dictionary.
        var result = dic.LastOrDefault();

        // If a key-value pair was found:
        if (!result.IsDefault())
        {
            // Remove the key-value pair from the dictionary.
            _ = dic.Remove(result.Key);

            // Create a success result containing the removed key-value pair.
            return Result<KeyValuePair<TKey, TValue>>.CreateSuccess(result);
        }

        // If the dictionary was empty, return a failure result.
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
        Check.MustBeArgumentNotNull(list);
        _ = list.Remove(list.GetItemByKey(key));
        return list;
    }

    /// <summary>
    /// Returns an IEnumerable of TSource that contains all elements of the source sequence that are
    /// not equal to the default value.
    /// </summary>
    public static IEnumerable<TSource> RemoveDefaults<TSource>(this IEnumerable<TSource> source, TSource? defaultValue = default) =>
        defaultValue is null ? source.Where(item => item is not null) : source.Where(item => (!item?.Equals(defaultValue)) ?? false);

    public static IEnumerable<T> RemoveDuplicates<T>(this IEnumerable<T> source) =>
                                                                                                                    source.GroupBy(x => x).Select(x => x.First());

    /// <summary>
    /// Removes the specified item from the source IEnumerable.
    /// </summary>
    /// <param name="source">The source IEnumerable.</param>
    /// <param name="item">The item to remove.</param>
    /// <returns>An IEnumerable without the specified item.</returns>
    public static IEnumerable<T> RemoveImmuted<T>(this IEnumerable<T>? source, T item)
    {
        if (source is null)
        {
            yield break;
        }
        foreach (var i in source)
        {
            if (i?.Equals(item) is not true)
            {
                yield return i;
            }
        }
    }

    /// <summary>
    /// Removes all null values from the given IEnumerable of type TSource.
    /// </summary>
    public static IEnumerable<TSource> RemoveNulls<TSource>(this IEnumerable<TSource> source)
        where TSource : class => RemoveDefaults(source);

    /// <summary>
    /// Generates a sequence that contains a specified value repeated a specified number of times.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value to repeat.</param>
    /// <param name="count">The number of times to repeat the value.</param>
    /// <returns>An IEnumerable that contains the repeated value.</returns>
    public static IEnumerable<T> Repeat<T>(T value, int count) =>
        Enumerable.Repeat(value, count);

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

    /// <summary>
    /// Selects all elements from a sequence of sequences.
    /// </summary>
    /// <param name="values">The sequence of sequences.</param>
    /// <returns>A sequence containing all elements of the input sequences.</returns>
    public static IEnumerable<TDestination> SelectAll<TSource, TDestination>(this IEnumerable<IEnumerable<TSource>> values, Func<TSource, TDestination> selector)
    {
        foreach (var value in values)
        {
            foreach (var item in value.CreateIterator(selector))
            {
                yield return item;
            }
        }
    }

    /// <summary>
    /// Gets all elements from a given root element and its children using a provided function.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="getRootElements">A function to get the root elements.</param>
    /// <param name="getChildren">A function to get the children of an element.</param>
    /// <returns>An enumerable of all elements.</returns>
    public static IEnumerable<T> SelectAllChildren<T>([DisallowNull] this IEnumerable<T> roots, [DisallowNull] Func<T, IEnumerable<T>?> getChildren)
    {
        foreach (var root in roots)
        {
            yield return root; // Yield the current root element

            var children = getChildren(root); // Get the children of the current root

            if (children != null)
            {
                foreach (var child in SelectAllChildren(children, getChildren)) // Recursively iterate over children, yielding the results
                {
                    yield return child;
                }
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

    public static TResult? SelectImmutable<TItem, TResult>(this IEnumerable<TItem?> items, in Func<TItem?, TResult?, TResult?> selector, in TResult? defaultResult = default)
    {
        var result = defaultResult;
        if (items is { } && items.Any())
        {
            Check.MustBeArgumentNotNull(selector);
            foreach (var item in items)
            {
                result = selector(item, result);
            }
        }
        return result;
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
        Check.MustBeArgumentNotNull(list);
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
        Check.MustBeArgumentNotNull(dic);

        //Check if the key already exists in the dictionary
        if (!dic.TryAdd(key, value))
        {
            //If it does, update the value
            dic[key] = value;
        }

        //Return the dictionary
        return dic;
    }

    /// <summary>
    /// Slices an IEnumerable into a new sequence based on provided parameters.
    /// </summary>
    /// <typeparam name="T">The type of elements in the IEnumerable.</typeparam>
    /// <param name="items">The source IEnumerable to be sliced.</param>
    /// <param name="start">The starting index for slicing. Default is 0.</param>
    /// <param name="end">The ending index for slicing. Default is 0 (no end limit).</param>
    /// <param name="steps">The step size for slicing. Default is 0 (no step limit).</param>
    /// <returns>An IEnumerable containing the sliced elements.</returns>
    public static IEnumerable<T> Slice<T>(this IEnumerable<T> items, int start = 0, int end = 0, int steps = 0)
    {
        var index = 0;

        // Placeholder empty methods
        void empty()
        {
        }
        bool getTrue() => true;

        // Define increment index actions based on the steps value
        var incIndex = steps switch
        {
            0 or 1 => (Action)empty, // No steps or step size is 1, no change in index
            < 1 => () => index--, // Negative steps, decrement index
            _ => () => index++, // Positive steps, increment index
        };

        // Define reset index action based on the steps value
        var resIndex = steps is 0 or 1 ? (Action)empty : () => { index = 0; };

        // Define a condition for returning items based on steps and index
        var shouldReturn = steps is 0 or 1 ? (Func<bool>)getTrue : () => index == 0 || steps == index;

        var buffer = items.AsEnumerable();

        // Apply reverse if steps are negative
        if (steps < 0)
        {
            buffer = buffer.Reverse();
        }
        // Apply skip and take based on start and end
        if (start != 0)
        {
            buffer = buffer.Skip(start);
        }
        if (end != 0)
        {
            buffer = buffer.Take(end);
        }

        // Yield return sliced items based on conditions
        foreach (var item in buffer)
        {
            if (shouldReturn())
            {
                yield return item;
                resIndex(); // Reset index after returning an item
            }
            incIndex(); // Increment or decrement index based on steps
        }
    }

    /// <summary>
    /// Creates an array from a single item.
    /// </summary>
    /// <typeparam name="T">The type of the item.</typeparam>
    /// <param name="item">The item to create an array from.</param>
    /// <returns>An array containing the item.</returns>
    public static T[] ToArray<T>(T item) =>
        Iterate(item).ToArray();

    /// <summary>
    /// Converts an enumerable collection of key-value pairs into a dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="pairs">An enumerable collection of key-value pairs to convert.</param>
    /// <returns>
    /// A dictionary containing the key-value pairs from the input collection, or null if the input
    /// is null.
    /// </returns>
    public static Dictionary<TKey, TValue>? ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>>? pairs)
        where TKey : notnull
    {
        // Check if the input collection of key-value pairs is null, if so, return null.
        if (pairs is null)
        {
            return null;
        }

        // Use LINQ's ToDictionary method to convert the key-value pairs into a dictionary. The
        // lambda expressions specify how to extract keys and values from the pairs.
        return pairs.ToDictionary(pair => pair.Key, pair => pair.Value) ?? [];
    }

    /// <summary>
    /// Converts a list to a dictionary using a selector function to extract the key-value pairs.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <typeparam name="TListItem">The type of the items in the list.</typeparam>
    /// <param name="list">The list to convert to a dictionary.</param>
    /// <param name="selector">A function that maps each item in the list to a key-value pair.</param>
    /// <returns>A dictionary containing the key-value pairs extracted from the list.</returns>
    /// <remarks>
    /// This extension method converts a list to a dictionary by applying a selector function to
    /// each item in the list. The selector function should return a tuple of the key and value for
    /// each item.
    /// </remarks>
    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue, TListItem>(this IList<TListItem> list, Func<TListItem, (TKey Key, TValue Value)> selector)
        where TKey : notnull
    {
        if (list == null)
        {
            return [];
        }

        var result = new Dictionary<TKey, TValue>();
        foreach (var item in list)
        {
            var (key, value) = selector(item);
            result.Add(key, value);
        }
        return result;
    }

    public static IEnumerable<T> ToEnumerable<T>(this IEnumerable<T> items)
    {
        if (items?.Any() != true)
        {
            yield break;
        }
        foreach (var item in items)
        {
            yield return item;
        }
    }

    /// <summary> Converts a Dictionary<TKey, TValue> to an IEnumerable of key-value pairs.
    /// </summary> <typeparam name="TKey">The type of keys in the dictionary.</typeparam> <typeparam
    /// name="TValue">The type of values in the dictionary.</typeparam> <param name="source">The
    /// input Dictionary to convert.</param> <returns>An IEnumerable of key-value pairs containing
    /// the items from the input Dictionary.</returns>
    public static IEnumerable<(TKey, TValue)> ToEnumerable<TKey, TValue>(this Dictionary<TKey, TValue> source)
        where TKey : notnull
    {
        // Check if the input dictionary is null, if so, return an empty enumerable.
        if (source is null)
        {
            yield break; // The sequence is empty, so we yield break to exit the enumeration.
        }

        // Iterate through each key-value pair in the input dictionary.
        foreach (var item in source)
        {
            // Yield return each key-value pair as a tuple (TKey, TValue).
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
        Check.MustBeArgumentNotNull(asyncItems);
        var result = New<List<TItem>>();
        await foreach (var item in asyncItems.WithCancellation(cancellationToken))
        {
            result.Add(item);
        }
        return result.Iterate();
    }

    /// <summary>
    /// Converts an IEnumerable to an ImmutableArray.
    /// </summary>
    /// <typeparam name="T">The type of elements in the IEnumerable.</typeparam>
    /// <param name="items">The IEnumerable to convert.</param>
    /// <returns>An ImmutableArray containing the elements of the IEnumerable.</returns>
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
        Check.MustBeArgumentNotNull(asyncItems);
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
    {
        var result = await (asyncItems is null ? empty() : data()).ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
        return result!;
        IAsyncEnumerable<TItem> empty() => EmptyAsyncEnumerable<TItem>.Empty;
        IAsyncEnumerable<TItem> data() => WhereAsync(asyncItems!, x => x is not null, cancellationToken)!;
    }

    /// <summary>
    /// Converts an IEnumerable of type T to an ObservableCollection of type T.
    /// </summary>
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source) =>
        source is ObservableCollection<T> o ? o : new(source);

    /// <summary>
    /// Converts an IEnumerable to an IReadOnlyList.
    /// </summary>
    public static IReadOnlyList<T> ToReadOnlyList<T>([DisallowNull] this IEnumerable<T> items) =>
        items is List<T> l ? l.AsReadOnly() : new List<T>(items).AsReadOnly();

    /// <summary>
    /// Converts an IEnumerable of type T to an IReadOnlySet of type T.
    /// </summary>
    public static IReadOnlySet<T> ToReadOnlySet<T>([DisallowNull] this IEnumerable<T> items) =>
        ImmutableList.CreateRange(items).ToHashSet();

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
    public static TryMethodResult<int> TryCountNonEnumerated<T>([DisallowNull] this IEnumerable<T> source) =>
        TryMethodResult<int>.TryParseResult(source.TryGetNonEnumeratedCount(out var count), count);

    public static TryMethodResult<TValue> TryGetValue<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
        where TKey : notnull => TryMethodResult<TValue>.TryParseResult(dic.TryGetValue(key, out var value), value);

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
    /// <param name="source">The IEnumerable to enumerate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An IEnumerable containing the elements of the source sequence.</returns>
    public static IEnumerable<T> WithCancellation<T>(this IEnumerable<T> source, CancellationToken cancellationToken = default)
    {
        //Check if the query is null
        if (source is null)
        {
            //If it is, return an empty sequence
            yield break;
        }
        //Get the enumerator for the query
        using var enumerator = source.GetEnumerator();
        //Loop through the query until the cancellation token is requested or the enumerator has no more elements
        while (!cancellationToken.IsCancellationRequested && enumerator.MoveNext())
        {
            //Return the current element
            yield return enumerator.Current;
        }
    }

    public static IFluentList<FluentList<TItem>, TItem> AsFluent<TItem>(this IList<TItem> list)
        => FluentList<TItem>.Create(list);
}