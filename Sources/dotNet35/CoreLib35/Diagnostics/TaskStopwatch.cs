#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Diagnostics;

namespace Library35.Diagnostics
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