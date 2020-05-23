#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;
using System.Runtime.Serialization;

namespace Mohammad.Validation.Exceptions
{
    [Serializable]
    public class NotNumberValidationException : ValidationExceptionBase
    {
        public NotNumberValidationException()
        {
        }

        protected NotNumberValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public NotNumberValidationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public NotNumberValidationException(string parameterName)
            : this() => this.ParameterName = parameterName;

        public NotNumberValidationException(string message, string instruction)
            : base(message, instruction)
        {
        }

        public NotNumberValidationException(string message, Exception inner, string instruction)
            : base(message, inner, instruction)
        {
        }

        public string ParameterName { get; set; }
    }
}