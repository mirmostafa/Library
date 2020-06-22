using System;
using System.Collections;
using Mohammad.Helpers;

namespace Mohammad.Threading
{
    [Obsolete("Use async/await instead.", true)]
    internal class AsyncImp : Async
    {
        private readonly IEnumerable _Args;
        private readonly Delegate _MethodInfo;

        internal AsyncImp(Delegate method, AsyncPool pool, IEnumerable args)
        {
            this._MethodInfo = method;
            this._Args = args;
            this.Pool = pool;
        }

        protected override void Execute()
        {
            //this._MethodInfo.Execute(this._Args);
            this._MethodInfo.DynamicInvoke(this._Args != null && this._Args.Count() > 0 ? EnumerableHelper.ToArray(this._Args) : null);
        }
    }
}