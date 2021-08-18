using Library.EventsArgs;

namespace Library
{
    public sealed record NotifyPropertyChanged(in object? Sender, in string? PropertyType, in string PropertyName, in object? PropertyValue);

    public interface IChangeNotifierProperty
    {
        event EventHandler<ItemActedEventArgs<NotifyPropertyChanged>>? Changed;
    }
}