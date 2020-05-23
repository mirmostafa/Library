using System;

namespace Mohammad.Win.EventsArgs
{
    public class AddedValueEventsArgs : EventArgs
    {
        public decimal CurrentValue { get; }
        public decimal BeforeValue { get; }

        public AddedValueEventsArgs(decimal currentValue, decimal beforeValue)
        {
            this.BeforeValue = beforeValue;
            this.CurrentValue = currentValue;
        }
    }
}