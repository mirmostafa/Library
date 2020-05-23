#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Library35.EventsArgs;
using Library35.Helpers;

namespace Library35.Collections.ObjectModel
{
	[Serializable]
	public class EventualCollection<T> : Collection<T>, IEventualCollection<T>
	{
		private ReadOnlyCollection<T> _ReadOnlyCollection;

		public virtual ReadOnlyCollection<T> ReadOnlyCollection
		{
			get { return PropertyHelper.Get(ref this._ReadOnlyCollection, () => new ReadOnlyCollection<T>(this)); }
		}

		#region IEventualCollection<T> Members
		public event EventHandler<ItemActedEventArgs<T>> ItemAdded;
		public event EventHandler<ItemActingEventArgs<T>> ItemAdding;
		public event EventHandler<ItemActedEventArgs<T>> ItemChanged;
		public event EventHandler<ItemActingEventArgs<T>> ItemChanging;
		public event EventHandler<ItemActedEventArgs<T>> ItemRemoved;
		public event EventHandler<ItemActingEventArgs<T>> ItemRemoving;
		public event EventHandler ItemsCleared;
		public event EventHandler<ActingEventArgs> ItemsClearing;
		#endregion

		protected override void ClearItems()
		{
			if (this.ItemsClearing.Raise(this, new ActingEventArgs()).Handled)
				return;
			base.ClearItems();
			this.ItemsCleared.Raise(this);
		}

		protected override void InsertItem(int index, T item)
		{
			if (this.ItemAdding.Raise(this, new ItemActingEventArgs<T>(item)).Handled)
				return;
			base.InsertItem(index, item);
			this.ItemAdded.Raise(this, new ItemActedEventArgs<T>(item));
		}

		protected override void RemoveItem(int index)
		{
			var item = this[index];
			if (this.ItemRemoving.Raise(this, new ItemActingEventArgs<T>(item)).Handled)
				return;
			base.RemoveItem(index);
			this.ItemRemoved.Raise(this, new ItemActedEventArgs<T>(item));
		}

		protected override void SetItem(int index, T item)
		{
			if (this.ItemChanging.Raise(this, new ItemActingEventArgs<T>(item)).Handled)
				return;
			base.SetItem(index, item);
			this.ItemChanged.Raise(this, new ItemActedEventArgs<T>(item));
		}

		public void Add(IEnumerable<T> items)
		{
			foreach (var item in items)
				this.Add(item);
		}
	}
}