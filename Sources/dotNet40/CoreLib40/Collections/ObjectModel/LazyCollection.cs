#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using Library40.Bcl;

namespace Library40.Collections.ObjectModel
{
	public class LazyCollection<T> : LazyInit<EventualCollection<T>>
	{
		public LazyCollection()
			: base(() => new EventualCollection<T>())
		{
		}

		public LazyCollection(LazyInitMode mode)
			: base(() => new EventualCollection<T>(), mode)
		{
		}
	}
}