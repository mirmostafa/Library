using Library.DesignPatterns.Markers;

namespace Library.CodeGeneration.v2.Back;

public interface IStruct : IType
{
    bool IsReadOnly { get; }
}

[Immutable]
public sealed class Struct(string name) : TypeBase(name), IStruct
{
    public bool IsReadOnly { get; init; }
}