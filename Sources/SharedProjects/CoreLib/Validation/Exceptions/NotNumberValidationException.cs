using System;
using System.Runtime.Serialization;

namespace Mohammad.Validation.Exceptions
{
    [Serializable]
    public class NotNumberValidationException : ValidationExceptionBase
    {
        public string ParameterName { get; set; }
        public NotNumberValidationException() { }

        protected NotNumberValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public NotNumberValidationException(string message, Exception inner)
            : base(message, inner) { }

        public NotNumberValidationException(string parameterName)
            : this() { this.ParameterName = parameterName; }

        public NotNumberValidationException(string message, string instruction)
            : base(message, instruction) { }

        public NotNumberValidationException(string message, Exception inner, string instruction)
            : base(message, inner, instruction) { }
    }
}