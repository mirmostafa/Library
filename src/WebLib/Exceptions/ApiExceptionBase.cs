using Library.Exceptions;

namespace Library.Web.Exceptions;

[Serializable]
public abstract class ApiExceptionBase : ExceptionBase, IApiException
{
    protected ApiExceptionBase(string? message) : base(message) =>
        this.Message = message?.ToString() ?? string.Empty;

    protected ApiExceptionBase(int? statusCode = null, string? message = null, object? owner = null)
        : this(message)
    {
        this.Message = message ?? string.Empty;
        this.StatusCode = statusCode;
        this.Owner = owner;
    }

    protected ApiExceptionBase()
        : this(string.Empty)
    {
    }

    protected ApiExceptionBase(string message, Exception innerException) : base(message, innerException)
    {
    }

    string? IException.Instruction { get; }

    /// <inheritdoc/>
    public override string Message { get; }

    public new object? Owner { get; set; }
    public int? StatusCode { get; }
    public new string? Title { get; }
}