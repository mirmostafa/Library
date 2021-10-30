using Library.Wpf.Markers;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Library.Wpf.Bases;

[ViewModel]
public abstract class ViewModelBase : IViewModel, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

}
