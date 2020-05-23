#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library35.Windows.Controls.BarChart
{
	public class BarEventArgs : EventArgs
	{
		private readonly HBarItem bar;

		public BarEventArgs()
		{
			this.bar = null;
			this.BarIndex = -1;
		}

		public BarEventArgs(HBarItem bar, int nBarIndex)
		{
			this.bar = bar;
			this.BarIndex = nBarIndex;
		}

		public HBarItem Bar
		{
			get { return this.bar; }
		}

		public int BarIndex { get; set; }
	}
}