using System;
using System.Runtime.Serialization;

namespace Mohammad.Validation.Exceptions
{
    [Serializable]
    public class NotLessThanValueValidationException : ValidationExceptionBase
    {
        public string ParameterName { get; set; }
        public object Value { get; set; }
        public NotLessThanValueValidationException() { }

        protected NotLessThanValueValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public NotLessThanValueValidationException(string message, Exception inner)
            : base(message, inner) { }

        public NotLessThanValueValidationException(string parameterName, object value)
            : this()
        {
            this.ParameterName = parameterName;
            this.Value = value;
        }

        public NotLessThanValueValidationException(string message, Exception inner, string instruction)
            : base(message, inner, instruction) { }

        public NotLessThanValueValidationException(string message, string instruction)
            : base(message, instruction) { }
    }
}