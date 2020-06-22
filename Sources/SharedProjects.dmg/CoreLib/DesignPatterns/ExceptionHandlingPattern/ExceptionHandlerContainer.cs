using System;
using Mohammad.EventsArgs;

namespace Mohammad.DesignPatterns.ExceptionHandlingPattern
{
    public class ExceptionHandlerContainer : IExceptionHandlerContainer
    {
        private ExceptionHandling _ExceptionHandling;

        protected virtual ExceptionHandling OnExceptionHandlingRequired()
        {
            var result = new ExceptionHandling();
            result.ExceptionOccurred += this.OnExceptionOccurred;
            return result;
        }

        protected virtual void OnExceptionOccurred(object sender, ExceptionOccurredEventArgs<Exception> e) { }

        public virtual ExceptionHandling ExceptionHandling
        {
            get { return this._ExceptionHandling ?? (this._ExceptionHandling = this.OnExceptionHandlingRequired()); }
            protected set { this._ExceptionHandling = value; }
        }
    }
}