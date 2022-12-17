namespace Library.CodeGeneration.v2.Back;

public interface IMethod : IMember
{
    ISet<string>? Body { get; }

    bool IsConstructor { get; }

    ISet<(IType Type, string Name)> Parameters { get; }

    IType ReturnType { get; }
}
