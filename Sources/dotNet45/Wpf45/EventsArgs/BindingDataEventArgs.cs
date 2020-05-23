using System;

namespace Mohammad.Wpf.EventsArgs
{
    public class BindingDataEventArgs : EventArgs
    {
        public bool IsFirstDataRebind { get; private set; }
        public BindingDataEventArgs(bool isFirstDataRebind) { this.IsFirstDataRebind = isFirstDataRebind; }
    }
}