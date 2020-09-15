using System;
using System.Runtime.Serialization;

namespace Mohammad.Validation.Exceptions
{
    [Serializable]
    public class NotEqualValidationException : ValidationExceptionBase
    {
        public NotEqualValidationException()
        {
        }

        protected NotEqualValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public NotEqualValidationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public NotEqualValidationException(object obj1Name, object obj2Name)
            : this()
        {
            this.Obj1Name = obj1Name;
            this.Obj2Name = obj2Name;
        }

        public NotEqualValidationException(string message, Exception inner, string instruction)
            : base(message, inner, instruction)
        {
        }

        public NotEqualValidationException(string message, string instruction)
            : base(message, instruction)
        {
        }

        public object Obj1Name { get; set; }
        public object Obj2Name { get; set; }
    }

    [Serializable]
    public class InvalidFormatValidationException : ValidationExceptionBase
    {
        public InvalidFormatValidationException()
        {
        }

        public InvalidFormatValidationException(string message)
            : base(message)
        {
        }

        public InvalidFormatValidationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected InvalidFormatValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}