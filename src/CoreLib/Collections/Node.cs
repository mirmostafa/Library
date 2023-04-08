using Library.DesignPatterns.Markers;
using Library.Helpers;
using Library.Interfaces;
using Library.Validations;

namespace Library.Collections;

[Immutable]
[Fluent]
public sealed class Node<T> : IEquatable<Node<T>>, IEquatable<T>, IHasChildren<Node<T>>
{
    public static readonly Node<T?> Empty = new(default, null);

    public Node(in T value, in string? display = null)
        => (this.Value, this.Display) = (value, display ?? value?.ToString());

    public IEnumerable<Node<T>> Children => this.ChildList.ToEnumerable();
    public IEnumerable<T> ChildValues => this.Children.Select(x => x.Cast().To<T>());
    public string? Display { get; }
    public Node<T>? Parent { get; private set; }
    public T Value { get; }
    protected FluentList<Node<T>> ChildList { get; } = new();

    public static implicit operator T?(Node<T> node)
        => node is null ? default : node.Value;

    public static bool operator !=(Node<T> x, Node<T> y)
        => !(x == y);

    public static bool operator ==(Node<T> x, Node<T> y)
        => x.Value?.Equals(y.Value) ?? y.Value is null;

    public static Node<T> ToNode(T t, string? display = null)
        => new(t, display);

    public Node<T> AddChild(in T value, string? display = null)
        => this.AddChild(ToNode(value, display));

    public Node<T> AddChild([DisallowNull] Node<T> node, params Node<T>[] nodes)
    {
        Check.IfArgumentNotNull(node, nameof(node));

        node.Parent = this;
        _ = this.ChildList.Add(node);
        _ = nodes.ForEach(x => AddChild(x)).Build();
        return this;
    }

    public Node<T> AddChildren([DisallowNull] IEnumerable<T> values)
    {
        Check.IfArgumentNotNull(values, nameof(values));

        foreach (var value in values)
        {
            _ = this.AddChild(value, this.Display);
        }
        return this;
    }

    public static Node<T> Create(Func<(T Value, string Display)> getRoot, Func<(T Value, string? Display), IEnumerable<(T Value, string? Display)>> getChidren)
    {
        Node<T> result = Node<T>.Empty;
        EnumerableHelper.BuildTree<(T Value, string? Display), Node<T>>(
            EnumerableHelper.ToEnumerable(getRoot())!
            , raw => new(raw.Value, raw.Display)
            , raw => getChidren(raw), raw => result = new(raw.Value, raw.Display)
            , (p, c) => p.AddChild(c));
        return result;
    }

    public static Node<T> Create(Func<T> getRoot, Func<T, IEnumerable<T>> getChidren)
    {
        Node<T> result = Node<T>.Empty;
        EnumerableHelper.BuildTree<T, Node<T>>(
            EnumerableHelper.ToEnumerable(getRoot())!
            , raw => new(raw)
            , raw => getChidren(raw), raw => result = raw
            , (p, c) => p.AddChild(c));
        return result;
    }

    public override bool Equals(object? obj)
            => obj is Node<T> other && this == other;

    public bool Equals(Node<T>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (this.Value is null && other.Value is null)
        {
            return true;
        }

        if (other.Value?.Equals(this.Value) == true)
        {
            return true;
        }

        return false;
    }

    public bool Equals(T? other)
        => (this.Value, other) switch
        {
            (null, null) => true,
            (not null, null) or (null, not null) => false,
            _ => this.Value.Equals(other)
        };

    public override int GetHashCode()
        => this.Value?.GetHashCode() ?? base.GetHashCode();

    public bool Is<TType>()
        => this.Value is TType;

    public bool Is<TType>(out TType? value)
    {
        if (this.Value is TType)
        {
            value = this.Value.Cast().To<TType>();
            return true;
        }
        value = default;
        return false;
    }

    public override string ToString()
        => this.Display ?? this.Value?.ToString() ?? "Empty Value!";

    public T ToType()
        => this.Value;

    public Node<T> WithParent(Node<T> parent, string? display = null)
    {
        var result = new Node<T>(this.Value, display ?? this.Display) { Parent = parent };
        _ = result.ChildList.AddRange(this.ChildList);
        return result;
    }

    public Node<T> WithParent(T parent, string? display = null) =>
        this.WithParent(new(parent, display), display);
}