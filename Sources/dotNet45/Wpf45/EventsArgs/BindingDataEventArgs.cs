using System;

namespace Mohammad.Wpf.EventsArgs
{
    public class BindingDataEventArgs : EventArgs
    {
        public bool IsFirstDataRebind { get; }
        public BindingDataEventArgs(bool isFirstDataRebind) => this.IsFirstDataRebind = isFirstDataRebind;
    }
}