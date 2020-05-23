#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:06 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using Library40.EventsArgs;
using Library40.LogSystem.Configuration;
using Library40.LogSystem.Entities;

namespace Library40.LogSystem.Interfaces
{
	public interface ILogger<TLogEntity>
		where TLogEntity : LogEntity
	{
		LoggingConfigurationHandler Config { get; }

		bool DebugMode { get; set; }

		bool RaiseEventOnly { get; set; }

		bool ShowInDebuggerTracer { get; set; }

		event EventHandler<ItemActingEventArgs<TLogEntity>> Logging;

		event EventHandler<ItemActedEventArgs<TLogEntity>> Logged;

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