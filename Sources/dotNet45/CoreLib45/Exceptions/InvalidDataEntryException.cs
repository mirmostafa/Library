#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;
using System.Runtime.Serialization;

namespace Mohammad.Exceptions
{
    [Serializable]
    public class InvalidDataEntryException : CommonExceptionBase
    {
        public InvalidDataEntryException()
        {
        }

        public InvalidDataEntryException(string message)
            : base(message)
        {
        }

        public InvalidDataEntryException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected InvalidDataEntryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public InvalidDataEntryException(string message, string instruction)
            : base(message, instruction)
        {
        }

        public InvalidDataEntryException(string message, Exception inner, string instruction)
            : base(message, inner, instruction)
        {
        }
    }

    [Serializable]
    public abstract class CommonExceptionBase : LibraryExceptionBase
    {
        protected CommonExceptionBase()
        {
        }

        protected CommonExceptionBase(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        protected CommonExceptionBase(string message)
            : base(message)
        {
        }

        protected CommonExceptionBase(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected CommonExceptionBase(string message, string instruction)
            : base(message, instruction)
        {
        }

        protected CommonExceptionBase(string message, Exception inner, string instruction)
            : base(message, inner, instruction)
        {
        }
    }
}