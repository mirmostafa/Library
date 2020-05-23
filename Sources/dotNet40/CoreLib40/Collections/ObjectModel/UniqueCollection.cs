#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library40.Collections.ObjectModel
{
	[Serializable]
	public class UniqueCollection<T> : EventualCollection<T>
	{
		protected override void InsertItem(int index, T item)
		{
			if (this.Contains(item))
				return;
			base.InsertItem(index, item);
		}
	}
}