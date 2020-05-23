using System;
using System.Runtime.Serialization;

namespace Mohammad.Exceptions
{
    [Serializable]
    public class InvalidDataEntryException : LibraryExceptionBase
    {
        public InvalidDataEntryException() { }

        public InvalidDataEntryException(string message)
            : base(message) { }

        public InvalidDataEntryException(string message, Exception inner)
            : base(message, inner) { }

        protected InvalidDataEntryException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public InvalidDataEntryException(string message, string instruction)
            : base(message, instruction) { }

        public InvalidDataEntryException(string message, Exception inner, string instruction)
            : base(message, inner, instruction) { }
    }
}