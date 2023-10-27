using Library.DesignPatterns.Markers;
using Library.Results;
using Library.Validations;

namespace Library.CodeGeneration.v2.Back;

public interface IProperty : IMember
{
    string? BackingFieldName { get; }
    PropertyAccessor? Getter { get; }
    PropertyAccessor? Setter { get; }
    TypePath Type { get; }

    static IProperty New(
        string name,
        TypePath type,
        AccessModifier modifier = AccessModifier.Public,
        InheritanceModifier inheritanceModifier = InheritanceModifier.None,
        PropertyAccessor? setter = null,
        PropertyAccessor? getter = null,
        string? backingField = null) =>
        new CodeGenProperty(name, type, modifier, inheritanceModifier, setter, getter, backingField);
}

public class CodeGenProperty(
    string name,
    TypePath type,
    AccessModifier modifier = AccessModifier.Public,
    InheritanceModifier inheritanceModifier = InheritanceModifier.None,
    PropertyAccessor? setter = null,
    PropertyAccessor? getter = null,
    string? backingField = null) : IProperty
{
    public AccessModifier AccessModifier { get; } = modifier;
    public string? BackingFieldName { get; } = backingField;
    public PropertyAccessor? Getter { get; } = getter;
    public InheritanceModifier InheritanceModifier { get; } = inheritanceModifier;
    public string Name { get; } = name.ArgumentNotNull();
    public PropertyAccessor? Setter { get; } = setter;
    public TypePath Type { get; } = type.ArgumentNotNull();

    [return: NotNull]
    public Result Validate() => Result.Success;
}

[Immutable]
public readonly record struct PropertyAccessor(AccessModifier AccessModifier = AccessModifier.None, string? Body = null);