using System;
using System.Runtime.Serialization;

namespace Mohammad.Exceptions
{
    public interface ILibraryException : IException {}

    /// <summary>
    /// </summary>
    [Serializable]
    public abstract class LibraryExceptionBase : ExceptionBase, ILibraryException
    {
        /// <summary>
        /// </summary>
        protected LibraryExceptionBase() { }

        protected LibraryExceptionBase(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        protected LibraryExceptionBase(string message)
            : base(message) { }

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        protected LibraryExceptionBase(string message, Exception inner)
            : base(message, inner) { }

        protected LibraryExceptionBase(string message, string instruction)
            : base(message, instruction) { }

        protected LibraryExceptionBase(string message, Exception inner, string instruction)
            : base(message, inner, instruction) { }
    }

    public sealed class LibraryException : LibraryExceptionBase
    {
        public LibraryException() { }

        public LibraryException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public LibraryException(string message)
            : base(message) { }

        public LibraryException(string message, Exception inner)
            : base(message, inner) { }

        public LibraryException(string message, string instruction)
            : base(message, instruction) { }

        public LibraryException(string message, Exception inner, string instruction)
            : base(message, inner, instruction) { }

        public static void WrapThrow(string message) { WrapThrow<LibraryException>(message); }
    }
}