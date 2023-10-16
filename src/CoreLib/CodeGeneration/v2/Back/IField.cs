using Library.DesignPatterns.Markers;

namespace Library.CodeGeneration.v2.Back;

public interface IField : IMember
{
    bool IsReadOnly { get; }
    TypePath Type { get; }
}

[Immutable]
public sealed class Field(string name, TypePath type) : Member(name), IField
{
    public bool IsReadOnly { get; init; }
    public TypePath Type { get; } = type;
}