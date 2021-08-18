using System.Collections;
using Library.EventsArgs;

namespace Library.Collections
{
    public sealed class EventualList<TItem> : IList<TItem>
    {
        private readonly List<TItem> _InnerList = new();

        public event EventHandler<ItemActedEventArgs<int>>? ItemIndexChanged;
        public event EventHandler<ItemActedEventArgs<int>>? ItemIndexRemoved;
        public event EventHandler<ItemActedEventArgs<TItem>>? ItemAdded;
        public event EventHandler<ItemActedEventArgs<TItem>>? ItemRemoved;
        public event EventHandler<ItemActedEventArgs<(int Index, TItem Item)>>? ItemInserted;
        public event EventHandler? Cleared;

        public TItem this[int index]
        {
            get => this._InnerList[index];
            set
            {
                this._InnerList[index] = value;
                ItemIndexChanged?.Invoke(this, new(index));
            }
        }

        public int Count
            => this._InnerList.Count;

        bool ICollection<TItem>.IsReadOnly => this._InnerList.As<IList<TItem>>()!.IsReadOnly;

        public void Add(TItem item)
        {
            this._InnerList.Add(item);
            ItemAdded?.Invoke(this, new(item));
        }

        public void Clear()
        {
            this._InnerList.Clear();
            Cleared?.Invoke(this, EventArgs.Empty);
        }
        public bool Contains(TItem item) => this._InnerList.Contains(item);
        public void CopyTo(TItem[] array, int arrayIndex) => this._InnerList.CopyTo(array, arrayIndex);
        public IEnumerator<TItem> GetEnumerator() => this._InnerList.GetEnumerator();
        public int IndexOf(TItem item) => this._InnerList.IndexOf(item);
        public void Insert(int index, TItem item)
        {
            this._InnerList.Insert(index, item);
            this.ItemInserted?.Invoke(this, new((index, item)));
        }

        public bool Remove(TItem item)
        {
            var result = this._InnerList.Remove(item);
            ItemRemoved?.Invoke(this, new(item));
            return result;
        }

        public void RemoveAt(int index)
        {
            this._InnerList.RemoveAt(index);
            ItemIndexRemoved?.Invoke(this, new(index));
        }

        IEnumerator IEnumerable.GetEnumerator() => this._InnerList.GetEnumerator();
    }
}
