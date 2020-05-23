#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Collections.Generic;

namespace Library35.Collections.ObjectModel
{
	public interface IEventualCollection<T> : IEventualEnumerable<T>, ICollection<T>
	{
	}
}