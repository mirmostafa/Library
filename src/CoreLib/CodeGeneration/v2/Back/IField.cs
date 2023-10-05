namespace Library.CodeGeneration.v2.Back;

public interface IField : IMember
{
    bool IsReadOnly { get; }
    IType Type { get; }
}