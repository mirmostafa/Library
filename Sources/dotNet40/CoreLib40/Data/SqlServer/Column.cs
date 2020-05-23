#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Library40.Helpers;

namespace Library40.Data.SqlServer
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

	public class Row : SqlObject<Row, Table>
	{
		public Row(Table owner, IEnumerable<KeyValuePair<string, object>> data)
			: base(owner, string.Empty)
		{
			this.Data = data;
		}

		private IEnumerable<KeyValuePair<string, object>> Data { get; set; }

		public object this[string colName]
		{
			get
			{
				var q = from pair in this.Data where pair.Key.EqualsTo(colName) select pair;
				if (!q.Any())
					throw new Exception("Column not found.");
				return q.First().Value;
			}
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			result = this[binder.Name];
			return true;
		}
	}
}