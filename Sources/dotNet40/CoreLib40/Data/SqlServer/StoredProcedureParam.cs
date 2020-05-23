#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

namespace Library40.Data.SqlServer
{
	public class StoredProcedureParam : SqlObject<StoredProcedureParam, StoredProcedure>
	{
		public StoredProcedureParam(StoredProcedure owner, string name)
			: base(owner, name)
		{
		}

		public string DefaultValue { get; set; }

		public long Id { get; set; }

		public int Length { get; set; }

		public int NumericPrecision { get; set; }

		public string SqlDataType { get; set; }
	}
}