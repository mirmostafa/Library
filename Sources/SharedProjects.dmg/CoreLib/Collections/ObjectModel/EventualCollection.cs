using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Mohammad.EventsArgs;
using Mohammad.Helpers;

namespace Mohammad.Collections.ObjectModel
{
    [Serializable]
    public class EventualCollection<T> : ObservableCollection<T>, IEventualCollection<T>
    {
        private LibReadOnlyCollection<T> _LibReadOnlyCollection;

        public virtual LibReadOnlyCollection<T> LibReadOnlyCollection
        {
            get { return PropertyHelper.Get(ref this._LibReadOnlyCollection, () => new LibReadOnlyCollection<T>(this)); }
        }

        protected override void ClearItems()
        {
            if (this.ItemsClearing.Raise(this, new ActingEventArgs()).Handled)
                return;
            base.ClearItems();
            this.ItemsCleared.RaiseAsync(this);
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
                base.Add(item);
        }

        public void Add(params T[] items) { this.Add(items.AsEnumerable()); }
        public event EventHandler<ItemActedEventArgs<T>> ItemAdded;
        public event EventHandler<ItemActingEventArgs<T>> ItemAdding;
        public event EventHandler<ItemActedEventArgs<T>> ItemChanged;
        public event EventHandler<ItemActingEventArgs<T>> ItemChanging;
        public event EventHandler<ItemActedEventArgs<T>> ItemRemoved;
        public event EventHandler<ItemActingEventArgs<T>> ItemRemoving;
        public event EventHandler ItemsCleared;
        public event EventHandler<ActingEventArgs> ItemsClearing;
    }
}