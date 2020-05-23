#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using Library40.Helpers;

namespace Library40.Data.Ado
{
	public static class SqlStatementBuilder
	{
		public static string CreateSelect(string tablename, params string[] columns)
		{
			if (columns.Length == 0 || (columns[0] == "*"))
				return string.Format("SELECT * FROM {0}", tablename);
			return string.Format("SELECT {0} FROM {1}", columns.Merge(","), tablename);
		}
	}
}