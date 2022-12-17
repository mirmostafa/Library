namespace Library.CodeGeneration.v2.Back;

public interface IClass : IType
{
    static IClass New(string name)
        => new Class(name);

    IClass AddProperty(string propertyName, Type type);
}

internal class Class : IClass
{
    public Class(string name) => this.Name = name;

    public ISet<IAttribute> Attributes { get; } = new HashSet<IAttribute>();

    public ISet<string>? BaseTypeNames { get; } = new HashSet<string>();

    public MemberPrefixes MemberPrefixes { get; set; } = MemberPrefixes.None;

    public ISet<IMember> Members { get; } = new HashSet<IMember>();

    public string Name { get; }

    public ISet<string> UsingNamespaces { get; } = new HashSet<string>();

    public IClass AddProperty(string propertyName, Type type)
    {
        _ = this.Members.Add(IProperty.New(propertyName, type!.FullName!));
        return this;
    }
}
