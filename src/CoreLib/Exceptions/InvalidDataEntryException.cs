namespace Library.Exceptions;

[Serializable]
internal class InvalidDataEntryException : LibraryExceptionBase
{
    public InvalidDataEntryException()
    {
    }

    public InvalidDataEntryException(string message) : base(message)
    {
    }

    public InvalidDataEntryException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public InvalidDataEntryException(string message, string? instruction = null, string? title = null, string? details = null, Exception? inner = null, object? owner = null)
        : base(message, instruction, title, details, inner, owner)
    {
    }
}
