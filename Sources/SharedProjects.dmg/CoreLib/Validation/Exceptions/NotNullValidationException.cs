using System;
using System.Runtime.Serialization;

namespace Mohammad.Validation.Exceptions
{
    [Serializable]
    public class NotNullValidationException : ValidationExceptionBase
    {
        public object Obj1Name { get; set; }
        public object Obj2Name { get; set; }
        public NotNullValidationException() { }

        protected NotNullValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public NotNullValidationException(string message, Exception inner)
            : base(message, inner) { }

        public NotNullValidationException(object obj1Name, object obj2Name)
            : this()
        {
            this.Obj1Name = obj1Name;
            this.Obj2Name = obj2Name;
        }

        public NotNullValidationException(string message, Exception inner, string instruction)
            : base(message, inner, instruction) { }

        public NotNullValidationException(string message, string instruction)
            : base(message, instruction) { }
    }
}