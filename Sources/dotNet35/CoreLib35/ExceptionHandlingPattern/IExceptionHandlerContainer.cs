#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library35.ExceptionHandlingPattern
{
	public interface IExceptionHandlerContainer<TException>
		where TException : Exception
	{
		ExceptionHandling<TException> ExceptionHandling { get; }

		//ExceptionHandling
	}

	public interface IExceptionHandlerContainer
	{
		ExceptionHandling ExceptionHandling { get; }
	}
}