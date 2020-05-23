using System;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using Mohammad.EventsArgs;
using Mohammad.Helpers;
using Mohammad.Wpf.Windows.Dialogs;

namespace Mohammad.Wpf.Windows.Input.LibCommands
{
    public class ExitAppCommand : LibCommand
    {
        public string ConfirmationPrompt { get; set; }
        public string ConfirmationInstruction { get; set; }
        public ExitAppCommand() { this.Content = "E_xit"; }

        protected override void OnExecuted()
        {
            if (!this.ConfirmationPrompt.IsNullOrEmpty() && !this.ConfirmationInstruction.IsNullOrEmpty())
            {
                if (MsgBoxEx.AskWithWarn(this.ConfirmationInstruction, this.ConfirmationPrompt) != TaskDialogResult.Yes)
                    return;
            }
            else if (!this.ConfirmationPrompt.IsNullOrEmpty())
            {
                if (MsgBoxEx.AskWithWarn(text: this.ConfirmationPrompt) != TaskDialogResult.Yes)
                    return;
            }
            else if (!this.ConfirmationInstruction.IsNullOrEmpty())
            {
                if (MsgBoxEx.AskWithWarn(this.ConfirmationInstruction) != TaskDialogResult.Yes)
                    return;
            }
            var args = new ActingEventArgs();
            this.OnExitting(args);
            if (args.Handled)
                return;

            Application.Current.Shutdown();
        }

        public event EventHandler<ActingEventArgs> Exitting;
        protected virtual void OnExitting(ActingEventArgs e) { this.Exitting.Raise(this, e); }
    }
}