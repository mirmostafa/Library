using Library.Interfaces;

namespace Library.CodeGeneration.v2.Back;

public interface INamespace<TSelf> : INew<TSelf, string>
    where TSelf : INamespace<TSelf>
{
    string Name { get; }
    ISet<IType> Types { get; }
    ISet<string> UsingNamespaces { get; }

    TSelf AddType(IType type);
}

internal class Namespace(string name) : INamespace<Namespace>
{
    public string Name { get; } = name;
    public ISet<IType> Types { get; } = new HashSet<IType>();
    public ISet<string> UsingNamespaces { get; } = new HashSet<string>();

    public static Namespace New(string name) =>
        new(name);

    public Namespace AddType(IType type) =>
        this.Fluent(this.Types.Add(type));
}