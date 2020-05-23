#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using Library35.LogSystem.Configuration;

namespace Library35.LogSystem.Interfaces
{
	public interface ILogger
	{
		LoggingConfigurationHandler Config { get; }

		bool DebugMode { get; set; }

		bool RaiseEventOnly { get; set; }

		bool ShowInDebuggerTracer { get; set; }

		void Debug(object text);

		void Error(object text, Exception ex);
		void Error(object text, Exception ex, bool raiseEventOnly);
		void Error(Exception ex);
		void Error(object text);
		void Error(object text, bool raiseEventOnly);

		void Fatal(object text, bool raiseEventOnly);
		void Fatal(object text, Exception ex, bool raiseEventOnly);
		void Fatal(object text, Exception ex);
		void Fatal(object text);
		void Info(object text);
		void Info(object text, bool raiseEventOnly);

		void Warn(object text);
		void Warn(object text, bool raiseEventOnly);
	}
}