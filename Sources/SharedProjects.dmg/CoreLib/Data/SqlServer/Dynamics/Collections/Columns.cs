using System.Collections.Generic;

namespace Mohammad.Data.SqlServer.Dynamics.Collections
{
    public class Columns : SqlObjects<Column>
    {
        internal Columns(IEnumerable<Column> items)
            : base(items) { }
    }
}