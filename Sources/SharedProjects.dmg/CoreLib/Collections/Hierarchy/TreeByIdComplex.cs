using System.Collections.Generic;
using System.Linq;

namespace Mohammad.Collections.Hierarchy
{
    public class TreeByIdComplex<TItem, TParent> : ReadOnlyTreeByIdComplex<TItem, TParent>
    {
        public TreeByIdComplex()
            : base(new List<NodeByIdComplex<TItem, TParent>>()) {}

        public void Add(TParent root, long id) { this.Nodes.Add(new NodeByIdComplex<TItem, TParent>(this, root, id)); }

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

        private NodeByIdComplex<TItem, TParent> GetNodeOfItem(long currentId) { return this.Nodes.SingleOrDefault(n => n.CurrentId == currentId); }
        private NodeByIdComplex<TItem, TParent> GetNodeOfParernt(long currentId) { return this.Nodes.SingleOrDefault(n => n.ParentId == currentId); }
        public ReadOnlyTreeByIdComplex<TItem, TParent> AsReadOnly() { return new ReadOnlyTreeByIdComplex<TItem, TParent>(this.Nodes); }
    }
}