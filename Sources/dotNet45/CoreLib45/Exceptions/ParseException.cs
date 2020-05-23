#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;
using System.Runtime.Serialization;

namespace Mohammad.Exceptions
{
    [Serializable]
    public class ParseException : CommonExceptionBase
    {
        public ParseException()
        {
        }

        public ParseException(string message)
            : base(message)
        {
        }

        public ParseException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ParseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public ParseException(string value, Type type)
            : this($"Cannot convert '{value}' to type '{type}'")
        {
        }

        public ParseException(string message, Exception inner, string instruction)
            : base(message, inner, instruction)
        {
        }

        public ParseException(string message, string instruction)
            : base(message, instruction)
        {
        }
    }
}