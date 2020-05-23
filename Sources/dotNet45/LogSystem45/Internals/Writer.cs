using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using Mohammad.Logging.Entities;

namespace Mohammad.Logging.Internals
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class Writer<TLog, TLogEntity> : IWriter<TLogEntity>
        where TLogEntity : LogEntity
    {
        private readonly ReaderWriterLock _StructureHolderLock = new ReaderWriterLock();
        private const int _DefaultTimeout = 2500;
        private static int _ReaderLockAcquireTimeout = _DefaultTimeout;
        public virtual TLog Log { get; protected set; }
        protected Writer(TLog log) { this.Log = log; }
        protected static void SetLockTimeouts(int readerTimeout, int writerTimeout) { _ReaderLockAcquireTimeout = readerTimeout; }
        protected static void ResetLockTimeouts() { _ReaderLockAcquireTimeout = _DefaultTimeout; }
        protected abstract void InnerWrite(TLogEntity logEntity);

        #region IWriter<TLogEntity> Members

        public bool ShowInDebuggerTracer { get; set; } = true;

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
    }
}