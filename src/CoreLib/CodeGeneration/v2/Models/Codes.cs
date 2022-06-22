using Library.CodeGeneration.v2.Models;
using Library.Collections;
using Library.DesignPatterns.Markers;

namespace Library.CodeGeneration.v2;

[Fluent]
[Immutable]
public sealed class Codes : SpecializedListBase<Code, Codes>, IIndexable<Code?, string>, IIndexable<Codes, Language>, IIndexable<Codes, bool>, IEnumerable<Code>
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

    protected override Codes OnGetNew(IEnumerable<Code> items) => new(items);
}