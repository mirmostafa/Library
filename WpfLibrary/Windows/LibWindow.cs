using System.Windows;

namespace Library.Wpf.Windows.Dialogs
{
    public class LibWindow : Window
    {
        public LibWindow()
            : base() => this.CommandManager = new(this);

        protected Input.Commands.CommandController CommandManager { get; }
    }
}
