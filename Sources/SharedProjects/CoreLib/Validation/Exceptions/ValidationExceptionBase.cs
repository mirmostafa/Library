using System;
using System.Runtime.Serialization;
using Mohammad.Exceptions;

namespace Mohammad.Validation.Exceptions
{
    public interface IValidationException {}

    [Serializable]
    public abstract class ValidationExceptionBase : LibraryExceptionBase, IValidationException
    {
        protected ValidationExceptionBase() { }

        protected ValidationExceptionBase(string message)
            : base(message) { }

        protected ValidationExceptionBase(string message, Exception inner)
            : base(message, inner) { }

        protected ValidationExceptionBase(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        protected ValidationExceptionBase(string message, string instruction)
            : base(message, instruction) { }

        protected ValidationExceptionBase(string message, Exception inner, string instruction)
            : base(message, inner, instruction) { }
    }

    public sealed class ValidationException : ValidationExceptionBase
    {
        public ValidationException() { }

        public ValidationException(string message)
            : base(message) { }

        public ValidationException(string message, Exception inner)
            : base(message, inner) { }

        public ValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public ValidationException(string message, string instruction)
            : base(message, instruction) { }

        public ValidationException(string message, Exception inner, string instruction)
            : base(message, inner, instruction) { }

        public static void WrapThrow(string message) { WrapThrow<ValidationException>(message); }

        public static void WrapThrow(string message, string instruction) { WrapThrow<ValidationException>(message, instruction); }
    }
}