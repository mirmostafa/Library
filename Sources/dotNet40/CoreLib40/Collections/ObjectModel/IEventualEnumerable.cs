#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using Library40.EventsArgs;

namespace Library40.Collections.ObjectModel
{
	public interface IEventualEnumerable<T> : IEnumerable<T>
	{
		/// <summary>
		///     Occurs when an item added to the list.
		/// </summary>
		event EventHandler<ItemActedEventArgs<T>> ItemAdded;

		/// <summary>
		///     Occurs when an item is being added.
		/// </summary>
		event EventHandler<ItemActingEventArgs<T>> ItemAdding;

		/// <summary>
		///     Occurs when an item is changed.
		/// </summary>
		event EventHandler<ItemActedEventArgs<T>> ItemChanged;

		/// <summary>
		///     Occurs when an item is changing.
		/// </summary>
		event EventHandler<ItemActingEventArgs<T>> ItemChanging;

		/// <summary>
		///     Occurs when an item is removed.
		/// </summary>
		event EventHandler<ItemActedEventArgs<T>> ItemRemoved;

		/// <summary>
		///     Occurs when an item is removing.
		/// </summary>
		event EventHandler<ItemActingEventArgs<T>> ItemRemoving;

		/// <summary>
		///     Occurs when all of the items are cleared.
		/// </summary>
		event EventHandler ItemsCleared;

		/// <summary>
		///     Occurs when all of the items are being cleared.
		/// </summary>
		event EventHandler<ActingEventArgs> ItemsClearing;
	}
}