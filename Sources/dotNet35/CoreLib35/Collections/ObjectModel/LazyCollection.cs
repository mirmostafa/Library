#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

namespace Library35.Collections.ObjectModel
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