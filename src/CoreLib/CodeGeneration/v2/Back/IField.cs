using Library.DesignPatterns.Markers;

namespace Library.CodeGeneration.v2.Back;

public interface IField : IMember
{
    bool IsReadOnly { get; }
    TypePath Type { get; }
}

[Immutable]
public sealed class Field : Member, IField
{
    public Field(string name, TypePath type)
        :base(name)
    {
        this.Type = type;
        this.AccessModifier = AccessModifier.Private | AccessModifier.ReadOnly;
    }
    public bool IsReadOnly { get; init; }
    public TypePath Type { get; }
}