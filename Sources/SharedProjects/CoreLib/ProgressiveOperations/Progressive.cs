using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mohammad.EventsArgs;
using Mohammad.Helpers;
using Mohammad.Interfaces;
using Mohammad.Threading.Tasks;

namespace Mohammad.ProgressiveOperations
{
    public abstract class Progressive<T> : IProgressiveOperation<T>
    {
        public CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();
        protected abstract Action[] Steps { get; set; }

        protected virtual T Data { get; set; }

        public bool CanThrowExceptions { get; set; } = true;
        protected virtual void OnProgressChanged(ProgressiveOperationEventArgs<T> e) { this.ProgressChanged?.Invoke(this, e); }

        public void Start()
        {
            for (var index = 0; index < this.Steps.Length; index++)
            {
                if (this.CancellationTokenSource.IsCancellationRequested)
                    return;
                var step = this.Steps[index];
                CodeHelper.Catch(() => step?.Invoke(), this.OnExceptionOccurred);
                var index1 = index;
                CodeHelper.Catch(() => this.OnProgressChanged(new ProgressiveOperationEventArgs<T>(index1, this.Steps.Length, this.Data)));
            }
            this.Steps.ForEach(step => { });
        }

        public async Task StartAsync() { await Async.Run(this.Start); }

        protected virtual void OnExceptionOccurred(Exception ex)
        {
            if (this.CanThrowExceptions)
                throw ex;
        }

        public static Progressive<T> Get(params Action[] steps) => new SimpleProgressive<T>(steps);
        public static Progressive<TData> Get<TData>(params Action[] steps) => new SimpleProgressive<TData>(steps);

        public static void Start(params Action[] steps) { Get(steps).Start(); }

        public static async Task StartAsync(params Action[] steps) { await Get(steps).StartAsync(); }

        public static Progressive<T> Get(IEnumerable<Action> steps, EventHandler<ProgressiveOperationEventArgs<T>> onProgressChanged, T data, bool canThrowExceptions)
        {
            var result = Get(steps.ToArray());
            result.CanThrowExceptions = canThrowExceptions;
            result.Data = data;
            result.ProgressChanged += onProgressChanged;
            return result;
        }

        public static void Start(IEnumerable<Action> steps, EventHandler<ProgressiveOperationEventArgs<T>> onProgressChanged, T data, bool canThrowExceptions)
        {
            var result = Get(steps, onProgressChanged, data, canThrowExceptions);
            result.Start();
        }

        public event EventHandler<ProgressiveOperationEventArgs<T>> ProgressChanged;
    }

    internal class SimpleProgressive<T> : Progressive<T>
    {
        protected sealed override Action[] Steps { get; set; }
        internal SimpleProgressive(Action[] steps) { this.Steps = steps; }
    }

    public class Progressive : Progressive<object>
    {
        protected sealed override Action[] Steps { get; set; }
        public Progressive(Action[] steps) { this.Steps = steps; }
    }
}