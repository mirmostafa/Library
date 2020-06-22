using System.Collections.Generic;

namespace Mohammad.Data.SqlServer.Dynamics.Collections
{
    public class Tables : SqlObjects<Table>
    {
        internal Tables(IEnumerable<Table> items)
            : base(items) { }
    }
}