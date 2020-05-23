using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Mohammad.Wpf.Helpers
{
    [Obsolete]
    public static class CommandHelper
    {
        public static void Initialize(this ICommand command, UIElement parent, ExecutedRoutedEventHandler execute, CanExecuteRoutedEventHandler canExecute = null,
            KeyGesture gesture = null, params ButtonBase[] buttons)
        {
            if (canExecute == null)
                canExecute = (sender, e) => e.CanExecute = true;
            var cb = new CommandBinding(command, execute, canExecute);
            parent.CommandBindings.Add(cb);

            if (gesture != null)
            {
                var inputBinding = new InputBinding(command, gesture);
                parent.InputBindings.Add(inputBinding);
            }
            foreach (var button in buttons)
                button.Command = command;
        }
    }
}