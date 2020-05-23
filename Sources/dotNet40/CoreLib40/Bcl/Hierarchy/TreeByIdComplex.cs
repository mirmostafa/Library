#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Collections.Generic;
using System.Linq;

namespace Library40.Bcl.Hierarchy
{
	public class TreeByIdComplex<TItem, TParent> : ReadOnlyTreeByIdComplex<TItem, TParent>
	{
		public TreeByIdComplex()
			: base(new List<NodeByIdComplex<TItem, TParent>>())
		{
		}

		public void Add(TParent root, long id)
		{
			this.Nodes.Add(new NodeByIdComplex<TItem, TParent>(this, root, id));
		}

		public void Add(TItem current, long currentId, TParent parent, long parentId)
		{
			this.Nodes.Add(new NodeByIdComplex<TItem, TParent>(this, current, currentId, parent, parentId));
		}

		public void RemoveParent(long parentId)
		{
			var children = this.Nodes.Where(n => n.ParentId == parentId).ToList();
			foreach (var child in children)
				this.Nodes.Remove(child);
			this.Nodes.Remove(this.GetNodeOfParernt(parentId));
		}

		private NodeByIdComplex<TItem, TParent> GetNodeOfItem(long currentId)
		{
			return this.Nodes.SingleOrDefault(n => n.CurrentId == currentId);
		}

		private NodeByIdComplex<TItem, TParent> GetNodeOfParernt(long currentId)
		{
			return this.Nodes.SingleOrDefault(n => n.ParentId == currentId);
		}

		public ReadOnlyTreeByIdComplex<TItem, TParent> AsReadOnly()
		{
			return new ReadOnlyTreeByIdComplex<TItem, TParent>(this.Nodes);
		}
	}
}