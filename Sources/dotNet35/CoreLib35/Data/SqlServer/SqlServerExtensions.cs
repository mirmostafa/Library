#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Collections.Generic;
using System.Data;
using System.Linq;
using Library35.Data.Ado;
using Library35.Helpers;

namespace Library35.Data.SqlServer
{
	public static class SqlServerExtensions
	{
		public static DataTable SelectDataTable(this Table table, params string[] columns)
		{
			var result = new SqlHelper(table.Owner.ConnectionString).FillByQuery(string.Format("SELECT {0} FROM {1}", columns.Any() ? columns.Merge(",") : "*", table.Name)).Tables[0];
			result.TableName = table.Name;
			return result;
		}

		public static IEnumerable<DataRow> Select(this Table table, params string[] columns)
		{
			var result = new SqlHelper(table.Owner.ConnectionString).FillByQuery(string.Format("SELECT {0} FROM {1}", columns.Any() ? columns.Merge(",") : "*", table.Name)).Tables[0];
			result.TableName = table.Name;
			return result.Select().AsEnumerable();
		}
	}
}