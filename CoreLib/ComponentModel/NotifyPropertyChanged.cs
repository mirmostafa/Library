using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Library.ComponentModel;

public abstract class NotifyPropertyChanged : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public bool ValidateOnPropertySet { get; set; } = true;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    protected virtual void SetProperty<TProperty>(ref TProperty backingField, TProperty value, [CallerMemberName] string? propertyName = null)
    {
        if (this.ValidateOnPropertySet && value?.Equals(backingField) is true)
        {
            return;
        }

        backingField = value;
        this.OnPropertyChanged(propertyName);
    }
}