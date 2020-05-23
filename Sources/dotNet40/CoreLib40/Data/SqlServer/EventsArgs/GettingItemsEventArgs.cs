#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;

namespace Library40.Data.SqlServer.EventsArgs
{
	public class GettingItemsEventArgs<TSqlObject> : EventArgs
	{
		public IEnumerable<TSqlObject> Items { get; set; }
	}
}