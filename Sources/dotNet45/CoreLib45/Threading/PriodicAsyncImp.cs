#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace Mohammad.Threading
{
    [Obsolete]
    public class PriodicAsyncImp : PriodicAsync
    {
        private readonly IEnumerable<object> _Args;
        private readonly Delegate            _MethodInfo;

        internal PriodicAsyncImp(Delegate method, TimeSpan interval, long maxCount)
            : base(interval, maxCount) => this._MethodInfo = method;

        internal PriodicAsyncImp(Delegate method, TimeSpan interval)
            : base(interval) => this._MethodInfo = method;

        internal PriodicAsyncImp(Delegate method, IEnumerable<object> args, TimeSpan interval)
            : base(interval)
        {
            this._MethodInfo = method;
            this._Args       = args;
        }

        protected override void Execute()
        {
            this._MethodInfo.DynamicInvoke(this._Args != null ? this._Args.ToArray() : null);
        }
    }
}