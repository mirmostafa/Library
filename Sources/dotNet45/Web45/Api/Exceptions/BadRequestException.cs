// Created on     2018/03/05
// Last update on 2018/03/05 by Mohammad Mir mostafa 

using System.Net;

namespace Mohammad.Web.Api.Exceptions
{
    public class BadRequestException : ApiExceptionBase
    {
        public BadRequestException(string message)
            : base(message, HttpStatusCode.BadRequest)
        {
        }

        public BadRequestException()
            : base(string.Empty, HttpStatusCode.BadRequest)
        {
        }
    }
}