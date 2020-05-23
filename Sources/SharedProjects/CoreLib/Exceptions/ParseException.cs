using System;
using System.Runtime.Serialization;

namespace Mohammad.Exceptions
{
    [Serializable]
    public class ParseException : LibraryExceptionBase
    {
        public ParseException() { }

        public ParseException(string message)
            : base(message) { }

        public ParseException(string message, Exception inner)
            : base(message, inner) { }

        protected ParseException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public ParseException(string value, Type type)
            : this(string.Format("Cannot convert '{0}' to type '{1}'", value, type)) { }

        public ParseException(string message, Exception inner, string instruction)
            : base(message, inner, instruction) { }

        public ParseException(string message, string instruction)
            : base(message, instruction) { }
    }
}