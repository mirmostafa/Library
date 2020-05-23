#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using Library40.Collections.Generic;

namespace Library40.Collections.ObjectModel
{
	public class TripleValueCollection<T1, T2, T3> : EventualCollection<TripleValue<T1, T2, T3>>
	{
		public TripleValue<T1, T2, T3> Add(T1 value1, T2 value2, T3 value3)
		{
			var result = new TripleValue<T1, T2, T3>(value1, value2, value3);
			Add(result);
			return result;
		}
	}
}