using System;
using System.Windows;

namespace Mohammad.Wpf.EventsArgs
{
    public class ApplySettingsEventArgs : EventArgs

    {
        public readonly Window Window;
        public ApplySettingsEventArgs(Window window) { this.Window = window; }
    }
}