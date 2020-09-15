using System;

namespace Mohammad.Wpf.EventsArgs
{
    public class BindingDataEventArgs : EventArgs
    {
        public BindingDataEventArgs(bool isFirstDataRebind) => this.IsFirstDataRebind = isFirstDataRebind;
        public bool IsFirstDataRebind { get; }
    }
}