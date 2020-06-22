using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Mohammad.Collections.Hierarchy
{
    public class ReadOnlyTreeById<T> : IReadOnlyTreeById<T>
    {
        protected List<NodeById<T>> Nodes { get; }
        public IEnumerable<T> Roots => this.RootNodes.Select(n => n.Current);
        public IEnumerable<NodeById<T>> RootNodes => this.Nodes.Where(n => !n.ParentId.HasValue);
        public ReadOnlyTreeById(List<NodeById<T>> nodes) { this.Nodes = nodes; }
        public IEnumerable<T> GetChildrenOf(long parentId) => this.GetChildNodesOf(parentId).Select(node => node.Current);
        public IEnumerable<NodeById<T>> GetChildNodesOf(long parentId) => this.Nodes.Where(n => n.ParentId == parentId);
        public IEnumerable<T> GetChildrenOf(NodeById<T> parent) => this.GetChildNodesOf(parent).Select(node => node.Current);
        public bool HasChild(long id) => this.GetChildNodesOf(id).Any();

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

        public T GetById(long id) => this.Nodes.Where(n => n.CurrentId == id).Select(n => n.Current).SingleOrDefault();
        public NodeById<T> GetNodeById(long id) => this.Nodes.SingleOrDefault(n => n.CurrentId == id);
        public bool HasChild(NodeById<T> parent) => this.GetChildNodesOf(parent).Any();
        public IEnumerable<NodeById<T>> GetChildNodesOf(NodeById<T> parent) => this.Nodes.Where(n => n.ParentId == parent.CurrentId);
        public NodeById<T> GetParentNodeOf(NodeById<T> child) => this.Nodes.SingleOrDefault(n => n.CurrentId == child.ParentId);
        public IEnumerator<T> GetEnumerator() => this.Nodes.Select(n => n.Current).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}