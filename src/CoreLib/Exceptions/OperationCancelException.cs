using System.Runtime.Serialization;

using Library.Windows;

namespace Library.Exceptions;

[Serializable]
public sealed class OperationCancelException : ExceptionBase, IException
{
    public OperationCancelException()
        : base("Operation cancelled.")
    {
    }

    public OperationCancelException(string message) : base(message)
    {
    }

    public OperationCancelException(NotificationMessage notificationMessage) : base(notificationMessage)
    {
    }

    public OperationCancelException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public OperationCancelException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
    {
    }

    public OperationCancelException(string message, string? instruction = null, string? title = null, string? details = null, Exception? inner = null, object? owner = null) : base(message, instruction, title, details, inner, owner)
    {
    }
}
