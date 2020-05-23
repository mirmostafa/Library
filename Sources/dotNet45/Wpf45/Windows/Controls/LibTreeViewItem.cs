using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Mohammad.Wpf.Windows.Controls
{
    public class LibTreeViewItem : LibTreeViewItem<object> {}

    public class LibTreeViewItem<T> : TreeViewItem
    {
        private LibTreeViewItems<T> _Items;
        public new T Tag { get; set; }
        public new LibTreeViewItems<T> Items { get { return this._Items ?? (this._Items = new LibTreeViewItems<T>()); } }
    }

    public class LibTreeViewItems<T> : IList<T>
    {
        public IEnumerator<T> GetEnumerator() { throw new NotImplementedException(); }
        IEnumerator IEnumerable.GetEnumerator() { return this.GetEnumerator(); }
        public void Add(T item) { throw new NotImplementedException(); }
        public void Clear() { throw new NotImplementedException(); }
        public bool Contains(T item) { throw new NotImplementedException(); }
        public void CopyTo(T[] array, int arrayIndex) { throw new NotImplementedException(); }
        public bool Remove(T item) { throw new NotImplementedException(); }
        public int Count { get; private set; }
        public bool IsReadOnly { get; private set; }
        public int IndexOf(T item) { throw new NotImplementedException(); }
        public void Insert(int index, T item) { throw new NotImplementedException(); }
        public void RemoveAt(int index) { throw new NotImplementedException(); }
        public T this[int index] { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }
    }
}