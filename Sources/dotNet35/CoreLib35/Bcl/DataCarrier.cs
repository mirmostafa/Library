#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

namespace Library35.Bcl
{
	public class DataCarrier<TDataType>
	{
		public DataCarrier(TDataType data, string display)
		{
			this.Data = data;
			this.Display = display;
		}

		public TDataType Data { get; set; }
		public string Display { get; set; }

		public override string ToString()
		{
			return this.Display;
		}
	}

	public class DataCarrier : DataCarrier<object>
	{
		public DataCarrier(object data, string display)
			: base(data, display)
		{
		}
	}
}