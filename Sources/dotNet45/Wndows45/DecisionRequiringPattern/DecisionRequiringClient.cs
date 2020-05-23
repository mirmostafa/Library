using System;
using Mohammad.Helpers;
using Mohammad.Win.EventsArgs;

namespace Mohammad.Win.DecisionRequiringPattern
{
    public class DecisionRequiringClient
    {
        public event EventHandler<DecidingEventArgs> Deciding;
        protected void OnDeciding(DecidingEventArgs e) { this.Deciding.Raise(this, e); }
        internal void PerformDeciding(DecidingEventArgs e) { this.OnDeciding(e); }
    }
}