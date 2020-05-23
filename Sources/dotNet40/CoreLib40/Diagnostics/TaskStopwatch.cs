#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Diagnostics;

namespace Library40.Diagnostics
{
	public class TaskStopwatch
	{
		public static TimeSpan Check(Action action)
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			action();
			stopwatch.Stop();
			return stopwatch.Elapsed;
		}
	}
}