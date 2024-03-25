﻿using Library.DesignPatterns.Markers;
using Library.Results;
using Library.Validations;

namespace Library.CodeGeneration.v2.Back;

public interface IMember : IValidatable, IHasAttributes
{
    AccessModifier AccessModifier { get; }
    InheritanceModifier InheritanceModifier { get; }
    string Name { get; }
}

[Immutable]
public abstract class Member : IMember
{
    protected Member([DisallowNull] string name) =>
        this.Name = name.ArgumentNotNull();

    public virtual AccessModifier AccessModifier { get; init; } = AccessModifier.Public;
    public ISet<ICodeGenAttribute> Attributes { get; } = new HashSet<ICodeGenAttribute>();
    public virtual InheritanceModifier InheritanceModifier { get; init; }
    public virtual string Name { get; }

    public Result Validate() =>
        this.OnValidate();

    protected virtual Result OnValidate() =>
        Result.Succeed;
}