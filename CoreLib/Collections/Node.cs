using Library.DesignPatterns.Markers;
using Library.Validations;

namespace Library.Collections;

[Immutable]
[Fluent]
public class Node<T>
{
    public static readonly Node<T?> Empty = new(default, null);

    public Node() { }

    public Node(in T value, in string? display = null)
        : this()
    {
        this.Value = value;
        this.Display = display;
    }

    public T Value { get; init; }
    public string? Display { get; init; }
    public Node<T>? Parent { get; private set; }
    protected FluentList<Node<T>> Childs { get; } = new();
    public IEnumerable<T> ChildValues => this.Children.Select(x => x.To<T>());
    public IEnumerable<Node<T>> Children => this.Childs.ToEnumerable();

    public Node<T> AddChild(in T value, string? display = null)
        => this.AddChild(ToNode(value, display));

    public Node<T> AddChild([DisallowNull] Node<T> node)
    {
        Check.IfArgumentNotNull(node, nameof(node));
        node.Parent = this;
        _ = this.Childs.Add(node);
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

    public override string ToString()
        => this.Display ?? this.Value?.ToString() ?? "Empty Value!";

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

    public static Node<T> ToNode(T t, string? display = null)
        => new(t, display);

    public Node<T> WithParent(Node<T> parent, string? display = null)
    {
        var result = new Node<T>(this.Value, display ?? this.Display) { Parent = parent };
        result.Childs.AddRange(this.Childs);
        return result;
    }

    public Node<T> WithParent(T parent, string? display = null) =>
        this.WithParent(new(parent, display), display);
}
