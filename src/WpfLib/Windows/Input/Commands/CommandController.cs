using System.Windows.Input;

using Library.Collections;
using Library.Exceptions.Validations;

namespace Library.Wpf.Windows.Input.Commands;

public sealed class CommandController : IIndexable<string, CommandExtender>, IIndexable<CommandBinding, CommandExtender>
{
    private readonly CommandExtenderList _commandExtenders = new();
    private readonly UIElement _owner;

    public CommandController(UIElement owner)
    {
        this._owner = owner;
        _ = this._commandExtenders.AddRange(this._owner.CommandBindings.Cast<CommandBinding>().Compact().Select(x => new CommandExtender(x))).AsReadOnly();
    }

    public CommandExtender this[CommandBinding index] => this._commandExtenders[index].NotNull(New<NotFoundValidationException>);

    public CommandExtender this[string commandName] => this._commandExtenders[commandName].NotNull(New<NotFoundValidationException>);

    public CommandController SetEnabled(CommandBinding commandBinding, bool isEnabled)
    {
        this[commandBinding].NotNull(New<NotFoundValidationException>).IsEnabled = isEnabled;
        return this;
    }
}