#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System.Collections.Generic;

namespace Mohammad.Data.SqlServer.Dynamics.Collections
{
    public class StoredProcedureParams : SqlObjects<StoredProcedureParam>
    {
        internal StoredProcedureParams(IEnumerable<StoredProcedureParam> items)
            : base(items)
        {
        }
    }
}