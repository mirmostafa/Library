#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using Library40.Helpers;
using Library40.Win.EventsArgs;

namespace Library40.Win.DecisionRequiringPattern
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