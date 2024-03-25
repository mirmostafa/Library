using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Library.Windows;

namespace Library.Exceptions;

[Serializable]
public sealed class CommonException : ExceptionBase, IException
{
    public CommonException()
    {
    }

    public CommonException(string message) : base(message)
    {
    }

    public CommonException(NotificationMessage notificationMessage) : base(notificationMessage)
    {
    }

    public CommonException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public CommonException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
    {
    }

    public CommonException(string message, string? instruction = null, string? title = null, string? details = null, Exception? inner = null, object? owner = null) 
        : base(message, instruction, title, details, inner, owner)
    {
    }
}
