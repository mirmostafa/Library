#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:01 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using Library35.Windows.EventsArgs;

namespace Library35.Windows.DecisionRequiringPattern
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