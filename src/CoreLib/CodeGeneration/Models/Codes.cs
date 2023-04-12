using System.Collections.Immutable;
using System.Collections.ObjectModel;

using Library.Collections;
using Library.DesignPatterns.Markers;

namespace Library.CodeGeneration.Models;

[Fluent]
[Immutable]
public sealed class Codes : ReadOnlyCollection<Code?>, IIndexable<string, Code?>, IIndexable<Language, IEnumerable<Code>>, IEnumerable<Code?>//, IImmutableList<Code>
{
    public Codes(IEnumerable<Code?> items)
        : base(items.ToList())
    {
    }

    public Codes(params Code?[] items)
        : base(items)
    {
    }

    public Code? this[string name] => this.FirstOrDefault(x => x?.Name == name);

    public IEnumerable<Code> this[Language language] => this.Where(x => x?.Language == language).Compact();

    public static Codes operator +(Codes c1, Codes c2) 
        => new(c1.AsEnumerable().AddRangeImmuted(c2.AsEnumerable()));

    public Codes Add(Code code)
        => new(this.AddImmuted(code));

    public Code ComposeAll(string? separator = null)
    {
        var result = Code.Empty;
        foreach (var code in this.Compact())
        {
            result = new(code.Name, code.Language, string.Concat(result.Statement, separator, code.Statement));
        }
        return result;
    }
}