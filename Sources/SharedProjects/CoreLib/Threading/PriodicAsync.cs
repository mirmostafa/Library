using System;
using System.Threading;
using Mohammad.EventsArgs;
using Mohammad.Helpers;

namespace Mohammad.Threading
{
    [Obsolete("Use async/await instead.")]
    public abstract class PriodicAsync : Async
    {
        private AsyncPool _PriodPool;
        public long? MaxCount { get; set; }
        public long Count { get; protected set; }
        public virtual TimeSpan Interval { get; set; }
        public bool ContinueOnError { get; set; }
        public AsyncPool PriodPool { get { return this._PriodPool ?? (this._PriodPool = this.Pool); } set { this._PriodPool = value; } }
        protected PriodicAsync(TimeSpan interval) { this.Interval = interval; }

        protected PriodicAsync(TimeSpan interval, long maxCount)
        {
            this.Interval = interval;
            this.MaxCount = maxCount;
        }

        public event EventHandler PriodEnded;
        public event EventHandler<ActingEventArgs> PriodStarting;

        protected override void AsyncStart()
        {
            try
            {
                if (this.OnStarting())
                    while (this.Status == AsyncStatus.Running)
                    {
                        try
                        {
                            try
                            {
                                this.Count++;
                                if (this.OnPriodStarting())
                                    this.Execute();
                            }
                            catch (Exception ex)
                            {
                                this.ExceptionHandling.HandleException(ex);
                                if (!this.ContinueOnError)
                                    return;
                            }
                        }
                        finally
                        {
                            this.OnPriodEnded();
                        }
                        if (this.MaxCount.HasValue && this.MaxCount.Value <= this.Count)
                            break;
                        Thread.Sleep(this.Interval);
                    }
            }
            finally
            {
                this.OnEnded();
            }
        }

        protected override void OnEnded()
        {
            this.Status = AsyncStatus.Ended;
            base.OnEnded();
        }

        protected virtual void OnPriodEnded() { this.PriodEnded.RaiseAsync(this); }

        protected virtual bool OnPriodStarting()
        {
            var e = new ActingEventArgs();
            this.PriodStarting.Raise(this, e);
            return !e.Handled;
        }

        protected override bool OnStarting()
        {
            var result = base.OnStarting();
            if (result)
                this.Status = AsyncStatus.Running;
            return result;
        }

        public static PriodicAsync GetPriodicAsyncInstance(Delegate methodInfo, TimeSpan interval, long maxCount)
        {
            var result = new PriodicAsyncImp(methodInfo, interval, maxCount);
            return result;
        }

        public static PriodicAsync GetPriodicAsyncInstance(Delegate methodInfo, TimeSpan interval)
        {
            var result = new PriodicAsyncImp(methodInfo, interval);
            return result;
        }
    }
}