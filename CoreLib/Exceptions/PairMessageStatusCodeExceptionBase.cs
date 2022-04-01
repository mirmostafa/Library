namespace Library.Exceptions;

[Serializable]
public abstract class PairMessageStatusCodeExceptionBase<TStatusCode> : LibraryExceptionBase, IApiException
{

    /// <summary>
    ///     Gets the status code.
    /// </summary>
    /// <value>
    ///     The status code.
    /// </value>
    public virtual TStatusCode StatusCode { get; init; }

    /// <summary>
    ///     Gets the information.
    /// </summary>
    /// <value>
    ///     The information.
    /// </value>
    public (TStatusCode statusCode, string? message) Info => (this.StatusCode, this.GetBaseException()?.Message);

    int? IApiException.StatusCode => this.StatusCode.ToInt();

    /// <summary>
    ///     Deconstructs the specified status code and base exception.
    /// </summary>
    /// <param name="statusCode">The status code.</param>
    /// <param name="baseException">The base exception.</param>
    public void Deconstruct(out TStatusCode statusCode, out Exception? baseException)
        => (statusCode, baseException) = (this.StatusCode, this.GetBaseException());
}
