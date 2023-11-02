using Library.DesignPatterns.Markers;
using Library.Results;
using Library.Validations;

namespace Library.CodeGeneration.v2.Back;

public interface IMethod : IMember, IHasGenericTypes
{
    string? Body { get; }
    bool IsConstructor { get; }
    bool IsExtension { get; }
    ISet<(TypePath Type, string Name)> Parameters { get; }
    TypePath? ReturnType { get; }
}

[Immutable]
public sealed class Method(string name) : Member(name), IMethod
{
    public string? Body { get; init; }
    public ISet<IGenericType> GenericTypes { get; } = new HashSet<IGenericType>();
    public bool IsConstructor { get; init; }
    public bool IsExtension { get; init; }
    public ISet<(TypePath Type, string Name)> Parameters { get; } = new HashSet<(TypePath Type, string Name)>();
    public TypePath? ReturnType { get; init; }

    protected override Result OnValidate() =>
        this.Check()
            .RuleFor(x => !(x.IsExtension && !x.Parameters.Any()), () => "Extension method cannot be parameterless.")
            .RuleFor(x => !(x.IsConstructor && x.IsExtension), () => "Constructor cannot be extension method.")
            .Build();
}

public static class MethodExtensions
{
    public static TMethod AddParameter<TMethod>(this TMethod method, string Type, string Name) where TMethod : IMethod =>
        method.Fluent(method.Parameters.Add((Type, Name)));

    public static IEnumerable<string> GetNameSpaces(this IMethod method)
    {
        Check.MustBeArgumentNotNull(method);
        return new HashSet<string>(gather(method)).ToEnumerable();

        static IEnumerable<string> gather(IMethod method)
        {
            if (method.ReturnType != null)
            {
                foreach (var item in method.ReturnType.GetNameSpaces())
                {
                    yield return item;
                }
            }

            foreach (var item in method.Parameters.Select(x => x.Type))
            {
                foreach (var ns in item.GetNameSpaces())
                {
                    yield return ns;
                }
            }
        }
    }
}