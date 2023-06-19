namespace Library.Interfaces;

/// <summary>
/// Defines a mechanism for computing the sum of two values.
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
/// <typeparam name="TOther">The type that will be added to <typeparamref name="TSelf"/>.</typeparam>
/// <typeparam name="TResult">
/// The type that contains the sum of <typeparamref name="TSelf"/> and <typeparamref name="TOther"/>.
/// </typeparam>
public interface IAdditionOperators<in TSelf, in TOther, out TResult>
    where TSelf : IAdditionOperators<TSelf, TOther, TResult>
{
    /// <summary>
    /// Adds two values together to compute their sum.
    /// </summary>
    /// <param name="left">The value to which <paramref name="right"/> is added.</param>
    /// <param name="right">The value which is added to <paramref name="left"/>.</param>
    /// <returns>The sum of <paramref name="left"/> and <paramref name="right"/>.</returns>
    static abstract TResult operator +(TSelf left, TOther right);

    //! Not supported yet.
    //x static TResult operator checked +(TSelf left, TOther right);
}

/// <summary>
/// Defines a mechanism for parsing a string to a value.
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface IParsable<TSelf>
    where TSelf : IParsable<TSelf>
{
    /// <summary>
    /// Parses a string into a value.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">
    /// An object that provides culture-specific formatting information about <paramref name="s"/>.
    /// </param>
    /// <returns>The result of parsing <paramref name="s"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> is <c>null</c>.</exception>
    /// <exception cref="FormatException"><paramref name="s"/> is not in the correct format.</exception>
    /// <exception cref="OverflowException">
    /// <paramref name="s"/> is not representable by <typeparamref name="TSelf"/>.
    /// </exception>
    static abstract TSelf Parse(string s, IFormatProvider? provider);

    /// <summary>
    /// Tries to parses a string into a value.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">
    /// An object that provides culture-specific formatting information about <paramref name="s"/>.
    /// </param>
    /// <param name="result">
    /// On return, contains the result of successfully parsing <paramref name="s"/> or an undefined
    /// value on failure.
    /// </param>
    /// <returns><c>true</c> if <paramref name="s"/> was successfully parsed; otherwise, <c>false</c>.</returns>
    static abstract bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(returnValue: false)] out TSelf result);
}

/// <summary>
/// Defines a mechanism for computing the difference of two values.
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
/// <typeparam name="TOther">The type that will be subtracted from <typeparamref name="TSelf"/>.</typeparam>
/// <typeparam name="TResult">
/// The type that contains the difference of <typeparamref name="TOther"/> subtracted from
/// <typeparamref name="TSelf"/>.
/// </typeparam>
public interface ISubtractionOperators<in TSelf, in TOther, out TResult>
    where TSelf : ISubtractionOperators<TSelf, TOther, TResult>
{
    /// <summary>
    /// Subtracts two values to compute their difference.
    /// </summary>
    /// <param name="left">The value from which <paramref name="right"/> is subtracted.</param>
    /// <param name="right">The value which is subtracted from <paramref name="left"/>.</param>
    /// <returns>The difference of <paramref name="right"/> subtracted from <paramref name="left"/>.</returns>
    static abstract TResult operator -(TSelf left, TOther right);
}

//public interface IBinaryOperators<in TSelf, in TOther, out TResult>
//    where TSelf : class
//    where TOther : class
//    where TResult : class
//{
//    static abstract TResult operator |(TSelf left, TOther right);
//}

/// <summary>
/// Defines a mechanism for getting the minimum and maximum value of a type.
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface IMinMaxValue<TSelf>
    where TSelf : IMinMaxValue<TSelf>?
{
    /// <summary>
    /// Gets the maximum value of the current type.
    /// </summary>
    static abstract TSelf MaxValue { get; }

    /// <summary>
    /// Gets the minimum value of the current type.
    /// </summary>
    static abstract TSelf MinValue { get; }
}