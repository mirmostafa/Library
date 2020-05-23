using System;
using System.Runtime.Serialization;

namespace Mohammad.Validation.Exceptions
{
    [Serializable]
    public class NotNullOrZeroValidationException : ValidationExceptionBase
    {
        public string ParameterName { get; set; }
        public NotNullOrZeroValidationException() { }

        protected NotNullOrZeroValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public NotNullOrZeroValidationException(string message, Exception inner)
            : base(message, inner) { }

        public NotNullOrZeroValidationException(string parameterName)
            : this() { this.ParameterName = parameterName; }

        public NotNullOrZeroValidationException(string message, string instruction)
            : base(message, instruction) { }

        public NotNullOrZeroValidationException(string message, Exception inner, string instruction)
            : base(message, inner, instruction) { }
    }
}