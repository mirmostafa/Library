#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Mohammad.Data.SqlServer.Dynamics.Collections
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
            {
                return null;
            }

            var builder = new SqlConnectionStringBuilder(this[0].ConnectionString);
            return base[builder.DataSource];
        }
    }
}