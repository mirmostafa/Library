using System.Windows.Input;

namespace Library.Wpf.DesignPatterns.Command;

public interface IControlCommand : ICommand
{
    string Text { get; }
    string CommandGroupName { get; }
    bool IsEnabled { get; }
    Visibility Visibility { get; }
}

public interface IControlCommandGroup : IControlCommand
{
    event EventHandler Invoke;

    IList<IControlCommand> Commands { get; }
    InputGestureCollection InputGestures { get; }
    string Name { get; }
}