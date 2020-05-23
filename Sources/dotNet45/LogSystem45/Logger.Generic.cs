#region Code Identifications

// Created on     2017/07/29
// Last update on 2018/06/17 by Mohammad Mir mostafa 

#endregion

using System;
using System.Configuration;
using System.Threading;
using Mohammad.DesignPatterns.ExceptionHandlingPattern;
using Mohammad.DesignPatterns.ExceptionHandlingPattern.Handlers;
using Mohammad.EventsArgs;
using Mohammad.Helpers;
using Mohammad.Logging.Configuration;
using Mohammad.Logging.Configurations;
using Mohammad.Logging.Entities;
using Mohammad.Logging.Internals;

namespace Mohammad.Logging
{
    public class Logger<TWriter, TLogEntity> : IExceptionHandlerContainer
        where TWriter : class, IWriter<TLogEntity> where TLogEntity : LogEntity, new()
    {
        private const int DEFAULT_TIMEOUT = 2500;
        private static int _ReaderLockAcquireTimeout = DEFAULT_TIMEOUT;
        private readonly ReaderWriterLock _StructureHolderLock = new ReaderWriterLock();
        private ExceptionHandling _ExceptionHandling = new ExceptionHandling();
        public static bool LoadLastLogOnInitialize { get; set; }
        public TWriter Writer { get; protected set; }
        public static int WriterLockAcquireTimeout { get; private set; } = DEFAULT_TIMEOUT;

        public bool DebugMode { get; set; }
        public LoggingConfigurationHandler Config { get; private set; }
        public bool RaiseEventOnly { get; set; }

        public bool ShowInDebuggerTracer
        {
            get => this.Writer.ShowInDebuggerTracer;
            set => this.Writer.ShowInDebuggerTracer = value;
        }

        public ExceptionHandling ExceptionHandling
        {
            get => this._ExceptionHandling ?? (this._ExceptionHandling = new ExceptionHandling());
            set => this._ExceptionHandling = value;
        }

        public Logger(TWriter writer, bool raiseEventOnly = false)
            : this()
        {
            this.Writer = writer;
            this.RaiseEventOnly = raiseEventOnly;
            if (LoadLastLogOnInitialize)
            {
                this.Writer.LoadLastLog();
            }
        }

        protected Logger()
        {
            this.ReadConfiguration();
        }

        //public void CurrentOperationStart(double maxSteps) { this.OnCurrentOperationStarted(new MultiStepStartedLogEventArgs(maxSteps)); }
        //public void CurrentOperationStepIncease(double max, double step, object text = null, LogLevel level = LogLevel.Info)
        //{
        //    this.OnCurrentOperationStepIncreased(new MultiStepLogEventArgs(max, step, text, level));
        //}
        //public void CurrentOperationEnd(bool succeed = true) { this.OnCurrentOperationEnded(new MultiStepEndedLogEventArgs(succeed)); }
        public void Info(object text, object moreInfo = null, bool? raiseEventOnly = null)
        {
            var entity = text as TLogEntity;
            var logEntity = entity ?? new TLogEntity
            {
                Text = text,
                MoreInfo = moreInfo
            };
            logEntity.Level = LogLevel.Info;
            this.Log(logEntity, raiseEventOnly ?? this.RaiseEventOnly);
        }

        public void Warn(object text, object moreInfo = null, bool? raiseEventOnly = null)
        {
            var entity = text as TLogEntity;
            var logEntity = entity ?? new TLogEntity
            {
                Text = text,
                MoreInfo = moreInfo
            };
            logEntity.Level = LogLevel.Warning;
            this.Log(logEntity, raiseEventOnly ?? this.RaiseEventOnly);
        }

        public void Error(object text, object moreInfo = null, Exception ex = null, bool? raiseEventOnly = null)
        {
            this.Log(new TLogEntity
                {
                    Exception = ex,
                    Level = LogLevel.Error,
                    Text = text,
                    MoreInfo = moreInfo
                },
                raiseEventOnly ?? this.RaiseEventOnly);
        }

        public void Fatal(object text, object moreInfo = null, Exception ex = null, bool? raiseEventOnly = null)
        {
            var entity = text as TLogEntity;
            var logEntity = entity ?? new TLogEntity
            {
                Exception = ex,
                Text = text,
                MoreInfo = moreInfo
            };
            logEntity.Level = LogLevel.Fatal;
            logEntity.Exception = ex;
            this.Log(logEntity, raiseEventOnly ?? this.RaiseEventOnly);
        }

        public void Debug(object text, object moreInfo = null, Exception ex = null, bool? raiseEventOnly = null)
        {
            if (!this.DebugMode)
            {
                return;
            }

            var entity = text as TLogEntity;
            var logEntity = entity ?? new TLogEntity
            {
                Exception = ex,
                Text = text,
                MoreInfo = moreInfo
            };
            logEntity.Level = LogLevel.Debug;
            this.Log(logEntity, raiseEventOnly ?? this.RaiseEventOnly);
        }

        public void SetStatus(object text, object moreInfo = null, Exception ex = null, bool? raiseEventOnly = null)
        {
            var entity = text as TLogEntity;
            var logEntity = entity ?? new TLogEntity
            {
                Exception = ex,
                Text = text,
                MoreInfo = moreInfo
            };
            logEntity.Level = LogLevel.Status;
            this.Log(logEntity, raiseEventOnly ?? this.RaiseEventOnly);
        }

        public override string ToString() => this.Writer?.ToString() ?? base.ToString();

        protected virtual void Log(TLogEntity logEntity, bool raiseEventOnly)
        {
            try
            {
                this._StructureHolderLock.AcquireReaderLock(_ReaderLockAcquireTimeout);

                switch (logEntity.Level)
                {
                    case LogLevel.Info:
                    case LogLevel.Warning:
                        if (this.Config.Severity != LoggingSeverity.High)
                        {
                            return;
                        }

                        break;
                    case LogLevel.Error:
                    case LogLevel.Fatal:
                        if (this.Config.Severity == LoggingSeverity.Never)
                        {
                            return;
                        }

                        break;
                }

                var loggingEventArgs = new ItemActingEventArgs<TLogEntity>(logEntity);
                var loggedEventArgs = new ItemActedEventArgs<TLogEntity>(logEntity);

                this.OnLogging(loggingEventArgs);
                if (loggingEventArgs.Handled)
                {
                    return;
                }

                if (!raiseEventOnly)
                {
                    this.Writer.Write(logEntity);
                }

                this.OnLogged(loggedEventArgs);
            }
            catch (Exception ex)
            {
                this.ExceptionHandling.HandleException(ex);
            }
            finally
            {
                this._StructureHolderLock.ReleaseReaderLock();
            }
        }

        //protected virtual void OnCurrentOperationStarted(MultiStepStartedLogEventArgs e)
        //{
        //    var handler = this.CurrentOperationStarted;
        //    if (handler != null)
        //        handler(this, e);
        //}
        //protected virtual void OnCurrentOperationStepIncreased(MultiStepLogEventArgs e)
        //{
        //    var handler = this.CurrentOperationStepIncreased;
        //    if (handler != null)
        //        handler(this, e);
        //}
        //protected virtual void OnCurrentOperationEnded(MultiStepEndedLogEventArgs e)
        //{
        //    var handler = this.CurrentOperationEnded;
        //    if (handler != null)
        //        handler(this, e);
        //}
        protected virtual void OnLogged(ItemActedEventArgs<TLogEntity> e)
        {
            this.Logged.Raise(this, e);
        }

        protected virtual void OnLogging(ItemActingEventArgs<TLogEntity> e)
        {
            this.Logging.Raise(this, e);
        }

        private void ReadConfiguration()
        {
            this.Config = CodeHelper.CatchFunc(
                () => (LoggingConfigurationHandler)ConfigurationManager.GetSection("library.LogSystem/loggingConfigurationHandler"),
                new LoggingConfigurationHandler());
        }

        //public event EventHandler<MultiStepStartedLogEventArgs> CurrentOperationStarted;
        //public event EventHandler<MultiStepLogEventArgs> CurrentOperationStepIncreased;
        //public event EventHandler<MultiStepEndedLogEventArgs> CurrentOperationEnded;
        public event EventHandler<ItemActingEventArgs<TLogEntity>> Logging;

        public event EventHandler<ItemActedEventArgs<TLogEntity>> Logged;

        internal static void SetLockTimeouts(int? readerTimeout, int? writerTimeout)
        {
            _ReaderLockAcquireTimeout = readerTimeout ?? DEFAULT_TIMEOUT;
            WriterLockAcquireTimeout = writerTimeout ?? DEFAULT_TIMEOUT;
        }

        internal static void ResetLockTimeouts()
        {
            _ReaderLockAcquireTimeout = DEFAULT_TIMEOUT;
            WriterLockAcquireTimeout = DEFAULT_TIMEOUT;
        }
    }
}