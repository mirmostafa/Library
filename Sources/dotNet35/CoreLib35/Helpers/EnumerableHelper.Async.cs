#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using Library35.Threading;

namespace Library35.Helpers
{
	partial class EnumerableHelper
	{
		public static void ForEachAsync<TSource>(this IEnumerable<TSource> source, Action<TSource> actor)
		{
			ForEachAsync(source, actor, null);
		}

		public static void ForEachAsync<TSource>(this IEnumerable<TSource> source, Action<TSource> actor, Action ended)
		{
			ForEachAsync(source, actor, true, ended);
		}

		public static void ForEachAsync<TSource>(this IEnumerable<TSource> source, Action<TSource> actor, bool join, Action ended)
		{
			ForEachAsync(source, actor, 2, join, ended);
		}

		public static void ForEachAsync<TSource>(this IEnumerable<TSource> source, Action<TSource> actor, int threadCount)
		{
			ForEachAsync(source, actor, threadCount, true, null);
		}

		public static void ForEachAsync<TSource>(this IEnumerable<TSource> source, Action<TSource> actor, int threadCount, bool join, Action ended)
		{
			Async.ForEach(source, actor, threadCount, join, ended);
		}
	}
}