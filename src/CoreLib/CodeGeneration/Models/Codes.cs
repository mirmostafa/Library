using Library.Collections;
using Library.DesignPatterns.Markers;

namespace Library.CodeGeneration.Models;

[Fluent]
[Immutable]
public sealed class Codes : SpecializedListBase<Code?, Codes>, IIndexable<string, Code?>, IIndexable<Language, Codes>, IIndexable<bool, Codes>, IEnumerable<Code?>
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

    public Code ComposeAll(string? separator = null)
    {
        var result = Code.Empty;
        foreach (var code in this.Compact())
        {
            result = new(code.Name, code.Language, string.Concat(result.Statement, separator, code.Statement));
        }
        return result;
    }

    protected override Codes OnGetNew(IEnumerable<Code?> items)
            => new(items);
}