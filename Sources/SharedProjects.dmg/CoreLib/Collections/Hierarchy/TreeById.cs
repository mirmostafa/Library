using System.Collections.Generic;
using System.Linq;

namespace Mohammad.Collections.Hierarchy
{
    public class TreeById<T> : ReadOnlyTreeById<T>
    {
        public TreeById()
            : base(new List<NodeById<T>>()) { }

        public TreeById(List<NodeById<T>> nodes)
            : base(nodes) { }

        public void Add(T root, long id) { this.Nodes.Add(new NodeById<T>(this, root, id)); }
        public void Add(T current, long currentId, long parentId) { this.Nodes.Add(new NodeById<T>(this, current, currentId, parentId)); }

        public void Remove(long parentId)
        {
            var children = this.Nodes.Where(n => n.ParentId == parentId).ToList();
            foreach (var child in children)
                this.Nodes.Remove(child);
            this.Nodes.Remove(this.GetNodeOf(parentId));
        }

        private NodeById<T> GetNodeOf(long currentId) { return this.Nodes.SingleOrDefault(n => n.CurrentId == currentId); }
        public ReadOnlyTreeById<T> AsReadOnly() { return new ReadOnlyTreeById<T>(this.Nodes); }
    }
}