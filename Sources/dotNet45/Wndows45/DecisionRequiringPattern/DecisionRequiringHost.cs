using Mohammad.Win.EventsArgs;

namespace Mohammad.Win.DecisionRequiringPattern
{
    public class DecisionRequiringHost
    {
        public DecisionRequiringHost(DecisionRequiringClient client) => this.Client = client;
        public DecisionRequiringClient Client { get; }

        public void Ask(DecidingEventArgs e)
        {
            this.Client.PerformDeciding(e);
        }
    }
}