using System.Linq;
using System.Windows;
using System.Windows.Input;
using Library.Collections;
using Library.Exceptions.Validations;
using Library.Helpers;

namespace Library.Wpf.Windows.Input.Commands
{
    public sealed class CommandController : IIndexable<CommandExtender, string>, IIndexable<CommandExtender, CommandBinding>
    {
        private readonly UIElement _Owner;
        private readonly CommandExtenderList _CommandExtenders = new();

        public CommandExtender this[CommandBinding index] => this._CommandExtenders[index].ArgumentNotNull();
        public CommandExtender this[string commandName] => this._CommandExtenders[commandName].NotNull(() => new NotFoundValidationException());

        public CommandController(UIElement owner)
        {
            this._Owner = owner;
            this.Initialize();
        }

        private void Initialize()
        {
            foreach (var cb in this._Owner.CommandBindings.Cast<CommandBinding>())
            {
                this._CommandExtenders.Add(new(cb));
            }
        }

        public CommandController SetEnabled(CommandBinding commandBinding, bool isEnabled)
        {
            this[commandBinding].NotNull(() => new NotFoundValidationException()).IsEnabled = isEnabled;
            return this;
        }
    }
}
