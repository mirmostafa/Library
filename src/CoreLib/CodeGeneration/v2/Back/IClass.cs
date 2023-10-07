using Library.DesignPatterns.Markers;

namespace Library.CodeGeneration.v2.Back;

public interface IClass : IType, IHasGenericTypes
{
    bool IsStatic { get; }
}

[Immutable]
public sealed class Class(string name) : TypeBase(name), IClass
{
    public ISet<IGenericType> GenericTypes { get; } = new HashSet<IGenericType>();
    public bool IsStatic { get; init; }
}

public static class ClassExtensions
{
    public static TClass AddMember<TClass>(this TClass @class, IMember member) where TClass : IClass =>
        @class.Fluent(@class.Members.Add(member));
}