#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library40.EventsArgs
{
	public class StatusChangedEventArgs<TStatusEnum> : EventArgs
	{
		public StatusChangedEventArgs(TStatusEnum status)
		{
			this.Status = status;
		}

		public object Tag { get; set; }

		public TStatusEnum Status { get; private set; }
	}
}