using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Mohammad.Collections.Hierarchy
{
    public class ReadOnlyTree<T> : IEnumerable<T>
    {
        private Func<T, T, bool> _ItemComparer;
        protected List<Node<T>> Nodes { get; }

        public Func<T, T, bool> ItemComparer
        {
            get { return this._ItemComparer ?? (this._ItemComparer = (t1, t2) => t1.Equals(t2)); }
            set { this._ItemComparer = value ?? ((t1, t2) => t1.Equals(t2)); }
        }

        public IEnumerable<T> Roots => this.Nodes.Where(n => this.ItemComparer(n.Parent, default(T))).Select(node => node.Current);
        public IEnumerable<T> Items => this.Nodes.Select(node => node.Current);
        public ReadOnlyTree(List<Node<T>> nodes) { this.Nodes = nodes; }
        public IEnumerable<T> GetChildrenOf(T parent) => this.Nodes.Where(node => this.ItemComparer(node.Parent, parent)).Select(node => node.Current);

        public T GetParentOf(T child)
        {
            var result = this.Nodes.FirstOrDefault(n => this.ItemComparer(n.Parent, child));
            return result == null ? default(T) : result.Current;
        }

        protected Node<T> GetNodeOf(T item) => this.Nodes.SingleOrDefault(n => this.ItemComparer(n.Current, item));

        public IEnumerable<T> LookBy(Func<T, bool> predicator)
        {
            Contract.Requires(predicator != null);
            return this.Where(predicator);
        }

        public bool Contains(Node<T> node) => this.Contains(node.Current);
        public bool Contains(T item) => this.Items.Contains(item);
        public IEnumerator<T> GetEnumerator() => this.Nodes.Select(n => n.Current).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}