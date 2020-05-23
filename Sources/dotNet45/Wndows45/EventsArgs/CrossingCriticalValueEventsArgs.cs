using System;

namespace Mohammad.Win.EventsArgs
{
    public class CrossingCriticalValueEventsArgs : EventArgs
    {
        public decimal CriticalValue { get; }
        public decimal CurrentValue { get; }

        public CrossingCriticalValueEventsArgs(decimal criticalValue, decimal currentValue)
        {
            this.CriticalValue = criticalValue;
            this.CurrentValue = currentValue;
        }
    }
}