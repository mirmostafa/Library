#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Design;
using Library35.Windows.Controls;

namespace Library35.Windows.Actions
{
	/// <summary>
	///     A collection of actions to be used in ActionList
	/// </summary>
	[Editor(typeof (ActionCollectionEditor), typeof (UITypeEditor))]
	public class ActionCollection : Collection<Action>
	{
		private readonly ActionList parent;

		/// <summary>
		///     Creates an new instance
		/// </summary>
		/// <param name="parent">Parent ActionList</param>
		public ActionCollection(ActionList parent)
		{
			this.parent = parent;
		}

		/// <summary>
		///     Gets the parent ActionList
		/// </summary>
		public ActionList Parent
		{
			get { return this.parent; }
		}

		/// <summary>
		///     Clears all of the action lists.
		/// </summary>
		protected override void ClearItems()
		{
			foreach (var action in this)
				action.ActionList = null;

			base.ClearItems();
		}

		/// <summary>
		///     Inserta an action in collection
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		protected override void InsertItem(int index, Action item)
		{
			base.InsertItem(index, item);
			item.ActionList = this.Parent;
		}

		/// <summary>
		///     Removes an item
		/// </summary>
		/// <param name="index">Action index</param>
		protected override void RemoveItem(int index)
		{
			this[index].ActionList = null;
			base.RemoveItem(index);
		}

		/// <summary>
		///     Sets a new action instead of the old one in given index
		/// </summary>
		/// <param name="index">The index of old action</param>
		/// <param name="item">The new action to be substituted.</param>
		protected override void SetItem(int index, Action item)
		{
			if (this.Count > index)
				this[index].ActionList = null;
			base.SetItem(index, item);

			item.ActionList = this.Parent;
		}
	}
}