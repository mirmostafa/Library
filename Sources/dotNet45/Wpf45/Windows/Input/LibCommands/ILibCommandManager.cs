using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Mohammad.Wpf.Windows.Input.LibCommands
{
    public interface ILibCommandManager : IEnumerable<LibCommand>, INotifyPropertyChanged
    {
        bool IsEnabled { get; set; }
        Visibility Visibility { get; set; }
        LibCommand this[string commandName] { get; }
    }
}