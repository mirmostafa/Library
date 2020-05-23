#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:01 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library35.Windows.EventsArgs
{
	public class AddedValueEventsArgs : EventArgs
	{
		public AddedValueEventsArgs(decimal currentValue, decimal beforeValue)
		{
			this.BeforeValue = beforeValue;
			this.CurrentValue = currentValue;
		}

		public decimal CurrentValue { get; private set; }

		public decimal BeforeValue { get; private set; }
	}
}