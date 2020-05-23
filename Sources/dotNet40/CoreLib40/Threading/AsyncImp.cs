#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections;
using Library40.Helpers;

namespace Library40.Threading
{
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