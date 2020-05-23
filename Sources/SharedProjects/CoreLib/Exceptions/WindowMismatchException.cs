using System;
using System.Runtime.Serialization;

namespace Mohammad.Exceptions
{
    [Serializable]
    public class WindowMismatchException : LibraryExceptionBase
    {
        public WindowMismatchException() { }

        public WindowMismatchException(string message)
            : base(message) { }

        public WindowMismatchException(string message, Exception inner)
            : base(message, inner) { }

        protected WindowMismatchException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public WindowMismatchException(string message, Exception inner, string instruction)
            : base(message, inner, instruction) { }

        public WindowMismatchException(string message, string instruction)
            : base(message, instruction) { }
    }
}