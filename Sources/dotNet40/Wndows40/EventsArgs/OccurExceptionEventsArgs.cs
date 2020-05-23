#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library40.Win.EventsArgs
{
	public class OccurExceptionEventsArgs : EventArgs
	{
		public OccurExceptionEventsArgs(string message)
		{
			this.Message = message;
		}

		public string Message { get; private set; }
	}
}