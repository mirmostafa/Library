using System.Windows.Data;

namespace Library.Wpf.Windows.Input.Commands;

public class CommandDependencyPropertyBag<TUiElement>
    where TUiElement : UIElement
{
    private readonly Dictionary<string, LibRoutedUICommand> _bag = new();

    public LibRoutedUICommand this[string key]
    {
        get => this._bag[key];
        set => this._bag[key] = value;
    }

    public void AddCommand(LibRoutedUICommand command, string? key = null)
    {
        Check.IfArgumentNotNull(command, nameof(command));
        this._bag.Add(key ?? command.Name, command);
    }

    //! Not logical
    //public DependencyProperty GetByKey(string key, Func<bool>? canExecute, Action execute)
    //    => DependencyProperty.Register(
    //        key,
    //        typeof(LibRoutedUICommand),
    //        typeof(TUiElement),
    //        new FrameworkPropertyMetadata(
    //            default,
    //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal,
    //            null)
    //        { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged },
    //        value => this[key].CanExecute(null, null));

    public DependencyProperty GetByKey(string key)
        => DependencyProperty.Register(
            key,
            typeof(LibRoutedUICommand),
            typeof(TUiElement),
            new FrameworkPropertyMetadata(
                default,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal,
                null)
            { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged },
            value => this[key].CanExecute(null, null));
}