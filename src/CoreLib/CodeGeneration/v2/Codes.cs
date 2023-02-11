using Library.Collections;
using Library.DesignPatterns.Markers;
using Library.Interfaces;
using Library.Validations;

namespace Library.CodeGeneration.v2;

[Fluent]
[Immutable]
public sealed class Codes : SpecializedListBase<Code, Codes>
    , IIndexable<string, Code?>
    , IIndexable<Language, Codes>
    , IIndexable<bool, Codes>
    , IEnumerable<Code>
    , IAdditionOperators<Codes, Code, Codes>
    , IAdditionOperators<Codes, Codes, Codes>
{
    public Codes(IEnumerable<Code> items)
        : base(items)
    {
    }

    public Codes(params Code[] items)
        : base(items)
    {
    }

    public Code? this[string name] => this.ByCriteria(x => x.Name == name);

    public Codes this[Language language] => this.InCriteria(x => x.Language == language);

    public Codes this[bool isPartial] => this.InCriteria(x => x.IsPartial == isPartial);

    public static Codes operator +(Codes left, Code right)
        => new(left.ArgumentNotNull().ToEnumerable().AddImmuted(right.ArgumentNotNull()));

    public static Codes operator +(Codes left, Codes right)
        => new(left.ArgumentNotNull().ToEnumerable().AddRangeImmuted(right.ArgumentNotNull()));

    protected override Codes OnGetNew(IEnumerable<Code> items)
        => new(items);
}