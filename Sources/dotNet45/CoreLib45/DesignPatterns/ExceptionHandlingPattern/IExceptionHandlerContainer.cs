#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

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