using Mohammad.Win.EventsArgs;

namespace Mohammad.Win.DecisionRequiringPattern
{
    public class DecisionRequiringHost
    {
        public DecisionRequiringClient Client { get; }
        public DecisionRequiringHost(DecisionRequiringClient client) { this.Client = client; }
        public void Ask(DecidingEventArgs e) { this.Client.PerformDeciding(e); }
    }
}