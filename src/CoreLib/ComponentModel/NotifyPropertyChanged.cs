using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Library.ComponentModel;

/// <summary>
/// Abstract base class for objects that implement the INotifyPropertyChanged interface.
/// </summary>
public abstract class NotifyPropertyChanged : INotifyPropertyChanged
{
    /// <summary>
    /// Event that is raised when a property value has changed.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Gets or sets a value indicating whether the property should be validated when it is set. Default value is true.
    /// </summary>
    public bool ValidateOnPropertySet { get; set; } = true;

    /// <summary>
    /// Raises the PropertyChanged event for the specified property.
    /// </summary>
    /// <param name="propertyName">The name of the property that has changed.</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    /// <summary>
    /// Sets the property value and raises the PropertyChanged event.
    /// </summary>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="backingField">The backing field of the property.</param>
    /// <param name="value">The new value of the property.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="orderPropertyNames">The names of the properties that should be raised after the property.</param>
    protected virtual void SetProperty<TProperty>(ref TProperty backingField, TProperty value, [CallerMemberName] string? propertyName = null, params string[] orderPropertyNames)
    {
        if (this.ValidateOnPropertySet && value?.Equals(backingField) is true)
        {
            return;
        }

        backingField = value;
        this.OnPropertyChanged(propertyName);
        _ = orderPropertyNames.ForEach(this.OnPropertyChanged).Build();
    }
}