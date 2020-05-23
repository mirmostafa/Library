#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

namespace Library35.Data.SqlServer
{
	public class Column : SqlObject<Column, Table>
	{
		public Column(Table owner, string name)
			: base(owner, name)
		{
		}

		public string CollationName { get; set; }

		public string DataType { get; set; }

		public bool IsNullable { get; set; }

		public int MaxLength { get; set; }

		public int Position { get; set; }

		public int Precision { get; set; }
	}
}