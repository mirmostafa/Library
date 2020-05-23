#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using Library35.LogSystem.Entities;

namespace Library35.LogSystem.Internals
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract class Writer<TLog, TLogEntity> : IWriter<TLogEntity>
		where TLogEntity : LogEntity
	{
		private const int _DefaultTimeout = 2500;

		private static int _ReaderLockAcquireTimeout = _DefaultTimeout;

		private readonly ReaderWriterLock _StructureHolderLock = new ReaderWriterLock();

		private bool _ShowInDebuggerTracer = true;

		protected Writer(TLog log)
		{
			this.Log = log;
		}

		public virtual TLog Log { get; protected set; }

		#region IWriter<TLogEntity> Members
		public bool ShowInDebuggerTracer
		{
			get { return this._ShowInDebuggerTracer; }
			set { this._ShowInDebuggerTracer = value; }
		}

		public void Write(TLogEntity logEntity)
		{
			this._StructureHolderLock.AcquireReaderLock(_ReaderLockAcquireTimeout);
			try
			{
				if (this.ShowInDebuggerTracer)
					Trace.WriteLine(logEntity);
				this.InnerWrite(logEntity);
			}
			finally
			{
				this._StructureHolderLock.ReleaseReaderLock();
			}
		}

		public abstract void LoadLastLog();
		#endregion

		protected static void SetLockTimeouts(int readerTimeout, int writerTimeout)
		{
			_ReaderLockAcquireTimeout = readerTimeout;
		}

		protected static void ResetLockTimeouts()
		{
			_ReaderLockAcquireTimeout = _DefaultTimeout;
		}

		protected abstract void InnerWrite(TLogEntity logEntity);
	}
}