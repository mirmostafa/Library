#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Diagnostics.Contracts;

namespace Library40.Bcl
{
	public class DataCarrier<TDataType>
	{
		public DataCarrier(TDataType data, string display = null, int? index = null)
		{
			this.Data = data;
			this.Display = display;
			this.Index = index;
		}

		public TDataType Data { get; set; }
		public string Display { get; set; }
		public int? Index { get; set; }

		public override string ToString()
		{
			Contract.Ensures(Contract.Result<string>() == this.Display);

			return this.Display;
		}
	}

	public class DataCarrier : DataCarrier<object>
	{
		public DataCarrier(object data, string display = null, int? index = null)
			: base(data, display, index)
		{
		}
	}

	public class StringCarrier : DataCarrier<string>
	{
		public StringCarrier(string data)
			: base(data, null)
		{
		}

		public StringCarrier(string data, string display)
			: base(data, display ?? data)
		{
		}

		public StringCarrier(string data, int? index)
			: base(data, data, index)
		{
		}

		public StringCarrier(string data, string display, int? index)
			: base(data, display, index)
		{
		}
	}
}