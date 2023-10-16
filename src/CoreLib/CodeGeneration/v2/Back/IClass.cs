namespace Library.CodeGeneration.v2.Back;

public interface IClass : IType
{
    bool IsPartial { get; }
    bool IsStatic { get; }

    static IClass New(string name, bool isStatic, bool isPartial) =>
        new Class(name) { IsStatic = isStatic, IsPartial = isPartial };
}

internal class Class(string name) : IClass
{
    public AccessModifier AccessModifier { get; set; } = AccessModifier.None;
    public ISet<IAttribute> Attributes { get; } = new HashSet<IAttribute>();
    public ISet<string>? BaseTypeNames { get; } = new HashSet<string>();
    public bool IsPartial { get; init; }
    public bool IsStatic { get; init; }
    public ISet<IMember> Members { get; } = new HashSet<IMember>();
    public string Name { get; } = name;
    public ISet<string> UsingNamespaces { get; } = new HashSet<string>();
}