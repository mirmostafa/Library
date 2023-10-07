using Library.DesignPatterns.Markers;
using Library.Results;
using Library.Validations;

namespace Library.CodeGeneration.v2.Back;

public interface IMethod : IMember, IHasGenericTypes
{
    string? Body { get; }
    bool IsConstructor { get; }
    ISet<(string Type, string Name)> Parameters { get; }
    public TypePath? ReturnType { get; }
}

[Immutable]
public sealed class Method(string name) : Member(name), IMethod
{
    public string? Body { get; init; }
    public ISet<IGenericType> GenericTypes { get; } = new HashSet<IGenericType>();
    public bool IsConstructor { get; init; }
    public bool IsExtension { get; init; }
    public ISet<(string Type, string Name)> Parameters { get; } = new HashSet<(string Type, string Name)>();
    public TypePath? ReturnType { get; init; }

    public override Result Validate() =>
        this.Check()
            .RuleFor(x => x.IsExtension && !x.Parameters.Any(), () => "Extension method cannot be parameterless.")
            .RuleFor(x => x.IsConstructor && x.IsExtension, () => "Constructor cannot be extension method.")
            .Build();
}

public static class MethodExtensions
{
    public static TMethod AddParameter<TMethod>(this TMethod method, string Type, string Name) where TMethod : IMethod =>
        method.Fluent(method.Parameters.Add((Type, Name)));
}