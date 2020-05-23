#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;

namespace Library35.Helpers
{
	partial class EnumerableHelper
	{
		//private static IEnumerable<dynamic> InnerSelectTreeDynamic<TElement>(TElement root, Func<TElement, IEnumerable<TElement>> getNext)
		//{
		//    foreach (TElement item in getNext(root))
		//    {
		//        yield return new
		//        {
		//            Parent = root,
		//            Item = item
		//        };
		//        foreach (dynamic child in InnerSelectTreeDynamic(item, getNext))
		//            yield return new
		//            {
		//                child.Parent,
		//                child.Item
		//            };
		//    }
		//}

		private static IEnumerable<TElement> InnerSelectTreeDynamic<TElement>(TElement root, Func<TElement, IEnumerable<TElement>> getNext)
		{
			foreach (var item in getNext(root))
			{
				yield return item;
				foreach (var child in InnerSelectTreeDynamic(item, getNext))
					yield return child;
			}
		}
	}
}