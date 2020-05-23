using System.Windows.Input;

namespace Mohammad.Wpf.Windows.Input.LibCommands
{
    public class LibCommandBinding : CommandBinding
    {
        public new ICommand Command { get { return base.Command; } set { base.Command = value; } }
        public LibCommandBinding() { }

        public LibCommandBinding(ICommand command)
            : base(command) { }

        public LibCommandBinding(ICommand command, ExecutedRoutedEventHandler executed)
            : base(command, executed) { }

        public LibCommandBinding(ICommand command, ExecutedRoutedEventHandler executed, CanExecuteRoutedEventHandler canExecute)
            : base(command, executed, canExecute) { }
    }
}