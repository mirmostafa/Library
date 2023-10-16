using Library.DesignPatterns.Markers;

namespace Library.CodeGeneration.v2.Back;

public interface IProperty : IMember
{
    string? BackingFieldName { get; }
    PropertyAccessor? Getter { get; }
    PropertyAccessor? Setter { get; }
    TypePath Type { get; }
}

[Immutable]
public readonly record struct PropertyAccessor(AccessModifier AccessModifier = AccessModifier.None, string? Body = null);