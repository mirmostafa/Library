#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Data;
using System.Linq;
using Library35.Data.SqlServer.Collections;
using Library35.Helpers;

namespace Library35.Data.SqlServer
{
	public class Table : SqlObject<Table, Database>
	{
		private Columns _Columns;

		public Table(Database owner, string name)
			: base(owner, name)
		{
		}

		public Columns Columns
		{
			get { return PropertyHelper.Get(ref this._Columns, this.GetColumns); }
		}

		public DateTime CreateDate { get; set; }

		public long Id { get; set; }

		public DateTime ModifyDate { get; set; }

		public static Tables GetTables(string connectionstring)
		{
			return Database.GetDatabase(connectionstring).Tables;
		}

		private Columns GetColumns()
		{
			return new Columns((from row in GetItems(this.Owner.ConnectionString, string.Format("INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='{0}'", this.Name))
				select new Column(this, row.Field<string>("COLUMN_NAME"))
				       {
					       CollationName = row.Field("COLLATION_NAME", Convert.ToString),
					       IsNullable = row.Field("IS_NULLABLE", str => str.ToString().EqualsTo("yes")),
					       MaxLength = row.Field("CHARACTER_MAXIMUM_LENGTH", Convert.ToInt32),
					       Precision = row.Field("NUMERIC_PRECISION", Convert.ToInt32),
					       Position = row.Field("ORDINAL_POSITION", Convert.ToInt32),
					       DataType = row.Field("DATA_TYPE", Convert.ToString)
				       }).ToList());
		}

		public DataTable ToDataTable()
		{
			return this.SelectDataTable(this.Columns.Select(col => col.Name).ToArray());
		}
	}
}