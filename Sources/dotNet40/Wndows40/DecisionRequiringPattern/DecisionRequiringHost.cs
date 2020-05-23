#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using Library40.Win.EventsArgs;

namespace Library40.Win.DecisionRequiringPattern
{
	public class DecisionRequiringHost
	{
		public DecisionRequiringHost(DecisionRequiringClient client)
		{
			this.Client = client;
		}

		public DecisionRequiringClient Client { get; private set; }

		public void Ask(DecidingEventArgs e)
		{
			this.Client.PerformDeciding(e);
		}
	}
}