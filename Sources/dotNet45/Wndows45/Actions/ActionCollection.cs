using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Design;
using Mohammad.Win.Controls;

namespace Mohammad.Win.Actions
{
    /// <summary>
    ///     A collection of actions to be used in ActionList
    /// </summary>
    [Editor(typeof(ActionCollectionEditor), typeof(UITypeEditor))]
    public class ActionCollection : Collection<Action>
    {
        /// <summary>
        ///     Gets the parent ActionList
        /// </summary>
        public ActionList Parent { get; }

        /// <summary>
        ///     Creates an new instance
        /// </summary>
        /// <param name="parent">Parent ActionList</param>
        public ActionCollection(ActionList parent) { this.Parent = parent; }

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