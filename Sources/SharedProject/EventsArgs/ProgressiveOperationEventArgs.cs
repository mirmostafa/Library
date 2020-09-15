using System;

namespace Mohammad.EventsArgs
{
    public class ProgressiveOperationEventArgs<T> : EventArgs
    {
        public ProgressiveOperationEventArgs(T data = default)
            : this(-1, -1, data)
        {
        }

        public ProgressiveOperationEventArgs(double step, double max, T data = default)
        {
            this.Step = step;
            this.Max = max;
            this.Data = data;
        }

        public double Step { get; }
        public double Max { get; }
        public T Data { get; set; }
    }
}