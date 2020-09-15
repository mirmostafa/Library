using System;

namespace Mohammad.DesignPatterns.ExceptionHandlingPattern
{
    public interface IExceptionHandlerContainer<TException>
        where TException : Exception
    {
        ExceptionHandling<TException> ExceptionHandling { get; }
    }

    public interface IExceptionHandlerContainer
    {
        ExceptionHandling ExceptionHandling { get; }
    }
}