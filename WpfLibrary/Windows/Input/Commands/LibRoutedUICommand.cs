using System;
using System.Windows.Input;

namespace WpfLibrary.Commands
{
    public class LibRoutedUICommand : RoutedUICommand
    {
        public LibRoutedUICommand()
        {
        }

        public LibRoutedUICommand(string text, string name, Type ownerType)
            : base(text, name, ownerType)
        {
        }

        public LibRoutedUICommand(string text, string name, Type ownerType, InputGestureCollection inputGestures)
            : base(text, name, ownerType, inputGestures)
        {
        }
    }
}
