#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Library40.Bcl.Hierarchy
{
	public class ReadOnlyTree<T> : IEnumerable<T>
	{
		private Func<T, T, bool> _ItemComparer;

		public ReadOnlyTree(List<Node<T>> nodes)
		{
			this.Nodes = nodes;
		}

		protected List<Node<T>> Nodes { get; private set; }

		public Func<T, T, bool> ItemComparer
		{
			get { return this._ItemComparer ?? (this._ItemComparer = (t1, t2) => t1.Equals(t2)); }
			set { this._ItemComparer = value ?? ((t1, t2) => t1.Equals(t2)); }
		}
		public IEnumerable<T> Roots
		{
			get { return this.Nodes.Where(n => this.ItemComparer(n.Parent, default(T))).Select(node => node.Current); }
		}

		#region IEnumerable<T> Members
		public IEnumerator<T> GetEnumerator()
		{
			return this.Nodes.Select(n => n.Current).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion

		public IEnumerable<T> GetChildrenOf(T parent)
		{
			return this.Nodes.Where(node => this.ItemComparer(node.Parent, parent)).Select(node => node.Current);
		}

		public T GetParentOf(T child)
		{
			var result = this.Nodes.SingleOrDefault(n => this.ItemComparer(n.Parent, child));
			return result == null ? default(T) : result.Current;
		}

		protected Node<T> GetNodeOf(T item)
		{
			return this.Nodes.SingleOrDefault(n => this.ItemComparer(n.Current, item));
		}

		public IEnumerable<T> LookBy(Func<T, bool> predicator)
		{
			Contract.Requires(predicator != null);

			return this.Where(predicator);
		}
	}
}