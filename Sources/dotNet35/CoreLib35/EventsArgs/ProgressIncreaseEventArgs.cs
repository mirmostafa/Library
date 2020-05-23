#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library35.EventsArgs
{
	public sealed class ProgressIncreaseEventArgs : EventArgs
	{
		public ProgressIncreaseEventArgs(int max, int step, string description)
			: this(max, step)
		{
			this.Description = description;
		}

		public ProgressIncreaseEventArgs(int max, int step)
		{
			this.Max = max;
			this.Step = step;
		}

		public int Max { get; private set; }

		public int Step { get; private set; }

		public string Description { get; private set; }
	}
}