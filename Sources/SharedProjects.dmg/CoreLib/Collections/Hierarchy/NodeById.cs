using System;
using System.Collections.Generic;
using System.Linq;

namespace Mohammad.Collections.Hierarchy
{
    public class NodeById<T> : IEquatable<NodeById<T>>
    {
        public NodeById<T> ParentNode => this.Container.GetParentNodeOf(this);
        public T Parent => this.Container.GetParentNodeOf(this).Current;
        public IEnumerable<NodeById<T>> ChildNodes => this.Container.GetChildNodesOf(this);
        public IEnumerable<T> Children => this.Container.GetChildNodesOf(this).Select(item => item.Current);
        protected IReadOnlyTreeById<T> Container { get; }
        public T Current { get; set; }
        public long CurrentId { get; }
        public long? ParentId { get; set; }

        public NodeById(IReadOnlyTreeById<T> container, T current, long currentId)
        {
            this.Container = container;
            this.Current = current;
            this.CurrentId = currentId;
        }

        public NodeById(IReadOnlyTreeById<T> container, T current, long currentId, long parentId)
            : this(container, current, currentId) { this.ParentId = parentId; }

        public override string ToString() => this.Current.ToString();

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return this.Equals((NodeById<T>) obj);
        }

        public override int GetHashCode() => this.CurrentId.GetHashCode();

        public static bool operator ==(NodeById<T> left, NodeById<T> right) => Equals(left, right);
        public static bool operator !=(NodeById<T> left, NodeById<T> right) => !Equals(left, right);

        public bool Equals(NodeById<T> other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return this.CurrentId == other.CurrentId;
        }
    }
}