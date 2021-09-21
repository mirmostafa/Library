namespace Library.Collections;

public class Node<T>
{
    private Node<T?>? _parent;

    public Node()
        => this.Children = new();

    public Node(T value)
        : this()
        => this.Value = value;

    public Node(T value, Node<T?> parent, IEnumerable<T>? children = null)
        : this(value)
    {
        if (parent is not null)
        {
            this.Parent = parent;
        }

        if (children?.Any() is true)
        {
            this.Children.AddRange(children.Select(v => new Node<T>(v)));
        }
    }

    public Node(T value, T? parent, IEnumerable<T>? children = null)
        : this(value, new(parent), children)
    {
    }

    public T? Value { get; init; }
    public Node<T?>? Parent { get => this._parent; init => this._parent = value; }
    public List<Node<T>> Children { get; }
    public IEnumerable<T> ChildValues => this.Children.Select(x => x.To<T>());

    public Node<T> AddChild(T value)
    {
        var node = new Node<T>(value, parent: this);
        this.Children.Add(node);
        return this;
    }

    public Node<T> AddChild(Node<T> node)
    {
        this._parent = this;
        this.Children.Add(node);
        return this;
    }

    public IEnumerable<Node<T>> AddChild(IEnumerable<T> values)
    {
        if (values is null)
        {
            throw new ArgumentNullException(nameof(values));
        }

        foreach (var value in values)
        {
            yield return this.AddChild(value);
        }
    }

    public override string ToString()
        => this.Value?.ToString() ?? "Empty Value!";

    public static implicit operator T?(Node<T?> node)
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

    public T? ToType()
        => this.Value;
}
