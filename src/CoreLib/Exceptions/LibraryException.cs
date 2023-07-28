using System.Diagnostics;

using Library.Interfaces;
using Library.Windows;

using CurrenntException = Library.Exceptions.LibraryException;

namespace Library.Exceptions;

[Serializable]
public sealed class LibraryException : LibraryExceptionBase, IThrowableException
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

    [DoesNotReturn]
    [StackTraceHidden]
    [DebuggerStepThrough]
    public static void Throw() =>
     throw new CurrenntException();

    [DoesNotReturn]
    [StackTraceHidden]
    [DebuggerStepThrough]
    public static void Throw(string message) =>
        throw new CurrenntException(message);

    [DoesNotReturn]
    [StackTraceHidden]
    [DebuggerStepThrough]
    public static void Throw(string message, Exception innerException) =>
        throw new CurrenntException(message, innerException);

    [StackTraceHidden]
    [DebuggerStepThrough]
    public static void ThrowIfNotValid([DoesNotReturnIf(false)] bool condition, string message)
    {
        if (condition)
        {
            Throw(message);
        }
    }
}