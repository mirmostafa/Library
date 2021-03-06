using System;
using System.Runtime.Serialization;

namespace Mohammad.Data.Common.Exceptions
{
    [Serializable]
    public sealed class MissingRequiredFieldException : DataValidationException
    {
        public MissingRequiredFieldException()
        {
        }

        public MissingRequiredFieldException(string message)
            : base(message)
        {
        }

        public MissingRequiredFieldException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public MissingRequiredFieldException(string message, Exception inner, string instruction)
            : base(message, inner, instruction)
        {
        }

        public MissingRequiredFieldException(string message, string instruction)
            : base(message, instruction)
        {
        }

        private MissingRequiredFieldException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}