using System;
using System.Runtime.Serialization;

namespace Mohammad.Exceptions
{
    [Serializable]
    public class ObjectDuplicateException : LibraryExceptionBase
    {
        public ObjectDuplicateException()
        {
        }

        public ObjectDuplicateException(string message)
            : base(message)
        {
        }

        public ObjectDuplicateException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public ObjectDuplicateException(string message, Exception inner, string instruction)
            : base(message, inner, instruction)
        {
        }

        public ObjectDuplicateException(string message, string instruction)
            : base(message, instruction)
        {
        }

        protected ObjectDuplicateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}