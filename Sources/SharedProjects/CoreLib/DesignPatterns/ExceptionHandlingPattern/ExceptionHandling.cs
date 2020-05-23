using System;
using System.Diagnostics;
using Mohammad.EventsArgs;
using Mohammad.Helpers;

namespace Mohammad.DesignPatterns.ExceptionHandlingPattern
{
    public class ExceptionHandling<TException>
        where TException : Exception
    {
        public TException LastException { get; private set; }
        public bool HasException => this.LastException != null;
        public bool RaiseExceptions { get; set; }
        private object Sender { get; set; }

        public ExceptionHandling(EventHandler<ExceptionOccurredEventArgs<TException>> exceptionOccurredHandler)
            : this() { this.ExceptionOccurred += exceptionOccurredHandler; }

        public ExceptionHandling() { }

        public event EventHandler<ExceptionOccurredEventArgs<TException>> ExceptionOccurred;
        internal void HandleException(TException ex) { this.HandleException(this.Sender, ex); }

        internal void HandleException(object sender, TException ex)
        {
            this.LastException = ex;
            this.ExceptionOccurred.Raise(sender, new ExceptionOccurredEventArgs<TException>(ex));
            if (this.RaiseExceptions)
                throw ex;
            //CodeHelper.Break();
        }

        internal void HandleException(object sender, TException ex, string moreInfo)
        {
            Debug.WriteLine($"{moreInfo} [{ex.GetBaseException().Message}]");
            this.LastException = ex;
            this.ExceptionOccurred.Raise(sender, new ExceptionOccurredEventArgs<TException>(ex, moreInfo));
            if (this.RaiseExceptions)
                throw ex;
            //CodeHelper.Break();
        }

        internal void Reset() { this.LastException = null; }
        internal void SetSender(object sender) { this.Sender = sender; }
        public override string ToString() { return this.LastException?.ToString() ?? "No error"; }
    }

    public class ExceptionHandling : ExceptionHandling<Exception>
    {
        public ExceptionHandling(EventHandler<ExceptionOccurredEventArgs<Exception>> exceptionOccurredHandler)
            : base(exceptionOccurredHandler) { }

        public ExceptionHandling() { }
    }

    //public class LazyExceptionHandling<TException> : LazyInitNew<ExceptionHandling<TException>>
    //    where TException : Exception
    //{
    //    protected LazyExceptionHandling(Func<ExceptionHandling<TException>> creator)
    //        : base(creator) { }

    //    protected LazyExceptionHandling(Func<ExceptionHandling<TException>> creator, LazyInitMode mode)
    //        : base(creator, mode) { }

    //    public LazyExceptionHandling(LazyInitMode mode = LazyInitMode.FirstGet)
    //        : base(mode) { }
    //}

    //public class LazyExceptionHandling : LazyInitNew<ExceptionHandling>
    //{
    //    protected LazyExceptionHandling(Func<ExceptionHandling> creator)
    //        : base(creator) { }

    //    protected LazyExceptionHandling(Func<ExceptionHandling> creator, LazyInitMode mode)
    //        : base(creator, mode) { }

    //    public LazyExceptionHandling(LazyInitMode mode = LazyInitMode.FirstGet)
    //        : base(mode) { }
    //}
}