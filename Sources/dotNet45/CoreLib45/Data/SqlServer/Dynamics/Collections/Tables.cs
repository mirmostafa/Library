#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System.Collections.Generic;

namespace Mohammad.Data.SqlServer.Dynamics.Collections
{
    public class Tables : SqlObjects<Table>
    {
        internal Tables(IEnumerable<Table> items)
            : base(items)
        {
        }
    }
}