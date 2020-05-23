#region Code Identifications

// Created on     2018/03/05
// Last update on 2018/03/05 by Mohammad Mir mostafa 

#endregion

using System.Net;

namespace Mohammad.Web.Api.Exceptions
{
    public sealed class InternalServerErrorException : ApiExceptionBase
    {
        public InternalServerErrorException(string message)
            : base(message, HttpStatusCode.InternalServerError)
        {
        }

        public InternalServerErrorException()
            : base(string.Empty, HttpStatusCode.InternalServerError)
        {
        }
    }

    public sealed class NotFoundException : ApiExceptionBase
    {
        public NotFoundException(string message) : base(message, HttpStatusCode.NotFound)
        {
        }

        public NotFoundException()
            : base(string.Empty, HttpStatusCode.NotFound)
        {
        }
    }

    public sealed class DuplicateException : ApiExceptionBase
    {
        public DuplicateException(string message) : base(message, HttpStatusCode.Ambiguous)
        {
        }

        public DuplicateException()
            : base(string.Empty, HttpStatusCode.Ambiguous)
        {
        }
    }

    public sealed class OutOfRangeException : ApiExceptionBase
    {
        public OutOfRangeException(string message) : base(message, HttpStatusCode.ExpectationFailed)
        {
        }

        public OutOfRangeException()
            : base(string.Empty, HttpStatusCode.Ambiguous)
        {
        }
    }
}