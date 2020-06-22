using System.Collections.Generic;
using System.Linq;

namespace Mohammad.Collections.Hierarchy
{
    public class Tree<TItem> : ReadOnlyTree<TItem>
    {
        public Tree()
            : base(new List<Node<TItem>>()) { }

        public void Add(TItem root)
        {
            if (!this.Contains(root))
                this.Nodes.Add(new Node<TItem>(root) {ItemComparer = this.ItemComparer});
        }

        public void Add(TItem item, TItem parent)
        {
            if (!this.Contains(item))
                this.Nodes.Add(new Node<TItem>(parent, item) {ItemComparer = this.ItemComparer});
        }

        public void Remove(TItem parent)
        {
            var children = this.GetChildrenOf(parent).ToList();
            foreach (var child in children)
                this.Nodes.Remove(this.GetNodeOf(child));
            this.Nodes.Remove(this.GetNodeOf(parent));
        }

        public ReadOnlyTree<TItem> ToReadOnly() => new ReadOnlyTree<TItem>(this.Nodes);
    }
}