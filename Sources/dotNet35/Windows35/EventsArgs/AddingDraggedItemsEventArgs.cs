#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:01 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;

namespace Library35.Windows.EventsArgs
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