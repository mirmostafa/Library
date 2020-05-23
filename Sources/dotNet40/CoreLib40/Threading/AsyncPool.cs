#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Library40.Collections.ObjectModel;
using Library40.EventsArgs;
using Library40.ExceptionHandlingPattern;
using Library40.Helpers;

namespace Library40.Threading
{
	public class AsyncPool
	{
		private readonly UniqueCollection<Object> _Groups = new UniqueCollection<object>();
		private readonly string _Name;
		private readonly Collection<Async> _Stocks;
		private bool _Aborted;
		private ExceptionHandling _ExceptionHandling;

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
			this.MaximumCount = maximumCount;
			this._Stocks = new Collection<Async>();
			this.AsyncsQ = new Queue<Async>();
			this._Name = name;
			this.ExceptionHandling = exceptionHandling;
		}

		public ExceptionHandling ExceptionHandling
		{
			get { return this._ExceptionHandling; }
			set
			{
				this._ExceptionHandling = value;
				this.AsyncsQ.ForEach(async1 => async1.ExceptionHandling = value);
			}
		}

		public IEnumerable<Async> Asyncs
		{
			get { return this.AsyncsQ.AsEnumerable(); }
		}

		protected virtual Queue<Async> AsyncsQ { get; private set; }

		public string Name
		{
			get { return this._Name; }
		}

		public uint MaximumCount { get; set; }

		protected Collection<Async> Stocks
		{
			get { return this._Stocks; }
		}

		public IEnumerable<Async> Jobs
		{
			get { return this.Stocks.AsEnumerable(); }
		}

		public bool Aborted
		{
			get { return this._Aborted; }
			private set
			{
				if (this._Aborted == value)
					return;
				this._Aborted = value;
				this.AbortedChanged.Raise(this);
			}
		}

		public event EventHandler AbortedChanged;

		public AsyncPool AbortAllAsyncs()
		{
			this.Aborted = true;
			var pool = new AsyncPool(Convert.ToUInt32(this.AsyncsQ.Count + this._Stocks.Count + 1));
			while (this.AsyncsQ.Count > 0)
			{
				Async.Run(() => MethodHelper.Catch(this.AsyncsQ.Dequeue().Abort), pool: pool);
				this.QuequeChanged.Raise(this);
			}
			var stocks = this.Stocks.ToList();
			stocks.ForEach(stock => Async.Run(() => MethodHelper.Catch(stock.Abort), pool: pool));
			return pool;
		}

		internal void Register(Async myAsync)
		{
			if (myAsync == null)
				throw new ArgumentNullException("myAsync");
			if (this.Aborted)
				return;
			this.AsyncsQ.Enqueue(myAsync);
			this.QuequeChanged.Raise(this);
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

		public event EventHandler<ItemActedEventArgs<object>> AsyncGroupEnded;
		public event EventHandler<ItemActedEventArgs<Object>> AsyncGroupStarted;

		private void Refresh()
		{
			if (this.Aborted)
				return;
			while ((this.Stocks.Count <= this.MaximumCount) && (this.AsyncsQ.Count > 0))
			{
				var item = this.AsyncsQ.Dequeue();
				this.QuequeChanged.Raise(this);
				item.Ended += (sender, e) =>
				              {
					              var a = sender as Async;
					              this.Stocks.Remove(a);
					              this.JobsChanged.Raise(this);
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
				this.JobsChanged.Raise(this);
				MethodHelper.Catch(a =>
				                   {
					                   a.InnerStart();
					                   this._Groups.Add(a.Group);
					                   if (!this._Groups.Contains(a.Group))
						                   this.AsyncGroupStarted.Raise(this, new ItemActedEventArgs<object>(a.Group));
				                   },
					item,
					this.ExceptionHandling);
			}
		}

		public event EventHandler QuequeChanged;
		public event EventHandler JobsChanged;

		public override string ToString()
		{
			return string.Format("{0} Asyncs: {1}, Jobs: {2}", this.Name, this.AsyncsQ.Count, this.Stocks.Count);
		}

		internal void Unregister(Async myAsync)
		{
		}
	}
}