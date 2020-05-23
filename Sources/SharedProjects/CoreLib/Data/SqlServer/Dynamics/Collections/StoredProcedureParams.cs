using System.Collections.Generic;

namespace Mohammad.Data.SqlServer.Dynamics.Collections
{
    public class StoredProcedureParams : SqlObjects<StoredProcedureParam>
    {
        internal StoredProcedureParams(IEnumerable<StoredProcedureParam> items)
            : base(items) { }
    }
}