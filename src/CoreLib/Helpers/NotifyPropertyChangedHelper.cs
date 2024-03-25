using System.ComponentModel;

namespace Library.Helpers;


public static class NotifyPropertyChangedHelper
{
    /// <summary>
    /// Handles the property changes for an object that implements INotifyPropertyChanged.
    /// </summary>
    /// <param name="o">The object to handle property changes for.</param>
    /// <param name="propertyChangedHandler">The PropertyChangedEventHandler to use.</param>
    /// <returns>The object with the PropertyChangedEventHandler attached.</returns>
    public static TNotifyPropertyChanged HandlePropertyChanges<TNotifyPropertyChanged>(this TNotifyPropertyChanged? o, in PropertyChangedEventHandler? propertyChangedHandler)
        where TNotifyPropertyChanged : INotifyPropertyChanged
    {
        if (o != null)
        {
            o.PropertyChanged -= propertyChangedHandler;
            o.PropertyChanged += propertyChangedHandler;
        }
        return o;
    }
}