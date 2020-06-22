using System;
using System.Collections.Generic;

namespace Mohammad.Data.SqlServer.Dynamics.Collections
{
    public class Databases : SqlObjects<Database>
    {
        internal Databases(IEnumerable<Database> items)
            : base(items) { }

        public Databases(Func<IEnumerable<Database>> itemsCreator)
            : base(itemsCreator) { }
    }
}