using Library.Interfaces;
using Library.Results;

namespace Library.Helpers;

public static class CommonHelpers
{
    /// <summary>
    /// Parses the specified input string into an object of type TSelf, using the specified format provider.
    /// </summary>
    /// <typeparam name="TSelf">The type of the self.</typeparam>
    /// <param name="input">The input string to parse.</param>
    /// <param name="formatProvider">The format provider.</param>
    /// <returns>An object of type TSelf.</returns>
    public static TSelf Parse<TSelf>(this string input, IFormatProvider? formatProvider = null)
        where TSelf : IParsable<TSelf> => TSelf.Parse(input, formatProvider);

    /// <summary>
    /// Tries to parse the specified input string into an object of type TSelf, which implements IParsable<TSelf>.
    /// </summary>
    /// <typeparam name="TSelf">The type of the object to parse.</typeparam>
    /// <param name="input">The input string to parse.</param>
    /// <param name="formatProvider">The format provider to use for parsing.</param>
    /// <returns>A TryMethodResult containing the result of the parse operation.</returns>
    public static TryMethodResult<TSelf> TryParse<TSelf>(this string input, IFormatProvider? formatProvider = null)
        where TSelf : IParsable<TSelf> => TryMethodResult<TSelf>.TryParseResult(TSelf.TryParse(input, formatProvider, out var result), result);
}