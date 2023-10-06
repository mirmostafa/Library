using Library.Results;

namespace Library.CodeGeneration.v2.Back;

public interface INamespace
{
    string Name { get; }
    ISet<IType> Types { get; }
    ISet<string> UsingNamespaces { get; }

    static INamespace New(string name) =>
        new Namespace(name);
}

internal class Namespace(string name) : INamespace
{
    public string Name { get; } = name;
    public ISet<IType> Types { get; } = new HashSet<IType>();
    public ISet<string> UsingNamespaces { get; } = new HashSet<string>();
}

public static class NameSpaceExtensions
{
    public static TNamespace AddType<TNamespace>(this TNamespace ns, IType type)
        where TNamespace : INamespace
    {
        _ = ns.Types.Add(type);
        return ns;
    }

    public static TryMethodResult<TNamespace> TryAddType<TNamespace>(this TNamespace ns, IType type)
        where TNamespace : INamespace
    {
        var result = ns.Types.Add(type);
        return TryMethodResult<TNamespace>.TryParseResult(result, ns);
    }
}