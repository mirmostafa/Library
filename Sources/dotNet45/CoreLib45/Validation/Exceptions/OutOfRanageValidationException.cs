using System;
using System.Runtime.Serialization;

namespace Mohammad.Validation.Exceptions
{
    [Serializable]
    public class OutOfRanageValidationException : ValidationExceptionBase
    {
        public OutOfRanageValidationException()
        {
        }

        public OutOfRanageValidationException(string message)
            : base(message)
        {
        }

        public OutOfRanageValidationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected OutOfRanageValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public OutOfRanageValidationException(object value, object minValue, object maxValue)
        {
            this.Value = value;
            this.MinValue = minValue;
            this.MaxValue = maxValue;
        }

        public OutOfRanageValidationException(string message, string instruction)
            : base(message, instruction)
        {
        }

        public OutOfRanageValidationException(string message, Exception inner, string instruction)
            : base(message, inner, instruction)
        {
        }

        public object Value { get; set; }
        public object MinValue { get; set; }
        public object MaxValue { get; set; }
    }
}