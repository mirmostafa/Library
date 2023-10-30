using Library.DesignPatterns.Markers;
using Library.Results;
using Library.Validations;

namespace Library.CodeGeneration.v2.Back;

public interface IType : IValidatable
{
    AccessModifier AccessModifier { get; }
    ISet<TypePath> BaseTypes { get; }
    InheritanceModifier InheritanceModifier { get; }
    ISet<IMember> Members { get; }
    string Name { get; }
    ISet<string> UsingNamesSpaces { get; }
}

[Immutable]
public abstract class TypeBase : IType
{
    protected TypeBase(in string name) =>
        this.Name = name;

    public virtual AccessModifier AccessModifier { get; init; } = AccessModifier.Public;
    public virtual ISet<TypePath> BaseTypes { get; } = new HashSet<TypePath>();
    public virtual InheritanceModifier InheritanceModifier { get; init; } = InheritanceModifier.Sealed;
    public virtual ISet<IMember> Members { get; } = new HashSet<IMember>();
    public virtual string Name { get; }
    public virtual ISet<string> UsingNamesSpaces { get; } = new HashSet<string>(new[] { nameof(System), nameof(System.Linq), nameof(System.Threading.Tasks), });

    public virtual Result Validate() => 
        Check.IfIsNull(this.Name).TryParse(out var vr1)
            ? vr1
            : Check.IfAnyNull(this.UsingNamesSpaces).TryParse(out var vr2) 
                ? vr2 
                : Result.Success;
}