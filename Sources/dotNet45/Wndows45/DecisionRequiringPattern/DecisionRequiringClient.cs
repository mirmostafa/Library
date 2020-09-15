using System;
using Mohammad.Helpers;
using Mohammad.Win.EventsArgs;

namespace Mohammad.Win.DecisionRequiringPattern
{
    public class DecisionRequiringClient
    {
        internal void PerformDeciding(DecidingEventArgs e)
        {
            this.OnDeciding(e);
        }

        protected void OnDeciding(DecidingEventArgs e)
        {
            this.Deciding.Raise(this, e);
        }

        public event EventHandler<DecidingEventArgs> Deciding;
    }
}