using System.Windows;
using System.Windows.Input;

namespace Mohammad.Wpf.Windows.Input.LibCommands
{
    public interface ILibCommand : ICommand
    {
        object Content { get; set; }
        bool IsEnabled { get; set; }
        Visibility Visibility { get; set; }
    }
}