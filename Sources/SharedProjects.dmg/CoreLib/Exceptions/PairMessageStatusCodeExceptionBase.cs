using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Mohammad.Exceptions
{
    public abstract class PairMessageStatusCodeExceptionBase<TStatusCode> : Exception
    {
        public virtual TStatusCode HttpStatusCode { get; }

        public (TStatusCode StatusCode, string Message) Info => (this.HttpStatusCode, this.Message);

        protected PairMessageStatusCodeExceptionBase(string message, TStatusCode statusCode)
            : base(message) => this.HttpStatusCode = statusCode;

        protected PairMessageStatusCodeExceptionBase() { }

        protected PairMessageStatusCodeExceptionBase(string message)
            : base(message) { }

        protected PairMessageStatusCodeExceptionBase(string message, Exception inner)
            : base(message, inner) { }

        protected PairMessageStatusCodeExceptionBase(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
