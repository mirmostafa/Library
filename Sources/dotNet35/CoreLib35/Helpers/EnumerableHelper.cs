#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using Library35.Collections.ObjectModel;

namespace Library35.Helpers
{
	/// <summary>
	///     A utility to do some common tasks about enumerables. No conflict with LINQ.
	/// </summary>
	[Guid("BE1ED9E9-160B-498D-984E-79876168BE35")]
	public static partial class EnumerableHelper
	{
		/// <summary>
		///     Casts the specified items to the target type using converter.
		/// </summary>
		/// <typeparam name="TOutput">The type of the output.</typeparam>
		/// <param name="items">The items.</param>
		/// <param name="converter">The converter.</param>
		/// <returns></returns>
		public static IEnumerable<TOutput> Cast<TOutput>(this IEnumerable items, Converter<Object, TOutput> converter)
		{
			return items.Cast<object>().Select(item => converter(item));
		}

		/// <summary>
		///     Casts the specified source.
		/// </summary>
		/// <typeparam name="TSource">The type of the source.</typeparam>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="converter">The converter.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException">source</exception>
		public static IEnumerable<TResult> Cast<TSource, TResult>(this IEnumerable<TSource> source, Converter<TSource, TResult> converter)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			return source.Select(item => converter(item));
		}

		/// <summary>
		///     Counts the specified source.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static int Count(this IEnumerable source)
		{
			var result = 0;
			source.ForEach(item => result++);
			return result;
		}

		/// <summary>
		///     Returns the element at index.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		/// <exception cref="System.IndexOutOfRangeException"></exception>
		public static object ElementAt(this IEnumerable source, int index)
		{
			var counter = 0;
			foreach (var item in source)
			{
				if (counter == index)
					return item;
				if (counter > index)
					break;
				counter++;
			}
			throw new IndexOutOfRangeException();
		}

		/// <summary>
		///     Converts the source to the array.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static object[] ToArray(IEnumerable source)
		{
			var arrayList = new ArrayList();
			foreach (var item in source)
				arrayList.Add(item);
			return arrayList.ToArray();
		}

		/// <summary>
		///     Creates an array from a <see cref="T:System.Collections.Generic.IEnumerable`1" />.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		public static T[] ToArray<T>(T item)
		{
			return AsEnumerable(item).ToArray();
		}

		/// <summary>
		///     Converts the given items to the array.
		/// </summary>
		/// <param name="items">The items.</param>
		/// <returns></returns>
		public static object[] ToArray(params object[] items)
		{
			return items;
		}

		/// <summary>
		///     Returns the input typed as <see cref="T:System.Collections.Generic.IEnumerable`1" />.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items">The items.</param>
		/// <returns></returns>
		public static IEnumerable<T> AsEnuemrable<T>(params T[] items)
		{
			return items.AsEnumerable();
		}

		/// <summary>
		///     Returns the input typed as object.
		/// </summary>
		/// <param name="items">The items.</param>
		/// <returns></returns>
		public static IEnumerable AsEnuemrable(params object[] items)
		{
			return items.AsEnumerable();
		}

		/// <summary>
		///     Adds items to the collection.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="newItems">The new items.</param>
		/// <exception cref="System.ArgumentNullException">
		///     collection
		///     or
		///     newItems
		/// </exception>
		public static void AddMany<T>(this ICollection<T> collection, IEnumerable<T> newItems)
		{
			if (collection == null)
				throw new ArgumentNullException("collection");
			if (newItems == null)
				throw new ArgumentNullException("newItems");

			foreach (var item in newItems)
				collection.Add(item);
		}

		/// <summary>
		///     Adds items to the dictionary.
		/// </summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="dict">The dictionary.</param>
		/// <param name="pairs">The pairs.</param>
		/// <exception cref="System.ArgumentNullException">
		///     dictionary
		///     or
		///     pairs
		/// </exception>
		public static void AddMany<TKey, TValue>(this IDictionary<TKey, TValue> dict, IEnumerable<KeyValuePair<TKey, TValue>> pairs)
		{
			if (dict == null)
				throw new ArgumentNullException("dict");
			if (pairs == null)
				throw new ArgumentNullException("pairs");

			foreach (var pair in pairs)
				dict.Add(pair);
		}

