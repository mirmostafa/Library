using System;

namespace Mohammad.Win.EventsArgs
{
    public class OccurExceptionEventsArgs : EventArgs
    {
        public string Message { get; private set; }
        public OccurExceptionEventsArgs(string message) { this.Message = message; }
    }
}