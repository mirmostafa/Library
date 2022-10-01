using System.Collections;

namespace ConAppTest;

[Obsolete("Not fast enough 😭")]
public sealed class FastList<T> : IEnumerable<T>
{
    internal FastNode<T> Root;

    internal FastList(FastNode<T> root)
        => this.Root = root;

    public static FastList<T> New(T value)
        => new(new(value));

    public IEnumerator<T> GetEnumerator()
        => new Enumerator(this.Root);

    IEnumerator IEnumerable.GetEnumerator()
        => this.GetEnumerator();

    internal struct Enumerator : IEnumerator<T>
    {
        private readonly FastNode<T> _root;
        private FastNode<T> _currentNode;

        internal Enumerator(FastNode<T> root)
            => (this._currentNode, this._root) = (root, root);

        public T Current => this._currentNode.Value;
        object? IEnumerator.Current => this.Current;

        public void Dispose()
        { }

        public bool MoveNext()
        {
            if (this._currentNode.Next == null)
            {
                return false;
            }

            this._currentNode = this._currentNode.Next;
            return true;
        }

        public void Reset()
            => this._currentNode = this._root;
    }
}

internal sealed class FastNode<T>
{
    public FastNode(T value)
        => this.Value = value;

    internal FastNode<T>? Next { get; set; }
    internal T Value { get; }
}

public static class FastListExtension
{
    [Obsolete]
    public static FastList<T> Add<T>(this FastList<T> list, T value)
    {
        var node = list.Root;
        while (node.Next is not null)
        {
            node = node.Next;
        }
        node.Next = new(value);
        return new(list.Root);
    }
}