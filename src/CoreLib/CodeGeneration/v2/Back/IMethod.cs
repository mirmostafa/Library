namespace Library.CodeGeneration.v2.Back;

public interface IMethod : IMember, IHasGenericTypes, ICanBePartial
{
    string? Body { get; }

    bool IsConstructor { get; }

    ISet<(string Type, string Name)> Parameters { get; }
}