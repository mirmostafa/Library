#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Configuration;
using System.Threading;
using Library35.EventsArgs;
using Library35.ExceptionHandlingPattern;
using Library35.ExceptionHandlingPattern.Handlers;
using Library35.Helpers;
using Library35.LogSystem.Configuration;
using Library35.LogSystem.Configurations;
using Library35.LogSystem.Entities;
using Library35.LogSystem.Interfaces;
using Library35.LogSystem.Internals;

namespace Library35.LogSystem
{
	/// <summary>
	///     Facade for writing a log entry.
	/// </summary>
	public class Logger<TWriter, TLogEntity> : LoggerDefaults, ILogger<TLogEntity>, ILogger, IExceptionHandlerContainer<Exception>
		where TWriter : class, IWriter<TLogEntity>
		where TLogEntity : LogEntity, new()
	{
		private const int _DefaultTimeout = 2500;

		private static int _ReaderLockAcquireTimeout = _DefaultTimeout;

		private static int _WriterLockAcquireTimeout = _DefaultTimeout;

		private readonly LazyInitNew<ExceptionHandling<Exception>> _ExceptionHandling = new LazyInitNew<ExceptionHandling<Exception>>();

		private readonly ReaderWriterLock _StructureHolderLock = new ReaderWriterLock();

		public Logger(TWriter writer, bool raiseEventOnly)
			: this()
		{
			this.Writer = writer;
			this.RaiseEventOnly = raiseEventOnly;
			if (LoadLastLogOnInitialize)
				this.Writer.LoadLastLog();
		}

		public Logger(TWriter writer)
			: this()
		{
			this.Writer = writer;
			if (LoadLastLogOnInitialize)
				this.Writer.LoadLastLog();
		}

		protected Logger()
		{
			this.ReadConfiguration();
		}

		public static bool LoadLastLogOnInitialize { get; set; }

		public TWriter Writer { get; protected set; }
		public static int WriterLockAcquireTimeout
		{
			get { return _WriterLockAcquireTimeout; }
		}

		#region IExceptionHandlerContainer<Exception> Members
		public ExceptionHandling<Exception> ExceptionHandling
		{
			get { return this._ExceptionHandling; }
			set { this._ExceptionHandling.Value = value; }
		}
		#endregion

		#region ILogger<TLogEntity> Members
		public event EventHandler<ItemActingEventArgs<TLogEntity>> Logging;

		public event EventHandler<ItemActedEventArgs<TLogEntity>> Logged;

		public bool DebugMode { get; set; }

		public LoggingConfigurationHandler Config { get; private set; }

		public bool RaiseEventOnly { get; set; }

		public bool ShowInDebuggerTracer
		{
			get { return this.Writer.ShowInDebuggerTracer; }
			set { this.Writer.ShowInDebuggerTracer = value; }
		}

		public void Debug(object text)
		{
			if (!this.DebugMode)
				return;
			var logEntity = text is TLogEntity
				? text as TLogEntity
				: new TLogEntity
				  {
					  Text = text
				  };
			logEntity.Level = LogLevel.Debug;
			this.Log(logEntity, this.RaiseEventOnly);
		}

		public void Info(object text)
		{
			var logEntity = text is TLogEntity
				? text as TLogEntity
				: new TLogEntity
				  {
					  Text = text
				  };
			logEntity.Level = LogLevel.Info;
			this.Log(logEntity, this.RaiseEventOnly);
		}

		public void Info(object text, bool raiseEventOnly)
		{
			var logEntity = text is TLogEntity
				? text as TLogEntity
				: new TLogEntity
				  {
					  Text = text
				  };
			logEntity.Level = LogLevel.Info;
			this.Log(logEntity, raiseEventOnly);
		}

		public void Warn(object text)
		{
			var logEntity = text is TLogEntity
				? text as TLogEntity
				: new TLogEntity
				  {
					  Text = text
				  };
			logEntity.Level = LogLevel.Warn;
			this.Log(logEntity, this.RaiseEventOnly);
		}

		public void Warn(object text, bool raiseEventOnly)
		{
			var logEntity = text is TLogEntity
				? text as TLogEntity
				: new TLogEntity
				  {
					  Text = text
				  };
			logEntity.Level = LogLevel.Warn;
			this.Log(logEntity, raiseEventOnly);
		}

		public void Error(Exception ex)
		{
			this.Log(new TLogEntity
			         {
				         Exception = ex,
				         Level = LogLevel.Error
			         },
				this.RaiseEventOnly);
		}

		public void Error(object text)
		{
			var logEntity = text is TLogEntity
				? text as TLogEntity
				: new TLogEntity
				  {
					  Text = text
				  };
			logEntity.Level = LogLevel.Error;
			this.Log(logEntity, this.RaiseEventOnly);
		}

		public void Error(object text, bool raiseEventOnly)
		{
			var logEntity = text is TLogEntity
				? text as TLogEntity
				: new TLogEntity
				  {
					  Text = text
				  };
			logEntity.Level = LogLevel.Error;
			this.Log(logEntity, raiseEventOnly);
		}

		public void Error(object text, Exception ex)
		{
			var logEntity = text is TLogEntity
				? text as TLogEntity
				: new TLogEntity
				  {
					  Text = text,
				  };
			logEntity.Level = LogLevel.Error;
			logEntity.Exception = ex;
			this.Log(logEntity, this.RaiseEventOnly);
		}

		public void Error(object text, Exception ex, bool raiseEventOnly)
		{
			var logEntity = text is TLogEntity
				? text as TLogEntity
				: new TLogEntity
				  {
					  Text = text,
				  };
			logEntity.Level = LogLevel.Error;
			logEntity.Exception = ex;
			this.Log(logEntity, raiseEventOnly);
		}

		public void Fatal(object text, Exception ex)
		{
			var logEntity = text is TLogEntity
				? text as TLogEntity
				: new TLogEntity
				  {
					  Text = text,
				  };
			logEntity.Level = LogLevel.Fatal;
			logEntity.Exception = ex;
			this.Log(logEntity, this.RaiseEventOnly);
		}

		public void Fatal(object text)
		{
			var logEntity = text is TLogEntity
				? text as TLogEntity
				: new TLogEntity
				  {
					  Text = text,
				  };
			logEntity.Level = LogLevel.Fatal;
			this.Log(logEntity, this.RaiseEventOnly);
		}

		public void Fatal(object text, Exception ex, bool raiseEventOnly)
		{
			var logEntity = text is TLogEntity
				? text as TLogEntity
				: new TLogEntity
				  {
					  Text = text,
				  };
			logEntity.Level = LogLevel.Fatal;
			logEntity.Exception = ex;
			this.Log(logEntity, raiseEventOnly);
		}

		public void Fatal(object text, bool raiseEventOnly)
		{
			var logEntity = text is TLogEntity
				? text as TLogEntity
				: new TLogEntity
				  {
					  Text = text,
				  };
			logEntity.Level = LogLevel.Fatal;
			this.Log(logEntity, raiseEventOnly);
		}
		#endregion

		private void ReadConfiguration()
		{
			try
			{
				this.Config = (LoggingConfigurationHandler)ConfigurationManager.GetSection("library.LogSystem/loggingConfigurationHandler");
			}
			catch
			{
			}
			if (this.Config == null)
				this.Config = new LoggingConfigurationHandler();
		}

		protected virtual void Log(TLogEntity logEntity, bool raiseEventOnly)
		{
			MethodHelper.Catch(() =>
			                   {
				                   this._StructureHolderLock.AcquireReaderLock(_ReaderLockAcquireTimeout);

				                   switch (logEntity.Level)
				                   {
					                   case LogLevel.Info:
					                   case LogLevel.Warn:
						                   if (this.Config.Severity != LoggingSeverity.High)
							                   return;
						                   break;
					                   case LogLevel.Error:
					                   case LogLevel.Fatal:
						                   if (this.Config.Severity == LoggingSeverity.Never)
							                   return;
						                   break;
				                   }

				                   var loggingEventArgs = new ItemActingEventArgs<TLogEntity>(logEntity);
				                   var loggedEventArgs = new ItemActedEventArgs<TLogEntity>(logEntity);

				                   this.OnLogging(loggingEventArgs);
				                   if (loggingEventArgs.Handled)
					                   return;
				                   if (!raiseEventOnly)
					                   this.Writer.Write(logEntity);
				                   this.OnLogged(loggedEventArgs);
			                   },
				ex => this.ExceptionHandling.HandleException(ex),
				() => this._StructureHolderLock.ReleaseReaderLock());
		}

		protected virtual void OnLogged(ItemActedEventArgs<TLogEntity> e)
		{
			this.Logged.Raise(this, e);
		}

		protected virtual void OnLogging(ItemActingEventArgs<TLogEntity> e)
		{
			this.Logging.Raise(this, e);
		}

		internal static void SetLockTimeouts(int? readerTimeout, int? writerTimeout)
		{
			_ReaderLockAcquireTimeout = readerTimeout ?? _DefaultTimeout;
			_WriterLockAcquireTimeout = writerTimeout ?? _DefaultTimeout;
		}

		internal static void ResetLockTimeouts()
		{
			_ReaderLockAcquireTimeout = _DefaultTimeout;
			_WriterLockAcquireTimeout = _DefaultTimeout;
		}

		public override string ToString()
		{
			return this.Writer == null ? base.ToString() : this.Writer.ToString();
		}
	}
}