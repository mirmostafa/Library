using System.Collections.ObjectModel;

using Library.Collections;
using Library.DesignPatterns.Markers;
using Library.Interfaces;

namespace Library.CodeGeneration.Models;

[Fluent]
[Immutable]
public sealed class Codes(params Code?[] items) : ReadOnlyCollection<Code?>(items)
    , IAdditionOperators<Codes, Codes, Codes>
    , IEmpty<Codes>, IIndexable<string, Code?>
    , INew<Codes>
    , IIndexable<Language, IEnumerable<Code>>
    , IEnumerable<Code?>
{
    private static Codes? _empty;

    /// <summary>
    /// Initializes a new instance of the Codes class with a given collection of Code items.
    /// </summary>
    /// <param name="items">The collection of Code items.</param>
    public Codes(IEnumerable<Code?> items)
        : this(items.ToArray()) { }

    /// <summary>
    /// Initializes a new instance of the Codes class with a given collection of Codes.
    /// </summary>
    /// <param name="allCodes">The collection of Codes.</param>
    public Codes(IEnumerable<Codes> allCodes)
        : this(allCodes.SelectAll()) { }

    /// <summary>
    /// Represents an empty instance of Codes class.
    /// </summary>
    public static Codes Empty { get; } = _empty ??= NewEmpty();

    /// <summary>
    /// Gets the Code item with the specified name.
    /// </summary>
    /// <param name="name">The name of the Code item.</param>
    /// <returns>The Code item with the specified name. If no such item exists, returns null.</returns>
    public Code? this[string name] => this.FirstOrDefault(x => x?.Name == name);

    /// <summary>
    /// Gets all the Code items with the specified language.
    /// </summary>
    /// <param name="language">The language of the Code items.</param>
    /// <returns>All the Code items with the specified language.</returns>
    public IEnumerable<Code> this[Language language] => this.Where(x => x?.Language == language).Compact();

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
    public static Codes New() =>
        new();

    /// <summary>
    /// Creates a new instance of the Codes class that is empty.
    /// </summary>
    /// <returns>A new empty instance of the Codes class.</returns>
    public static Codes NewEmpty() =>
        new();

    /// <summary>
    /// Combines two Codes instances into one.
    /// </summary>
    /// <param name="c1">The first Codes instance.</param>
    /// <param name="c2">The second Codes instance.</param>
    /// <returns>A new Codes instance that combines the Code items from both input instances.</returns>
    public static Codes operator +(Codes c1, Codes c2) =>
        new(c1.AsEnumerable().AddRangeImmuted(c2.AsEnumerable()));

    /// <summary>
    /// Adds a new Code item to the Codes collection.
    /// </summary>
    /// <param name="code">The Code item to be added.</param>
    /// <returns>A new Codes instance containing the added Code item.</returns>
    public Codes Add(Code code) =>
        new(this.AddImmuted(code));

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
}