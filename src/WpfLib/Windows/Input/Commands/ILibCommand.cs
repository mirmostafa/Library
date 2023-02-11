using System.Windows.Input;

namespace Library.Wpf.Windows.Input.Commands;

public interface ILibCommand : ICommand
{
    bool IsEnabled { set; get; }
}