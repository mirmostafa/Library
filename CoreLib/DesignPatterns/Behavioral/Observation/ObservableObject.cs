﻿using Library.Coding;
using Library.EventsArgs;

namespace Library.DesignPatterns.Behavioral.Observation
{
    public class ObservableObject<TValue>
    {
        public event EventHandler<ItemActingEventArgs<TValue?>>? Changing;
        public event EventHandler<ItemActedEventArgs<TValue?>>? Changed;

        private TValue? _Value;
        public TValue? Value
        {
            get => this._Value;
            set
            {
                var e = this.OnChanging(new ItemActingEventArgs<TValue?>(value));
                if (e.Handled)
                {
                    return;
                }

                this._Value = value;
                this.OnChanged(new ItemActedEventArgs<TValue?>(this.Value));
            }
        }

        protected ItemActingEventArgs<TValue?> OnChanging(ItemActingEventArgs<TValue?> e)
            => e.Fluent(() => Changing?.Invoke(this, e));

        protected void OnChanged(ItemActedEventArgs<TValue?> e)
            => Changed?.Invoke(this, e);
    }

}
