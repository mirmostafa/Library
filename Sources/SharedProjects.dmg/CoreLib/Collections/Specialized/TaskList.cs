using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mohammad.Threading.Tasks;

namespace Mohammad.Collections.Specialized
{
    public class TaskList : List<Task>
    {
        private readonly CancellationTokenSource _CancellationTokenSource;
        public bool IsCancellationRequested => this._CancellationTokenSource.IsCancellationRequested;

        public TaskList() { this._CancellationTokenSource = new CancellationTokenSource(); }

        public IEnumerable<Task> Add(params Task[] items) => items.Select(this.Add);

        public bool WaitAll(TimeSpan timeout) => Task.WaitAll(this.ToArray(), timeout);

        public IEnumerable<Task> Add(IEnumerable<Task> items) => items.Select(this.Add);

        public void WaitAll() { Task.WaitAll(this.ToArray()); }
        public void WaitAny() { Task.WaitAny(this.ToArray()); }

        public async Task WaitAllAsync() { await Async.Run(() => Task.WaitAll(this.ToArray())); }
        public async Task WaitAnyAsync() { await Async.Run(() => Task.WaitAny(this.ToArray())); }

        public Task Run(Action action) => this.Add(Async.Run(action, this._CancellationTokenSource.Token));

        public IEnumerable<Task> Run(params Action[] actions) => actions.Select(this.Run);

        public IEnumerable<Task> Run(IEnumerable<Action> actions) => actions.Select(this.Run);

        public void CancelAll() { this._CancellationTokenSource.Cancel(); }

        public new Task Add(Task item)
        {
            base.Add(item);
            return item;
        }
    }
}