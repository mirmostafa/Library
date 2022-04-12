﻿namespace Library.Exceptions;

[Serializable]
public class NoItemSelectedException : LibraryExceptionBase
{
    public NoItemSelectedException()
        : this("No item selected.")
    {
    }

    public NoItemSelectedException(string message) : base(message)
    {
    }

    public NoItemSelectedException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public NoItemSelectedException(string message, string? instruction = null, string? title = null, string? details = null, Exception? inner = null, object? owner = null) : base(message, instruction, title, details, inner, owner)
    {
    }
}