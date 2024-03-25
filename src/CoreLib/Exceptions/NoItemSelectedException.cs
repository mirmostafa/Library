using System.Diagnostics;

using Library.Interfaces;

using CurrenntException = Library.Exceptions.NoItemSelectedException;

namespace Library.Exceptions;

[Serializable]
public sealed class NoItemSelectedException : LibraryExceptionBase, IThrowableException
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