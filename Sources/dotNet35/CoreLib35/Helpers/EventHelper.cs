#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library35.Helpers
{
	/// <summary>
	///     A utility to do some common tasks about event
	/// </summary>
	public static class EventHelper
	{
		public static object Raise(this EventHandler handler, object sender)
		{
			if (handler != null)
				handler(sender, EventArgs.Empty);
			return null;
		}

		public static EventArgs Raise(this Delegate handler, object sender, EventArgs e)
		{
			if (handler != null)
				handler.DynamicInvoke(sender, e);
			return e;
		}

		public static TEventArgs Raise<TEventArgs>(this EventHandler<TEventArgs> handler, object sender, TEventArgs e) where TEventArgs : EventArgs
		{
			if (handler != null)
				handler(sender, e);
			return e;
		}
	}
}