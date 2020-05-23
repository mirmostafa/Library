#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Collections.Generic;
using System.Linq;

namespace Library40.Bcl.Hierarchy
{
	public class TreeById<T> : ReadOnlyTreeById<T>
	{
		public TreeById()
			: base(new List<NodeById<T>>())
		{
		}

		public void Add(T root, long id)
		{
			this.Nodes.Add(new NodeById<T>(this, root, id));
		}

		public void Add(T current, long currentId, long parentId)
		{
			this.Nodes.Add(new NodeById<T>(this, current, currentId, parentId));
		}

		public void Remove(long parentId)
		{
			var children = this.Nodes.Where(n => n.ParentId == parentId).ToList();
			foreach (var child in children)
				this.Nodes.Remove(child);
			this.Nodes.Remove(this.GetNodeOf(parentId));
		}

		private NodeById<T> GetNodeOf(long currentId)
		{
			return this.Nodes.SingleOrDefault(n => n.CurrentId == currentId);
		}

		public ReadOnlyTreeById<T> AsReadOnly()
		{
			return new ReadOnlyTreeById<T>(this.Nodes);
		}
	}
}