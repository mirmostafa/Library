namespace Library.CodeGeneration.v2.Back;

public interface INamespace
{
    string Name { get; }

    ISet<IType> Types { get; }

    ISet<string> UsingNamespaces { get; }

    static INamespace New(string name)
        => new Namespace(name);

    INamespace AddType(IType type);
}

internal class Namespace : INamespace
{
    public Namespace(string name)
        => this.Name = name;

    public string Name { get; }

    public ISet<IType> Types { get; } = new HashSet<IType>();

    public ISet<string> UsingNamespaces { get; } = new HashSet<string>();

    public INamespace AddType(IType type)
    {
        _ = this.Types.Add(type);
        return this;
    }

    INamespace INamespace.AddType(IType type)
        => this.AddType(type);
}
