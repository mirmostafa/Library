#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Linq;
using Library35.Collections.ObjectModel;
using Library35.Helpers;

namespace Library35.Threading
{
	public class AsyncCollection : EventualCollection<Async>
	{
		public Async this[string name]
		{
			get { return (from async1 in this.Items where async1.Name.Equals(name) select async1).FirstOrDefault(); }
		}

		public void AbortAll()
		{
			var asycns = this.Items.ToList();
			foreach (var async1 in asycns.Where(async1 => async1.Status != AsyncStatus.Ended))
				MethodHelper.CatchByExceptionHandling(async1.Abort, async1.ExceptionHandling);
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
	}
}