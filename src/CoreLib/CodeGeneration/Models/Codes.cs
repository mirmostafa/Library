using System.Collections.ObjectModel;
using System.Numerics;

using Library.Collections;
using Library.DesignPatterns.Markers;
using Library.Interfaces;

namespace Library.CodeGeneration.Models;

[Fluent]
[Immutable]
public sealed class Codes(IEnumerable<Code?> items) : ReadOnlyCollection<Code?>(items.ToList())
    , IEnumerable<Code?>
    , IAdditionOperators<Codes, Codes, Codes>
    , IIndexable<string, Code?>
    , INew<Codes, IEnumerable<Code>>
    , IEmpty<Codes>
{
    private static Codes? _empty;

    /// <summary>
    /// Initializes a new instance of the Codes class with a given collection of Code items.
    /// </summary>
    /// <param name="items">The collection of Code items.</param>
    public Codes(params Code?[] items)
        : this(items.AsEnumerable()) { }

    /// <summary>
    /// Initializes a new instance of the Codes class with a given collection of Codes.
    /// </summary>
    /// <param name="allCodes">The collection of Codes.</param>
    public Codes(IEnumerable<Codes> allCodes)
        : this(allCodes.SelectAll()) { }

    /// <summary>
    /// Represents an empty instance of Codes class.
    /// </summary>
    public static new Codes Empty { get; } = _empty ??= NewEmpty();

    /// <summary>
    /// Gets the Code item with the specified name.
    /// </summary>
    /// <param name="name">The name of the Code item.</param>
    /// <returns>The Code item with the specified name. If no such item exists, returns null.</returns>
    public Code? this[string name] => this.SingleOrDefault(x => x?.Name == name);

    /// <summary>
    /// Combines two Codes instances into one.
    /// </summary>
    /// <param name="x">The first Codes instance.</param>
    /// <param name="y">The second Codes instance.</param>
    /// <returns>A new Codes instance that combines the Code items from both input instances.</returns>
    public static Codes Combine(Codes x, Codes y) =>
        x + y;

    /// <summary>
    /// Creates a new instance of the Codes class.
    /// </summary>
    /// <returns>A new instance of the Codes class.</returns>
    public static Codes New(IEnumerable<Code> arg) =>
        new(arg);

    public static Codes New(IEnumerable<Codes> arg) =>
        new(arg);

    public static Codes New(params Code[] codes) =>
        new(codes);

    public static Codes New(params Codes[] codes) =>
        new(codes);

    /// <summary>
    /// Creates a new instance of the Codes class that is empty.
    /// </summary>
    /// <returns>A new empty instance of the Codes class.</returns>
    public static Codes NewEmpty() =>
        [];

    /// <summary>
    /// Combines two Codes instances into one.
    /// </summary>
    /// <param name="c1">The first Codes instance.</param>
    /// <param name="c2">The second Codes instance.</param>
    /// <returns>A new Codes instance that combines the Code items from both input instances.</returns>
    public static Codes operator +(Codes c1, Codes c2) =>
        new(c1.Iterate().AddRangeImmuted(c2.Iterate()));

    public Codes Add(Code code) =>
        new(this.AddImmuted(code));

    public Codes AddRange(IEnumerable<Code> codes) =>
        new(this.AddRangeImmuted(codes));

    /// <summary>
    /// Composes all Code items in the Codes collection into a single Code.
    /// </summary>
    /// <param name="separator">The separator to be used between Code statements.</param>
    /// <returns>A composed Code item representing all Code items in the Codes collection.</returns>
    public Code ComposeAll(string? separator = null)
    {
        var result = Code.Empty;
        foreach (var code in this.Compact())
        {
            result = new(code.Name, code.Language, string.Concat(result.Statement, separator, code.Statement));
        }
        return result;
    }

    public override string? ToString() =>
        this.Count == 1 ? this[0]!.ToString() : $"{nameof(Codes)} ({this.Count})";
}