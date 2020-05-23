using System;
using System.Runtime.Serialization;

namespace Mohammad.Validation.Exceptions
{
    [Serializable]
    public class NotEqualValidationException : ValidationExceptionBase
    {
        public object Obj1Name { get; set; }
        public object Obj2Name { get; set; }
        public NotEqualValidationException() { }

        protected NotEqualValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public NotEqualValidationException(string message, Exception inner)
            : base(message, inner) { }

        public NotEqualValidationException(object obj1Name, object obj2Name)
            : this()
        {
            this.Obj1Name = obj1Name;
            this.Obj2Name = obj2Name;
        }

        public NotEqualValidationException(string message, Exception inner, string instruction)
            : base(message, inner, instruction) { }

        public NotEqualValidationException(string message, string instruction)
            : base(message, instruction) { }
    }

    [System.Serializable]
    public class InvalidFormatValidationException : ValidationExceptionBase
    {
        public InvalidFormatValidationException() { }

        public InvalidFormatValidationException(string message)
            : base(message) { }

        public InvalidFormatValidationException(string message, System.Exception inner)
            : base(message, inner) { }

        protected InvalidFormatValidationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}