#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:06 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Windows.Forms;
using Library40.EventsArgs;
using Library40.Helpers;
using Library40.Win.Dialogs;

namespace Library40.Win.Helpers
{
	public static class ApplicationHelper
	{
		public static event EventHandler<ExceptionOccurredEventArgs> ExceptionOccurred;

		public static void CatchExceptions(Action<object, Exception> handler = null)
		{
			if (handler == null)
				handler = DefualtExecptionHandler;
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			Application.ThreadException += (sender, e) =>
			                               {
				                               var exception = e.Exception;
				                               handler(sender, exception);
			                               };
			AppDomain.CurrentDomain.UnhandledException += (sender, e) => handler(sender, e.ExceptionObject.As<Exception>());
		}

		private static void DefualtExecptionHandler(object sender, Exception e)
		{
			ExceptionOccurred.Raise(sender, new ExceptionOccurredEventArgs(e));
			if (e.InnerException == null)
				MsgBoxEx.Error(text: e.Message);
			else
				MsgBoxEx.Error(text: e.Message, detailsExpandedText: e.GetBaseException().Message);
		}
	}
}