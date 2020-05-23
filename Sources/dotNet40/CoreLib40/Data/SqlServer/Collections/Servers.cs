#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Library40.Data.SqlServer.Collections
{
	public class Servers : SqlObjects<Server>
	{
		internal Servers(IEnumerable<Server> items)
			: base(items)
		{
		}

		public Server GetCurrent()
		{
			if (!this.Items.Any())
				return null;
			var builder = new SqlConnectionStringBuilder(this[0].ConnectionString);
			return base[builder.DataSource];
		}
	}
}