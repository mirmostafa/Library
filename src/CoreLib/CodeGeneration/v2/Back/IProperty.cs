using Library.DesignPatterns.Markers;
using Library.Validations;

namespace Library.CodeGeneration.v2.Back;

public interface IProperty:IMember
{
    string? BackingFieldName { get; }
    PropertyAccessor? Getter { get; }
    PropertyAccessor? Setter { get; }
    TypePath Type { get; }
}

public class CodeGenProperty : Member, IProperty
{
    public CodeGenProperty(
    string name,
    TypePath type,
    AccessModifier modifier = AccessModifier.Public,
    InheritanceModifier inheritanceModifier = InheritanceModifier.None,
    PropertyAccessor? setter = null,
    PropertyAccessor? getter = null,
    string? backingField = null) : base(name)
    {
        this.Type = type.ArgumentNotNull();
        this.AccessModifier = modifier;
        this.InheritanceModifier = inheritanceModifier;
        this.Setter = setter;
        this.Getter = getter;
    }

    public string? BackingFieldName { get; }
    public PropertyAccessor? Getter { get; }
    public PropertyAccessor? Setter { get; }
    public TypePath Type { get; }
}

[Immutable]
public readonly record struct PropertyAccessor(AccessModifier AccessModifier = AccessModifier.None, string? Body = null);