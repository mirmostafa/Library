using Mohammad.Wpf.Windows.Input.LibCommands;

namespace Mohammad.Wpf.Windows.Input
{
    public interface ILibCommandSource
    {
        /// <summary>Gets the command that will be executed when the command source is invoked.</summary>
        /// <returns>The command that will be executed when the command source is invoked.</returns>
        ILibCommand Command { get; }

        /// <summary>Represents a user defined data value that can be passed to the command when it is executed.</summary>
        /// <returns>The command specific data.</returns>
        object CommandParameter { get; }

        /// <summary>The object that the command is being executed on.</summary>
        /// <returns>The object that the command is being executed on.</returns>
        ILibInputElement CommandTarget { get; }
    }
}