namespace Library.CodeGeneration.v2.Back;

public interface IClass : IType, IHasGenericTypes, ICanBePartial
{
    bool IsStatic { get; }
}

internal class Class(string name) : IClass
{
    public AccessModifier AccessModifier { get; } = AccessModifier.None;
    public ISet<IAttribute> Attributes { get; } = new HashSet<IAttribute>();
    public ISet<TypePath> BaseTypes { get; } = new HashSet<TypePath>();
    public ISet<IGenericType> GenericTypes { get; } = new HashSet<IGenericType>();
    public InheritanceModifier InheritanceModifier { get; }
    public bool IsPartial { get; }
    public bool IsStatic { get; init; }
    public ISet<IMember> Members { get; } = new HashSet<IMember>();
    public string Name { get; } = name;
    public ISet<string> UsingNamesSpaces { get; } = new HashSet<string>();
}