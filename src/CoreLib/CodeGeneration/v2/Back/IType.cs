namespace Library.CodeGeneration.v2.Back;

public interface IType
{
    AccessModifier AccessModifier { get; }
    ISet<IAttribute> Attributes { get; }
    ISet<TypePath> BaseTypes { get; }
    InheritanceModifier InheritanceModifier { get; }
    ISet<IMember> Members { get; }
    string Name { get; }
    ISet<string> UsingNamesSpaces { get; }
}