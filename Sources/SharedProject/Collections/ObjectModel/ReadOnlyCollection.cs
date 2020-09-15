using System;
using System.Collections.ObjectModel;
using Mohammad.EventsArgs;
using Mohammad.Helpers;

namespace Mohammad.Collections.ObjectModel
{
    [Serializable]
    public class LibReadOnlyCollection<T> : ReadOnlyCollection<T>, IEventualEnumerable<T>
    {
        public LibReadOnlyCollection(EventualCollection<T> collection)
            : base(collection)
        {
            collection.ItemAdded += (sender, e) => this.ItemAdded.Raise(this, e);
            collection.ItemAdding += (sender, e) => this.ItemAdding.Raise(this, e);
            collection.ItemChanged += (sender, e) => this.ItemChanged.Raise(this, e);
            collection.ItemChanging += (sender, e) => this.ItemChanging.Raise(this, e);
            collection.ItemRemoved += (sender, e) => this.ItemRemoved.Raise(this, e);
            collection.ItemRemoving += (sender, e) => this.ItemRemoving.Raise(this, e);
            collection.ItemsCleared += (sender, e) => this.ItemsCleared.Raise(this, e);
            collection.ItemsClearing += (sender, e) => this.ItemsClearing.Raise(this, e);
        }

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