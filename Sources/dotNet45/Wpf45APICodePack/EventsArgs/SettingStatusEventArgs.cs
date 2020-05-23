using System;
using Mohammad.Logging;

namespace Mohammad.Wpf.EventsArgs
{
    public class SettingStatusEventArgs : EventArgs
    {
        public string Status { get; private set; }
        public bool? IsWorking { get; private set; }
        public string Detail { get; private set; }
        public LogLevel Level { get; private set; }

        public SettingStatusEventArgs(string status, bool? isWorking, string detail, LogLevel level)
        {
            this.Status = status;
            this.IsWorking = isWorking;
            this.Detail = detail;
            this.Level = level;
        }
    }
}