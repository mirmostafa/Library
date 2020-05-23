#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library40.Win.EventsArgs
{
	public class CrossingCriticalValueEventsArgs : EventArgs
	{
		public CrossingCriticalValueEventsArgs(decimal criticalValue, decimal currentValue)
		{
			this.CriticalValue = criticalValue;
			this.CurrentValue = currentValue;
		}

		public decimal CriticalValue { get; private set; }

		public decimal CurrentValue { get; private set; }
	}
}