using Library.Collections;
using Library.DesignPatterns.Markers;

namespace Library.CodeGeneration.Models;

[Fluent]
[Immutable]
public sealed class Codes : SpecializedListBase<Code?, Codes>, IIndexable<Code?, string>, IIndexable<Codes, Language>, IIndexable<Codes, bool>, IEnumerable<Code?>
{
    public Codes(IEnumerable<Code?> items)
        : base(items)
    {
    }

    public Codes(params Code?[] items)
        : base(items)
    {
    }

    public Code? this[string name] => this.ByCriteria(x => x?.Name == name);
    public Codes this[Language language] => this.InCriteria(x => x?.Language == language);
    public Codes this[bool isPartial] => this.InCriteria(x => x?.IsPartial == isPartial);

    public Code ComposeAll()
    {
        var result = Code.Empty;
        foreach (var code in this.Compact())
        {
            result = new(code.Name, code.Language, string.Concat(result.Statement, code.Statement));
        }
        return result;
    }

    protected override Codes OnGetNew(IEnumerable<Code?> items)
            => new(items);
}