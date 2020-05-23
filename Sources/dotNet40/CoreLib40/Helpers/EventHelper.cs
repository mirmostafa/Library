#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

#region
using System;
using System.Threading.Tasks;
#endregion

namespace Library40.Helpers
{
	/// <summary>
	///     A utility to do some common tasks about event
	/// </summary>
	public static class EventHelper
	{
		public static object Raise(this EventHandler handler, object sender, TaskScheduler scheduler = null, Task task = null)
		{
			if (handler != null)
				if (task != null)
					if (scheduler != null)
						task.ContinueWith(t => handler(sender, EventArgs.Empty), scheduler);
					else
						task.ContinueWith(t => handler(sender, EventArgs.Empty));
				else
					handler(sender, EventArgs.Empty);
			return null;
		}

		public static EventArgs Raise(this Delegate handler, object sender, EventArgs e)
		{
			if (handler != null)
				handler.DynamicInvoke(sender, e);
			return e;
		}

		public static TEventArgs Raise<TEventArgs>(this EventHandler<TEventArgs> handler, object sender, TEventArgs e, TaskScheduler scheduler = null, Task task = null)
			where TEventArgs : EventArgs
		{
			if (handler != null)
				if (task != null)
					if (scheduler != null)
						task.ContinueWith(t => handler(sender, e), scheduler);
					else
						task.ContinueWith(t => handler(sender, e));
				else
					handler(sender, e);
			return e;
		}
	}
}