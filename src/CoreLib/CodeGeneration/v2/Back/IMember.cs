using Library.DesignPatterns.Markers;
using Library.Results;
using Library.Validations;

namespace Library.CodeGeneration.v2.Back;

public interface IMember : IValidatable
{
    AccessModifier AccessModifier { get; }
    InheritanceModifier InheritanceModifier { get; }
    string Name { get; }
}

[Immutable]
public abstract class Member : IMember
{
    protected Member(string name) =>
        this.Name = name.ArgumentNotNull();

    public virtual AccessModifier AccessModifier { get; }
    public virtual InheritanceModifier InheritanceModifier { get; }
    public virtual string Name { get; }

    public virtual Result Validate() =>
        Result.Success;
}