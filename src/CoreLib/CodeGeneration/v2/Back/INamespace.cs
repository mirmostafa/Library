using Library.DesignPatterns.Markers;

namespace Library.CodeGeneration.v2.Back;

public interface INamespace
{
    string Name { get; }
    ISet<IType> Types { get; }
    ISet<string> UsingNamespaces { get; }

    static INamespace New(string name) =>
        new Namespace(name);
}

[Immutable]
public sealed class Namespace(string name) : INamespace
{
    public string Name { get; } = name;
    public ISet<IType> Types { get; } = new HashSet<IType>();
    public ISet<string> UsingNamespaces { get; } = new HashSet<string>();
}