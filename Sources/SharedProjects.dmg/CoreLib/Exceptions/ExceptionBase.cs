using System;
using System.Runtime.Serialization;
using Mohammad.Helpers;

namespace Mohammad.Exceptions
{
    public interface IException
    {
        string Instruction { get; }
        string Message { get; }
        object Owner { get; set; }
        Exception GetBaseException();
    }

    [Serializable]
    public abstract class ExceptionBase : Exception, IException
    {
        protected ExceptionBase() { }

        protected ExceptionBase(SerializationInfo info, StreamingContext context)
            : base(info, context) {}

        protected ExceptionBase(string message)
            : base(message) {}

        protected ExceptionBase(string message, Exception inner)
            : base(message, inner) {}

        protected ExceptionBase(string message, string instruction)
            : this(message)
        {
            this.Instruction = instruction;
        }

        protected ExceptionBase(string message, Exception inner, string instruction)
            : this(message, inner)
        {
            this.Instruction = instruction;
        }

        public static void WrapThrow<TException>() where TException : ExceptionBase { WrapThrow<TException>(null, null, (Exception) null); }

        public static void WrapThrow<TException>(string message) where TException : ExceptionBase { WrapThrow<TException>(message, null, (IException) null); }

        public static void WrapThrow<TException>(string message, string instruction) where TException : ExceptionBase
        {
            WrapThrow<TException>(message, instruction, (IException) null);
        }

        public static void WrapThrow<TException>(string message, object owner) where TException : ExceptionBase
        {
            WrapThrow<TException>(message, null, null, owner);
        }

        public static void WrapThrow<TException>(string message, string instruction, object owner) where TException : ExceptionBase
        {
            WrapThrow<TException>(message, instruction, null, owner);
        }

        public static void WrapThrow<TException>(string message, string instruction, IException ex) where TException : ExceptionBase
        {
            WrapThrow<TException>(message, instruction, ex, null);
        }

        public static void WrapThrow<TException>(string message, string instruction, IException ex, object owner) where TException : ExceptionBase
        {
            var me = ObjectHelper.CreateInstance<TException>(new[] {typeof(string), typeof(Exception), typeof(string)}, new object[] {message, ex, instruction});
            me.Owner = owner;
            throw me;
        }

        public static void WrapThrow<TException>(string message, string instruction, Exception ex) where TException : ExceptionBase
        {
            var me = ObjectHelper.CreateInstance<TException>(new[] {typeof(string), typeof(Exception), typeof(string)}, new object[] {message, ex, instruction});
            throw me;
        }

        public string Instruction { get; set; }
        public object Owner { get; set; }
    }
}