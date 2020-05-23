using System;
using System.Windows;
using Mohammad.EventsArgs;
using Mohammad.Helpers;
using Mohammad.Wpf.Helpers;

namespace Mohammad.Wpf.Windows.Input.LibCommands
{
    public class CloseCommand : LibCommand
    {
        public bool? DialogResult { get; set; }
        public CloseCommand() { this.Content = "_Close"; }

        protected override void OnExecuted()
        {
            var args = new ActingEventArgs();
            this.OnClosing(args);
            if (args.Handled)
                return;
            var wnd = this.Parent is Window ? this.Parent.As<Window>() : this.Parent.GetParent<Window>();
            if (wnd == null)
                return;
            if (!Equals(wnd.DialogResult, this.DialogResult))
                CodeHelper.Catch(() => wnd.DialogResult = this.DialogResult);
            else
                wnd.Close();
        }

        public event EventHandler<ActingEventArgs> Closing;
        protected virtual void OnClosing(ActingEventArgs e) { this.Closing.Raise(this, e); }
    }
}