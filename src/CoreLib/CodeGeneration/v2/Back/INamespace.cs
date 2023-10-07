using Library.DesignPatterns.Markers;
using Library.Results;
using Library.Validations;

namespace Library.CodeGeneration.v2.Back;

public interface INamespace : IValidatable
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
    public string Name { get; } = name.ArgumentNotNull();
    public ISet<IType> Types { get; } = new HashSet<IType>();
    public ISet<string> UsingNamespaces { get; } = new HashSet<string>();

    public Result Validate() => Result.Success;
}

public static class NamSpaceExtensions
{
    public static TNameSpace AddType<TNameSpace>(this TNameSpace nameSpace, IType type) where TNameSpace : INamespace =>
        nameSpace.Fluent(nameSpace.Types.Add(type));
}