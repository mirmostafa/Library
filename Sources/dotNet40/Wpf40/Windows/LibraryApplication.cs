#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:06 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Windows;
using System.Windows.Threading;

namespace Library40.Wpf.Windows
{
	public class LibraryApplication : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			AppDomain.CurrentDomain.UnhandledException += this.CurrentDomainUnhandledException;

			base.OnStartup(e);
		}

		protected virtual void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			//if (e.ExceptionObject is ExceptionBase)
			{
				//var exp = e.ExceptionObject as ExceptionBase;
				this.HandleException(sender, e.ExceptionObject as Exception);
			}
		}

		protected virtual void HandleException(object sender, Exception exp)
		{
			//Current.MainWindow.ShowError(instructionText: exp.Message, detailsExpandedText: exp.GetBaseException().Message);
		}

		protected virtual void ApplicationDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			//if (e.Exception is ExceptionBase)
			{
				//var exp = e.Exception as ExceptionBase;
				this.HandleException(sender, e.Exception);
				e.Handled = true;
			}
		}
	}
}