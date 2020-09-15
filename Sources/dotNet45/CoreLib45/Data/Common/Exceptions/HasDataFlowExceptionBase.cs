using System;
using System.Runtime.Serialization;

namespace Mohammad.Data.Common.Exceptions
{
    [Serializable]
    public sealed class HasDataFlowException : DataValidationException
    {
        // Methods
        public HasDataFlowException()
        {
        }

        public HasDataFlowException(string message)
            : base(message)
        {
        }

        public HasDataFlowException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public HasDataFlowException(string message, Exception inner, string instruction)
            : base(message, inner, instruction)
        {
        }

        public HasDataFlowException(string message, string instruction)
            : base(message, instruction)
        {
        }

        private HasDataFlowException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}