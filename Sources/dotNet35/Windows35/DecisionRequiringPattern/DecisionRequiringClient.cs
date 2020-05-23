#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:01 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using Library35.Helpers;
using Library35.Windows.EventsArgs;

namespace Library35.Windows.DecisionRequiringPattern
{
	public class DecisionRequiringClient
	{
		public event EventHandler<DecidingEventArgs> Deciding;

		protected void OnDeciding(DecidingEventArgs e)
		{
			this.Deciding.Raise(this, e);
		}

		internal void PerformDeciding(DecidingEventArgs e)
		{
			this.OnDeciding(e);
		}
	}
}