#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library35.EventsArgs
{
	public class ActingEventArgs : EventArgs
	{
		public bool Handled { get; set; }
	}

	public class ItemActedEventArgs<TItem> : EventArgs
	{
		public ItemActedEventArgs(TItem item)
		{
			this.Item = item;
		}

		public TItem Item { get; set; }
	}

	public class ItemActingEventArgs<TItem> : ActingEventArgs
	{
		public ItemActingEventArgs(TItem item)
		{
			this.Item = item;
		}

		public TItem Item { get; set; }
	}

	public class ChangingEventArgs<T> : ActingEventArgs
	{
		private readonly T _OldValue;

		public ChangingEventArgs(T oldValue, T newValue)
		{
			this._OldValue = oldValue;
			this.NewValue = newValue;
		}

		public T OldValue
		{
			get { return this._OldValue; }
		}

		public T NewValue { get; set; }
	}
}