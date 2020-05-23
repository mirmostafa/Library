using System;

namespace Mohammad.Win.EventsArgs
{
    public class AddedValueEventsArgs : EventArgs
    {
        public decimal CurrentValue { get; private set; }
        public decimal BeforeValue { get; private set; }

        public AddedValueEventsArgs(decimal currentValue, decimal beforeValue)
        {
            this.BeforeValue = beforeValue;
            this.CurrentValue = currentValue;
        }
    }
}