using System.Diagnostics.CodeAnalysis;
using Library.DesignPatterns.Markers;
using Library.Validations;

namespace Library.Collections;

[Immutable]
[Fluent]
public class Node<T>
{
    public static readonly Node<T?> Empty = new(default);

    private Node<T>? _parent;

    public Node() { }

    public Node(in T value)
        : this()
        => this.Value = value;

    public T Value { get; init; }
    public Node<T>? Parent { get => this._parent; init => this._parent = value; }
    protected FluentList<Node<T>> Childs { get; } = new();
    public IEnumerable<T> ChildValues => this.Children.Select(x => x.To<T>());
    public IEnumerable<Node<T>> Children => this.Childs.ToEnumerable();

    public Node<T> AddChild(in T value)
        => this.AddChild(ToNode(value));

    public Node<T> AddChild([DisallowNull] Node<T> node)
    {
        Check.IfArgumentNotNull(node, nameof(node));
        node._parent = this;
        this.Childs.Add(node);
        return this;
    }

    public Node<T> AddChildren([DisallowNull] IEnumerable<T> values)
    {
        Check.IfArgumentNotNull(values, nameof(values));

        foreach (var value in values)
        {
            this.AddChild(value);
        }
        return this;
    }

    public override string ToString()
        => this.Value?.ToString() ?? "Empty Value!";

    public static implicit operator T?(Node<T> node)
        => node is null ? default : node.Value;

    public static bool operator ==(Node<T> x, Node<T> y)
        => x.Value?.Equals(y.Value) ?? y.Value is null;

    public static bool operator !=(Node<T> x, Node<T> y)
        => !(x == y);

    public override bool Equals(object? obj)
        => obj is Node<T> other && this == other;
    public override int GetHashCode()
        => this.Value?.GetHashCode() ?? base.GetHashCode();

    public bool Is<TType>()
        => this.Value is TType;

    public bool Is<TType>(out TType? value)
    {
        value = default;
        if (this.Value is TType)
        {
            value = this.Value.To<TType>();
            return true;
        }
        return false;
    }

    public T ToType()
        => this.Value;

    public static Node<T> ToNode(T t)
        => new(t);

    public Node<T> WithParent(Node<T> parent)
    {
        var result = new Node<T>(this.Value) { Parent = parent };
        result.Childs.AddRange(this.Childs);
        return result;
    }

    public Node<T> WithParent(T parent) => 
        this.WithParent(new(parent));
}
