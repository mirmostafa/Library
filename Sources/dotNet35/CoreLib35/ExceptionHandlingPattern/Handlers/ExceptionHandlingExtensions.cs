#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library35.ExceptionHandlingPattern.Handlers
{
	public static class ExceptionHandlingExtensions
	{
		public static void HandleException<TException>(this ExceptionHandling<TException> exceptionHandling, TException ex) where TException : Exception
		{
			exceptionHandling.HandleException(ex);
		}

		public static void HandleException<TException>(this ExceptionHandling<TException> exceptionHandling, object sender, TException ex) where TException : Exception
		{
			exceptionHandling.HandleException(sender, ex);
		}

		public static void Reset<TException>(this ExceptionHandling<TException> exceptionHandling) where TException : Exception
		{
			exceptionHandling.Reset();
		}

		public static void SetSender<TException>(ExceptionHandling<TException> exceptionHandling, object sender) where TException : Exception
		{
			exceptionHandling.SetSender(sender);
		}
	}
}