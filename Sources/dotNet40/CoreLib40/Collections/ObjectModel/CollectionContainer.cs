#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Collections.ObjectModel;
using Library40.Helpers;

namespace Library40.Collections.ObjectModel
{
	/// <summary>
	/// </summary>
	/// <typeparam name="T">The type of the data.</typeparam>
	/// <typeparam name="TCollection">The type of the collection.</typeparam>
	public abstract class CollectionContainer<T, TCollection>
		where TCollection : Collection<T>, new()
	{
		private TCollection _Items;
		/// <summary>
		///     Gets the items.
		/// </summary>
		/// <value>The items.</value>
		protected TCollection Items
		{
			get { return PropertyHelper.Get(ref this._Items); }
		}

		/// <summary>
		///     Gets the <see cref="T" /> at the specified index.
		/// </summary>
		/// <value></value>
		public T this[int index]
		{
			get { return this.Items[index]; }
		}
	}
}