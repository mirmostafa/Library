using System;
using System.Runtime.Serialization;
using Mohammad.Exceptions;

namespace Mohammad.Data.Common.Exceptions
{
    [Serializable]
    public class DataValidationException : LibraryExceptionBase
    {
        public DataValidationException()
        {
        }

        public DataValidationException(string message)
            : base(message)
        {
        }

        public DataValidationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public DataValidationException(string message, Exception inner, string instruction)
            : base(message, inner, instruction)
        {
        }

        public DataValidationException(string message, string instruction)
            : base(message, instruction)
        {
        }

        protected DataValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}