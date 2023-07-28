using System.Diagnostics;

using Library.Interfaces;

using CurrenntException = Library.Exceptions.Validations.ValidationException;

namespace Library.Exceptions.Validations;

[Serializable]
public sealed class ValidationException : ValidationExceptionBase, IThrowableException
{
    public ValidationException()
    {
    }

    public ValidationException(string message)
        : base(message)
    {
    }

    public ValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public ValidationException(
        string message,
        string? instruction = null,
        string? title = null,
        string? details = null,
        Exception? inner = null,
        object? owner = null)
        : base(message, instruction, title, details, inner, owner)
    {
    }

    private ValidationException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
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