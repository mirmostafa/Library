using System.Diagnostics;

using Library.Interfaces;

using CurrenntException = Library.Exceptions.NoItemFoundException;

namespace Library.Exceptions;

[Serializable]
public sealed class NoItemFoundException : LibraryExceptionBase, IThrowableException
{
    public NoItemFoundException()
        : this("No item found.")
    {
    }

    public NoItemFoundException(string message)
        : base(message)
    {
    }

    public NoItemFoundException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public NoItemFoundException(string message, string? instruction = null, string? title = null, string? details = null, Exception? inner = null, object? owner = null)
        : base(message, instruction, title, details, inner, owner)
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