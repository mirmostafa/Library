#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Collections.Generic;
using System.Linq;

namespace Library40.Bcl.Hierarchy
{
	public class NodeById<T>
	{
		public NodeById(IReadOnlyTreeById<T> container, T current, long currentId)
		{
			this.Container = container;
			this.Current = current;
			this.CurrentId = currentId;
		}

		public NodeById(IReadOnlyTreeById<T> container, T current, long currentId, long parentId)
			: this(container, current, currentId)
		{
			this.ParentId = parentId;
		}

		public NodeById<T> ParentNode
		{
			get { return this.Container.GetParentNodeOf(this); }
		}

		public T Parent
		{
			get { return this.Container.GetParentNodeOf(this).Current; }
		}

		public IEnumerable<NodeById<T>> ChildNodes
		{
			get { return this.Container.GetChildNodesOf(this); }
		}

		public IEnumerable<T> Children
		{
			get { return this.Container.GetChildNodesOf(this).Select(item => item.Current); }
		}

		protected IReadOnlyTreeById<T> Container { get; private set; }

		public T Current { get; set; }
		public long CurrentId { get; set; }
		public long? ParentId { get; set; }

		public override string ToString()
		{
			return this.Current.ToString();
		}
	}
}