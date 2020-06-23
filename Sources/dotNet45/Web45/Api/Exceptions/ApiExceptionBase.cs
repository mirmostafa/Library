// Created on     2017/12/17
// Last update on 2018/03/05 by Mohammad Mirmostafa 

using System;
using System.Net;
using System.Runtime.Serialization;
using Mohammad.Exceptions;

namespace Mohammad.Web.Api.Exceptions
{
    [Serializable]
    public abstract class ApiExceptionBase : PairMessageStatusCodeExceptionBase<HttpStatusCode>
    {
        /// <inheritdoc />
        protected ApiExceptionBase(string message, HttpStatusCode statusCode)
            : base(message, statusCode)
        {
        }

        /// <inheritdoc />
        protected ApiExceptionBase(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <inheritdoc />
        protected ApiExceptionBase(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    public sealed class ApiException : ApiExceptionBase
    {
        public ApiException(string message, HttpStatusCode statusCode)
            : base(message, statusCode)
        {
        }

        public ApiException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public ApiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}