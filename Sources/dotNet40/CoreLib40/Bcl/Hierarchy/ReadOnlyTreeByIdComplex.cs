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
	public class ReadOnlyTreeByIdComplex<TItem, TParent> : IEnumerable<TItem>, IReadOnlyTreeByIdComplex<TItem, TParent>
	{
		public ReadOnlyTreeByIdComplex(List<NodeByIdComplex<TItem, TParent>> nodes)
		{
			this.Nodes = nodes;
		}

		protected List<NodeByIdComplex<TItem, TParent>> Nodes { get; private set; }

		#region IEnumerable<TItem> Members
		public IEnumerator<TItem> GetEnumerator()
		{
			return this.Nodes.Select(n => n.Current).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion

		public IEnumerable<NodeByIdComplex<TItem, TParent>> GetChildNodesOf(long parentId)
		{
			return this.Nodes.Where(n => n.ParentId == parentId);
		}

		public NodeByIdComplex<TItem, TParent> GetParentNodeOf(long childId)
		{
			return this.Nodes.Single(n => n.CurrentId == childId);
		}

		public IEnumerable<TItem> GetChildrenOf(long parentId)
		{
			return this.GetChildNodesOf(parentId).Select(node => node.Current);
		}

		public TParent GetParentOf(long childId)
		{
			return this.Nodes.Where(n => n.CurrentId == childId).Select(n => n.Parent).Single();
		}

		public IEnumerable<TItem> LookBy(Func<TItem, bool> predicator)
		{
			Contract.Requires(predicator != null);
			return this.Where(predicator);
		}

		public IEnumerable<NodeByIdComplex<TItem, TParent>> LookForNodeBy(Func<NodeByIdComplex<TItem, TParent>, bool> predicator)
		{
			Contract.Requires(predicator != null);
			return this.Nodes.Where(predicator);
		}

		public IEnumerable<TParent> LookBy(Func<TParent, bool> predicator)
		{
			Contract.Requires(predicator != null);
			return this.Nodes.Select(n => n.Parent).Where(predicator);
		}

		public TItem GetByItemId(long id)
		{
			return this.Nodes.Where(n => n.CurrentId == id).Select(n => n.Current).SingleOrDefault();
		}

		public NodeByIdComplex<TItem, TParent> GetNodeByItemId(long id)
		{
			return this.Nodes.SingleOrDefault(n => n.CurrentId == id);
		}
	}
}