#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using Library40.EventsArgs;
using Library40.Helpers;

namespace Library40.Collections.ObjectModel
{
	[Serializable]
	public class ReadOnlyCollection<T> : System.Collections.ObjectModel.ReadOnlyCollection<T>, IEventualEnumerable<T>
	{
		private readonly EventualCollection<T> _Collection;

		public ReadOnlyCollection(EventualCollection<T> collection)
			: base(collection)
		{
			this._Collection = collection;
			this._Collection.ItemAdded += (sender, e) => this.ItemAdded.Raise(this, e);
			this._Collection.ItemAdding += (sender, e) => this.ItemAdding.Raise(this, e);
			this._Collection.ItemChanged += (sender, e) => this.ItemChanged.Raise(this, e);
			this._Collection.ItemChanging += (sender, e) => this.ItemChanging.Raise(this, e);
			this._Collection.ItemRemoved += (sender, e) => this.ItemRemoved.Raise(this, e);
			this._Collection.ItemRemoving += (sender, e) => this.ItemRemoving.Raise(this, e);
			this._Collection.ItemsCleared += (sender, e) => this.ItemsCleared.Raise(this, e);
			this._Collection.ItemsClearing += (sender, e) => this.ItemsClearing.Raise(this, e);
		}

		#region IEventualEnumerable<T> Members
		public event EventHandler<ItemActedEventArgs<T>> ItemAdded;

		public event EventHandler<ItemActingEventArgs<T>> ItemAdding;

		public event EventHandler<ItemActedEventArgs<T>> ItemChanged;

		public event EventHandler<ItemActingEventArgs<T>> ItemChanging;

		public event EventHandler<ItemActedEventArgs<T>> ItemRemoved;

		public event EventHandler<ItemActingEventArgs<T>> ItemRemoving;

		public event EventHandler ItemsCleared;

		public event EventHandler<ActingEventArgs> ItemsClearing;
		#endregion
	}
}