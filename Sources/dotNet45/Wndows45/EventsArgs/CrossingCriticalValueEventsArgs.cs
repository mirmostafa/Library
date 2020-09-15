using System;

namespace Mohammad.Win.EventsArgs
{
    public class CrossingCriticalValueEventsArgs : EventArgs
    {
        public CrossingCriticalValueEventsArgs(decimal criticalValue, decimal currentValue)
        {
            this.CriticalValue = criticalValue;
            this.CurrentValue = currentValue;
        }

        public decimal CriticalValue { get; }
        public decimal CurrentValue { get; }
    }
}