using System;

namespace Mohammad.Win.EventsArgs
{
    public class CrossingCriticalValueEventsArgs : EventArgs
    {
        public decimal CriticalValue { get; private set; }
        public decimal CurrentValue { get; private set; }

        public CrossingCriticalValueEventsArgs(decimal criticalValue, decimal currentValue)
        {
            this.CriticalValue = criticalValue;
            this.CurrentValue = currentValue;
        }
    }
}