using System;
using System.Runtime.Serialization;
using Mohammad.Validation.Exceptions;
using Mohammad.Win.Properties;

namespace Mohammad.Win.Exceptions
{
    [Serializable]
    public class PasswordAndConfirmNotMatchException : ValidationExceptionBase
    {
        public PasswordAndConfirmNotMatchException()
            : base(Resources.PasswordAnConfirmNotMatch) { }

        public PasswordAndConfirmNotMatchException(string message)
            : base(message) { }

        public PasswordAndConfirmNotMatchException(string message, Exception inner)
            : base(message, inner) { }

        protected PasswordAndConfirmNotMatchException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}