		/// <summary>
		///     Adds items to the dictionary using the given function to convert the item to a specific pair key/value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="dict">The dict.</param>
		/// <param name="newItems">The new items.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <exception cref="System.ArgumentNullException">
		///     dict
		///     or
		///     newItems
		///     or
		///     key
		///     or
		///     value
		/// </exception>
		public static void AddMany<T, TKey, TValue>(this IDictionary<TKey, TValue> dict, IEnumerable<T> newItems, Func<T, TKey> key, Func<T, TValue> value)
		{
			if (dict == null)
				throw new ArgumentNullException("dict");
			if (newItems == null)
				throw new ArgumentNullException("newItems");
			if (key == null)
				throw new ArgumentNullException("key");
			if (value == null)
				throw new ArgumentNullException("value");

			foreach (var item in newItems)
				dict.Add(key(item), value(item));
		}

		/// <summary>
		///     Takes groups in items.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items">The items.</param>
		/// <param name="groupSize">Size of the group.</param>
		/// <param name="takeEvery">Takes every "takeEvery" numbers.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException">items</exception>
		/// <exception cref="System.ArgumentOutOfRangeException">
		///     takeEvery;Argument takeEvery must be less than or equal to
		///     groupSize.
		/// </exception>
		public static IEnumerable<IEnumerable<T>> TakeGroups<T>(IEnumerable<T> items, int groupSize, int takeEvery)
		{
			if (items == null)
				throw new ArgumentNullException("items");
			if (takeEvery > groupSize)
				throw new ArgumentOutOfRangeException("takeEvery", "Argument takeEvery must be less than or equal to groupSize.");

			var group = new Queue<T>(groupSize);

			foreach (var t in items)
				if (group.Count < groupSize)
					group.Enqueue(t);
				else
				{
					// return a copy
					yield return group.ToList();

					if (groupSize == takeEvery)
						group.Clear();
					else
						for (var x = 0; x < takeEvery; x++)
							group.Dequeue();

					group.Enqueue(t);
				}

			yield return group;
		}

		/// <summary>
		///     Takes groups in items.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items">The items.</param>
		/// <param name="groupSize">Size of the group.</param>
		/// <returns></returns>
		public static IEnumerable<IEnumerable<T>> TakeGroups<T>(IEnumerable<T> items, int groupSize)
		{
			return TakeGroups(items, groupSize, groupSize);
		}

