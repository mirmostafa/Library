#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Mohammad.Threading
{
    [Obsolete("Use async/await instead.")]
    public class AsyncResultAwaiter<TResult> : INotifyCompletion
    {
        private readonly Func<IAsyncResult, TResult> _GetResult;
        private Action _Continuation;
        private TResult _Result;

        public IAsyncResult AsyncResult { get; }
        public bool IsCompleted => this.AsyncResult.IsCompleted;

        public AsyncResultAwaiter(IAsyncResult asyncResult, Func<IAsyncResult, TResult> getResult)
        {
            this.AsyncResult = asyncResult ?? throw new ArgumentNullException(nameof(asyncResult));
            this._GetResult = getResult ?? throw new ArgumentNullException(nameof(getResult));
            ThreadPool.RegisterWaitForSingleObject(asyncResult.AsyncWaitHandle, this.AsyncResultAvailable, null, -1, true);
        }

        public void OnCompleted(Action continuation)
        {
            this._Continuation = continuation ?? throw new ArgumentNullException(nameof(continuation));
        }

        public AsyncResultAwaiter<TResult> GetAwaiter() => this;
        public TResult GetResult() => this._Result;

        private void AsyncResultAvailable(object state, bool timedout)
        {
            this._Result = this._GetResult(this.AsyncResult);
            this._Continuation();
        }
    }
}