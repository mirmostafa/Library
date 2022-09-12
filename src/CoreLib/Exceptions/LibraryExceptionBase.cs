using Library.Windows;

namespace Library.Exceptions;

/// <summary>
/// </summary>
[Serializable]
public abstract class LibraryExceptionBase : ExceptionBase, ILibraryException
{
    protected LibraryExceptionBase()
    {
    }

    protected LibraryExceptionBase(string message) : base(message)
    {
    }

    protected LibraryExceptionBase(NotificationMessage notificationMessage)
        : base(notificationMessage)
    {
    }

    protected LibraryExceptionBase(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected LibraryExceptionBase(string message, string? instruction = null, string? title = null, string? details = null, Exception? inner = null, object? owner = null)
        : base(message, instruction, title, details, inner, owner)
    {
    }
}
