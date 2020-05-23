using System;
using System.Collections.Generic;

namespace Mohammad.Collections.Hierarchy
{
    public class Node<T> : IEquatable<Node<T>>
    {
        private Func<T, T, bool> _ItemComparer;
        public static IEqualityComparer<Node<T>> CurrentComparer { get; } = new CurrentEqualityComparer();

        public Func<T, T, bool> ItemComparer
        {
            get { return this._ItemComparer ?? (this._ItemComparer = (t1, t2) => t1.Equals(t2)); }
            set { this._ItemComparer = value ?? ((t1, t2) => t1.Equals(t2)); }
        }

        public T Current { get; }
        public T Parent { get; set; }

        public Node(T parent, T current)
        {
            this.Current = current;
            this.Parent = parent;
        }

        public Node(T current) { this.Current = current; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == this.GetType() && this.Equals((Node<T>) obj);
        }

        public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(this.Current);
        public static bool operator ==(Node<T> left, Node<T> right) => Equals(left, right);
        public static bool operator !=(Node<T> left, Node<T> right) => !Equals(left, right);

        public bool Equals(Node<T> other)
        {
            if (ReferenceEquals(null, other))
                return false;
            return ReferenceEquals(this, other) || EqualityComparer<T>.Default.Equals(this.Current, other.Current);
        }

        private sealed class CurrentEqualityComparer : IEqualityComparer<Node<T>>
        {
            public bool Equals(Node<T> x, Node<T> y)
            {
                if (ReferenceEquals(x, y))
                    return true;
                if (ReferenceEquals(x, null))
                    return false;
                if (ReferenceEquals(y, null))
                    return false;
                return x.GetType() == y.GetType() && EqualityComparer<T>.Default.Equals(x.Current, y.Current);
            }

            public int GetHashCode(Node<T> obj) => EqualityComparer<T>.Default.GetHashCode(obj.Current);
        }
    }
}