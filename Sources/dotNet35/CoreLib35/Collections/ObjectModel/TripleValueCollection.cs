#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using Library35.Collections.Generic;

namespace Library35.Collections.ObjectModel
{
	public class TripleValueCollection<T1, T2, T3> : EventualCollection<TripleValue<T1, T2, T3>>
	{
		public TripleValue<T1, T2, T3> Add(T1 value1, T2 value2, T3 value3)
		{
			var result = new TripleValue<T1, T2, T3>(value1, value2, value3);
			this.Add(result);
			return result;
		}
	}
}