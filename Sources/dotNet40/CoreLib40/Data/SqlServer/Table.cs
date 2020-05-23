#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Library40.Data.Ado;
using Library40.Data.SqlServer.Collections;
using Library40.Helpers;

namespace Library40.Data.SqlServer
{
	public class Table : SqlObject<Table, Database>, IEnumerable
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

		#region IEnumerable Members
		public IEnumerator GetEnumerator()
		{
			return this.SelectDataTable(this.Columns.Select(col => col.Name).ToArray()).Rows.GetEnumerator();
		}
		#endregion

		public static Tables GetByConnectionString(string connectionstring)
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

		public DataTable SelectDataTable(params string[] columns)
		{
			return new SqlHelper(this.Owner.ConnectionString).FillByQuery(string.Format("SELECT {0} FROM {1}", columns.Any() ? columns.Merge(",") : "*", this.Name)).Tables[0];
		}

		public DataReader SelectReader(params string[] columns)
		{
			var result =
				new DataReader(new SqlHelper(this.Owner.ConnectionString).ExecuteReader(string.Format("SELECT {0} FROM {1}", columns.Any() ? columns.Merge(",") : "*", this.Name)),
					this.Owner,
					this.Name);
			return result;
		}

		public SqlDataReader Select(params string[] columns)
		{
			var result = new SqlHelper(this.Owner.ConnectionString).ExecuteReader(string.Format("SELECT {0} FROM {1}", columns.Any() ? columns.Merge(",") : "*", this.Name));
			return result;
		}

		public IEnumerable<Row> SelectRows(params string[] columns)
		{
			throw new NotImplementedException();
		}
	}
}