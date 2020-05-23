using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mohammad.Helpers;

namespace Mohammad.Collections.Generic
{
    public class HierarchicalList<TData> : IList<HierarchicalItem<TData>>, IIndexerEnumerable<HierarchicalItem<TData>>
    {
        private readonly List<HierarchicalItem<TData>> _InnerList;
        public HierarchicalItem<TData> this[TData data] => this._InnerList.FirstOrDefault(item => item.Data.Equals(data));
        public HierarchicalList(List<HierarchicalItem<TData>> innerList) { this._InnerList = innerList; }

        public HierarchicalList()
            : this(new List<HierarchicalItem<TData>>()) {}

        public HierarchicalList(IEnumerable<TData> rawData)
        {
            this._InnerList = new List<HierarchicalItem<TData>>();
            this._InnerList.AddRange(rawData.Select(data => new HierarchicalItem<TData>(data)));
        }

        public HierarchicalItem<TData> Add(TData data, TData parent = default(TData))
        {
            var result = new HierarchicalItem<TData>(data, parent);
            this._InnerList.Add(result);
            return result;
        }

        public IEnumerable<HierarchicalItem<TData>> GetChildren(HierarchicalItem<TData> item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            return this.Where(itm => itm.Parent != null && itm.Parent.Data.Equals(item.Data));
        }

        public IEnumerable<TData> GetChildren(TData data) => this.Where(item => item.Parent != null && item.Parent.Data.Equals(data)).Select(item => item.Data);

        public void BuildTree<TItem>(Func<TData, TItem> getNewItem, Action<TItem> addToRoots, Action<TItem, TItem> addChild)
        {
            this.GetRootElements().BuildTree(getNewItem, this.GetChildren, addToRoots, addChild);
        }

        public IEnumerable<TData> GetRootElements() => this.GetRootItems().Select(item => item.Data);
        public IEnumerable<HierarchicalItem<TData>> GetRootItems() => this.Where(item => item.Parent == null);
        public IEnumerator<HierarchicalItem<TData>> GetEnumerator() => this._InnerList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        public void Add(HierarchicalItem<TData> item) { this._InnerList.Add(item); }
        public void Clear() { this._InnerList.Clear(); }
        public bool Contains(HierarchicalItem<TData> item) => this._InnerList.Contains(item);
        public void CopyTo(HierarchicalItem<TData>[] array, int arrayIndex) { this._InnerList.CopyTo(array, arrayIndex); }
        public bool Remove(HierarchicalItem<TData> item) => this._InnerList.Remove(item);
        public int Count => this._InnerList.Count;
        bool ICollection<HierarchicalItem<TData>>.IsReadOnly { get { throw new NotSupportedException(); } }
        public int IndexOf(HierarchicalItem<TData> item) => this._InnerList.IndexOf(item);
        public void Insert(int index, HierarchicalItem<TData> item) { this._InnerList.Insert(index, item); }
        public void RemoveAt(int index) { this._InnerList.RemoveAt(index); }
        public HierarchicalItem<TData> this[int index] { get { return this._InnerList[index]; } set { this._InnerList[index] = value; } }
    }
}