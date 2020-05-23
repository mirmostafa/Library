#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;
using System.Collections.Generic;

namespace Mohammad.Data.SqlServer.Dynamics.EventsArgs
{
    public class GettingItemsEventArgs<TSqlObject> : EventArgs
    {
        public IEnumerable<TSqlObject> Items { get; set; }
    }
}