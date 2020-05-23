using System;
using Mohammad.Logging;

namespace Mohammad.Wpf.EventsArgs
{
    public class SettingStatusEventArgs : EventArgs
    {
        public string Status { get; }
        public bool? IsWorking { get; }
        public string Detail { get; }
        public LogLevel Level { get; }

        public SettingStatusEventArgs(string status, bool? isWorking, string detail, LogLevel level)
        {
            this.Status = status;
            this.IsWorking = isWorking;
            this.Detail = detail;
            this.Level = level;
        }
    }
}