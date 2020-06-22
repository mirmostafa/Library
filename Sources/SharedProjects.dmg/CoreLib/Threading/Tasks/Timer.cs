using System;
using System.Threading;

namespace Mohammad.Threading.Tasks
{
    public partial class Timer : IEquatable<Timer>
    {
        internal readonly ManualResetEvent Working = new ManualResetEvent(false);
        public int Id { get; }
        public long? TickIndex { get; internal set; }
        public long? TickCount { get; }
        public object Tag { get; internal set; }
        public DateTime SignalTime { get; internal set; }
        internal System.Threading.Timer InnerTimer { get; set; }
        internal bool IsStopRequested { get; set; }

        internal TimeSpan Interval { get; set; }

        internal Timer(int id, long? tickCount)
        {
            this.Id = id;
            this.TickCount = tickCount;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == this.GetType() && this.Equals((Timer) obj);
        }

        public void WaitForStop() { this.Working.WaitOne(); }
        public async void WaitForStopAsync() { await Async.Run(this.Working.WaitOne); }

        public override int GetHashCode() => this.Id;
        public static bool operator ==(Timer left, Timer right) => Equals(left, right);
        public static bool operator !=(Timer left, Timer right) => !Equals(left, right);

        public override string ToString()
        {
            var result = $"{nameof(this.Id)}: {this.Id}, {nameof(this.Interval)}: {this.Interval}";
            if (this.TickIndex.HasValue)
                result = $"{result}, {nameof(this.TickIndex)}: {this.TickIndex}";
            return result;
        }

        public bool Equals(Timer other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return this.Id == other.Id;
        }
    }
}