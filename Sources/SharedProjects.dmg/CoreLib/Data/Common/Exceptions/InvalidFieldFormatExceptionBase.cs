using System;
using System.Runtime.Serialization;

namespace Mohammad.Data.Exceptions
{
    [Serializable]
    public sealed class InvalidFieldFormatException : DataValidationException
    {
        public InvalidFieldFormatException() { }

        public InvalidFieldFormatException(string message)
            : base(message) { }

        public InvalidFieldFormatException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public InvalidFieldFormatException(string message, Exception inner)
            : base(message, inner) { }

        public InvalidFieldFormatException(string message, Exception inner, string instruction)
            : base(message, inner, instruction) { }

        public InvalidFieldFormatException(string message, string instruction)
            : base(message, instruction) { }
    }
}