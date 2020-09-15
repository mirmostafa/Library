using System;

namespace Mohammad.Threading
{
    [Obsolete("Use async/await instead.")]
    public enum AsyncStatus
    {
        Initializing,
        Waiting,
        Running,
        Ended
    }
}