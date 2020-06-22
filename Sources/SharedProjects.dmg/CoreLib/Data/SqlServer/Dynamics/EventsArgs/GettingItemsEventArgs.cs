using System;
using System.Collections.Generic;

namespace Mohammad.Data.SqlServer.Dynamics.EventsArgs
{
    public class GettingItemsEventArgs<TSqlObject> : EventArgs
    {
        public IEnumerable<TSqlObject> Items { get; set; }
    }
}