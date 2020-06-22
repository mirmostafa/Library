using System;

namespace Mohammad.EventsArgs
{
    public class ActingEventArgs : EventArgs
    {
        public bool Handled { get; set; }
    }

    public class ItemActedEventArgs<TItem> : EventArgs
    {
        public TItem Item { get; set; }
        public ItemActedEventArgs(TItem item) { this.Item = item; }
    }

    public class ItemActingEventArgs<TItem> : ActingEventArgs
    {
        public TItem Item { get; set; }
        public ItemActingEventArgs() { }
        public ItemActingEventArgs(TItem item) { this.Item = item; }
    }

    public class ChangedEventArgs<T> : EventArgs
    {
        public T OldValue { get; private set; }
        public T NewValue { get; set; }

        public ChangedEventArgs(T oldValue, T newValue)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }
    }

    public class ChangingEventArgs<T> : ActingEventArgs
    {
        public T OldValue { get; private set; }
        public T NewValue { get; set; }

        public ChangingEventArgs(T oldValue, T newValue)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }
    }
}