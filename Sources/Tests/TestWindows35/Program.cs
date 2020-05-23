#region File Notice
// Created at: 2013/12/24 3:46 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Windows.Forms;
using Library35.EventsArgs;
using Library35.Windows.Forms;
using Library35.Windows.Helpers;

namespace TestWindows35
{
	internal static class Program
	{
		/// <summary>
		///     The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			WindowsApplicationHelper.PrepareApplication("fa-IR", OnExceptionOccurred);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}

		private static void OnExceptionOccurred(object sender, ExceptionOccurredEventArgs<Exception> e)
		{
			MsgBoxEx.Error(e.Exception.Message);
		}
	}
}