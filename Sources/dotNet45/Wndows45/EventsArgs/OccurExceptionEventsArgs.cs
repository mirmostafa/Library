using System;

namespace Mohammad.Win.EventsArgs
{
    public class OccurExceptionEventsArgs : EventArgs
    {
        public OccurExceptionEventsArgs(string message) => this.Message = message;
        public string Message { get; }
    }
}