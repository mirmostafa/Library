using System;
using System.Runtime.Serialization;

namespace Mohammad.Exceptions
{
    [Serializable]
    public abstract class PairMessageStatusCodeExceptionBase<TStatusCode> : CommonExceptionBase
    {
        protected PairMessageStatusCodeExceptionBase(string message, TStatusCode statusCode)
            : base(message) => this.StatusCode = statusCode;

        protected PairMessageStatusCodeExceptionBase()
        {
        }

        protected PairMessageStatusCodeExceptionBase(string message)
            : base(message)
        {
        }

        protected PairMessageStatusCodeExceptionBase(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected PairMessageStatusCodeExceptionBase(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public virtual TStatusCode StatusCode { get; }

        public (TStatusCode StatusCode, string Message) Info => (this.StatusCode, this.GetBaseException().Message);
    }
}