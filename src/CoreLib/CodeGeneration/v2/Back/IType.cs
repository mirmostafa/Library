namespace Library.CodeGeneration.v2.Back;

public interface IType : IHasMemberPrefix, IHasAttribute
{
    ISet<string>? BaseTypeNames { get; }

    ISet<IMember> Members { get; }

    string Name { get; }

    ISet<string> UsingNamespaces { get; }
}
