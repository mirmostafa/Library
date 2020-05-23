using System;

namespace Mohammad.Wpf.Windows
{
    [Flags]
    public enum TaskbarProgressBarState
    {
        NoProgress = 0,
        Indeterminate = 1,
        Normal = 2,
        Error = 4,
        Paused = 8
    }
}