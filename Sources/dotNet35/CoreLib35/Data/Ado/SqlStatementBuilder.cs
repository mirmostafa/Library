#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using Library35.Helpers;

namespace Library35.Data.Ado
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