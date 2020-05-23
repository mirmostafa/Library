#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

#region
using System.Collections.Generic;
using System.Linq;
#endregion

namespace Library35.Collections.ObjectModel
{
	public abstract class SpecialCollection<T>
	{
		protected SpecialCollection()
		{
			this.InnerItems = new EventualCollection<T>();
		}

		#region Pick
		protected IEnumerable<T1> Pick<T1>()
		{
			return this.InnerItems.Where(item => item is T1).Cast<T1>();
		}
		#endregion

		protected EventualCollection<T> InnerItems { get; private set; }

		protected virtual T AddItem(T item)
		{
			this.InnerItems.Add(item);
			return item;
		}
	}
}