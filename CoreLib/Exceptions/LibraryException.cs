using Library.Interfaces;
using Library.Windows;

namespace Library.Exceptions;

[Serializable]
public sealed class LibraryException : LibraryExceptionBase, IThrowableException<LibraryException>
{
    public LibraryException()
    {
    }

    public LibraryException(string message)
        : base(message)
    {
    }

    public LibraryException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public LibraryException(string message, string? instruction = null, string? title = null, string? details = null, Exception? inner = null, object? owner = null)
        : base(message, instruction, title, details, inner, owner)
    {
    }

    public LibraryException(NotificationMessage notificationMessage)
        : base(notificationMessage)
    {
    }
}