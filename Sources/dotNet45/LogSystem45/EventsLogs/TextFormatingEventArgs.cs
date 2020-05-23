using System;

namespace Mohammad.Logging.EventsLogs
{
    public class TextFormatingEventArgs : EventArgs
    {
        public string Text { get; set; }
        public TextFormatingEventArgs(string text) { this.Text = text; }
    }
}