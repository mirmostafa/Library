using System;
using System.Runtime.Serialization;

namespace Mohammad.Exceptions
{
    [Serializable]
    public class ObjectNotFoundException : LibraryExceptionBase
    {
        public ObjectNotFoundException()
        {
        }

        public ObjectNotFoundException(string message)
            : base(message)
        {
        }

        public ObjectNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ObjectNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public ObjectNotFoundException(string message, Exception inner, string instruction)
            : base(message, inner, instruction)
        {
        }

        public ObjectNotFoundException(string message, string instruction)
            : base(message, instruction)
        {
        }
    }
}