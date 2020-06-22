using System;

namespace Mohammad.EventsArgs
{
    public class ProgressiveOperationEventArgs<T> : EventArgs
    {
        public double Step { get; }
        public double Max { get; }
        public T Data { get; set; }

        public ProgressiveOperationEventArgs(T data = default(T))
            : this(-1, -1, data) { }

        public ProgressiveOperationEventArgs(double step, double max, T data = default(T))
        {
            this.Step = step;
            this.Max = max;
            this.Data = data;
        }
    }
}