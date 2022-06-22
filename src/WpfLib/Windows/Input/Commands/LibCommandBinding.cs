using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Library.Wpf.Windows.Input.Commands;

public class LibCommandBinding : CommandBinding
{
    public LibCommandBinding()
    {
        Initialize();
    }

    private void Initialize()
    {
        CanExecute += this.LibCommandBinding_CanExecute;
    }

    private void LibCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.Handled = this.Command is ILibCommand command && !command.IsEnabled;
        if (e.Handled)
        {
            
        }
    }

    private string? _commandText;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string? CommandText
    {
        get => this._commandText;
        set
        {
            this._commandText = value;
            this.OnPropertyChanged();
            this.OnCaptionChanged();
        }
    }

    protected virtual void OnCaptionChanged()
    {
        if (this.Command.As<RoutedUICommand>() is { } command)
        {
            command.Text = this.CommandText;
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}