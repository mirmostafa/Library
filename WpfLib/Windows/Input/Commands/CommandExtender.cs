﻿using System.Windows.Input;
using Library.Exceptions.Validations;
using Library.Validations;

namespace Library.Wpf.Windows.Input.Commands
{
    public sealed class CommandExtender : IEquatable<RoutedUICommand>, IEquatable<CommandExtender>, ILibCommand
    {
        private bool _IsEnabled;

        public CommandExtender(CommandBinding commandBinding)
        {
            this.Command = commandBinding
               ?.Command
                .As<RoutedUICommand>()
                .NotNull(() => new TypeMismatchValidationException($"{nameof(commandBinding)} is null. Or could not convert CommandBinding.Commmand to {typeof(RoutedUICommand)}"))!;
            this.CommandBinding = commandBinding!;
            this.IsEnabled = this.Command.CanExecute(null, null);
            this.Command.CanExecuteChanged += this.Command_CanExecuteChanged;
        }

        public RoutedUICommand Command { get; }
        public CommandBinding CommandBinding { get; }
        public bool IsEnabled
        {
            get => this._IsEnabled;
            set => this._IsEnabled = value;//_ = this.Command.
        }
        public string Text { get => this.Command.Text; set => this.Command.Text = value; }

        public event EventHandler? CanExecuteChanged;

        public bool Equals(RoutedUICommand? other)
            => other is not null && this.Command == other;
        public bool Equals(CommandExtender? other)
            => other?.Command.Equals(this) ?? false;
        public override bool Equals(object? obj)
            => obj is CommandExtender extender && this.Equals(extender);

        public override int GetHashCode()
            => HashCode.Combine(this.Command);

        public static bool operator ==(CommandExtender left, CommandExtender right)
            => left?.Equals(right) ?? (right is null);
        public static bool operator !=(CommandExtender left, CommandExtender right)
            => !(left == right);

        private void Command_CanExecuteChanged(object? sender, EventArgs e)
        {
            this.IsEnabled = !this.IsEnabled;
            this.CanExecuteChanged?.Invoke(this, e);
        }

        //
        // Summary:
        //     Gets the collection of System.Windows.Input.InputGesture objects that are associated
        //     with this command.
        //
        // Returns:
        //     The input gestures.
        public InputGestureCollection InputGestures => this.Command.InputGestures;

        //
        // Summary:
        //     Gets the name of the command.
        //
        // Returns:
        //     The name of the command.
        public string Name => this.Command.Name;

        //
        // Summary:
        //     Gets the type that is registered with the command.
        //
        // Returns:
        //     The type of the command owner.
        public Type OwnerType => this.Command.OwnerType;


        //
        // Summary:
        //     Determines whether this System.Windows.Input.RoutedCommand can execute in its
        //     current state.
        //
        // Parameters:
        //   parameter:
        //     A user defined data type.
        //
        //   target:
        //     The command target.
        //
        // Returns:
        //     true if the command can execute on the current command target; otherwise, false.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     target is not a System.Windows.UIElement or System.Windows.ContentElement.
        public bool CanExecute(object parameter, IInputElement target)
            => this.Command.CanExecute(parameter, target);

        //
        // Summary:
        //     Executes the System.Windows.Input.RoutedCommand on the current command target.
        //
        // Parameters:
        //   parameter:
        //     User defined parameter to be passed to the handler.
        //
        //   target:
        //     Element at which to begin looking for command handlers.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     target is not a System.Windows.UIElement or System.Windows.ContentElement.
        public void Execute(object parameter, IInputElement target)
            => this.Command.Execute(parameter, target);

        //
        // Summary:
        //     For a description of this members, see System.Windows.Input.ICommand.CanExecute(System.Object).
        //
        // Parameters:
        //   parameter:
        //     Data used by the command. If the command does not require data to be passed,
        //     this object can be set to null.
        //
        // Returns:
        //     true if this command can be executed; otherwise, false.
        bool ICommand.CanExecute(object? parameter)
            => this.Command.To<ICommand>().CanExecute(parameter);

        //
        // Summary:
        //     For a description of this members, see System.Windows.Input.ICommand.Execute(System.Object).
        //
        // Parameters:
        //   parameter:
        //     Data used by the command. If the command does not require data to be passed,
        //     this object can be set to null.
        void ICommand.Execute(object? parameter)
            => this.Command.To<ICommand>().Execute(parameter);
    }
}
