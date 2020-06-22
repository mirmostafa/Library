namespace Mohammad.Collections.Hierarchy
{
    public class NodeByIdComplex<TItem, TParent>
    {
        protected IReadOnlyTreeByIdComplex<TItem, TParent> Container { get; private set; }
        public TItem Current { get; set; }
        public long CurrentId { get; set; }
        public TParent Parent { get; set; }
        public long? ParentId { get; set; }

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

        public override string ToString() { return this.Current.ToString(); }
    }
}