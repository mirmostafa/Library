namespace Library.CodeGeneration.v2.Back;

public interface IType
{
    AccessModifier AccessModifier { get; set; }
    ISet<IAttribute> Attributes { get; }
    ISet<string>? BaseTypeNames { get; }
    bool IsAbstract { get; }
    ISet<IMember> Members { get; }
    string Name { get; }
    ISet<string> UsingNamespaces { get; }
}