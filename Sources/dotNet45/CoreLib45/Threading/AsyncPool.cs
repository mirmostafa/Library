#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Mohammad.Collections.ObjectModel;
using Mohammad.DesignPatterns.ExceptionHandlingPattern;
using Mohammad.EventsArgs;
using Mohammad.Helpers;

namespace Mohammad.Threading
{
    [Obsolete("Use async/await instead.", true)]
    public class AsyncPool
    {
        private readonly UniqueCollection<object> _Groups = new UniqueCollection<object>();
        private          bool                     _Aborted;
        private          ExceptionHandling        _ExceptionHandling;

        public AsyncPool(uint maximumCount)
            : this(maximumCount, string.Empty)
        {
        }

        public AsyncPool(uint maximumCount, ExceptionHandling exceptionHandling)
            : this(maximumCount, string.Empty, exceptionHandling)
        {
        }

        public AsyncPool(uint maximumCount, string name)
            : this(maximumCount, name, null)
        {
        }

        public AsyncPool(uint maximumCount, string name, ExceptionHandling exceptionHandling)
        {
            this.MaximumCount      = maximumCount;
            this.Stocks            = new Collection<Async>();
            this.AsyncsQ           = new Queue<Async>();
            this.Name              = name;
            this.ExceptionHandling = exceptionHandling;
        }

        public ExceptionHandling ExceptionHandling
        {
            get => this._ExceptionHandling;
            set
            {
                this._ExceptionHandling = value;
                this.AsyncsQ.ForEach(async1 => async1.ExceptionHandling = value);
            }
        }

        public            IEnumerable<Async> Asyncs       => this.AsyncsQ.AsEnumerable();
        protected virtual Queue<Async>       AsyncsQ      { get; }
        public            string             Name         { get; }
        public            uint               MaximumCount { get; set; }
        protected         Collection<Async>  Stocks       { get; }
        public            IEnumerable<Async> Jobs         => this.Stocks.AsEnumerable();

        public bool Aborted
        {
            get => this._Aborted;
            private set
            {
                if (this._Aborted == value)
                    return;
                this._Aborted = value;
                this.AbortedChanged.RaiseAsync(this);
            }
        }

        public event EventHandler<ItemActedEventArgs<object>> AsyncGroupEnded;
        public event EventHandler<ItemActedEventArgs<object>> AsyncGroupStarted;
        public event EventHandler                             QuequeChanged;
        public event EventHandler                             JobsChanged;
        public event EventHandler                             AbortedChanged;

        public AsyncPool AbortAllAsyncs()
        {
            this.Aborted = true;
            var pool = new AsyncPool(Convert.ToUInt32(this.AsyncsQ.Count + this.Stocks.Count + 1));
            while (this.AsyncsQ.Count > 0)
            {
                Async.Run(() => CodeHelper.Catch(this.AsyncsQ.Dequeue().Abort), pool: pool);
                this.QuequeChanged.RaiseAsync(this);
            }

            var stocks = this.Stocks.ToList();
            stocks.ForEach(stock => Async.Run(() => CodeHelper.Catch(stock.Abort), pool: pool));
            return pool;
        }

        internal void Register(Async myAsync)
        {
            if (myAsync == null)
                throw new ArgumentNullException("myAsync");
            if (this.Aborted)
                return;
            this.AsyncsQ.Enqueue(myAsync);
            this.QuequeChanged.RaiseAsync(this);
        }

        internal void Start()
        {
            if (this.Aborted)
                return;
            if (this.Stocks.Count >= this.MaximumCount)
                return;
            this.Aborted = false;
            this.Refresh();
        }

        private void Refresh()
        {
            if (this.Aborted)
                return;
            while (this.Stocks.Count <= this.MaximumCount && this.AsyncsQ.Count > 0)
            {
                var item = this.AsyncsQ.Dequeue();
                this.QuequeChanged.RaiseAsync(this);
                item.Ended += (sender, e) =>
                {
                    var a = sender as Async;
                    this.Stocks.Remove(a);
                    this.JobsChanged.RaiseAsync(this);
                    if (a != null)
                        if (a.Group != null)
                        {
                            this._Groups.Remove(a.Group);
                            if (!this._Groups.Contains(a.Group))
                                this.AsyncGroupEnded.Raise(this, new ItemActedEventArgs<object>(a.Group));
                        }

                    this.Refresh();
                };
                this.Stocks.Add(item);
                this.JobsChanged.RaiseAsync(this);
                CodeHelper.Catch(() =>
                                 {
                                     item.InnerStart();
                                     this._Groups.Add(item.Group);
                                     if (!this._Groups.Contains(item.Group))
                                         this.AsyncGroupStarted.Raise(this, new ItemActedEventArgs<object>(item.Group));
                                 },
                                 handling: this.ExceptionHandling);
            }
        }

        public override string ToString() => string.Format("{0} Asyncs: {1}, Jobs: {2}", this.Name, this.AsyncsQ.Count, this.Stocks.Count);

        internal void Unregister(Async myAsync)
        {
        }
    }
}