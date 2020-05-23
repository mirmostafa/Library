using System;
using System.Windows;
using Mohammad.EventsArgs;
using Mohammad.Exceptions;
using Mohammad.Helpers;
using Mohammad.Wpf.Windows.Controls;

namespace Mohammad.Wpf.Windows.Input.LibCommands
{
    public class ShowCommonDialogCommand : LibCommand
    {
        public bool ShowStatusBar { get; set; }
        public Type PageType { get; set; }
        public bool IsMultiInstance { get; set; }
        public LibraryCommonDialog Dialog { get; set; }
        public string Title { get; set; }
        public bool IsDialog { get; set; }
        public bool? DialogResult { get; private set; }
        public bool ShowCommandBar { get; set; }
        public Window OwnerWindow { get { return this.Parent as Window; } set { this.Parent = value; } }

        protected override void OnExecuted()
        {
            if (this.IsMultiInstance && this.Dialog != null)
            {
                this.Dialog.BringIntoView();
                this.Dialog.WindowState = WindowState.Normal;
                this.Dialog.Focus();
                return;
            }
            if (this.PageType.IsAssignableFrom(typeof(LibraryCommonPage)))
                throw new ParseException($"Cannot convert '{this.PageType}' to 'LibraryCommonPage'");
            var constructor = this.PageType.GetConstructor(new Type[] {});
            if (constructor == null)
                return;
            var page = (LibraryCommonPage) constructor.Invoke(null);
            this.Dialog = new LibraryCommonDialog {IsDialog = this.IsDialog, ShowStatusBar = this.ShowStatusBar, ShowCommandBar = this.ShowCommandBar, Page = page};
            var window = (this.OwnerWindow ?? this.Parent) as Window;
            if (!Equals(window, this.Dialog))
                this.Dialog.Owner = window;
            if (!this.Title.IsNullOrEmpty())
                this.Dialog.Title = this.Title;
            var dialog = this.Dialog;
            this.Dialog.Closed += (_, __) => this.Dialog = null;
            try
            {
                var parent = this.Parent as Window;
                if (parent != null)
                    parent.Opacity = .75;
                this.DialogResult = this.Dialog.ShowDialog();
            }
            finally
            {
                var parent = this.Parent as Window;
                if (parent != null)
                    parent.Opacity = 1;
                this.OnDialogClosed(dialog);
            }
        }

        public event EventHandler<ItemActedEventArgs<LibraryCommonDialog>> DialogClosed;
        private void OnDialogClosed(LibraryCommonDialog dialog) { this.DialogClosed.Raise(this, new ItemActedEventArgs<LibraryCommonDialog>(dialog)); }
    }
}