#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;

namespace Library40.Win.EventsArgs
{
	public class AddingDraggedItemsEventArgs : EventArgs
	{
		public AddingDraggedItemsEventArgs(IEnumerable<string> data)
		{
			this.Data = data;
		}

		public IEnumerable<string> Data { get; private set; }
	}
}