		/// <summary>
		///     Selects many items recursively.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items">The items.</param>
		/// <param name="selector">The selector.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException">
		///     items
		///     or
		///     selector
		/// </exception>
		public static IEnumerable<T> SelectManyRecursive<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> selector)
		{
			if (items == null)
				throw new ArgumentNullException("items");
			if (selector == null)
				throw new ArgumentNullException("selector");

			var children = items.SelectMany(selector);
			return children.Any() ? items.Concat(SelectManyRecursive(children, selector)) : items.Concat(children);
		}

		/// <summary>
		///     Selects the tree.
		/// </summary>
		/// <typeparam name="TElement">The type of the element.</typeparam>
		/// <param name="root">The root element.</param>
		/// <param name="getNext">The get next.</param>
		/// <returns></returns>
		public static IEnumerable<TElement> SelectTree<TElement>(TElement root, Func<TElement, IEnumerable<TElement>> getNext)
		{
			yield return root;
			foreach (var child in InnerSelectTreeDynamic(root, getNext))
				yield return child;
		}

		/// <summary>
		///     Zips the specified source1.
		/// </summary>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <typeparam name="T1">The type of the source 1.</typeparam>
		/// <typeparam name="T2">The type of the source 2.</typeparam>
		/// <param name="source1">The source1.</param>
		/// <param name="source2">The source2.</param>
		/// <param name="zipper">The zipper.</param>
		/// <returns></returns>
		public static IEnumerable<TResult> Zip<TResult, T1, T2>(IEnumerable<T1> source1, IEnumerable<T2> source2, Func<T1, T2, TResult> zipper)
		{
			return source1.SelectMany(item1 => source2, zipper);
		}

		/// <summary>
		///     Generates the specified generator.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="generator">The generator.</param>
		/// <param name="count">Count of items to be generated.</param>
		/// <returns></returns>
		public static IEnumerable<T> Generate<T>(Func<T> generator, int count)
		{
			for (var i = 0; i < count; i++)
				yield return generator();
		}

		/// <summary>
		///     Repeats the specified actor many times.
		/// </summary>
		/// <param name="actor">The actor.</param>
		/// <param name="count">The count of times for actor to be called.</param>
		public static void Repeat(Action actor, int count)
		{
			for (var i = 0; i < count; i++)
				actor();
		}

		/// <summary>
		///     Repeats the specified items.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items">The count of times for actor to be called.</param>
		/// <param name="actor">The actor.</param>
		public static void Repeat<T>(this IEnumerable<T> items, Action<T> actor)
		{
			foreach (var item in items)
				actor(item);
		}

		/// <summary>
		///     Shrinks the specified source.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		public static void Shrink<T>(this ICollection<T> source) where T : class
		{
			var buffer = new List<T>();
			buffer.AddMany(source);
			foreach (var item in buffer.Where(item => item == null))
				source.Remove(item);
		}

		/// <summary>
		///     Determines whether [contains] [the specified source].
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="item">The item.</param>
		/// <returns>
		///     <c>true</c> if [contains] [the specified source]; otherwise, <c>false</c> .
		/// </returns>
		public static bool Contains(this IEnumerable<string> source, string item)
		{
			return source.Contains(item, (argument, cmdSwitch) => String.Compare(argument, cmdSwitch, StringComparison.OrdinalIgnoreCase));
		}

		/// <summary>
		///     Determines whether the specified source contains the given item, according the comparison.
		/// </summary>
		/// <typeparam name="TSource">The type of the source.</typeparam>
		/// <param name="source">The source items.</param>
		/// <param name="item">The item.</param>
		/// <param name="comparison">The comparison to compare the items.</param>
		/// <returns>
		///     <c>true</c> if the specified source contains the given item; otherwise, <c>false</c> .
		/// </returns>
		public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource item, Comparison<TSource> comparison)
		{
			return (from currItem in source where comparison(currItem, item) == 0 select currItem).Any();
		}

		/// <summary>
		///     Performs the specified action on each element of the <see cref="T:System.Collections.Generic.IEnumerable`1" />.
		/// </summary>
		/// <typeparam name="TSource">The type of the source.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="actor">The actor.</param>
		/// <exception cref="System.ArgumentNullException">
		///     source
		///     or
		///     actor
		/// </exception>
		public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> actor)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (actor == null)
				throw new ArgumentNullException("actor");
			foreach (var item in source)
				actor(item);
		}

		/// <summary>
		///     Performs the specified action on each element of the <see cref="T:System.Collections.Generic.IEnumerable`1" />.
		/// </summary>
		/// <typeparam name="TSource">The type of the source.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="actor">The actor.</param>
		/// <exception cref="System.ArgumentNullException">
		///     source
		///     or
		///     actor
		/// </exception>
		public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource, int> actor)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (actor == null)
				throw new ArgumentNullException("actor");

			var index = 0;
			foreach (var item in source)
				actor(item, index++);
		}

		/// <summary>
		///     Performs the actor to each items in specific source
		/// </summary>
		/// <param name="source">The items to be looped through.</param>
		/// <param name="actor">
		///     The actor applies to each item
		///     <see cref="T:Library35.Exceptions.CompanyException">CompanyException</see>
		/// </param>
		/// <exception cref="System.ArgumentNullException">
		///     source
		///     or
		///     actor
		/// </exception>
		public static void ForEach(this IEnumerable source, Action<Object> actor)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (actor == null)
				throw new ArgumentNullException("actor");
			foreach (var item in source)
				actor(item);
		}

		/// <summary>
		///     Performs the specified action on each element of the <see cref="T:System.Collections.Generic.IEnumerable`1" />.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="actor">The actor.</param>
		public static void ForEach(this IDictionary source, Action<Object, Object> actor)
		{
			foreach (var key in source.Keys)
				actor(key, source[key]);
		}

		/// <summary>
		///     Performs the specified action on each element of the <see cref="T:System.Collections.Generic.IEnumerable`1" />.
		/// </summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="actor">The actor.</param>
		public static void ForEach<TKey, TValue>(this IDictionary<TKey, TValue> source, Action<TKey, TValue> actor)
		{
			foreach (var key in source.Keys)
				actor(key, source[key]);
		}

		/// <summary>
		///     Performs the specified action on each element of the <see cref="T:System.Collections.Generic.IEnumerable`1" />.
		/// </summary>
		/// <typeparam name="TSource">The type of the source.</typeparam>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="func">The func.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException">source</exception>
		public static IEnumerable<TResult> ForEach<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> func)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			return source.Select(func);
		}

		/// <summary>
		///     Generates a sequence of integral numbers within a specified range.
		/// </summary>
		/// <param name="start">The start point.</param>
		/// <param name="count">The count point.</param>
		/// <param name="step">The step length.</param>
		/// <returns></returns>
		public static IEnumerable<int> Range(int start, int count, int step)
		{
			for (var number = start; number < start + count; number += step)
				yield return number;
		}

		/// <summary>
		///     Creates a collection from the given source.
		/// </summary>
		/// <typeparam name="TSource">The type of the source.</typeparam>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static Collection<TSource> ToCollection<TSource>(this IEnumerable<TSource> source)
		{
			var result = new Collection<TSource>();
			source.ForEach(result.Add);
			return result;
		}

		/// <summary>
		///     Creates a Livrary collection from the given source.
		/// </summary>
		/// <typeparam name="TSource">The type of the source.</typeparam>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static EventualCollection<TSource> ToLibraryCollection<TSource>(this IEnumerable<TSource> source)
		{
			var result = new EventualCollection<TSource>();
			source.ForEach(result.Add);
			return result;
		}

		/// <summary>
		///     Returns the input typed as <see cref="T:System.Collections.Generic.IEnumerable`1" />.
		/// </summary>
		/// <typeparam name="TType">The type of the type.</typeparam>
		/// <param name="obj">The obj.</param>
		/// <returns></returns>
		public static IEnumerable<TType> AsEnumerable<TType>(TType obj)
		{
			var result = new List<TType>
			             {
				             obj
			             };
			return result.AsEnumerable();
		}

		/// <summary>
		///     Returns the every "step" index of source.
		/// </summary>
		/// <typeparam name="TSource">The type of the source.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="steps">The steps.</param>
		/// <param name="includeFirstItem">if set to <c>true</c> [include first item].</param>
		/// <returns></returns>
		public static IEnumerable<TSource> Steps<TSource>(this IEnumerable<TSource> source, int steps, bool includeFirstItem = false) where TSource : class
		{
			var items = source as TSource[] ?? source.ToArray();
			if (includeFirstItem && items.ElementAtOrDefault(0) != null)
				yield return items.ElementAtOrDefault(0);

			var done = false;
			var index = 0;
			while (!done)
			{
				index += steps;
				var item = items.ElementAtOrDefault(index);

				if (item == null)
					done = true;
				else
					yield return item;
			}
		}

		/// <summary>
		///     Creates a copy of list and adds a first item to the list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		public static T[] AddFirst<T>(this IList<T> list, T item)
		{
			var array = new T[list.Count + 1];
			array[0] = item;
			list.CopyTo(array, 1);
			return array;
		}

		/// <summary>
		///     Creates a <see cref="T:System.Collections.Generic.Dictionary`2" /> from an
		///     <see cref="T:System.Collections.Generic.IEnumerable`1" />.
		/// </summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="pairs">The pairs.</param>
		/// <returns></returns>
		public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> pairs)
		{
			return pairs.ToDictionary(pair => pair.Key, pair => pair.Value);
		}
	}
}