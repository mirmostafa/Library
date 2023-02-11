using System.Windows.Input;

using Library.Collections;

namespace Library.Wpf.Windows.Input.Commands;

internal sealed class CommandExtenderList : HashSet<CommandExtender>,
    IIndexable<RoutedUICommand, CommandExtender?>,
    IIndexable<CommandBinding, CommandExtender?>,
    IIndexable<string, CommandExtender?>
{
    public CommandExtender? this[RoutedUICommand index] => this.FirstOrDefault(item => item.Command == index);

    public CommandExtender? this[string commandName] => this.FirstOrDefault(item => item.Command?.Name == commandName);

    public CommandExtender? this[CommandBinding commandBinding] => this.FirstOrDefault(item => item.CommandBinding == commandBinding);

    public CommandExtender? FirstOrDefault(in Predicate<CommandExtender> predicate)
    {
        foreach (var item in this)
        {
            if (predicate(item))
            {
                return item;
            }
        }
        return null;
    }

    public CommandExtender? FirstOrDefault(in Predicate<RoutedUICommand> predicate)
    {
        foreach (var item in this)
        {
            if (predicate(item.Command))
            {
                return item;
            }
        }
        return null;
    }
}