using Library.DesignPatterns.Markers;
using Library.Results;
using Library.Validations;

namespace Library.CodeGeneration.v2.Back;

public interface IType : IValidatable
{
    AccessModifier AccessModifier { get; }
    ISet<IAttribute> Attributes { get; }
    ISet<TypePath> BaseTypes { get; }
    InheritanceModifier InheritanceModifier { get; }
    ISet<IMember> Members { get; }
    string Name { get; }
    ISet<string> UsingNamesSpaces { get; }
}

[Immutable]
public abstract class TypeBase : IType
{
    protected TypeBase(string name) =>
        this.Name = name;

    public virtual AccessModifier AccessModifier { get; init; } = AccessModifier.Public;
    public virtual ISet<IAttribute> Attributes { get; } = new HashSet<IAttribute>();
    public virtual ISet<TypePath> BaseTypes { get; } = new HashSet<TypePath>();
    public virtual InheritanceModifier InheritanceModifier { get; init; } = InheritanceModifier.Sealed;
    public virtual ISet<IMember> Members { get; } = new HashSet<IMember>();
    public virtual string Name { get; }
    public virtual ISet<string> UsingNamesSpaces { get; } = new HashSet<string>(new[] { nameof(System), nameof(System.Linq), nameof(System.Threading.Tasks), });

    public virtual Result Validate() =>
        Result.Success;
}