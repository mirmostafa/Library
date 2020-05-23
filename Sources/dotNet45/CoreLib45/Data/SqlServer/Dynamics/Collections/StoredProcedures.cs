#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System.Collections.Generic;

namespace Mohammad.Data.SqlServer.Dynamics.Collections
{
    public class StoredProcedures : SqlObjects<StoredProcedure>
    {
        internal StoredProcedures(IEnumerable<StoredProcedure> items)
            : base(items)
        {
        }
    }
}