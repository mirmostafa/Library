using System;

namespace Mohammad.EventsArgs
{
    public sealed class ExceptionOccurredEventArgs<TException> : EventArgs
        where TException : Exception
    {
        public ExceptionOccurredEventArgs(TException exception) => this.Exception = exception;

        public ExceptionOccurredEventArgs(TException exception, string moreInfo)
        {
            this.Exception = exception;
            this.MoreInfo = moreInfo;
        }

        public bool Handled { get; set; }
        public TException Exception { get; }
        public string MoreInfo { get; }
    }

    public sealed class ExceptionOccurredEventArgs : EventArgs
    {
        public ExceptionOccurredEventArgs(Exception exception) => this.Exception = exception;

        public ExceptionOccurredEventArgs(Exception exception, string moreInfo)
        {
            this.Exception = exception;
            this.MoreInfo = moreInfo;
        }

        public Exception Exception { get; }

        public string MoreInfo { get; }
    }
}