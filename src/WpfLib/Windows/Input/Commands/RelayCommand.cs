using System.Windows.Input;

namespace Library.Wpf.Windows.Input.Commands;

public sealed class RelayCommand : ICommand
{
    private readonly Action<object?> _Execute;
    private readonly Func<object?, bool>? _CanExecute;

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        this._Execute = execute.ArgumentNotNull(nameof(execute));
        this._CanExecute = canExecute;
    }

    public bool CanExecute(object? parameter)
        => this._CanExecute?.Invoke(parameter) ?? true;

    public void Execute(object? parameter)
        => this._Execute(parameter);
}
