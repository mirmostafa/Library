#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using Library40.Collections.ObjectModel;

namespace Library40.Helpers
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
		/// <typeparam name="TOutput"> The type of the output. </typeparam>
		/// <param name="items"> The items. </param>
		/// <param name="converter"> The converter. </param>
		/// <returns> </returns>
		public static IEnumerable<TOutput> Cast<TOutput>(this IEnumerable items, Converter<Object, TOutput> converter)
		{
			return items.Cast<object>().Select(item => converter(item));
		}

		/// <summary>
		///     Casts the specified source.
		/// </summary>
		/// <typeparam name="TSource"> The type of the source. </typeparam>
		/// <typeparam name="TResult"> The type of the result. </typeparam>
		/// <param name="source"> The source. </param>
		/// <param name="converter"> The converter. </param>
		/// <returns> </returns>
		/// <exception cref="ArgumentNullException">source</exception>
		public static IEnumerable<TResult> Cast<TSource, TResult>(this IEnumerable<TSource> source, Converter<TSource, TResult> converter)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			return source.Select(item => converter(item));
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
		/// <param name="source"> The source. </param>
		/// <returns> </returns>
		public static object[] ToArray(IEnumerable source)
		{
			var arrayList = new ArrayList();
			foreach (var item in source)
				arrayList.Add(item);
			return arrayList.ToArray();
		}

		/// <summary>
		///     Ases the array.
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="item"> The item. </param>
		/// <returns> </returns>
		public static T[] AsArray<T>(T item)
		{
			return ToEnumerable(item).ToArray();
		}

		/// <summary>
		///     /// Converts the given numbers to the array.
		/// </summary>
		/// <param name="ts"> The numbers. </param>
		/// <returns> </returns>
		public static object[] ToArray(params object[] ts)
		{
			return ts;
		}

		/// <summary>
		///     Ases the enuemrable.
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="ts"> The ts. </param>
		/// <returns> </returns>
		public static IEnumerable<T> AsEnuemrable<T>(params T[] ts)
		{
			return ts.AsEnumerable();
		}

		/// <summary>
		///     Ases the enuemrable.
		/// </summary>
		/// <param name="ts"> The ts. </param>
		/// <returns> </returns>
		public static IEnumerable AsEnuemrable(params object[] ts)
		{
			return ts.AsEnumerable();
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
				throw new ArgumentNullException("collection");
			if (newItems == null)
				throw new ArgumentNullException("newItems");

			foreach (var item in newItems)
				collection.Add(item);
		}

		public static void AddMany(this IList list, IEnumerable newItems)
		{
			if (list == null)
				throw new ArgumentNullException("list");
			if (newItems == null)
				throw new ArgumentNullException("newItems");

			foreach (var item in newItems)
				list.Add(item);
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
				throw new ArgumentNullException("dict");
			if (pairs == null)
				throw new ArgumentNullException("pairs");

			foreach (var pair in pairs)
				dict.Add(pair);
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
		///     Takes the groups.
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="items"> The items. </param>
		/// <param name="groupSize"> Size of the group. </param>
		/// <returns> </returns>
		public static IEnumerable<IEnumerable<T>> TakeGroups<T>(IEnumerable<T> items, int groupSize)
		{
			return TakeGroups(items, groupSize, groupSize);
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
				throw new ArgumentNullException("items");
			if (selector == null)
				throw new ArgumentNullException("selector");

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
				yield return child;
		}

		//public static IEnumerable<dynamic> SelectTreeDynamic<TElement>(TElement root, Func<TElement, IEnumerable<TElement>> getNext)
		//{
		//    yield return new
		//        {
		//            Parent = default(TElement),
		//            Item = root
		//        };
		//    foreach (dynamic child in InnerSelectTreeDynamic(root, getNext))
		//        yield return new
		//            {
		//                child.Parent,
		//                child.Item
		//            };
		//}

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
		public static IEnumerable<TResult> Zip<TResult, T1, T2>(IEnumerable<T1> source1, IEnumerable<T2> source2, Func<T1, T2, TResult> zipper)
		{
			return source1.SelectMany(item1 => source2, zipper);
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
				yield return generator();
		}

		/// <summary>
		///     Repeats the specified actor.
		/// </summary>
		/// <param name="actor"> The actor. </param>
		/// <param name="count"> The count. </param>
		public static void Repeat(Action actor, int count)
		{
			for (var i = 0; i < count; i++)
				actor();
		}

		/// <summary>
		///     Repeats the specified items.
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="items"> The items. </param>
		/// <param name="actor"> The actor. </param>
		public static void Repeat<T>(this IEnumerable<T> items, Action<T> actor)
		{
			foreach (var item in items)
				actor(item);
		}

		/// <summary>
		///     Shrinks the specified source.
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="source"> The source. </param>
		public static void Shrink<T>(this ICollection<T> source) where T : class
		{
			var buffer = new List<T>();
			buffer.AddMany(source);
			foreach (var item in buffer.Where(item => item == null))
				source.Remove(item);
		}

		/// <summary>
		///     Determines whether the specified source contains the given item, according the comparision.
		/// </summary>
		/// <typeparam name="TSource"> The type of the source. </typeparam>
		/// <param name="source"> The source items. </param>
		/// <param name="item"> The item. </param>
		/// <param name="comparison"> The comparison to compare the items. </param>
		/// <returns>
		///     <c>true</c> if the specified source contains the given item; otherwise, <c>false</c> .
		/// </returns>
		public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource item, Comparison<TSource> comparison)
		{
			return (from currItem in source where comparison(currItem, item) == 0 select currItem).Any();
		}

		/// <exception cref="ArgumentNullException">source</exception>
		public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> actor)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (actor == null)
				throw new ArgumentNullException("actor");
			foreach (var item in source)
				actor(item);
		}

		/// <exception cref="ArgumentNullException">source</exception>
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
		/// <param name="source"> The items to be looped through. </param>
		/// <param name="actor"> The actor applies to each item </param>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="source" />
		///     is
		///     <c>null</c>
		///     .
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
		///     Fors the each.
		/// </summary>
		/// <param name="source"> The source. </param>
		/// <param name="actor"> The actor. </param>
		public static void ForEach(this IDictionary source, Action<Object, Object> actor)
		{
			foreach (var key in source.Keys)
				actor(key, source[key]);
		}

		public static void ForEach<TKey, TValue>(this IDictionary<TKey, TValue> source, Action<TKey, TValue> actor)
		{
			foreach (var key in source.Keys)
				actor(key, source[key]);
		}

		/// <exception cref="ArgumentNullException">source</exception>
		public static IEnumerable<TResult> ForEach<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> func)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			return source.Select(func);
		}

		public static IEnumerable IfZero(IEnumerable source, IEnumerable defaultSource)
		{
			return !source.Cast<object>().Any() ? defaultSource : source;
		}

		public static IEnumerable<int> Range(int start, int count, int step)
		{
			for (var number = start; number < start + count; number += step)
				yield return number;
		}

		public static Collection<TSource> ToCollection<TSource>(this IEnumerable<TSource> source)
		{
			var result = new Collection<TSource>();
			source.ForEach(result.Add);
			return result;
		}

		public static EventualCollection<TSource> ToLibraryCollection<TSource>(this IEnumerable<TSource> source)
		{
			var result = new EventualCollection<TSource>();
			source.ForEach(result.Add);
			return result;
		}

		public static IEnumerable<TType> ToEnumerable<TType>(TType obj)
		{
			var result = new List<TType>
			             {
				             obj
			             };
			return result.AsEnumerable();
		}

		public static IEnumerable<TSource> Steps<TSource>(this IEnumerable<TSource> source, int steps, bool includeFirstItem = false) where TSource : class
		{
			if (includeFirstItem && source.ElementAtOrDefault(0) != null)
				yield return source.ElementAtOrDefault(0);

			var done = false;
			var index = 0;
			while (!done)
			{
				index += steps;
				var item = source.ElementAtOrDefault(index);

				if (item == null)
					done = true;
				else
					yield return item;
			}
		}

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

		public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> pairs)
		{
			return pairs.ToDictionary(pair => pair.Key, pair => pair.Value);
		}

		public static void ForEachTreeNode<T>(this IEnumerable<T> roots, Func<T, IEnumerable<T>> getChildren, Action<T> rootAction, Action<T, T> childAction) where T : class
		{
			roots.ForEach(r => ForEachTreeNode(r, getChildren, rootAction, childAction));
		}

		public static void ForEachTreeNode<T>(T root, Func<T, IEnumerable<T>> getChildren, Action<T> rootAction, Action<T, T> childAction) where T : class
		{
			if (root == null)
				return;
			if (rootAction != null)
				rootAction(root);
			getChildren(root).ForEach(c =>
			                          {
				                          if (childAction != null)
					                          childAction(c, root);
				                          ForEachTreeNode(c, getChildren, rootAction, childAction);
			                          });
		}

		/// <summary>
		///     Gets index of a specific element.
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="source"> The source. </param>
		/// <param name="predicate"> The predicate. </param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <returns> </returns>
		public static int IndexOf<T>(this IEnumerable<T> source, Func<T, bool> predicate)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (!source.Any())
				return -1;
			var result = 0;
			var enumerator = source.GetEnumerator();
			var found = false;
			var done = !enumerator.MoveNext();
			while (!done && !(found = predicate(enumerator.Current)))
			{
				result++;
				done = !enumerator.MoveNext();
			}
			return found ? result : -1;
		}

		/// <summary>
		///     Builds a tree.
		/// </summary>
		/// <typeparam name="TSiteMap"> The type of the site map. </typeparam>
		/// <typeparam name="TMenuItem"> The type of the menu item. </typeparam>
		/// <param name="siteMaps"> The site maps. </param>
		/// <param name="getNewTMenuItem"> The get new T menu item. </param>
		/// <param name="getRootElements"> The get root elements. </param>
		/// <param name="getChildren"> The get children. </param>
		/// <param name="addToRoots"> The add to roots. </param>
		/// <param name="addToChiildren"> The add to chiildren. </param>
		/// <exception cref="ArgumentNullException">getChildren</exception>
		public static void BuildTree<TSiteMap, TMenuItem>(IEnumerable<TSiteMap> siteMaps,
			Func<TSiteMap, TMenuItem> getNewTMenuItem,
			Func<IEnumerable<TSiteMap>> getRootElements,
			Func<TSiteMap, IEnumerable<TSiteMap>> getChildren,
			Action<TMenuItem> addToRoots,
			Action<TMenuItem, TMenuItem> addToChiildren)
		{
			if (getChildren == null)
				throw new ArgumentNullException("getChildren");
			if (getNewTMenuItem == null)
				throw new ArgumentNullException("getNewTMenuItem");
			if (addToRoots == null)
				throw new ArgumentNullException("addToRoots");
			if (addToChiildren == null)
				throw new ArgumentNullException("addToChiildren");
			if (getRootElements == null)
				throw new ArgumentNullException("getRootElements");
			Action<TSiteMap, TMenuItem> addChildren = null;
			addChildren = delegate(TSiteMap siteMap, TMenuItem parent)
			              {
				              foreach (var sm in getChildren(siteMap))
				              {
					              var newChile = getNewTMenuItem(sm);
					              addToChiildren(parent, newChile);
					              addChildren(sm, newChile);
				              }
			              };

			foreach (var siteMap in getRootElements())
			{
				var root = getNewTMenuItem(siteMap);
				addToRoots(root);
				addChildren(siteMap, root);
			}
		}

		public static IEnumerable<T> ExceptNulls<T>(this IEnumerable<T> source) where T : class
		{
			return source.Where(item => item != null);
		}

		public static IEnumerable<T> ExceptDefaults<T>(this IEnumerable<T> source) where T : struct
		{
			return source.Where(item => !item.Equals(default(T)));
		}

		public static IEnumerable<T> CastOrNull<T>(this IEnumerable source) where T : class
		{
			return Cast(source, item => item.As<T>());
		}

		public static bool HasItemsAtLeast<T>(this IEnumerable source, uint count) where T : class
		{
			var enumerator = source.GetEnumerator();
			var index = 0;
			while (enumerator.MoveNext() && (index < count))
				index++;
			return index == count;
		}
	}
}