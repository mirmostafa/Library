#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library40.Helpers
{
	public static class ExceptionHelper
	{
		public static void ThrowException<TException>(Action action, Action<Exception> catchAction = null, Action finallyAction = null) where TException : Exception
		{
			if (action == null)
				throw new ArgumentNullException("action");
			CatchException(action,
				delegate(Exception ex)
				{
					if (catchAction != null)
						catchAction(ex);
					var exception = ObjectHelper.CreateInstance<TException>(new[]
					                                                        {
						                                                        typeof (Exception)
					                                                        },
						new object[]
						{
							ex
						});
					throw exception;
				},
				finallyAction);
		}

		public static Exception CatchException(Action action, Action<Exception> catchAction = null, Action finallyAction = null)
		{
			if (action == null)
				throw new ArgumentNullException("action");
			Exception result = null;
			try
			{
				action();
			}
			catch (Exception exception)
			{
				if (catchAction != null)
					catchAction(exception);
				result = exception;
			}
			finally
			{
				if (finallyAction != null)
					finallyAction();
			}
			return result;
		}

		internal static void ThrowException(Action action, Action<Exception> catchAction = null, Action finallyAction = null)
		{
			ThrowException<Exception>(action, catchAction, finallyAction);
		}
	}
}