#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Collections.Generic;
using System.Linq;

namespace Library40.Bcl.Hierarchy
{
	public class Tree<T> : ReadOnlyTree<T>
	{
		public Tree()
			: base(new List<Node<T>>())
		{
		}

		public void Add(T root)
		{
			this.Nodes.Add(new Node<T>(root));
		}

		public void Add(T parent, T item)
		{
			this.Nodes.Add(new Node<T>(parent, item));
		}

		public void Remove(T parent)
		{
			var children = this.GetChildrenOf(parent).ToList();
			foreach (var child in children)
				this.Nodes.Remove(this.GetNodeOf(child));
			this.Nodes.Remove(this.GetNodeOf(parent));
		}

		public ReadOnlyTree<T> AsReadOnly()
		{
			return new ReadOnlyTree<T>(this.Nodes);
		}
	}
}