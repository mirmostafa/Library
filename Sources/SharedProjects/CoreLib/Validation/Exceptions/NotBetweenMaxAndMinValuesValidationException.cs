using System;
using System.Runtime.Serialization;

namespace Mohammad.Validation.Exceptions
{
    [Serializable]
    public class NotBetweenMaxAndMinValuesValidationException : ValidationExceptionBase
    {
        public object Value { get; set; }
        public object MinValue { get; set; }
        public object MaxValue { get; set; }
        public NotBetweenMaxAndMinValuesValidationException() { }

        public NotBetweenMaxAndMinValuesValidationException(string message)
            : base(message) { }

        public NotBetweenMaxAndMinValuesValidationException(string message, Exception inner)
            : base(message, inner) { }

        protected NotBetweenMaxAndMinValuesValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public NotBetweenMaxAndMinValuesValidationException(object value, object minValue, object maxValue)
        {
            this.Value = value;
            this.MinValue = minValue;
            this.MaxValue = maxValue;
        }

        public NotBetweenMaxAndMinValuesValidationException(string message, string instruction)
            : base(message, instruction) { }

        public NotBetweenMaxAndMinValuesValidationException(string message, Exception inner, string instruction)
            : base(message, inner, instruction) { }
    }
}