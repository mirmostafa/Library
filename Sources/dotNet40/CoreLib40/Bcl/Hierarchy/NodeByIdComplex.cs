#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

namespace Library40.Bcl.Hierarchy
{
	public class NodeByIdComplex<TItem, TParent>
	{
		public NodeByIdComplex(IReadOnlyTreeByIdComplex<TItem, TParent> container, TParent parent, long parentId)
		{
			this.Container = container;
			this.Parent = parent;
			this.ParentId = parentId;
		}

		public NodeByIdComplex(IReadOnlyTreeByIdComplex<TItem, TParent> container, TItem current, long currentId, TParent parent, long parentId)
			: this(container, parent, parentId)
		{
			this.Current = current;
			this.CurrentId = currentId;
		}

		protected IReadOnlyTreeByIdComplex<TItem, TParent> Container { get; private set; }

		public TItem Current { get; set; }
		public long CurrentId { get; set; }
		public TParent Parent { get; set; }
		public long? ParentId { get; set; }

		public override string ToString()
		{
			return this.Current.ToString();
		}
	}
}