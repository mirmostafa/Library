


using System;
using System.Runtime.Serialization;

namespace Mohammad.Exceptions
{
    [Serializable]
    public class OperationCancelledException : CommonExceptionBase
    {
        public OperationCancelledException()
        {
        }

        public OperationCancelledException(string message)
            : base(message)
        {
        }

        public OperationCancelledException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected OperationCancelledException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public OperationCancelledException(string message, Exception inner, string instruction)
            : base(message, inner, instruction)
        {
        }

        public OperationCancelledException(string message, string instruction)
            : base(message, instruction)
        {
        }
    }
}