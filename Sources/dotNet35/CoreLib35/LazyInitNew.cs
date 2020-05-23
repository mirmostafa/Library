#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library35
{
	public class LazyInitNew<T> : LazyInit<T>
		where T : class, new()
	{
		public LazyInitNew()
			: this(() => new T())
		{
		}

		protected LazyInitNew(Func<T> creator)
			: base(creator)
		{
		}

		protected LazyInitNew(Func<T> creator, LazyInitMode mode)
			: base(creator, mode)
		{
		}

		public LazyInitNew(LazyInitMode mode)
			: base(() => new T(), mode)
		{
		}
	}
}