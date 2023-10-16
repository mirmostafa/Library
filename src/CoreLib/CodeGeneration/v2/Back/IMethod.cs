namespace Library.CodeGeneration.v2.Back;

public interface IMethod : IMember
{
    bool IsAbstract { get; }

    ISet<string>? Body { get; }

    bool IsConstructor { get; }

    ISet<(IType Type, string Name)> Parameters { get; }

    IType ReturnType { get; }
}
