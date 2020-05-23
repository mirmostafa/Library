#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;

namespace Mohammad.EventsArgs
{
    public class ActingEventArgs : EventArgs
    {
        public bool Handled { get; set; }
    }

    public class ItemActedEventArgs<TItem> : EventArgs
    {
        public ItemActedEventArgs(TItem item) => this.Item = item;
        public TItem Item { get; set; }
    }

    public class ItemActingEventArgs<TItem> : ActingEventArgs
    {
        public ItemActingEventArgs()
        {
        }

        public ItemActingEventArgs(TItem item) => this.Item = item;
        public TItem Item { get; set; }
    }

    public class ChangedEventArgs<T> : EventArgs
    {
        public ChangedEventArgs(T oldValue, T newValue)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        public T OldValue { get; }
        public T NewValue { get; set; }
    }

    public class ChangingEventArgs<T> : ActingEventArgs
    {
        public ChangingEventArgs(T oldValue, T newValue)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        public T OldValue { get; }
        public T NewValue { get; set; }
    }
}