using Library.Interfaces;

namespace Library.Exceptions;

[Serializable]
public sealed class NoItemSelectedException : LibraryExceptionBase, IThrowsException<NoItemSelectedException>, IThrowableException
{
    public NoItemSelectedException()
        : this("No item selected.")
    {
    }

    public NoItemSelectedException(string message)
        : base(message)
    {
    }

    public NoItemSelectedException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public NoItemSelectedException(string message, string? instruction = null, string? title = null, string? details = null, Exception? inner = null, object? owner = null)
        : base(message, instruction, title, details, inner, owner)
    {
    }

    [DoesNotReturn]
    public static void Throw()
        => throw new NoItemSelectedException();

    [DoesNotReturn]
    public static void Throw(string message)
        => throw new NoItemSelectedException(message);

    [DoesNotReturn]
    public static void Throw(string? message, Exception? innerException)
        => throw new NoItemSelectedException(message, innerException);
}