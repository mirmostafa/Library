#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace Library35.Threading
{
	public class PriodicAsyncImp : PriodicAsync
	{
		private readonly IEnumerable<Object> _Args;
		private readonly Delegate _MethodInfo;

		internal PriodicAsyncImp(Delegate method, TimeSpan interval, long maxCount)
			: base(interval, maxCount)
		{
			this._MethodInfo = method;
		}

		internal PriodicAsyncImp(Delegate method, TimeSpan interval)
			: base(interval)
		{
			this._MethodInfo = method;
		}

		internal PriodicAsyncImp(Delegate method, IEnumerable<Object> args, TimeSpan interval)
			: base(interval)
		{
			this._MethodInfo = method;
			this._Args = args;
		}

		protected override void Execute()
		{
			this._MethodInfo.DynamicInvoke(this._Args != null ? this._Args.ToArray() : null);
		}
	}
}