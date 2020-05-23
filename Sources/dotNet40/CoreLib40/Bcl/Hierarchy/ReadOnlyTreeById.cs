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
	public class ReadOnlyTreeById<T> : IReadOnlyTreeById<T>
	{
		public ReadOnlyTreeById(List<NodeById<T>> nodes)
		{
			this.Nodes = nodes;
		}

		protected List<NodeById<T>> Nodes { get; private set; }

		public IEnumerable<T> Roots
		{
			get { return this.RootNodes.Select(n => n.Current); }
		}

		public IEnumerable<NodeById<T>> RootNodes
		{
			get { return this.Nodes.Where(n => !n.ParentId.HasValue); }
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

		public bool HasChild(NodeById<T> parent)
		{
			return GetChildNodesOf(parent).Any();
		}

		public IEnumerable<NodeById<T>> GetChildNodesOf(NodeById<T> parent)
		{
			return this.Nodes.Where(n => n.ParentId == parent.CurrentId);
		}

		public NodeById<T> GetParentNodeOf(NodeById<T> child)
		{
			return this.Nodes.SingleOrDefault(n => n.CurrentId == child.ParentId);
		}

		public IEnumerable<T> GetChildrenOf(long parentId)
		{
			return this.GetChildNodesOf(parentId).Select(node => node.Current);
		}

		public IEnumerable<NodeById<T>> GetChildNodesOf(long parentId)
		{
			return this.Nodes.Where(n => n.ParentId == parentId);
		}

		public IEnumerable<T> GetChildrenOf(NodeById<T> parent)
		{
			return this.GetChildNodesOf(parent).Select(node => node.Current);
		}

		public bool HasChild(long id)
		{
			return GetChildNodesOf(id).Any();
		}

		public T GetParentOf(long childId)
		{
			var result = this.Nodes.SingleOrDefault(n => n.CurrentId == childId);
			return result == null ? default(T) : this.Nodes.Where(n => n.CurrentId == result.CurrentId).Select(n => n.Current).Single();
		}

		public NodeById<T> GetParentNodeOf(long childId)
		{
			var result = this.Nodes.SingleOrDefault(n => n.CurrentId == childId);
			return result == null ? null : this.Nodes.Single(n => n.CurrentId == result.CurrentId);
		}

		public IEnumerable<T> LookBy(Func<T, bool> predicator)
		{
			Contract.Requires(predicator != null);
			return this.Where(predicator);
		}

		public IEnumerable<NodeById<T>> LookForNodeBy(Func<NodeById<T>, bool> predicator)
		{
			Contract.Requires(predicator != null);
			return this.Nodes.Where(predicator);
		}

		public T GetById(long id)
		{
			return this.Nodes.Where(n => n.CurrentId == id).Select(n => n.Current).SingleOrDefault();
		}

		public NodeById<T> GetNodeById(long id)
		{
			return this.Nodes.SingleOrDefault(n => n.CurrentId == id);
		}
	}
}