using System.Xml.Linq;

using Library.DesignPatterns.Markers;

namespace Library.CodeGeneration.v2.Back;

public interface IClass : IType, IHasGenericTypes
{
    bool IsStatic { get; }
}

[Immutable]
public sealed class Class(string name) : TypeBase(name), IClass
{
    public bool IsStatic { get; init; }
    public ISet<IGenericType> GenericTypes { get; } = new HashSet<IGenericType>();
}