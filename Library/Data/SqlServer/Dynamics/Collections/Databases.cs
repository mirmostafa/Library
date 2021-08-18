﻿namespace Library.Data.SqlServer.Dynamics.Collections
{
    public class Databases : SqlObjects<Database>
    {
        public Databases(Func<IEnumerable<Database>> itemsCreator)
            : base(itemsCreator)
        {
        }

        internal Databases(IEnumerable<Database> items)
            : base(items)
        {
        }
    }
}