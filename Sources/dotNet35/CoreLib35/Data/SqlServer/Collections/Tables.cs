#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Collections.Generic;

namespace Library35.Data.SqlServer.Collections
{
	public class Tables : SqlObjects<Table>
	{
		internal Tables(IEnumerable<Table> items)
			: base(items)
		{
		}
	}
}