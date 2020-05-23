using System;
using System.Runtime.Serialization;
using Mohammad.Exceptions;

namespace Mohammad.CodeGeneration.Exceptions
{
    [Serializable]
    public class UnsupportedSqlObjectException : CodeGenerationExceptionBase
    {
        public UnsupportedSqlObjectException()
        {
        }

        public UnsupportedSqlObjectException(string message)
            : base(message)
        {
        }

        public UnsupportedSqlObjectException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected UnsupportedSqlObjectException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    [Serializable]
    public class CodeGenerationException : CodeGenerationExceptionBase
    {
        public CodeGenerationException()
        {
        }

        public CodeGenerationException(string message)
            : base(message)
        {
        }

        public CodeGenerationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected CodeGenerationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    [Serializable]
    public class CodeGenerationExceptionBase : LibraryExceptionBase
    {
        public CodeGenerationExceptionBase()
        {
        }

        public CodeGenerationExceptionBase(string message)
            : base(message)
        {
        }

        public CodeGenerationExceptionBase(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected CodeGenerationExceptionBase(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}