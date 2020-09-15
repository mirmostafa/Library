using System;

namespace Mohammad.Logging.EventsLogs
{
    public class TextFormatingEventArgs : EventArgs
    {
        public TextFormatingEventArgs(string text) => this.Text = text;
        public string Text { get; set; }
    }
}