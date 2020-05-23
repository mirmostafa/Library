#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library35.EventsArgs
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