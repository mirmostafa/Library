#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

namespace Library35.Collections.Generic
{
	public class TripleValue<T1, T2, T3>
	{
		public TripleValue()
		{
		}

		public TripleValue(T1 value1, T2 value2, T3 value3)
		{
			this.Value1 = value1;
			this.Value2 = value2;
			this.Value3 = value3;
		}

		public T1 Value1 { get; set; }

		public T2 Value2 { get; set; }

		public T3 Value3 { get; set; }
	}
}