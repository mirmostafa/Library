using Library.DesignPatterns.Markers;

namespace Library.CodeGeneration.v2.Back;

public interface IProperty : IMember
{
    string? BackingFieldName { get; }
    PropertyAccessor Getter { get; }
    bool HasGetter { get; }
    bool HasSetter { get; }
    bool IsAbstract { get; }
    PropertyAccessor Setter { get; }

    string TypeFullName { get; }

    static IProperty New(string name, string typeFullName)
        => new Property(name, typeFullName);
}

internal class Property : IProperty
{
    public Property(string name, string type)
        => (this.Name, this.TypeFullName) = (name, type);

    public ISet<IAttribute> Attributes { get; } = new HashSet<IAttribute>();

    public string? BackingFieldName { get; }

    public string? Comment { get; }

    public PropertyAccessor Getter { get; }

    public bool HasGetter { get; }

    public bool HasSetter { get; }

    public bool IsAbstract { get; }

    public bool IsStatic { get; }

    public AccessModifier MemberPrefix { get; }

    public string Name { get; }

    public PropertyAccessor Setter { get; }

    public string TypeFullName { get; }
}

[Immutable]
public readonly struct PropertyAccessor(in bool has = true, in bool? isPrivate = null, in string? code = null)
{
    public string? Code { get; } = code;
    public bool Has { get; } = has;
    public bool? IsPrivate { get; } = isPrivate;
}